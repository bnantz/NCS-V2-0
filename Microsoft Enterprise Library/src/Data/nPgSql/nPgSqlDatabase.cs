//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using System.Security.Permissions;
using System.Xml;

using Npgsql;
using NpgsqlTypes;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.nPgSql
{
	/// <summary>
	/// <para>Represents a SQL Server database.</para>
	/// </summary>
	/// <remarks> 
	/// <para>
	/// Internally uses SQL Server .NET Managed Provider from Microsoft (System.Data.SqlClient) to connect to the database.
	/// </para>  
	/// </remarks>
	//[SqlClientPermission(SecurityAction.Demand)]
    [DatabaseAssembler(typeof(nPgSqlDatabaseAssembler))]
	public class nPgSqlDatabase : Database
	{
		/// <summary>
        /// Initializes a new instance of the <see cref="nPgSqlDatabase"/> class with a connection string.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
        public nPgSqlDatabase(string connectionString)
			: base(connectionString, NpgsqlClientFactory.Instance)
		{
		}

		/// <summary>
		/// <para>Gets the parameter token used to delimit parameters for the SQL Server database.</para>
		/// </summary>
		/// <value>
		/// <para>The '@' symbol.</para>
		/// </value>
		protected char ParameterToken
		{
			get { return '@'; }
		}

        ///// <summary>
        ///// <para>Executes the <see cref="SqlCommand"/> and returns a new <see cref="XmlReader"/>.</para>
        ///// </summary>
        ///// <param name="command">
        ///// <para>The <see cref="SqlCommand"/> to execute.</para>
        ///// </param>
        ///// <returns>
        ///// <para>An <see cref="XmlReader"/> object.</para>
        ///// </returns>
        //public XmlReader ExecuteXmlReader(DbCommand command)
        //{
        //    NpgsqlCommand sqlCommand = CheckIfSqlCommand(command);

        //    DbConnection connection = OpenConnection();
        //    PrepareCommand(command, connection);
        //    return DoExecuteXmlReader(sqlCommand);
        //}
        //
        ///// <summary>
        ///// <para>Executes the <see cref="SqlCommand"/> in a transaction and returns a new <see cref="XmlReader"/>.</para>
        ///// </summary>
        ///// <param name="command">
        ///// <para>The <see cref="SqlCommand"/> to execute.</para>
        ///// </param>
        ///// <param name="transaction">
        ///// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        ///// </param>
        ///// <returns>
        ///// <para>An <see cref="XmlReader"/> object.</para>
        ///// </returns>
        //public XmlReader ExecuteXmlReader(DbCommand command, DbTransaction transaction)
        //{
        //    NpgsqlCommand sqlCommand = CheckIfSqlCommand(command);

        //    PrepareCommand(sqlCommand, transaction);
        //    return DoExecuteXmlReader(sqlCommand);
        //}

        ///// <devdoc>
        ///// Execute the actual XML Reader call.
        ///// </devdoc>        
        //private XmlReader DoExecuteXmlReader(NpgsqlCommand sqlCommand)
        //{
        //    try
        //    {
        //        DateTime startTime = DateTime.Now;
        //        XmlReader reader = sqlCommand.ExecuteXmlReader();
        //        instrumentationProvider.FireCommandExecutedEvent(startTime);
        //        return reader;
        //    }
        //    catch (Exception e)
        //    {
        //        instrumentationProvider.FireCommandFailedEvent(sqlCommand.CommandText, ConnectionStringNoCredentials, e);
        //        throw;
        //    }
        //}

		private static NpgsqlCommand CheckIfSqlCommand(DbCommand command)
		{
            NpgsqlCommand sqlCommand = (IDbCommand)command as NpgsqlCommand;
			if (sqlCommand == null) throw new ArgumentException(Resources.ExceptionCommandNotSqlCommand, "command");
			return sqlCommand;
		}

		/// <devdoc>
		/// Listens for the RowUpdate event on a dataadapter to support UpdateBehavior.Continue
		/// </devdoc>
		private void OnSqlRowUpdated(object sender, NpgsqlRowUpdatedEventArgs rowThatCouldNotBeWritten)
		{
			if (rowThatCouldNotBeWritten.RecordsAffected == 0)
			{
				if (rowThatCouldNotBeWritten.Errors != null)
				{
					rowThatCouldNotBeWritten.Row.RowError = Resources.ExceptionMessageUpdateDataSetRowFailure;
					rowThatCouldNotBeWritten.Status = UpdateStatus.SkipCurrentRow;
				}
			}
		}

		/// <summary>
		/// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
		/// </summary>
		/// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
		/// <remarks>The <see cref="DbCommand"/> must be a <see cref="NpgsqlCommand"/> instance.</remarks>
		protected override void DeriveParameters(DbCommand discoveryCommand)
		{
            NpgsqlCommandBuilder.DeriveParameters((NpgsqlCommand)(IDbCommand)discoveryCommand);
		}

		/// <summary>
		/// Returns the starting index for parameters in a command.
		/// </summary>
		/// <returns>The starting index for parameters in a command.</returns>
		protected override int UserParametersStartIndex()
		{
			return 1;
		}

		/// <summary>
		/// Builds a value parameter name for the current database.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <returns>A correctly formated parameter name.</returns>
		public override string BuildParameterName(string name)
		{
			if (name[0] != this.ParameterToken)
			{
				return name.Insert(0, new string(this.ParameterToken, 1));
			}
			return name;
		}

		/// <summary>
		/// Sets the RowUpdated event for the data adapter.
		/// </summary>
		/// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
		protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
		{
			((NpgsqlDataAdapter)adapter).RowUpdated += new NpgsqlRowUpdatedEventHandler(OnSqlRowUpdated);
		}

		/// <summary>
		/// Determines if the number of parameters in the command matches the array of parameter values.
		/// </summary>
		/// <param name="command">The <see cref="DbCommand"/> containing the parameters.</param>
		/// <param name="values">The array of parameter values.</param>
		/// <returns><see langword="true"/> if the number of parameters and values match; otherwise, <see langword="false"/>.</returns>
		protected override bool SameNumberOfParametersAndValues(DbCommand command, object[] values)
		{
			int returnParameterCount = 1;
			int numberOfParametersToStoredProcedure = command.Parameters.Count - returnParameterCount;
			int numberOfValuesProvidedForStoredProcedure = values.Length;
			return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
		}

		/// <summary>
		/// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
		/// </summary>
		/// <param name="command">The command to add the parameter.</param>
		/// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="NpgsqlDbType"/> values.</para></param>
		/// <param name="size"><para>The maximum size of the data within the column.</para></param>
		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
		/// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
		/// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
		/// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
		/// <param name="value"><para>The value of the parameter.</para></param>       
        public virtual void AddParameter(DbCommand command, string name, NpgsqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
			command.Parameters.Add(parameter);
		}

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="NpgsqlDbType"/> values.</para></param>        
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>    
        public void AddParameter(DbCommand command, string name, NpgsqlDbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        /// <summary>
        /// Adds a new Out <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the out parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="NpgsqlDbType"/> values.</para></param>        
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>        
        public void AddOutParameter(DbCommand command, string name, NpgsqlDbType dbType, int size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the in parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="NpgsqlDbType"/> values.</para></param>                
        /// <remarks>
        /// <para>This version of the method is used when you can have the same parameter object multiple times with different values.</para>
        /// </remarks>        
        public void AddInParameter(DbCommand command, string name, NpgsqlDbType dbType)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The commmand to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="NpgsqlDbType"/> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddInParameter(DbCommand command, string name, NpgsqlDbType dbType, object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="NpgsqlDbType"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the value.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        public void AddInParameter(DbCommand command, string name, NpgsqlDbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="NpgsqlDbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        protected DbParameter CreateParameter(string name, NpgsqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            NpgsqlParameter param = (IDbDataParameter)CreateParameter(name) as NpgsqlParameter;
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return (DbParameter)(IDbDataParameter)param;
        }

		/// <summary>
		/// Configures a given <see cref="DbParameter"/>.
		/// </summary>
		/// <param name="param">The <see cref="DbParameter"/> to configure.</param>
		/// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="NpgsqlDbType"/> values.</para></param>
		/// <param name="size"><para>The maximum size of the data within the column.</para></param>
		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
		/// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
		/// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
		/// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
		/// <param name="value"><para>The value of the parameter.</para></param>  
        protected virtual void ConfigureParameter(NpgsqlParameter param, string name, NpgsqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
            param.NpgsqlDbType = dbType;
			param.Size = size;
			param.Value = (value == null) ? DBNull.Value : value;
			param.Direction = direction;
			param.IsNullable = nullable;
			param.SourceColumn = sourceColumn;
			param.SourceVersion = sourceVersion;
		}
	}

    /// <summary>
    /// 
    /// </summary>
    public class NpgsqlClientFactory : DbProviderFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly NpgsqlClientFactory Instance;
 
        static NpgsqlClientFactory()
        {
            NpgsqlClientFactory.Instance = new NpgsqlClientFactory();
        }

        private NpgsqlClientFactory()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DbCommand CreateCommand()
        {
            return (DbCommand)(IDbCommand)new NpgsqlCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DbCommandBuilder CreateCommandBuilder()
        {
              return (DbCommandBuilder)(System.ComponentModel.Component)new NpgsqlCommandBuilder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
 
        public override DbConnection CreateConnection()
        {
              return (DbConnection) (IDbConnection) new NpgsqlConnection();
        }

         
        //public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        //{
        //      return new NpgsqlConnectionStringBuilder();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
 
        public override DbDataAdapter CreateDataAdapter()
        {
              return new NpgsqlDataAdapter();
        }

         
        //public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        //{
        //      return NpgsqlDataSourceEnumerator.Instance;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
 
        public override DbParameter CreateParameter()
        {
            return (DbParameter)(IDbDataParameter)new NpgsqlParameter();
        }

         
        //public override CodeAccessPermission CreatePermission(PermissionState state)
        //{
        //      return new NpgsqlClientPermission(state);
        //}

         /// <summary>
         /// 
         /// </summary>
        public override bool CanCreateDataSourceEnumerator
        {
              get
              {
                    return true;
              }
        }
    }
}