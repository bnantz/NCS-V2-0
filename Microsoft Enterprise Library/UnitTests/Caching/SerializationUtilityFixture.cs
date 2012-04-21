//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif


namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
	[TestClass]
    public class SerializationUtilityFixture
    {
		[TestMethod]
        public void CanSerializeString()
        {
            Assert.AreEqual("this is a string", ToObject(ToBytes("this is a string")));
        }

		[TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void CanSerializeUnserializableObject()
        {
            ToBytes(new NonSerializableClass());
        }

		[TestMethod]
        public void CanSerializeSerializableObject()
        {
            SerializableClass serializedInstance = (SerializableClass)ToObject(ToBytes(new SerializableClass()));
            Assert.AreEqual(1, serializedInstance.Counter);
        }

		[TestMethod]
        public void CanSerializeMBRObject()
        {
            using (FileStream outputStream = new FileStream("test.out", FileMode.Create))
            {
                new BinaryFormatter().Serialize(outputStream, new MarshalByRefClass(13));
            }

            object deserializedObject = null;
            using (FileStream inputStream = new FileStream("test.out", FileMode.Open))
            {
                deserializedObject = new BinaryFormatter().Deserialize(inputStream);
            }

            File.Delete("test.out");

            MarshalByRefClass serializedInstance = (MarshalByRefClass)deserializedObject;
            Assert.AreEqual(13, serializedInstance.Counter);
        }

		[TestMethod]
        public void TryingToSerializeNullObjectReturnsNull()
        {
            Assert.IsNull(ToBytes(null));
        }

		[TestMethod]
        public void TryingToDeserializeNullArrayOfBytesReturnsNull()
        {
            Assert.IsNull(ToObject(null));
        }

		[TestMethod]
        public void CanSerializeAndDeserializeRefreshAction()
        {
            byte[] refreshBytes = ToBytes(new RefreshAction());
            object refreshObject = ToObject(refreshBytes);

            Assert.AreEqual(typeof(RefreshAction), refreshObject.GetType());
            Assert.IsTrue(refreshBytes.Length < 2048);
        }

        private byte[] ToBytes(object objectToSerialize)
        {
            return SerializationUtility.ToBytes(objectToSerialize);
        }

        private object ToObject(byte[] serializedObject)
        {
            return SerializationUtility.ToObject(serializedObject);
        }

        [Serializable]
        private class RefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason)
            {             
            }         
        }
    }

    [Serializable]
    internal class SerializableClass
    {
        public int Counter
        {
            get { return counter; }
        }

        private int counter = 1;
    }

    internal class NonSerializableClass
    {
    }

    [Serializable]
    internal class MarshalByRefClass : MarshalByRefObject
    {
        private int counter;

        public MarshalByRefClass(int counter)
        {
            this.counter = counter;
        }

        public int Counter
        {
            get { return counter; }
        }
    }
}

