#define INSTALL_DB_ON_STARTUP

using System;
using System.Collections.Generic;
using System.Net;
using L2.Net.CacheService.Network;
using L2.Net.DataProvider;
using L2.Net.CacheService.Properties;

namespace L2.Net.CacheService
{
    internal static class Service
    {
        internal static volatile bool NetworkListenerIsActive;

        private static void Main( string[] args )
        {
            // initializing logger 
            Logger.Initialize();

            // scripts compiler initialization
            SmartCompiler.Initialize("Scripts", "Assemblies.txt");
            SmartCompiler.Compile(null, ScriptFileType.CS, true, null);

#if INSTALL_DB_ON_STARTUP
            InstallDataBase();
#endif

            // initializing data provider
            DataProvider.Initialize
                (
                    Settings.Default.SqlEngine,
                    Settings.Default.SqlServerConnectionString,
                    Settings.Default.SqlServerConnectionsPoolSize,
                    Settings.Default.SqlServerDumpInterval
                );

            // initializing network listener
            NetworkListener.Initialize
                (
                    new IPEndPoint
                        (
                            IPAddress.Parse(Settings.Default.CacheServiceListenerAddress),
                            Settings.Default.CacheServiceListenerPort
                        ),
                    Settings.Default.CacheServiceEnableFirewall
                );

            while ( Console.ReadKey(true) != null ) { }
        }

        private static void InstallDataBase()
        {
            List<DataBaseInstaller> compiledInstallers = new List<DataBaseInstaller>(SmartCompiler.FindTypeByBase<DataBaseInstaller>());

            if ( compiledInstallers.Count > 0 )
            {
                foreach ( DataBaseInstaller installer in compiledInstallers )
                {
                    if ( installer != null && installer.Engine == Settings.Default.SqlEngine )
                    {
                        installer.ConnectionString = Settings.Default.SqlServerConnectionString;
                        installer.OnInstallationError += new InstallationErrorEventHandler(OnScriptsExecutionError);
                        installer.Install();
                        break;
                    }
                }
            }

            compiledInstallers = null;
        }

        /// <summary>
        /// Executes after some scripts execution error occurs.
        /// </summary>
        /// <param name="e">Occurred <see cref="Exception"/>.</param>
        private static void OnScriptsExecutionError( Exception e )
        {
            Logger.Exception(e);
            Logger.WriteLine("Continue? y/n");

            if( Console.ReadKey().Key ==  ConsoleKey.N )
                Environment.Exit(-1);
        }
    }
}