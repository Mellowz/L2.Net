using System;
using System.Data;

namespace L2.Net.DataProvider
{
    /// <summary>
    /// Base class for database commands.
    /// </summary>
    public abstract class Command : IDisposable
    {
        /// <summary>
        /// <see cref="Command"/> text.
        /// </summary>
        protected internal string m_CommandText;

        /// <summary>
        /// Dispose flag.
        /// </summary>
        protected bool m_Disposed;

        /// <summary>
        /// Initializes new instance of <see cref="Command"/> class.
        /// </summary>
        public Command() { }

        /// <summary>
        /// Initializes new instance of <see cref="Command"/> class.
        /// </summary>
        /// <param name="commandText"><see cref="Command"/> text.</param>
        internal Command( string commandText )
        {
            m_CommandText = commandText;
        }

        /// <summary>
        /// Forces current <see cref="Command"/> object to release used objects.
        /// </summary>
        internal protected abstract void Release();

        /// <summary>
        /// Forces current <see cref="Command"/> object to release used objects.
        /// </summary>
        internal protected abstract bool Release( bool value );

        #region Virtual execution methods

        /// <summary>
        /// Executes current <see cref="Command"/> without any output result.
        /// </summary>
        protected internal abstract void ExecuteNonQuery();

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Amount of rows, affected during <see cref="Command"/> execution.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool ExecuteRowsAffected( ref int value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="object"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref object value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="byte"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref byte value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="sbyte"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref sbyte value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="short"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref short value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="ushort"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref ushort value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="int"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref int value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="uint"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref uint value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="long"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref long value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="ulong"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref ulong value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="double"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref double value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="System.DateTime"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref DateTime value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="TimeSpan"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref TimeSpan value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="string"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref string value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="bool"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref bool value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="DataRow"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref DataRow value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="DataTable"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref DataTable value );

        /// <summary>
        /// Executes current <see cref="Command"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="DataSet"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal abstract bool Execute( ref DataSet value );

        #endregion

        /// <summary>
        /// Releases all resources used by the <see cref="Command"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Command"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="dispose">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        public abstract void Dispose( bool dispose );
    }
}