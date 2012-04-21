//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using System.Security.Permissions;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// <para>A symmetric provider for any symmetric algorithm which derives from <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.</para>
	/// </summary>
	[ConfigurationElementType(typeof(SymmetricAlgorithmProviderData))]
	public class SymmetricAlgorithmProvider : ISymmetricCryptoProvider, IInstrumentationEventProvider
	{
		private Type algorithmType;
		private ProtectedKey key;

		SymmetricAlgorithmInstrumentationProvider instrumentationProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="SymmetricAlgorithmProvider"/> class.
		/// </summary>
		/// <param name="algorithmType">The cryptographic algorithm type.</param>
		/// <param name="protectedKeyFileName">Input file from which DPAPI-protected key is to be read.</param>
		/// <param name="protectionScope"><see cref="DataProtectionScope"/> used to protect the key on disk. </param>
		public SymmetricAlgorithmProvider(Type algorithmType, string protectedKeyFileName, DataProtectionScope protectionScope)
			: this(algorithmType, KeyManager.Read(protectedKeyFileName, protectionScope))
		{
		}

		internal SymmetricAlgorithmProvider(Type algorithmType, Stream protectedKeyStream, DataProtectionScope protectionScope)
			: this(algorithmType, KeyManager.Read(protectedKeyStream, protectionScope))
		{
		}

		internal SymmetricAlgorithmProvider(Type algorithmType, ProtectedKey key)
		{
			if (algorithmType == null) throw new ArgumentNullException("algorithmType");
			if (!typeof(SymmetricAlgorithm).IsAssignableFrom(algorithmType)) throw new ArgumentException(Resources.ExceptionCreatingSymmetricAlgorithmInstance, "algorithmType");

			this.algorithmType = algorithmType;

			this.key = key;

			this.instrumentationProvider = new SymmetricAlgorithmInstrumentationProvider();
		}

		/// <summary>
		/// <para>Encrypts a secret using the configured <c>SymmetricAlgorithm</c>.</para>
		/// </summary>
		/// <param name="plaintext"><para>The input to be encrypted. It is the responsibility of the caller to clear this
		/// byte array when finished.</para></param>
		/// <returns><para>The resulting cipher text.</para></returns>
		/// <seealso cref="ISymmetricCryptoProvider.Encrypt"/>
		public byte[] Encrypt(byte[] plaintext)
		{
			if (plaintext == null) throw new ArgumentNullException("plainText");
			if (plaintext.Length == 0) throw new ArgumentException(Resources.ExceptionByteArrayValueMustBeGreaterThanZeroBytes, "plaintext");

			byte[] output = null;

			try
			{
				using (SymmetricCryptographer crypto = new SymmetricCryptographer(algorithmType, key))
				{
					output = crypto.Encrypt(plaintext);
				}
			}
			catch (Exception e)
			{
				InstrumentationProvider.FireCyptographicOperationFailed(Resources.EncryptionFailed, e);
				throw;
			}
			InstrumentationProvider.FireSymmetricEncryptionPerformed();

			return output;
		}

		/// <summary>
		/// Decrypts a secret using the configured <c>SymmetricAlgorithm</c>.
		/// <seealso cref="ISymmetricCryptoProvider.Decrypt"/>
		/// </summary>
		/// <param name="ciphertext"><para>The cipher text to be decrypted.</para></param>
		/// <returns><para>The resulting plain text. It is the responsibility of the caller to clear the returned byte array
		/// when finished.</para></returns>
		/// <seealso cref="ISymmetricCryptoProvider.Decrypt"/>
		public byte[] Decrypt(byte[] ciphertext)
		{
			if (ciphertext == null) throw new ArgumentNullException("ciphertext");
			if (ciphertext.Length == 0) throw new ArgumentException(Resources.ExceptionByteArrayValueMustBeGreaterThanZeroBytes, "ciphertext");

			byte[] output = null;

			try
			{
				using (SymmetricCryptographer crypto = new SymmetricCryptographer(algorithmType, key))
				{
					output = crypto.Decrypt(ciphertext);
				}
			}
			catch (Exception e)
			{
				InstrumentationProvider.FireCyptographicOperationFailed(Resources.DecryptionFailed, e);
				throw;
			}
			InstrumentationProvider.FireSymmetricDecryptionPerformed();

			return output;
		}

		/// <summary>
		/// Gets the <see cref="SymmetricAlgorithmInstrumentationProvider"/> instance that defines the logical events 
		/// used to instrument this symmetric crypto provider.
		/// </summary>
		/// <returns>The <see cref="SymmetricAlgorithmInstrumentationProvider"/> instance that defines the logical 
		/// events used to instrument this symmetric crypto provider.</returns>
		public object GetInstrumentationEventProvider()
		{
			return instrumentationProvider;
		}

		/// <summary>
		/// Gets the <see cref="SymmetricAlgorithmInstrumentationProvider"/> instance that defines the logical events 
		/// used to instrument this symmetric crypto provider.
		/// </summary>
		protected SymmetricAlgorithmInstrumentationProvider InstrumentationProvider
		{
			get { return instrumentationProvider; }
		}
	}
}