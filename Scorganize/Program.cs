using System.Configuration;

namespace Scorganize
{
    internal static class Program
    {
        public static Configuration Conf;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Program.Conf = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            MainForm mainForm = new MainForm();
            Application.Run(mainForm);
        }
    }
}