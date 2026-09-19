using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StockOS.Domain.Entities;

namespace StockOS.DataAccess.Persistence;

public partial class StockOsContext : DbContext
{
    public StockOsContext()
    {
    }

    public StockOsContext(DbContextOptions<StockOsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Caja> Cajas { get; set; }

    public virtual DbSet<CajaSesion> CajaSesiones { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<MovimientoCaja> MovimientoCajas { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedores { get; set; }

    public virtual DbSet<Rol> Roles { get; set; }

    public virtual DbSet<StockSucursal> StockSucursales { get; set; }

    public virtual DbSet<Sucursal> Sucursales { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Caja>(entity =>
        {
            entity.HasKey(e => e.IdCaja).HasName("PK__caja__C71E2476813DAF89");

            entity.ToTable("caja");

            entity.HasIndex(e => new { e.IdSucursal, e.NombreNumero }, "UQ__caja__962A9B5E59E93DDE").IsUnique();

            entity.Property(e => e.IdCaja).HasColumnName("id_caja");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.NombreNumero)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_numero");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Cajas)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__caja__id_sucursa__7F2BE32F");
        });

        modelBuilder.Entity<CajaSesion>(entity =>
        {
            entity.HasKey(e => e.IdCajaSesion).HasName("PK__caja_ses__C258AAC4E62564F3");

            entity.ToTable("caja_sesion");

            entity.Property(e => e.IdCajaSesion).HasColumnName("id_caja_sesion");
            entity.Property(e => e.Estado)
                .HasDefaultValue((byte)1)
                .HasColumnName("estado");
            entity.Property(e => e.FechaApertura)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_apertura");
            entity.Property(e => e.FechaCierre)
                .HasPrecision(0)
                .HasColumnName("fecha_cierre");
            entity.Property(e => e.IdCaja).HasColumnName("id_caja");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.MontoApertura)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("monto_apertura");
            entity.Property(e => e.MontoCierreReal)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("monto_cierre_real");

            entity.HasOne(d => d.IdCajaNavigation).WithMany(p => p.CajaSesions)
                .HasForeignKey(d => d.IdCaja)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__caja_sesi__id_ca__06CD04F7");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.CajaSesions)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__caja_sesi__id_em__07C12930");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__categori__CD54BC5A1C9A5615");

            entity.ToTable("categoria");

            entity.HasIndex(e => e.Nombre, "UQ__categori__72AFBCC6BC45FABD").IsUnique();

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__cliente__677F38F5427C9467");

            entity.ToTable("cliente");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.CondicionIva)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("condicion_iva");
            entity.Property(e => e.CuitDni)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("cuit_dni");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("razon_social");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__compra__C4BAA604D6B52A21");

            entity.ToTable("compra");

            entity.Property(e => e.IdCompra).HasColumnName("id_compra");
            entity.Property(e => e.FechaHora)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.NumeroComprobante)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numero_comprobante");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__compra__id_emple__72C60C4A");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__compra__id_prove__71D1E811");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__compra__id_sucur__73BA3083");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCompra).HasName("PK__detalle___BD16E279BD690C8F");

            entity.ToTable("detalle_compra");

            entity.HasIndex(e => new { e.IdCompra, e.IdProducto }, "UQ__detalle___0B49E7C5878FED6E").IsUnique();

            entity.Property(e => e.IdDetalleCompra).HasColumnName("id_detalle_compra");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdCompra).HasColumnName("id_compra");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.PrecioUnitarioCompra)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precio_unitario_compra");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalle_c__id_co__797309D9");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalle_c__id_pr__7A672E12");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.IdDetalleVenta).HasName("PK__detalle___5B265D47CF586975");

            entity.ToTable("detalle_venta");

            entity.HasIndex(e => new { e.IdVenta, e.IdProducto }, "UQ__detalle___8A66727E6DB9E739").IsUnique();

            entity.Property(e => e.IdDetalleVenta).HasColumnName("id_detalle_venta");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Descuento)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("descuento");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.PrecioUnitarioHistorico)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precio_unitario_historico");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalle_v__id_pr__1EA48E88");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalle_v__id_ve__1DB06A4F");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__empleado__88B513945776D389");

            entity.ToTable("empleado");

            entity.HasIndex(e => e.Email, "UQ__empleado__AB6E61640605CC37").IsUnique();

            entity.HasIndex(e => e.Dni, "UQ__empleado__D87608A799F5A209").IsUnique();

            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__empleado__id_rol__5441852A");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__empleado__id_suc__5535A963");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__factura__6C08ED53D5EE4A1A");

            entity.ToTable("factura");

            entity.HasIndex(e => e.NumeroFactura, "UQ__factura__3DC4B241C4C9B617").IsUnique();

            entity.HasIndex(e => e.IdVenta, "UQ__factura__459533BE34B3B71E").IsUnique();

            entity.Property(e => e.IdFactura).HasColumnName("id_factura");
            entity.Property(e => e.CaeAutorizacion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("cae_autorizacion");
            entity.Property(e => e.FechaEmision).HasColumnName("fecha_emision");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.IvaTotal)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("iva_total");
            entity.Property(e => e.MontoNeto)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("monto_neto");
            entity.Property(e => e.NumeroFactura)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("numero_factura");
            entity.Property(e => e.TipoComprobante)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo_comprobante");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdVentaNavigation).WithOne(p => p.Factura)
                .HasForeignKey<Factura>(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__factura__id_vent__2645B050");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__metodo_p__85BE0EBC4925BE6A");

            entity.ToTable("metodo_pago");

            entity.HasIndex(e => e.Nombre, "UQ__metodo_p__72AFBCC6D98B4EEB").IsUnique();

            entity.Property(e => e.IdMetodoPago).HasColumnName("id_metodo_pago");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<MovimientoCaja>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento).HasName("PK__movimien__2A071C24DF16C0C9");

            entity.ToTable("movimiento_caja");

            entity.Property(e => e.IdMovimiento).HasColumnName("id_movimiento");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaHora)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.IdCajaSesion).HasColumnName("id_caja_sesion");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo_movimiento");

            entity.HasOne(d => d.IdCajaSesionNavigation).WithMany(p => p.MovimientoCajas)
                .HasForeignKey(d => d.IdCajaSesion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__movimient__id_ca__0D7A0286");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__pago__0941B07462439BCD");

            entity.ToTable("pago");

            entity.Property(e => e.IdPago).HasColumnName("id_pago");
            entity.Property(e => e.IdMetodoPago).HasColumnName("id_metodo_pago");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.ReferenciaTransaccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("referencia_transaccion");

            entity.HasOne(d => d.IdMetodoPagoNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdMetodoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pago__id_metodo___2DE6D218");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pago__id_venta__2EDAF651");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__producto__FF341C0D527C4C46");

            entity.ToTable("producto");

            entity.HasIndex(e => e.CodigoBarra, "UQ__producto__685EAC7AD4B61B5C").IsUnique();

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.CodigoBarra)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigo_barra");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PorcentajeIva)
                .HasDefaultValue(2100m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porcentaje_iva");
            entity.Property(e => e.PrecioVentaActual)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precio_venta_actual");

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__producto__id_cat__628FA481");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK_producto_proveedor");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__proveedo__8D3DFE2873F6E4EF");

            entity.ToTable("proveedor");

            entity.HasIndex(e => e.Cuit, "UQ__proveedo__2CDD9897B9F18916").IsUnique();

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Cuit)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("cuit");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("razon_social");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__rol__6ABCB5E0993B8767");

            entity.ToTable("rol");

            entity.HasIndex(e => e.Nombre, "UQ__rol__72AFBCC62E9E4AE0").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<StockSucursal>(entity =>
        {
            entity.HasKey(e => new { e.IdProducto, e.IdSucursal }).HasName("PK__stock_su__DBF3440C9C349607");

            entity.ToTable("stock_sucursal");

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.StockActual).HasColumnName("stock_actual");
            entity.Property(e => e.StockMinimo).HasColumnName("stock_minimo");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.StockSucursals)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__stock_suc__id_pr__68487DD7");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.StockSucursals)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__stock_suc__id_su__693CA210");
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.IdSucursal).HasName("PK__sucursal__4C7580139FD5DC03");

            entity.ToTable("sucursal");

            entity.HasIndex(e => e.Nombre, "UQ__sucursal__72AFBCC689390BDE").IsUnique();

            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__venta__459533BF42B14444");

            entity.ToTable("venta");

            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.DescuentoTotal)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("descuento_total");
            entity.Property(e => e.Estado)
                .HasDefaultValue((byte)1)
                .HasColumnName("estado");
            entity.Property(e => e.FechaHora)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.IdCajaSesion).HasColumnName("id_caja_sesion");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.TotalVenta)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total_venta");

            entity.HasOne(d => d.IdCajaSesionNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCajaSesion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__venta__id_caja_s__160F4887");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__venta__id_client__17036CC0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

