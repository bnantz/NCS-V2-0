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
	public class SymmetricKeyArchivalFixture
	{
		const string passphrase = "BaseballIsLife";

		MemoryStream stream;
		byte[] keyToBeArchived;

		[TestInitialize]
		public void SetUp()
		{
			stream = new MemoryStream();
			keyToBeArchived = GenerateKeyToBeArchived();
		}

		[TestCleanup]
		public void DisposeStream()
		{
			stream.Dispose();
		}

		[TestMethod]
		public void SymmetricKeysCanBeRestoredFromStream()
		{
			int versionNumber = KeyReaderWriter.versionNumber;
			WriteEncryptedKey(versionNumber);

			IKeyReader reader = new KeyReaderWriter();
			ProtectedKey restoredKey = reader.Restore(stream, passphrase, DataProtectionScope.LocalMachine);

			AssertHelpers.AssertArraysEqual(keyToBeArchived, restoredKey.DecryptedKey);
		}

		[TestMethod]
		public void SymmetricKeyCanBeRestoredAsUnprotectedKeyFromStream()
		{
			int versionNumber = KeyReaderWriter.versionNumber;
			WriteEncryptedKey(versionNumber);

			IKeyReader reader = new KeyReaderWriter();
			byte[] restoredKey = reader.Restore(stream, passphrase);

			AssertHelpers.AssertArraysEqual(keyToBeArchived, restoredKey);
		}

		[TestMethod]
		public void SymmetricKeysCanBeArchivedToStream()
		{
			byte[] keyToBeArchived = GenerateKeyToBeArchived();
			ProtectedKey key = ProtectedKey.CreateFromPlaintextKey(keyToBeArchived, DataProtectionScope.LocalMachine);

			IKeyWriter writer = new KeyReaderWriter();
			writer.Archive(stream, key, passphrase);

			stream.Seek(0, SeekOrigin.Begin);

			IKeyReader reader = new KeyReaderWriter();
			ProtectedKey restoredKey = reader.Restore(stream, passphrase, DataProtectionScope.LocalMachine);

			AssertHelpers.AssertArraysEqual(keyToBeArchived, restoredKey.DecryptedKey);
		}

		[TestMethod, ExpectedException(typeof(InvalidOperationException))]
		public void MismatchedVersionNumberOnRestoreThrowsException()
		{
			int versionNumber = 7777;
			WriteEncryptedKey(versionNumber);

			IKeyReader reader = new KeyReaderWriter();
			ProtectedKey restoredKey = reader.Restore(stream, passphrase, DataProtectionScope.LocalMachine);
		}

		[TestMethod, ExpectedException(typeof(CryptographicException))]
		public void MismatchedPassphrasesOnRestoreThrowsException()
		{
			int versionNumber = KeyReaderWriter.versionNumber;
			WriteEncryptedKey(versionNumber);

			IKeyReader reader = new KeyReaderWriter();
			ProtectedKey restoredKey = reader.Restore(stream, "badPassphrase", DataProtectionScope.LocalMachine);
		}

		private void WriteEncryptedKey(int versionNumber)
		{
			byte[] versionNumberBytes = BitConverter.GetBytes(versionNumber);
			byte[] salt = GenerateSalt();
			byte[] encryptedKey = EncryptKeyForArchival(keyToBeArchived, salt);

			stream.Write(versionNumberBytes, 0, versionNumberBytes.Length);
			stream.Write(salt, 0, salt.Length);
			stream.Write(encryptedKey, 0, encryptedKey.Length);

			stream.Seek(0, SeekOrigin.Begin);
		}
		private static byte[] Transform(byte[] data, ICryptoTransform transformer)
		{
			return CryptographyUtility.Transform(transformer, data);
		}
		private byte[] GenerateKeyToBeArchived()
		{
			RijndaelManaged symmetricAlgorithm = (RijndaelManaged)RijndaelManaged.Create();
			symmetricAlgorithm.GenerateKey();
			return symmetricAlgorithm.Key;
		}
		private byte[] GenerateSalt()
		{
			return CryptographyUtility.GetRandomBytes(16);
		}
		private byte[] GenerateArchivalKey(SymmetricAlgorithm archivalEncryptionAlgorithm, byte[] salt)
		{
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(passphrase, salt);
			byte[] archivalKey = pdb.GetBytes(archivalEncryptionAlgorithm.KeySize / 8);

			return archivalKey;
		}
		private byte[] EncryptKeyForArchival(byte[] keyToExport, byte[] salt)
		{
			RijndaelManaged archivalEncryptionAlgorithm = new RijndaelManaged();
			byte[] archivalKey = GenerateArchivalKey(archivalEncryptionAlgorithm, salt);
			byte[] iv = new byte[archivalEncryptionAlgorithm.BlockSize / 8];
			byte[] encryptedKey = Transform(keyToExport, archivalEncryptionAlgorithm.CreateEncryptor(archivalKey, iv));
			return encryptedKey;
		}
	}
}
