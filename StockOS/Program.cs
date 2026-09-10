namespace Vistas_Usuarios//clase logica
{
    internal static class Program 
    {
   
        [STAThread]//modelo de un solo hilo
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Comentar o descomentar el formulario a probar
            //Application.Run(new FormLogin());
            Application.Run(new FormInicio());
            //Application.Run(new FormRegistroUsuario());
        }
    }
}
