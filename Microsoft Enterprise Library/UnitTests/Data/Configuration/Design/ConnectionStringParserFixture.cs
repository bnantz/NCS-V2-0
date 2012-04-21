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

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
	[TestClass]
	public class ConnectionStringParserFixture
	{
		private string connectionString = @"data source=.\SQLEXPRESS;Integrated Security=SSPI;";

		[TestMethod]
		public void ParseConnectionString()
		{
			ICollection<ConnectionStringNameValuePair> parameters = ConnectionStringParser.Parse(connectionString);
			List<ConnectionStringNameValuePair> pairs = new List<ConnectionStringNameValuePair>(parameters);

			Assert.AreEqual(2, parameters.Count);
			AssertContainsDataSource(pairs);
			AssertContainsIntegratedSecurity(pairs);			
		}

		[TestMethod]
		public void BuildConnectionString()
		{
			List<ConnectionStringNameValuePair> pairs = new List<ConnectionStringNameValuePair>();
			pairs.Add(new ConnectionStringNameValuePair("data source", @".\SQLEXPRESS"));
			pairs.Add(new ConnectionStringNameValuePair("Integrated Security", "SSPI"));

			string conString = ConnectionStringParser.Build(pairs);

			Assert.AreEqual(connectionString, conString);
		}

		private static void AssertContainsDataSource(List<ConnectionStringNameValuePair> pairs)
		{
			ConnectionStringNameValuePair pair = pairs.Find(delegate(ConnectionStringNameValuePair connectionStringPair)
					 {
						 if ((connectionStringPair.Name == "data source") && (connectionStringPair.Value == @".\SQLEXPRESS"))
						 {
							 return true;
						 }
						 return false;
					 });

			Assert.IsNotNull(pair);			
		}

		private static void AssertContainsIntegratedSecurity(List<ConnectionStringNameValuePair> pairs)
		{
			ConnectionStringNameValuePair pair = pairs.Find(delegate(ConnectionStringNameValuePair connectionStringPair)
					 {
						 if ((connectionStringPair.Name == "Integrated Security") && (connectionStringPair.Value == "SSPI"))
						 {
							 return true;
						 }
						 return false;
					 });

			Assert.IsNotNull(pair);			
		}
	}
}
