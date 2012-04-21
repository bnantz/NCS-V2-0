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
    public partial class KeyedHashAlgorithmProviderFixture 
    {	

		private HashProviderHelper HashProviderHelper
		{
			get
			{
				HashAlgorithmProvider defaultProvider = new KeyedHashAlgorithmProvider(typeof(HMACSHA1), false, key);
				HashAlgorithmProvider saltedProvider = new KeyedHashAlgorithmProvider(typeof(HMACSHA1), true, key);

				return new HashProviderHelper(defaultProvider, saltedProvider);
			}
		}

		ProtectedKey key;

		[TestInitialize]
		public void CreateKey()
		{
			key = KeyManager.GenerateKeyedHashKey(typeof(HMACSHA1), DataProtectionScope.CurrentUser);
		}
		
		[TestMethod]
		public void CreateHash()
		{
			HashProviderHelper.CreateHash();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructWithNullKeyThrows()
		{
			new KeyedHashAlgorithmProvider(typeof(HMACSHA1), true, (ProtectedKey)null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructWithNonKeyedHashAlgorithmThrows()
		{
			new KeyedHashAlgorithmProvider(typeof(SHA1), true, key);
		}

		[TestMethod]
		public void CompareEqualHash()
		{
			HashProviderHelper.CompareEqualHash();
		}

		[TestMethod]
		public void CompareHashOfDifferentText()
		{
			HashProviderHelper.CompareHashOfDifferentText();
		}

		[TestMethod]
		public void HashWithSalt()
		{
			HashProviderHelper.HashWithSalt();
		}

		[TestMethod]
		public void UniqueSaltedHashes()
		{
			HashProviderHelper.UniqueSaltedHashes();
		}

		[TestMethod]
		public void CompareHashWithSalt()
		{
			HashProviderHelper.CompareHashWithSalt();
		}

		[TestMethod]
		public void VerifyHashAsUnique()
		{
			HashProviderHelper.VerifyHashAsUnique();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CompareHashZeroLengthHashedTextThrows()
		{
			HashProviderHelper.CompareHashZeroLengthHashedText();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CompareHashNullHashedTextThrows()
		{
			HashProviderHelper.CompareHashNullHashedText();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CompareHashNullPlainTextThrows()
		{
			HashProviderHelper.CompareHashNullPlainText();
		}

		[TestMethod]
		public void CompareHashZeroLengthPlainText()
		{
			HashProviderHelper.CompareHashZeroLengthPlainText();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateHashNullPlainTextThrows()
		{
			HashProviderHelper.CreateHashNullPlainText();
		}

		[TestMethod]
		public void CreateHashZeroLengthPlainText()
		{
			HashProviderHelper.CreateHashZeroLengthPlainText();
		}

		[TestMethod]
		public void CompareHashInvalidHashedText()
		{
			HashProviderHelper.CompareHashInvalidHashedText();
		}
    }
}

