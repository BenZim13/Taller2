namespace StockOS.UI.WinForms.Forms
{
    internal static class Program 
    {

        [STAThread]//modelo de un solo hilo
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Comentar o descomentar el formulario a probar
            //Application.Run(new FormLogin());
            System.Windows.Forms.Application.Run(new FormInicio()); //asi para que no genere conflictos con StockOS.Application en el futuro 
                                                                        //Application.Run(lo toma como si fuera una llamada a ese namespace)
            //Application.Run(new FormRegistroUsuario());
        }
    }
}
