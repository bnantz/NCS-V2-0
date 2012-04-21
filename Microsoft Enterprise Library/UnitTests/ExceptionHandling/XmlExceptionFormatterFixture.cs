//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Text;
using System.Xml;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
	[TestClass]
    public class XmlExceptionFormatterFixture
    {
		[TestMethod]
        public void CreateXmlWriterTest()
        {
            StringBuilder sb = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
			Exception ex = new MockException();
            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex);

            Assert.AreSame(writer, formatter.Writer);
            Assert.AreSame(ex, formatter.Exception);
        }

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateWithNullXmlWriterThrows()
		{			
			new XmlExceptionFormatter((XmlWriter)null, new Exception());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateWithNullTextWriterThrows()
		{
			new XmlExceptionFormatter((TextWriter)null, new Exception());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateWithNullExceptionThrows()
		{
			StringBuilder sb = new StringBuilder();
			XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
			new XmlExceptionFormatter(writer, null);
		}

		[TestMethod]
		
		public void VerifyInnerExceptionGetsFormatted()
		{
			StringBuilder sb = new StringBuilder();
			StringWriter writer = new StringWriter(sb);
			Exception exception = new MockException("Foo Bar", new MockException());

			XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, exception);
			Assert.IsTrue(sb.Length == 0);
			formatter.Format();

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(sb.ToString());
			XmlNode element = doc.DocumentElement.SelectSingleNode("//InnerException");
			Assert.IsNotNull(element);
			
		}

		[TestMethod]
        public void CreateTextWriterTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
			Exception ex = new MockException();
            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex);

            // TextWriter won't be the same so we can only test the Exception
            Assert.AreSame(ex, formatter.Exception);
        }

		[TestMethod]
        public void SimpleXmlWriterFormatterTest()
        {
            StringBuilder sb = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
			Exception ex = new MockException();
            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex);

            Assert.IsTrue(sb.Length == 0);

            formatter.Format();

            Assert.IsTrue(sb.Length > 0);
        }

		[TestMethod]
        public void SimpleTextWriterFormatterTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
			Exception ex = new MockException();
            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex);

            Assert.IsTrue(sb.Length == 0);

            formatter.Format();

            Assert.IsTrue(sb.Length > 0);
        }

		[TestMethod]
        public void WellFormedTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
			Exception ex = new MockException();
            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex);
            formatter.Format();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
        }
    }
}

