# INFORME TÉCNICO: CICLO DE VIDA DE CAJA Y VOLATILIDAD DEL CARRITO DE VENTAS AL CAMBIAR DE PANTALLA

**Proyecto:** StockOS — Sistema de Punto de Venta (POS) y Administración de Stock  
**Módulo Analizado:** Punto de Venta (`UcVentas`), Navegación Principal (`FormInicio`) e Inyección de Dependencias (`Program`)  
**Fecha:** Noviembre 2026  

---

## 1. Planteamiento del Caso Observado

Durante la operatoria del sistema se presenta el siguiente escenario:
1. El usuario abre una sesión de caja en el módulo **Ventas** ([`FormAperturaCaja.cs`](file:///C:/Proyecto/StockOS/src/StockOS.UI.WinForms/Forms/FormAperturaCaja.cs)).
2. Escanea o carga uno o más productos en la grilla del ticket ([`UcVentas.cs`](file:///C:/Proyecto/StockOS/src/StockOS.UI.WinForms/Forms/UcVentas.cs)), pero **aún no presiona "Cobrar"**.
3. Sin finalizar la venta, utiliza la barra de navegación inferior (`dockPanel`) para dirigirse a otro módulo (por ejemplo, **Inventario** para ingresar más stock en [`FormIngresoStock.cs`](file:///C:/Proyecto/StockOS/src/StockOS.UI.WinForms/Forms/FormIngresoStock.cs) o **Usuarios** en [`UcListarUsuarios.cs`](file:///C:/Proyecto/StockOS/src/StockOS.UI.WinForms/Forms/UcListarUsuarios.cs)).
4. Al regresar al módulo **Ventas**, **la caja permanece abierta**, pero **los productos que habían sido cargados previamente en el carrito ya no están en la grilla**.

---

## 2. Causas Técnicas en la Arquitectura de StockOS

Este comportamiento responde a la conjunción de **tres mecanismos técnicos** implementados en la capa de presentación y en el contenedor de Inyección de Dependencias:

### 2.1. Reemplazo de Controles en el Panel Contenedor (`FormInicio.cs`)
En [`FormInicio.cs`](file:///C:/Proyecto/StockOS/src/StockOS.UI.WinForms/Forms/FormInicio.cs#L205-L265), la navegación entre secciones se gestiona mediante el método `CambiarVista(string nombreVista)`:

```csharp
private void CambiarVista(string nombreVista)
{
    pnlContenedor.Controls.Clear();
    UserControl? nuevaVista = null;

    switch (nombreVista)
    {
        // ...
        case "Ventas":
            nuevaVista = _serviceProvider.GetRequiredService<UcVentas>();
            break;
    }

    if (nuevaVista != null)
    {
        nuevaVista.Dock = DockStyle.Fill;
        pnlContenedor.Controls.Add(nuevaVista);
    }
}
```

- Al salir de **Ventas** hacia **Inventario** o **Usuarios**, `pnlContenedor.Controls.Clear()` elimina la vista actual del contenedor visual.
- Al volver a pulsar **Ventas**, se solicita nuevamente `UcVentas` al proveedor de servicios (`_serviceProvider.GetRequiredService<UcVentas>()`).

---

### 2.2. Ciclo de Vida `Transient` en Inyección de Dependencias (`Program.cs`)
En [`Program.cs`](file:///C:/Proyecto/StockOS/src/StockOS.UI.WinForms/Program.cs#L123-L129), todos los `UserControl` de las vistas principales están registrados con ciclo de vida **Transitorio (`AddTransient`)**:

```csharp
// Controles de usuario (User Controls)
services.AddTransient<UcInicio>();
services.AddTransient<UcInventario>();
services.AddTransient<UcVentas>();
services.AddTransient<UcReportes>();
services.AddTransient<UcConfig>();
```

- En Microsoft.Extensions.DependencyInjection, `AddTransient<UcVentas>()` significa que **cada vez que se invoca `GetRequiredService<UcVentas>()`, el contenedor crea una instancia completamente nueva (`new UcVentas(...)`)**.
- Por lo tanto, la instancia anterior de `UcVentas` (donde estaban cargados los productos sin cobrar) queda descartada para el recolector de basura (*Garbage Collector*), y la nueva instancia se inicializa con su grilla y acumuladores en cero.

---

### 2.3. Almacenamiento Local del Carrito vs. Persistencia de la Sesión de Caja (`UcVentas.cs` y `SesionActual.cs`)
Existe una diferencia arquitectónica deliberada entre el **estado de la caja** y el **estado del carrito en curso**:

| Elemento | Dónde se almacena | ¿Persiste al cambiar de pestaña? | Motivo Técnico |
| :--- | :--- | :---: | :--- |
| **Apertura de Caja** | Base de datos SQL Server (`caja_sesion`) + Memoria de sesión global ([`SesionActual.IdCajaSesionActual`](file:///C:/Proyecto/StockOS/src/StockOS.Application/Services/SesionActual.cs)) | **SÍ** | Al abrir la caja mediante [`CajaService.AbrirCaja`](file:///C:/Proyecto/StockOS/src/StockOS.Application/Services/CajaService.cs), el registro se guarda físicamente en SQL Server y su ID queda asociado a la sesión activa del usuario hasta que ejecute el cierre de caja o cierre sesión. |
| **Carrito de Venta (sin cobrar)** | Memoria privada de la instancia visual [`UcVentas.cs`](file:///C:/Proyecto/StockOS/src/StockOS.UI.WinForms/Forms/UcVentas.cs) (`dgvTicket.Rows`, `_subtotalVenta`, `_descuentoTotal`, `_totalFinal`, `_ivaProductos`) | **NO** | Antes de confirmar el cobro, los productos forman parte únicamente de los controles visuales (`DataGridView`) y variables privadas de esa instancia de `UcVentas`. Al destruirse y recrearse la vista al cambiar de pantalla, el carrito comienza limpio. |

---

## 3. Justificación Funcional y de Integridad de Datos

Desde el punto de vista de las reglas de negocio e integridad transaccional, que `UcVentas` se reinicie al salir hacia otros módulos evita inconsistencias críticas:

1. **Consistencia de Precios y Costos:** Si el usuario carga un producto en el carrito, cambia a **Inventario**, registra un ingreso de stock con un **nuevo costo y nuevo margen de ganancia** (lo que actualiza el `PrecioVenta` del producto en la base de datos), un carrito retenido en memoria conservaría el precio viejo desactualizado.
2. **Consistencia de Estado Activo/Inactivo:** Si durante la visita al módulo de **Inventario** se da de baja un producto que estaba en el carrito sin cobrar, al recrearse `UcVentas` se impide que un artículo inactivo quede pre-validado en la grilla.
3. **Validación en Tiempo Real de Existencias (`StockSucursal`):** En [`AgregarProductoAlTicket`](file:///C:/Proyecto/StockOS/src/StockOS.UI.WinForms/Forms/UcVentas.cs#L107-L163), el stock disponible se verifica en el momento de agregar el ítem a `dgvTicket`. Reiniciar la vista tras operar en otros módulos garantiza que toda carga al ticket se valide contra el stock real actualizado.

---

## 4. Alternativas Arquitectónicas (En caso de requerir persistencia del carrito en el futuro)

Si en una futura evolución del sistema se deseara que el carrito pendiente permanezca al navegar entre pestañas sin perder la consistencia de datos, existen dos alternativas limpias dentro de la arquitectura actual:
1. **Servicio de Estado de Carrito (`Scoped`):** Crear un `ICarritoVentaStateService` registrado con `AddScoped` (mismo ciclo de vida que la sesión del usuario autenticado) que almacene los ítems pendientes por `IdProducto` y `Cantidad`, y que al cargar `UcVentas` vuelva a consultar el precio y stock vigentes en base de datos para reconstruir la grilla.
2. **Ciclo de Vida `Scoped` en `UcVentas` con Refresco en `VisibleChanged`:** Registrar `UcVentas` como `AddScoped` para reutilizar la misma instancia durante la sesión del cajero, revalidando precios y stock de las filas activas cada vez que el control vuelve a mostrarse en pantalla.

