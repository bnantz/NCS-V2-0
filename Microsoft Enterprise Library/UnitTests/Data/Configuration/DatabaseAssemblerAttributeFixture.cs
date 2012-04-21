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
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration
{
	[TestClass]
	public class DatabaseAssemblerAttributeFixture
	{
		[TestMethod]
		public void CreationOfAttributeWithCorrectTypeSucceeds()
		{
			DatabaseAssemblerAttribute attribute = new DatabaseAssemblerAttribute(typeof(SqlDatabaseAssembler));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreationOfAttributeWithNullTypeThrows()
		{
			new DatabaseAssemblerAttribute(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CreationOfAttributeWithInvalidTypeThrows()
		{
			new DatabaseAssemblerAttribute(typeof(object));
		}
	}
}
