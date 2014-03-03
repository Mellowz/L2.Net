using System;
using L2.Net.DataProvider;
using L2.Net.CacheService.Realtime;
using L2.Net.CacheService.Properties;
using L2.Net.Network;

namespace L2.Net.CacheService
{
    /// <summary>
    /// Provides database engine access.
    /// </summary>
    internal static class DataProvider
    {
        /// <summary>
        /// <see cref="Provider"/> instance.
        /// </summary>
        private static Provider Instance;

        /// <summary>
        /// Indicates if <see cref="DataProvider"/> is aviable now.
        /// </summary>
        internal static volatile bool Aviable;

        /// <summary>
        /// Initializes data provider.
        /// </summary>
        /// <param name="sqlType"><see cref="SqlEngine"/> of data provider to use.</param>
        /// <param name="connectionString">Database engine connection string.</param>
        /// <param name="poolSize"><see cref="Provider"/> connections pool capacity.</param>
        /// <param name="queueDumpInterval">Interval in which cache service will execute cached commands.</param>
        internal static void Initialize( SqlEngine sqlType, string connectionString, byte poolSize, TimeSpan queueDumpInterval )
        {
            switch ( sqlType )
            {
                case SqlEngine.MsSql:
                    {
                        Instance = new MsSqlDataProvider(connectionString, poolSize, queueDumpInterval);
                        Instance.OnInitialized += new ProviderInitializedEventDelegate(OnProviderInitialized);
                        Instance.OnStopped += new ProviderStoppedEventDelegate(OnProviderStopped);
                        Instance.Initialize();
                        return;
                    }
                default:
                    {
                        Logger.WriteLine(Source.DataProvider, "Unsupported data provider: {0}", sqlType);
                        return;
                    }
            }
        }

        /// <summary>
        /// Executes when <see cref="Provider"/> initialization is complete.
        /// </summary>
        /// <param name="type">Aviable <see cref="Provider"/> <see cref="SqlEngine"/>.</param>
        /// <param name="activeConnectionsCount"><see cref="Provider"/> active connections count.</param>
        private static void OnProviderInitialized( SqlEngine type, byte activeConnectionsCount )
        {
            Aviable = true;

            // audit record
            DataProvider.DataBase.ServiceAudit
                (
                    Settings.Default.ServiceUniqueID,
                    ( byte )ServiceType.CacheService,
                    ServiceAuditType.ServiceStarted,
                    null
                );

            Logger.WriteLine(Source.DataProvider, "{0} data provider initialized with {1} opened connections", type, activeConnectionsCount);

            // starting real time caching
            RealtimeManager.StartUp();
        }

        /// <summary>
        /// Executes when <see cref="Provider"/> has no active connections.
        /// </summary>
        private static void OnProviderStopped()
        {
            Aviable = false;
            Logger.WriteLine(Source.DataProvider, "Provider stopped");
        }

        /// <summary>
        /// Provides access to <see cref="Provider"/> operations.
        /// </summary>
        internal static Provider DataBase
        {
            get
            {
                return Instance;
            }
        }
    }
}