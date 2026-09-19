namespace StockOS.Domain.Enums
{
    public static class Permisos
    {
        // Módulo Usuarios
        public const string USUARIOS_VER = "USUARIOS_VER";
        public const string USUARIOS_CREAR = "USUARIOS_CREAR";
        public const string USUARIOS_EDITAR = "USUARIOS_EDITAR";

        // Módulo Productos y Categorías
        public const string PRODUCTOS_VER = "PRODUCTOS_VER";
        public const string PRODUCTOS_CREAR = "PRODUCTOS_CREAR";
        public const string PRODUCTOS_EDITAR = "PRODUCTOS_EDITAR";
        public const string CATEGORIAS_GESTIONAR = "CATEGORIAS_GESTIONAR";

        // Módulo Stock
        public const string STOCK_VER = "STOCK_VER";
        public const string STOCK_INGRESAR = "STOCK_INGRESAR";

        // Módulo Ventas
        public const string VENTAS_REALIZAR = "VENTAS_REALIZAR";

        // Módulo Caja
        public const string CAJA_ABRIR = "CAJA_ABRIR";
        public const string CAJA_CERRAR = "CAJA_CERRAR";
        public const string CAJA_MOVIMIENTOS = "CAJA_MOVIMIENTOS";

        // Módulos (Actuales y Futuros)
        public const string REPORTES_VER = "REPORTES_VER";
        public const string COMPRAS_GESTIONAR = "COMPRAS_GESTIONAR";
        public const string PROVEEDORES_GESTIONAR = "PROVEEDORES_GESTIONAR";
        public const string CLIENTES_GESTIONAR = "CLIENTES_GESTIONAR";
        public const string CONFIGURACION_GESTIONAR = "CONFIGURACION_GESTIONAR";
    }
}