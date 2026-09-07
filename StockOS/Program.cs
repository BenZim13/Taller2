namespace Vistas_Usuarios
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Puedes comentar o descomentar el formulario que desees probar
            // Application.Run(new FormLogin());
            Application.Run(new FormRegistroUsuario());
        }
    }
}
