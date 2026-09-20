# 📋 Sistema de Tickets Dinámico - Estado Actual

Este documento describe el estado actual del sistema de tickets y las mejoras pendientes.

---

## ✅ COMPLETADO - Ticket Totalmente Dinámico

### Datos del Comercio
| Item | Estado | Descripción |
|------|--------|-------------|
| Configuración en `appsettings.json` | ✅ | Los datos del comercio se leen desde el archivo de configuración |
| `IConfiguracionService` | ✅ | Servicio creado para obtener datos del comercio |
| `ConfiguracionService` | ✅ | Implementación que lee de `IConfiguration` |
| Registro fiscal dinámico | ✅ | Se lee de `DatosComercio.RegistroFiscal` |

### Datos del Pago
| Item | Estado | Descripción |
|------|--------|-------------|
| Método de pago dinámico | ✅ | Muestra Efectivo, Tarjeta Débito, Tarjeta Crédito o Mercado Pago |
| Captura de monto recibido | ✅ | `FormCobro` ahora tiene campo para ingresar monto en efectivo |
| Cálculo de vuelto | ✅ | Se calcula automáticamente cuando el monto es mayor al total |
| Propiedades `MontoRecibido` y `Vuelto` | ✅ | Expuestas en `FormCobro` para uso en `UcVentas` |

### Datos del Ticket
| Item | Estado | Descripción |
|------|--------|-------------|
| Subtotal correcto | ✅ | Muestra `venta.Subtotal` (antes de descuentos) |
| Descuento visible | ✅ | Si hay descuento, se muestra en el ticket |
| Nombre del cajero | ✅ | Se imprime en el ticket |
| IVA por producto | ✅ | Usa `PorcentajeIva` de cada producto, agrupa por alícuota |

---

## 📝 Archivos Modificados

### Nuevos Archivos Creados
- `StockOS.Application/Services/IConfiguracionService.cs`
- `StockOS.Application/Services/ConfiguracionService.cs`

### Archivos Modificados
| Archivo | Cambios |
|---------|---------|
| `appsettings.json` | Agregada sección `DatosComercio` con todos los campos |
| `Program.cs` | Registrado `IConfiguracionService` |
| `ITicketService.cs` | Agregado `DatosPago`, `RegistroFiscal` en `DatosComercio` |
| `TicketService.cs` | IVA por alícuota, datos de pago dinámicos |
| `FormCobro.cs` | Captura monto recibido, calcula vuelto |
| `FormCobro.Designer.cs` | Agregados `txtMontoRecibido`, `lblVuelto`, `lblVueltoValor` |
| `UcVentas.cs` | Inyecta `IConfiguracionService`, usa datos dinámicos del pago, diccionario de IVA por producto |

---

## 🔧 Pendiente (Opcional - No afecta funcionamiento)

### 1. Implementar Pantalla de Configuración (`UcConfig`)
**Prioridad:** Media

El control `UcConfig` está vacío. Para permitir cambiar la configuración desde la UI:

1. Agregar campos de texto para cada propiedad de `DatosComercio`
2. Botón "Guardar" que escriba los cambios en `appsettings.json`
3. Validaciones (formato CUIT, campos obligatorios)

```csharp
// Ejemplo de cómo guardar cambios en appsettings.json
var json = File.ReadAllText("appsettings.json");
var jsonObj = JsonDocument.Parse(json);
// ... modificar y guardar
```

### 2. Soporte para Pagos Múltiples (Split Payment)
**Prioridad:** Baja

La tabla `pago` ya soporta múltiples pagos por venta. Para implementar:

1. Modificar `FormCobro` para permitir agregar múltiples métodos de pago
2. Actualizar `VentaService.RegistrarVenta` para registrar varios pagos
3. Modificar `TicketService` para listar todos los métodos usados

---

## 📋 Configuración Actual (`appsettings.json`)

```json
{
  "ConnectionStrings": {
	"StockOS": "Server=localhost;Database=StockOS;..."
  },
  "DatosComercio": {
	"Nombre": "StockOS",
	"Cuit": "30-12345678-9",
	"Direccion": "Av. Falsa 123, Corrientes",
	"IngresosBrutos": "30-12345678-9",
	"InicioActividades": "01/01/2024",
	"CondicionIva": "IVA RESPONSABLE INSCRIPTO",
	"PuntoVenta": "00001",
	"RegistroFiscal": "EPEPAA0000048463"
  }
}
```

**Para cambiar los datos del comercio:** Editar directamente `appsettings.json` y reiniciar la aplicación.

---

*Actualizado: Septiembre 2026*
