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
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
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
    public class ExceptionFormatterFixture
    {
		private const string fileNotFoundMessage = "The file can't be found";
		private const string theFile = "theFile";
		private const string loggedTimeStampFailMessage = "Logged TimeStamp is not within a one minute time window";
		private const string machineName = "MachineName";
		private const string timeStamp = "TimeStamp";
		private const string appDomainName = "AppDomainName";
		private const string threadIdentity = "ThreadIdentity";
		private const string windowsIdentity = "WindowsIdentity";
		private const string fieldString = "FieldString";
		private const string mockFieldString = "MockFieldString";
		private const string propertyString = "PropertyString";
		private const string mockPropertyString = "MockPropertyString";
		private const string message = "Message";
		private const string computerName = "COMPUTERNAME";
		private const string permissionDenied = "Permission Denied";


		[TestMethod]
        public void AdditionalInfoTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

			Exception exception = new FileNotFoundException(fileNotFoundMessage, theFile);
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);

            formatter.Format();

            Assert.AreEqual(Environment.MachineName, formatter.AdditionalInfo[machineName]);

            DateTime minimumTime = DateTime.UtcNow.AddMinutes(-1);
            DateTime loggedTime = DateTime.Parse(formatter.AdditionalInfo[timeStamp]);
            if (DateTime.Compare(minimumTime, loggedTime) > 0)
            {
                Assert.Fail(loggedTimeStampFailMessage);
            }

            Assert.AreEqual(AppDomain.CurrentDomain.FriendlyName, formatter.AdditionalInfo[appDomainName]);
            Assert.AreEqual(Thread.CurrentPrincipal.Identity.Name, formatter.AdditionalInfo[threadIdentity]);
            Assert.AreEqual(WindowsIdentity.GetCurrent().Name, formatter.AdditionalInfo[windowsIdentity]);
        }

		[TestMethod]
        public void ReflectionFormatterReadTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            MockTextExceptionFormatter formatter = new MockTextExceptionFormatter(writer, new MockException());

            formatter.Format();

            Assert.AreEqual(formatter.fields[fieldString], mockFieldString);
            Assert.AreEqual(formatter.properties[propertyString], mockPropertyString);
            // The message should be null because the reflection formatter should ignore this property
            Assert.AreEqual(null, formatter.properties[message]);
        }

		[TestMethod]
		public void CanGetMachineNameWithoutSecurity()
		{
			EnvironmentPermission denyPermission = new EnvironmentPermission(EnvironmentPermissionAccess.Read, computerName);
			PermissionSet permissions = new PermissionSet(PermissionState.None);
			permissions.AddPermission(denyPermission);
			permissions.Deny();

			StringBuilder sb = new StringBuilder();
			StringWriter writer = new StringWriter(sb);
			Exception exception = new MockException();
			TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);
			Assert.IsTrue(sb.Length == 0);
			formatter.Format();

			Assert.IsTrue(sb.ToString().Contains(machineName + " : " + permissionDenied));
		}

		[TestMethod]		
		public void CanGetWindowsIdentityWithoutSecurity()
		{
			SecurityPermission denyPermission = new SecurityPermission(SecurityPermissionFlag.ControlPrincipal);
			PermissionSet permissions = new PermissionSet(PermissionState.None);
			permissions.AddPermission(denyPermission);
			permissions.Deny();

			StringBuilder sb = new StringBuilder();
			StringWriter writer = new StringWriter(sb);
			Exception exception = new MockException();
			TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);
			Assert.IsTrue(sb.Length == 0);
			formatter.Format();
			Console.WriteLine(sb.ToString());
			Assert.IsTrue(sb.ToString().Contains(windowsIdentity + " : " + permissionDenied));
		}				
    }
}

