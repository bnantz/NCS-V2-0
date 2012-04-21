//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
	[TestClass]
	public class OracleExecuteNonQueryFixture
	{
		private ExecuteNonQueryFixture baseFixture;
		private Database db;
		private const string insertString = "insert into Region values (77, 'Elbonia')";
		private const string countQuery = "select count(*) from Region";

		[TestInitialize]
		public void SetUp()
		{
			DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
			db = factory.Create("OracleTest");

			DbCommand insertionCommand = db.GetSqlStringCommand(insertString);
			DbCommand countCommand = db.GetSqlStringCommand(countQuery);

			baseFixture = new ExecuteNonQueryFixture(db, insertString, countQuery, insertionCommand, countCommand);
		}

		[TestMethod]
		public void CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction()
		{
			baseFixture.CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction();
		}

		[TestMethod]
		public void CanExecuteNonQueryWithDbCommand()
		{
			baseFixture.CanExecuteNonQueryWithDbCommand();
		}

		[TestMethod]
		public void CanExecuteNonQueryThroughTransaction()
		{
			baseFixture.CanExecuteNonQueryThroughTransaction();
		}

		[TestMethod]
		public void TransactionActuallyRollsBack()
		{
			baseFixture.TransactionActuallyRollsBack();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ExecuteNonQueryWithNullDbTransaction()
		{
			baseFixture.ExecuteNonQueryWithNullDbTransaction();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ExecuteNonQueryWithNullDbCommandAndTransaction()
		{
			baseFixture.ExecuteNonQueryWithNullDbCommandAndTransaction();
		}
	}
}

