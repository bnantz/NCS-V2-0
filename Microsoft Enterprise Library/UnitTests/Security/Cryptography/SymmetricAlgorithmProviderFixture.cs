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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using System.IO;

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
	public class SymmetricAlgorithmProviderFixture
	{
		byte[] plaintext = new byte[] { 0x01, 0x02, 0x03, 0x04 };
		MemoryStream stream;
		SymmetricAlgorithmProvider provider;

		private SymmetricProviderHelper SymmetricProviderHelper
		{
			get
			{
				return new SymmetricProviderHelper(provider);
			}
		}

		[TestInitialize]
		public void SetUp()
		{
			stream = CreateSymmetricKey();
			provider = new SymmetricAlgorithmProvider(typeof(RijndaelManaged), stream, DataProtectionScope.CurrentUser);
		}

		[TestCleanup]
		public void TearDown()
		{
			stream.Dispose();
		}

		[TestMethod]
		public void CanEncryptAndDecryptMessage()
		{
			byte[] encryptedData = provider.Encrypt(plaintext);
			byte[] decryptedData = provider.Decrypt(encryptedData);

			AssertHelpers.AssertArraysEqual(plaintext, decryptedData);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructWithNullAlgorithmTypeThrows()
		{
			new SymmetricAlgorithmProvider(null, ProtectedKey.CreateFromPlaintextKey(new byte[] { 0x00, 0x01 }, DataProtectionScope.CurrentUser));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructWithNullKeyFileNameThrowsException()
		{
			new SymmetricAlgorithmProvider(typeof(RijndaelManaged), (string)null, DataProtectionScope.CurrentUser);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructWithEmptyKeyFileNameThrowsException()
		{
			new SymmetricAlgorithmProvider(typeof(RijndaelManaged), String.Empty, DataProtectionScope.CurrentUser);
		}

		[TestMethod]
		[ExpectedException(typeof(FileNotFoundException))]
		public void ConstructWithBadFileNameThrowsException()
		{
			new SymmetricAlgorithmProvider(typeof(RijndaelManaged), "BadFileName", DataProtectionScope.CurrentUser);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructWithNonSymmetricAlgorithmTypeThrows()
		{
			new SymmetricAlgorithmProvider(typeof(object), ProtectedKey.CreateFromPlaintextKey(new byte[] { 0x00, 0x01 }, DataProtectionScope.CurrentUser));
		}

		[TestMethod]
		[ExpectedException(typeof(CryptographicException))]
		public void DecryptBadTextThrows()
		{
			SymmetricProviderHelper.DefaultSymmProvider.Decrypt(new byte[] { 0, 1, 2, 3, 4 });
		}

		[TestMethod]
		public void EncryptAndDecrypt()
		{
			SymmetricProviderHelper.EncryptAndDecrypt(); ;
		}

		[TestMethod]
		public void EncryptAndDecryptOneByte()
		{
			SymmetricProviderHelper.EncryptAndDecryptOneByte();
		}

		[TestMethod]
		public void EncryptAndDecryptOneKilobyte()
		{
			SymmetricProviderHelper.EncryptAndDecryptOneKilobyte();
		}

		[TestMethod]
		public void EncryptAndDecryptOneMegabyte()
		{
			SymmetricProviderHelper.EncryptAndDecryptOneMegabyte();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EncryptNullThrows()
		{
			SymmetricProviderHelper.EncryptNull();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EncryptZeroLengthThrows()
		{
			SymmetricProviderHelper.EncryptZeroLength();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecryptNullThrows()
		{
			SymmetricProviderHelper.DecryptNull();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecryptZeroLengthThrows()
		{
			SymmetricProviderHelper.DecryptZeroLength();
		}

		[TestMethod]
		public void InstrumentationUsesExplicitBinder()
		{
			byte[] key = Guid.NewGuid().ToByteArray();

			InstrumentationAttacherFactory attacherFactory = new InstrumentationAttacherFactory();
			IInstrumentationAttacher binder = attacherFactory.CreateBinder(provider.GetInstrumentationEventProvider(), new object[] { "foo", true, true, true }, new ConfigurationReflectionCache());
			binder.BindInstrumentation();

			Assert.AreSame(typeof(ExplicitInstrumentationAttacher), binder.GetType());
		}

		[TestMethod]
		[ExpectedException(typeof(CryptographicException))]
		public void CryptoFailureThrowsWithInstrumentationEnabled()
		{
			byte[] key = Guid.NewGuid().ToByteArray();

			ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
			binder.Bind(provider.GetInstrumentationEventProvider(), new SymmetricAlgorithmInstrumentationListener("foo", true, true, true));

			provider.Decrypt(new byte[] { 0, 1, 2, 3, 4 });
		}

		private static MemoryStream CreateSymmetricKey()
		{
			MemoryStream stream = new MemoryStream();

			RijndaelManaged algo = (RijndaelManaged)RijndaelManaged.Create();
			algo.GenerateKey();
			KeyManager.Write(stream, ProtectedKey.CreateFromPlaintextKey(algo.Key, DataProtectionScope.CurrentUser));
			stream.Seek(0, SeekOrigin.Begin);

			return stream;
		}
	}
}

