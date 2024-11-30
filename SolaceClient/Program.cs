using log4net.Config;

namespace SolaceClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            if (args.Length > 0)
            {
                SolaceCliClient.Runner.Main(args);
            }
            else
            {
                InitializeLogConfiguration();
                ApplicationConfiguration.Initialize();
                Application.Run(new Main());
            }
        }

        static void InitializeLogConfiguration() {
            XmlConfigurator.Configure(new FileInfo("log4net.config")); 
        }
    }
}