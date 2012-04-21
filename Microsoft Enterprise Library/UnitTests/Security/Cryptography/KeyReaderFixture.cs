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
using System.Text;
using System.Collections.Generic;
using System.IO;
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
	public class KeyReaderFixture
	{
		RijndaelManaged symmetricAlgo;
		byte[] encryptedKey;
		MemoryStream stream;

		[TestInitialize]
		public void CreateSymmetricKey()
		{
			symmetricAlgo = (RijndaelManaged)RijndaelManaged.Create();
			symmetricAlgo.GenerateKey();
			encryptedKey = ProtectedData.Protect(symmetricAlgo.Key, null, DataProtectionScope.CurrentUser);

			stream = new MemoryStream();
		}

		[TestCleanup]
		public void CleanupStream()
		{
			stream.Dispose();
		}

		[TestMethod]
		public void CanReadProtectedKeyFromStream()
		{
			byte[] correctVersionNumber = BitConverter.GetBytes(KeyReaderWriter.versionNumber);

			WriteKeyToStream(correctVersionNumber);
			stream.Seek(0, SeekOrigin.Begin);

			KeyReaderWriter reader = new KeyReaderWriter();
			ProtectedKey readKey = reader.Read(stream, DataProtectionScope.CurrentUser);

			AssertHelpers.AssertArraysEqual(symmetricAlgo.Key, readKey.DecryptedKey);
		}

		[TestMethod, ExpectedException(typeof(InvalidOperationException))]
		public void MismatchedVersionNumbersThrowsException()
		{
			byte[] badVersionNumber = BitConverter.GetBytes(0xff);

			WriteKeyToStream(badVersionNumber);
			stream.Seek(0, SeekOrigin.Begin);

			KeyReaderWriter reader = new KeyReaderWriter();
			ProtectedKey readKey = reader.Read(stream, DataProtectionScope.CurrentUser);
		}

		private void WriteKeyToStream(byte[] correctVersionNumber)
		{
			stream.Write(correctVersionNumber, 0, 4);
			stream.Write(encryptedKey, 0, encryptedKey.Length);
		}
	}
}
