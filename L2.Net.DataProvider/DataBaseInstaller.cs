using System;

namespace L2.Net.DataProvider
{
    /// <summary>
    /// Represents method that will handle database installation error event.
    /// </summary>
    /// <param name="e">Occurred <see cref="Exception"/>.</param>
    public delegate void InstallationErrorEventHandler( Exception e );

    /// <summary>
    /// Database installer interface.
    /// </summary>
    public abstract class DataBaseInstaller
    {
        /// <summary>
        /// Occurs when some error happens during installation.
        /// </summary>
        public abstract event InstallationErrorEventHandler OnInstallationError;

        /// <summary>
        /// Database engine connection string.
        /// </summary>
        public string ConnectionString;

        /// <summary>
        /// Database engine type.
        /// </summary>
        public readonly SqlEngine Engine;

        /// <summary>
        /// Initializes new instance of <see cref="DataBaseInstaller"/> object.
        /// </summary>
        /// <param name="engine">Database engine type.</param>
        public DataBaseInstaller( SqlEngine engine )
        {
            Engine = engine;
        }

        /// <summary>
        /// Installs scripts.
        /// </summary>
        public abstract void Install();
    }
}
