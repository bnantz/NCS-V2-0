//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
	[TestClass]
	public class KeyedHashKeyGeneratorFixture
	{
		private KeyedHashKeyGenerator keyedHashKeyGenerator;

		[TestInitialize]
		public void CreateKeyGenerator()
		{
			keyedHashKeyGenerator = new KeyedHashKeyGenerator();
		}

		[TestMethod]
		public void KeyedHashKeysCanBeGeneratedFromAlgorithmNames()
		{
			ProtectedKey generatedKey = keyedHashKeyGenerator.GenerateKey("hmacsha1", DataProtectionScope.CurrentUser);
			Assert.IsNotNull(generatedKey.EncryptedKey);
		}

		[TestMethod]
		public void KeyedHashKeyCanBeGeneratedFromAlgorithmType()
		{
			ProtectedKey generatedKey = keyedHashKeyGenerator.GenerateKey(typeof(HMACRIPEMD160), DataProtectionScope.CurrentUser);
			Assert.IsNotNull(generatedKey.EncryptedKey);
		}

		[TestMethod]
		public void UnprotectedKeyedHashKeyCanBeGeneratedFromAlgorithmNames()
		{
			byte [] generatedKey = keyedHashKeyGenerator.GenerateKey("hmacsha1");
			Assert.IsNotNull(generatedKey);
		}

		[TestMethod]
		public void UnprotectedKeyedHashKeyCanBeGeneratedFromAlgorithmTypes()
		{
			byte [] generatedKey = keyedHashKeyGenerator.GenerateKey(typeof(HMACRIPEMD160));
			Assert.IsNotNull(generatedKey);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void AttemptingToCreateKeyForUnknownAlgorithmNameThrowsException()
		{
			keyedHashKeyGenerator.GenerateKey("UnknownAlgorithmName", DataProtectionScope.CurrentUser);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void FailuretoCreateKeyForAlgorithmTypeThrowsException()
		{
			keyedHashKeyGenerator.GenerateKey(typeof(object), DataProtectionScope.CurrentUser);
		}
	}
}
