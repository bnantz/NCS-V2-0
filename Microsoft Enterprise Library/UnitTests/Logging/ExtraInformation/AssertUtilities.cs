//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
    internal static class AssertUtilities
    {
        internal static void AssertStringDoesNotContain(string o, string s, string message)
        {
            Assert.IsNotNull(o);
            Assert.IsNotNull(s);
            Assert.IsNotNull(message);
            Assert.IsFalse(o.StartsWith(s), string.Format("\nIn {2}, the string:\n\t{0}\ncontains\n\t{1}\nwhen it should not.\n", o, s, message));
        }
    }
}

