# StockOS 🛒📦
> **Sistema de Punto de Venta (POS) y Administración Integral de Stock Comercial**

[![.NET Version](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![Database](https://img.shields.io/badge/SQL_Server-2022-CC292B?style=flat-square&logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)

StockOS es un software de escritorio robusto y modular desarrollado en **C# / .NET 8**, diseñado para la gestión integral de comercios minoristas. Integra operaciones de punto de venta (POS), control de stock en tiempo real, administración de compras a proveedores, control estricto de sesiones de caja y generación de reportes analíticos.

---

## 🚀 Características Principales

- **Punto de Venta (POS) Ágil:** Facturación rápida por escaneo de código de barras (HID), cálculo automático de montos y soporte para múltiples medios de pago (Efectivo, Tarjetas, Transferencias/QR, Cuenta Corriente).
- **Control de Stock en Tiempo Real:** Gestión de inventario con jerarquía de categorías, alertas de stock mínimo, mermas, ajustes manuales y actualización automática de existencias tras ventas y compras.
- **Gestión de Sesiones de Caja:** Control de turnos con registro de fondos iniciales, arqueo ciego, cálculo de saldo esperado y detección de diferencias.
- **Abastecimiento y Proveedores:** Registro de comprobantes de compra, actualización de costos de reposición e incremento automático de stock.
- **Reportes y Auditoría:** Generación y exportación de reportes operativos (ventas, compras, inventario valorizado, movimientos de caja) a formatos **PDF**, **Excel** y **Word**.
- **Seguridad y Respaldo:** Autenticación por roles con contraseñas cifradas, log de auditoría para transacciones críticas y utilidades de Backup/Restore de la base de datos.

---

## 🏗️ Arquitectura del Sistema

El sistema implementa una **Arquitectura en Capas (N-Tier)** con desacoplamiento de responsabilidades y aplicación del patrón de diseño **Observer** para la reactividad en el inventario y eventos de caja:

```text
StockOS/
│
├── 📁 src/
│   │
│   ├── 📦 StockOS.Domain/                  # Entidades del negocio, enumeraciones, interfaces puras y eventos
│   │   ├── 📁 Entities/                    # Modelos (Producto, Venta, SesionCaja, Proveedor, etc.)
│   │   ├── 📁 Enums/                       # MetodoPago, RolUsuario, EstadoCaja
│   │   ├── 📁 Interfaces/                  # Contratos de repositorios y servicios base
│   │   └── 📁 Events/                      # Clases e interfaces para el patrón Observer
│   │
│   ├── 📦 StockOS.Application/             # Lógica del negocio y casos de uso
│   │   ├── 📁 Services/                    # VentaService, StockService, CajaService, etc.
│   │   ├── 📁 DTOs/                        # Objetos de transferencia de datos para la UI
│   │   ├── 📁 Validators/                  # Validaciones de reglas de negocio
│   │   ├── 📁 Reports/                     # Exportación de reportes (PDF, Excel, Doc) -Va en aplicacion.  
│   │   └── 📁 Observers/                   # Implementaciones concretas de observadores
│   │
│   ├── 📦 StockOS.DataAccess/              # Acceso a datos y servicios externos
│   │   ├── 📁 Persistence/                 # Contexto de base de datos (SQL Server), mapeos y migraciones
│   │   ├── 📁 Repositories/                # Implementaciones de repositorios (CRUD y consultas)
│   │   ├── 📁 Hardware/                    # Comunicación con lector HID y tickets ESC/POS
│   │   └── 📁 Security/                    # Cifrado de contraseñas y respaldos (Backup/Restore)
│   │
│   └── 📦 StockOS.UI.WinForms/             # Capa de Presentación (Interfaz gráfica)
│       ├── 📁 Forms/                       # Vistas organizadas por módulo (Auth, POS, Inventario, Caja, Reportes)
│       ├── 📁 Controls/                    # Componentes visuales reutilizables
│       ├── 📁 Utils/                       # Helpers y estilos de UI
│       └── Program.cs                      # Punto de entrada y configuración de Inyección de Dependencias
│
└── 📁 tests/                               # Pruebas automatizadas
    ├── 📦 StockOS.Domain.Tests/            # Pruebas unitarias de entidades y dominio
    └── 📦 StockOS.Application.Tests/       # Pruebas de servicios y lógica de negocio
