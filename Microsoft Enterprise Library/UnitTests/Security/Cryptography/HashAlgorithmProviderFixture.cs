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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;

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
    public class HashAlgorithmProviderFixture 
    {
		private readonly byte[] plainText = new byte[] { 0, 1, 2, 3, 4, 5, 6 };
		private const string hashInstance = "hashAlgorithm1";

		

		private HashProviderHelper HashProviderHelper
		{
			get
			{
				HashAlgorithmProvider defaultProvider = new HashAlgorithmProvider(typeof(SHA1Managed), false);
				HashAlgorithmProvider saltedProvider = new HashAlgorithmProvider(typeof(SHA1Managed), true);

				return new HashProviderHelper(defaultProvider, saltedProvider);
			}
		}

		

		[TestMethod]
		public void HashSha1()
		{
			IHashProvider hashProvider = HashProviderHelper.DefaultHashProvider;
			SHA1 sha1 = SHA1Managed.Create();
			byte[] origHash = sha1.ComputeHash(plainText);
			byte[] providerHash = hashProvider.CreateHash(plainText);

			Assert.IsTrue(CryptographyUtility.CompareBytes(origHash, providerHash));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HashWithBadTypeThrows()
		{
			HashAlgorithmProvider hashProvider = new HashAlgorithmProvider(typeof(Exception), false);
			hashProvider.CreateHash(plainText);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructWithNullAlgorithmTypeThrows()
		{
			new HashAlgorithmProvider(null, true);
		}

		[TestMethod]
		public void CreateHash()
		{
			HashProviderHelper.CreateHash();
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

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HashFailureThrowsWithInstrumentationEnabled()
		{
			HashAlgorithmProvider hashProvider = new HashAlgorithmProvider(typeof(SHA1Managed), false);
			ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
			binder.Bind(hashProvider.GetInstrumentationEventProvider(), new HashAlgorithmInstrumentationListener("foo", true, true, true));

			hashProvider.CreateHash(null);
		}
		
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ExceptionThrownIfPlaintextByteArrayIsNull()
		{
			HashProviderHelper.ThrowExceptionWhenByteArrayIsNull();
		}

	}
}

