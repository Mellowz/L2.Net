using System;
using System.Data;
using System.Data.SqlClient;

namespace L2.Net.DataProvider
{
    /// <summary>
    /// MsSql data command class.
    /// </summary>
    internal sealed class MsSqlDataCommand : Command
    {
        /// <summary>
        /// <see cref="SqlCommand"/> object which current <see cref="MsSqlDataCommand"/> operates on.
        /// </summary>
        private SqlCommand m_Command;

        /// <summary>
        /// <see cref="MsSqlDataCommand"/> type.
        /// </summary>
        private readonly CommandType m_CommandType;

        /// <summary>
        /// <see cref="MsSqlDataCommand"/> parameters collection.
        /// </summary>
        private readonly SqlParameter[] m_Parameters;

        /// <summary>
        /// Used <see cref="MsSqlDataConnection"/> reference.
        /// </summary>
        private MsSqlDataConnection m_Connection;

        /// <summary>
        /// <see cref="SqlDataAdapter"/> object.
        /// </summary>
        private SqlDataAdapter m_Adapter;

        /// <summary>
        /// <see cref="DataTable"/> object.
        /// </summary>
        private DataTable m_Table;

        /// <summary>
        /// Initializes new instance of <see cref="MsSqlDataCommand"/> object.
        /// </summary>
        public MsSqlDataCommand() : base() { }

        /// <summary>
        /// Initializes new instance of <see cref="MsSqlDataCommand"/> object.
        /// </summary>
        /// <param name="commandText">Current <see cref="MsSqlDataCommand"/> text.</param>
        /// <param name="commandType">Current <see cref="MsSqlDataCommand"/> <see cref="CommandType"/>.</param>
        /// <param name="parameters">Current <see cref="MsSqlDataCommand"/> <see cref="SqlParameter"/> objects collection.</param>
        internal MsSqlDataCommand( string commandText, CommandType commandType, params SqlParameter[] parameters )
            : base(commandText)
        {
            m_CommandType = commandType;

            if ( parameters.Length > 0 )
                m_Parameters = parameters;
        }

        /// <summary>
        ///  Prepares current <see cref="MsSqlDataCommand"/> to execute.
        /// </summary>
        /// <param name="connection"><see cref="MsSqlDataConnection"/> to use.</param>
        internal void Prepare( ref MsSqlDataConnection connection )
        {
            m_Connection = connection;
            m_Connection.Idle = false;

            m_Command = new SqlCommand(m_CommandText, connection.SqlServerConnection);
            m_Command.CommandType = m_CommandType;

            if ( m_Parameters != null )
                m_Command.Parameters.AddRange(m_Parameters);

            m_Command.Prepare();
        }

        /// <summary>
        /// Releases resources, used by current <see cref="MsSqlDataCommand"/> during execution.
        /// </summary>
        protected internal override void Release()
        {
            if ( m_Adapter != null )
            {
                m_Adapter.Dispose();
                m_Adapter = null;
            }

            if ( m_Command != null )
            {
                m_Command.Dispose();
                m_Command = null;
            }

            if ( m_Table != null )
            {
                m_Table.Dispose();
                m_Table = null;
            }
        }

        /// <summary>
        /// Releases resources, used by current <see cref="MsSqlDataCommand"/> during execution.
        /// </summary>
        /// <param name="value">Execution return value.</param>
        /// <returns>Execution return value.</returns>
        protected internal override bool Release( bool value )
        {
            if ( m_Connection != null )
                m_Connection.Idle = true;

            return value;
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/> without any output result.
        /// </summary>
        protected internal override void ExecuteNonQuery()
        {
            try
            {
                m_Command.ExecuteNonQuery();
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
            }

            Release(true);
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Amount of rows, affected during <see cref="Command"/> execution.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool ExecuteRowsAffected( ref int value )
        {
            try
            {
                value = m_Command.ExecuteNonQuery();
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="object"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref object value )
        {
            try
            {
                value = m_Command.ExecuteScalar();
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="byte"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref byte value )
        {
            try
            {
                value = TypesConverter.GetByte(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="sbyte"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref sbyte value )
        {
            try
            {
                value = TypesConverter.GetSByte(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="short"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref short value )
        {
            try
            {
                value = TypesConverter.GetShort(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="ushort"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref ushort value )
        {
            try
            {
                value = TypesConverter.GetUShort(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="int"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref int value )
        {
            try
            {
                value = TypesConverter.GetInt(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="uint"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref uint value )
        {
            try
            {
                value = TypesConverter.GetUInt(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="long"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref long value )
        {
            try
            {
                value = TypesConverter.GetLong(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="ulong"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref ulong value )
        {
            try
            {
                value = TypesConverter.GetULong(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="double"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref double value )
        {
            try
            {
                value = TypesConverter.GetDouble(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="DateTime"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref DateTime value )
        {
            try
            {
                value = TypesConverter.GetDateTime(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="TimeSpan"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref TimeSpan value )
        {
            try
            {
                value = TypesConverter.GetTimeSpan(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="string"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref string value )
        {
            try
            {
                value = TypesConverter.GetString(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="bool"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref bool value )
        {
            try
            {
                value = TypesConverter.GetBoolean(m_Command.ExecuteScalar());
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(true);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="DataRow"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref DataRow value )
        {
            try
            {
                m_Adapter = new SqlDataAdapter(m_Command);
                m_Table = new DataTable();
                m_Adapter.Fill(m_Table);

                if ( m_Table.Rows.Count > 0 )
                    value = m_Table.Rows[0];

                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="DataTable"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref DataTable value )
        {
            try
            {
                m_Adapter = new SqlDataAdapter(m_Command);
                m_Adapter.Fill(value);
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Executes current <see cref="MsSqlDataCommand"/>.
        /// </summary>
        /// <param name="value">Reference to return <see cref="DataSet"/> value.</param>
        /// <returns>True, if command executed successfully, otherwise false.</returns>
        protected internal override bool Execute( ref DataSet value )
        {
            try
            {
                m_Adapter = new SqlDataAdapter(m_Command);
                m_Adapter.Fill(value);
                return Release(true);
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return Release(false);
            }
        }

        /// <summary>
        /// Provides access to <see cref="SqlCommand"/> parameters collection.
        /// </summary>
        internal SqlParameterCollection Parameters
        {
            get
            {
                if ( m_Command != null )
                    return m_Command.Parameters;

                throw new InvalidOperationException("Parameters are nonviable, SqlCommand object is null.");
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="MsSqlDataCommand"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="dispose">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        public override void Dispose( bool dispose )
        {
            if ( !m_Disposed )
            {
                lock ( this )
                {
                    if ( dispose )
                        Release();

                    m_Disposed = true;
                }
            }
        }
    }
}