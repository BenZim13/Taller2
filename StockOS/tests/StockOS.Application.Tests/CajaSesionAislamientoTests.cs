using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using StockOS.Application.Services;
using StockOS.Domain.Entities;
using StockOS.Domain.Interfaces;

namespace StockOS.Application.Tests
{
    public class CajaSesionAislamientoTests : IDisposable
    {
        private class FakeCajaSesionRecord
        {
            public int IdCajaSesion { get; set; }
            public int IdCaja { get; set; }
            public int IdEmpleado { get; set; }
            public decimal MontoApertura { get; set; }
            public decimal? MontoCierre { get; set; }
            public int Estado { get; set; } // 1 = abierta, 0 = cerrada
        }

        private readonly List<FakeCajaSesionRecord> _tablaCajaSesiones;
        private readonly Mock<ICajaSesionRepository> _mockSesionRepo;
        private readonly Mock<ICajaRepository> _mockCajaRepo;
        private readonly Mock<IAuthorizationService> _mockAuth;
        private readonly CajaService _cajaService;
        private int _nextSesionId = 100;

        private readonly Empleado _cajeroA;
        private readonly Empleado _cajeroB;

        public CajaSesionAislamientoTests()
        {
            // Limpiar estado global antes de cada test
            SesionActual.Limpiar();

            _tablaCajaSesiones = new List<FakeCajaSesionRecord>();
            _mockSesionRepo = new Mock<ICajaSesionRepository>();
            _mockCajaRepo = new Mock<ICajaRepository>();
            _mockAuth = new Mock<IAuthorizationService>();

            // Configurar comportamiento fiel de repositorio en memoria
            _mockSesionRepo.Setup(r => r.AbrirCaja(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()))
                .Returns((int idCaja, int idEmpleado, decimal monto) =>
                {
                    _nextSesionId++;
                    var record = new FakeCajaSesionRecord
                    {
                        IdCajaSesion = _nextSesionId,
                        IdCaja = idCaja,
                        IdEmpleado = idEmpleado,
                        MontoApertura = monto,
                        Estado = 1
                    };
                    _tablaCajaSesiones.Add(record);
                    return record.IdCajaSesion;
                });

            _mockSesionRepo.Setup(r => r.VerificarCajaAbierta(It.IsAny<int>()))
                .Returns((int idEmpleado) => _tablaCajaSesiones.Any(s => s.IdEmpleado == idEmpleado && s.Estado == 1));

            _mockSesionRepo.Setup(r => r.VerificarCajaFisicaAbierta(It.IsAny<int>()))
                .Returns((int idCaja) => _tablaCajaSesiones.Any(s => s.IdCaja == idCaja && s.Estado == 1));

            _mockSesionRepo.Setup(r => r.ObtenerIdSesionAbierta(It.IsAny<int>()))
                .Returns((int idEmpleado) =>
                {
                    var active = _tablaCajaSesiones.FirstOrDefault(s => s.IdEmpleado == idEmpleado && s.Estado == 1);
                    return active?.IdCajaSesion;
                });

            _mockCajaRepo.Setup(r => r.ObtenerMontoEsperado(It.IsAny<int>()))
                .Returns((int idCajaSesion) =>
                {
                    var record = _tablaCajaSesiones.FirstOrDefault(s => s.IdCajaSesion == idCajaSesion);
                    return record != null ? record.MontoApertura : 0m;
                });

            _mockCajaRepo.Setup(r => r.CerrarCaja(It.IsAny<int>(), It.IsAny<decimal>()))
                .Callback((int idCajaSesion, decimal montoCierre) =>
                {
                    var record = _tablaCajaSesiones.FirstOrDefault(s => s.IdCajaSesion == idCajaSesion);
                    if (record != null)
                    {
                        record.Estado = 0;
                        record.MontoCierre = montoCierre;
                    }
                });

            _cajaService = new CajaService(_mockSesionRepo.Object, _mockCajaRepo.Object, _mockAuth.Object);

            _cajeroA = new Empleado { IdEmpleado = 1, Nombre = "Cajero", Apellido = "A", IdSucursal = 1, Estado = true };
            _cajeroB = new Empleado { IdEmpleado = 2, Nombre = "Cajero", Apellido = "B", IdSucursal = 1, Estado = true };
        }

        public void Dispose()
        {
            SesionActual.Limpiar();
        }

        [Fact]
        public void FlujoCompleto_CajeroA_AbreCaja_Y_AlCerrarSesion_SeCierraCajaEnBaseDeDatosYEnMemoria()
        {
            // 1. Cajero A inicia sesión
            SesionActual.Usuario = _cajeroA;
            SesionActual.IdCajaSesionAbierta = null;

            // 2. Cajero A abre Caja física 1 con $5,000
            int idSesionA = _cajaService.AbrirCaja(idCaja: 1, idEmpleado: _cajeroA.IdEmpleado, montoApertura: 5000m);
            SesionActual.IdCajaSesionAbierta = idSesionA;

            Assert.True(idSesionA > 0);
            Assert.Equal(idSesionA, SesionActual.IdCajaSesionAbierta);
            Assert.True(_cajaService.VerificarCajaAbierta(_cajeroA.IdEmpleado));
            Assert.True(_mockSesionRepo.Object.VerificarCajaFisicaAbierta(1));

            // 3. Cajero A cierra sesión (evento de FormInicio / Program.cs)
            _cajaService.CerrarCajaPorCierreSesion(_cajeroA.IdEmpleado);
            SesionActual.Limpiar();

            // 4. Verificaciones: La caja quedó cerrada tanto en BD como en memoria
            Assert.Null(SesionActual.Usuario);
            Assert.Null(SesionActual.IdCajaSesionAbierta);
            Assert.False(_cajaService.VerificarCajaAbierta(_cajeroA.IdEmpleado));
            Assert.False(_mockSesionRepo.Object.VerificarCajaFisicaAbierta(1));
            Assert.Null(_cajaService.ObtenerIdSesionAbierta(_cajeroA.IdEmpleado));
        }

        [Fact]
        public void FlujoCompleto_CajeroB_IngresaDespuesDeCajeroA_IniciaSinCajaAbierta_Y_NoPuedeCobrarHastaAbrirla()
        {
            // 1. Cajero A inicia sesión, abre Caja 1, y luego cierra sesión
            SesionActual.Usuario = _cajeroA;
            int idSesionA = _cajaService.AbrirCaja(idCaja: 1, idEmpleado: _cajeroA.IdEmpleado, montoApertura: 3000m);
            SesionActual.IdCajaSesionAbierta = idSesionA;

            // Cajero A cierra su sesión
            _cajaService.CerrarCajaPorCierreSesion(_cajeroA.IdEmpleado);
            SesionActual.Limpiar();

            // 2. Cajero B ingresa al sistema
            SesionActual.Usuario = _cajeroB;
            // Al hacer login, toda sesión debe iniciar con IdCajaSesionAbierta = null
            SesionActual.IdCajaSesionAbierta = null;

            // ASSERT de Aislamiento:
            // Cajero B NO tiene caja abierta por culpa de Cajero A
            Assert.Null(SesionActual.IdCajaSesionAbierta);
            Assert.False(_cajaService.VerificarCajaAbierta(_cajeroB.IdEmpleado));
            Assert.Null(_cajaService.ObtenerIdSesionAbierta(_cajeroB.IdEmpleado));

            // La condición de cobro de UcVentas rechaza la operación si no hay caja abierta
            bool puedeCobrar = SesionActual.IdCajaSesionAbierta.HasValue && SesionActual.IdCajaSesionAbierta.Value > 0;
            Assert.False(puedeCobrar, "Cajero B no debe poder cobrar hasta que abra su turno de caja de forma explícita.");

            // 3. Cajero B abre su propia caja (Caja 1 ahora está liberada y disponible)
            int idSesionB = _cajaService.AbrirCaja(idCaja: 1, idEmpleado: _cajeroB.IdEmpleado, montoApertura: 2500m);
            SesionActual.IdCajaSesionAbierta = idSesionB;

            Assert.NotEqual(idSesionA, idSesionB);
            Assert.True(_cajaService.VerificarCajaAbierta(_cajeroB.IdEmpleado));
            Assert.True(_mockSesionRepo.Object.VerificarCajaFisicaAbierta(1));

            // Ahora sí puede cobrar
            puedeCobrar = SesionActual.IdCajaSesionAbierta.HasValue && SesionActual.IdCajaSesionAbierta.Value > 0;
            Assert.True(puedeCobrar);

            // 4. Cajero B cierra sesión
            _cajaService.CerrarCajaPorCierreSesion(_cajeroB.IdEmpleado);
            SesionActual.Limpiar();

            Assert.False(_cajaService.VerificarCajaAbierta(_cajeroB.IdEmpleado));
            Assert.False(_mockSesionRepo.Object.VerificarCajaFisicaAbierta(1));
        }

        [Fact]
        public void Concurrencia_DosCajeros_NoPuedenAbrirMismaCajaFisicaSimultaneamente()
        {
            // Cajero A abre Caja 1
            _cajaService.AbrirCaja(idCaja: 1, idEmpleado: _cajeroA.IdEmpleado, montoApertura: 4000m);

            // Cajero B intenta abrir la MISMA Caja 1 mientras sigue abierta
            var ex = Assert.Throws<InvalidOperationException>(() =>
                _cajaService.AbrirCaja(idCaja: 1, idEmpleado: _cajeroB.IdEmpleado, montoApertura: 1000m)
            );

            Assert.Contains("La caja seleccionada ya tiene un turno abierto por otro cajero", ex.Message);
            Assert.False(_cajaService.VerificarCajaAbierta(_cajeroB.IdEmpleado));
        }

        [Fact]
        public void Reingreso_CajeroA_LuegoDeCerrarSesion_IniciaSinCaja_Y_NoContinuaAbierta()
        {
            // Cajero A abre Caja 1
            SesionActual.Usuario = _cajeroA;
            int idSesion1 = _cajaService.AbrirCaja(idCaja: 1, idEmpleado: _cajeroA.IdEmpleado, montoApertura: 1500m);
            SesionActual.IdCajaSesionAbierta = idSesion1;

            // Cajero A cierra sesión
            _cajaService.CerrarCajaPorCierreSesion(_cajeroA.IdEmpleado);
            SesionActual.Limpiar();

            // Cajero A vuelve a iniciar sesión (FormLogin simulación)
            SesionActual.Usuario = _cajeroA;
            // En FormLogin: se limpia cualquier remanente y se fuerza null
            _cajaService.CerrarCajaPorCierreSesion(_cajeroA.IdEmpleado);
            SesionActual.IdCajaSesionAbierta = null;

            // Verificar que al reingresar NO tiene la caja abierta
            Assert.Null(SesionActual.IdCajaSesionAbierta);
            Assert.False(_cajaService.VerificarCajaAbierta(_cajeroA.IdEmpleado));
            Assert.Null(_cajaService.ObtenerIdSesionAbierta(_cajeroA.IdEmpleado));

            // Si intenta cobrar, se le bloquea
            bool puedeCobrar = SesionActual.IdCajaSesionAbierta.HasValue && SesionActual.IdCajaSesionAbierta.Value > 0;
            Assert.False(puedeCobrar);

            // Abre una nueva sesión con nuevo turno
            int idSesion2 = _cajaService.AbrirCaja(idCaja: 1, idEmpleado: _cajeroA.IdEmpleado, montoApertura: 1500m);
            Assert.NotEqual(idSesion1, idSesion2);
        }
    }
}
