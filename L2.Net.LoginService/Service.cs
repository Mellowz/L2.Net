using System;
using System.Net;
using L2.Net.LoginService.InnerNetwork;
using L2.Net.LoginService.OuterNetwork;
using L2.Net.LoginService.Properties;
using L2.Net.Network;

namespace L2.Net.LoginService
{
    /// <summary>
    /// Main service class.
    /// </summary>
    internal static class Service
    {
        /// <summary>
        /// Main service start method.
        /// </summary>
        internal static void Main()
        {
            // initializing logger
            Logger.Initialize();

            // setting unhandled exceptions handler
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // initializing user connections listener
            UserConnectionsListener.Initialize
                (
                    new IPEndPoint
                        (
                            IPAddress.Parse(Settings.Default.LoginServiceAddress),
                            Settings.Default.LoginServicePort
                        ),
                    Settings.Default.LoginServiceBacklog,
                    Settings.Default.LoginServiceEnableFirewall
                );

            // initializing cache server connection
            CacheServiceConnection.Initialize
                (
                    new IPEndPoint
                            (
                                IPAddress.Parse(Settings.Default.CacheServiceAddress),
                                Settings.Default.CacheServicePort
                            ),
                    Settings.Default.CacheServiceReconnectInterval);

            while ( Console.ReadKey(true) != null ) { }
        }

        /// <summary>
        /// Executes when service throws unhandled exception.
        /// </summary>
        /// <param name="sender">Exception sender.</param>
        /// <param name="e"><see cref="UnhandledExceptionEventArgs"/> object.</param>
        private static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
        {
            if ( e != null )
            {
                if ( e.ExceptionObject != null )
                    Terminate(new ServiceShutdownEventArgs("AppDomain exception caught", ( Exception )e.ExceptionObject));
                else
                    Terminate(new ServiceShutdownEventArgs("AppDomain exception thrown without any exception data?"));
            }
        }

        /// <summary>
        /// Terminates current service instance.
        /// </summary>
        /// <param name="e">For more information, please see <see cref="ServiceShutdownEventArgs"/> class.</param>
        internal static void Terminate( ServiceShutdownEventArgs e )
        {
            if ( e != null && e.LastException != null )
                Logger.Exception(e.LastException, e.Message);

            Logger.WriteLine(Source.Service, "Unexpected service termination.");

            Environment.Exit(-1);
        }
    }
}