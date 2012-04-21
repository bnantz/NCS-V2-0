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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

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
	public class SymmetricKeyGeneratorFixture
	{
		private SymmetricKeyGenerator symmetricKeyGenerator;

		[TestInitialize]
		public void CreateKeyGenerator()
		{
			symmetricKeyGenerator = new SymmetricKeyGenerator();
		}

		[TestMethod]
		public void SymmetricKeysCanBeGeneratedFromAlgorithmNames()
		{
			ProtectedKey generatedKey = symmetricKeyGenerator.GenerateKey("Rijndael", DataProtectionScope.CurrentUser);
			Assert.IsNotNull(generatedKey.EncryptedKey);
		}

		[TestMethod]
		public void SymmetricKeysCanBeGeneratedFromAlgorithmType()
		{
			ProtectedKey generatedKey = symmetricKeyGenerator.GenerateKey(typeof(RijndaelManaged), DataProtectionScope.CurrentUser);
			Assert.IsNotNull(generatedKey.EncryptedKey);
		}

		[TestMethod]
		public void UnprotectedSymmetricKeysCanBeGeneratedFromAlgorithmNames()
		{
			byte [] generatedKey = symmetricKeyGenerator.GenerateKey("Rijndael");
			Assert.IsNotNull(generatedKey);
		}

		[TestMethod]
		public void UnprotectedSymmetricKeysCanBeGeneratedFromAlgorithmTypes()
		{
			byte [] generatedKey = symmetricKeyGenerator.GenerateKey(typeof(RijndaelManaged));
			Assert.IsNotNull(generatedKey);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void AttemptingToCreateKeyForUnknownAlgorithmNameThrowsException()
		{
			symmetricKeyGenerator.GenerateKey("UnknownAlgorithmName", DataProtectionScope.CurrentUser);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void FailuretoCreateKeyForAlgorithmTypeThrowsException()
		{
			symmetricKeyGenerator.GenerateKey(typeof(object), DataProtectionScope.CurrentUser);
		}
	}
}
