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

using System;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class MsmqTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInMsmqTraceListenerNodeThrows()
        {
            new MsmqTraceListenerNode(null);
        }

        [TestMethod]
        public void MsmqTraceListenerNodeDefaults()
        {
            MsmqTraceListenerNode msmqListener = new MsmqTraceListenerNode();

            Assert.AreEqual("Msmq TraceListener", msmqListener.Name);
            Assert.AreEqual(DefaultValues.MsmqQueuePath, msmqListener.QueuePath);
            Assert.AreEqual(MessageQueueTransactionType.None, msmqListener.TransactionType);
            Assert.AreEqual(false, msmqListener.UseAuthentication);
            Assert.AreEqual(false, msmqListener.UseDeadLetterQueue);
            Assert.AreEqual(false, msmqListener.UseEncryption);
            Assert.AreEqual(false, msmqListener.Recoverable);
            Assert.AreEqual(Message.InfiniteTimeout, msmqListener.TimeToBeReceived);
            Assert.AreEqual(Message.InfiniteTimeout, msmqListener.TimeToReachQueue);
            Assert.AreEqual(MessagePriority.Normal, msmqListener.MessagePriority);
        }

        [TestMethod]
        public void MsmqTraceListenerNodeTest()
        {
            string name = "some name";
            string messageQueuePath = "some mq path";
            bool useDeadLetterQueue = true;
            bool useAuthentication = true;
            bool useEncryption = true;
            bool recoverable = false;
            TimeSpan timeToBeReceived = new TimeSpan(123);
            TimeSpan timeToReachQueue = new TimeSpan(123);
            MessagePriority messagePriority = MessagePriority.VeryHigh;
            MessageQueueTransactionType transactionType = MessageQueueTransactionType.Automatic;

            MsmqTraceListenerNode msmqTraceListenerNode = new MsmqTraceListenerNode();
            msmqTraceListenerNode.Name = name;
            msmqTraceListenerNode.QueuePath = messageQueuePath;
            msmqTraceListenerNode.MessagePriority = messagePriority;
            msmqTraceListenerNode.TransactionType = transactionType;
            msmqTraceListenerNode.UseEncryption = useEncryption;
            msmqTraceListenerNode.UseAuthentication = useAuthentication;
            msmqTraceListenerNode.UseDeadLetterQueue = useDeadLetterQueue;
            msmqTraceListenerNode.TimeToReachQueue = timeToReachQueue;
            msmqTraceListenerNode.TimeToBeReceived = timeToBeReceived;
            msmqTraceListenerNode.Recoverable = recoverable;

            ApplicationNode.AddNode(msmqTraceListenerNode);

            MsmqTraceListenerData nodeData = (MsmqTraceListenerData)msmqTraceListenerNode.TraceListenerData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(messageQueuePath, nodeData.QueuePath);
            Assert.AreEqual(transactionType, nodeData.TransactionType);
            Assert.AreEqual(messagePriority, nodeData.MessagePriority);
            Assert.AreEqual(useDeadLetterQueue, nodeData.UseDeadLetterQueue);
            Assert.AreEqual(useAuthentication, nodeData.UseAuthentication);
            Assert.AreEqual(useEncryption, nodeData.UseEncryption);
            Assert.AreEqual(timeToBeReceived, nodeData.TimeToBeReceived);
            Assert.AreEqual(timeToReachQueue, nodeData.TimeToReachQueue);
            Assert.AreEqual(recoverable, nodeData.Recoverable);
            
        }

        [TestMethod]
        public void MsmqTraceListenerNodeDataTest()
        {
            string name = "some name";
            string messageQueuePath = "some mq path";
            bool useDeadLetterQueue = true;
            bool useAuthentication = true;
            bool useEncryption = true;
            bool recoverable = false;
            TimeSpan timeToBeReceived = new TimeSpan(123);
            TimeSpan timeToReachQueue = new TimeSpan(123);
            MessagePriority messagePriority = MessagePriority.VeryHigh;
            MessageQueueTransactionType transactionType = MessageQueueTransactionType.Automatic;

            MsmqTraceListenerData msmqTraceListenerData = new MsmqTraceListenerData();
            msmqTraceListenerData.Name = name;
            msmqTraceListenerData.QueuePath = messageQueuePath;
            msmqTraceListenerData.MessagePriority = messagePriority;
            msmqTraceListenerData.TransactionType = transactionType;
            msmqTraceListenerData.UseEncryption = useEncryption;
            msmqTraceListenerData.UseAuthentication = useAuthentication;
            msmqTraceListenerData.UseDeadLetterQueue = useDeadLetterQueue;
            msmqTraceListenerData.TimeToReachQueue = timeToReachQueue;
            msmqTraceListenerData.TimeToBeReceived = timeToBeReceived;
            msmqTraceListenerData.Recoverable = recoverable;

            MsmqTraceListenerNode msmqTraceListenerNode = new MsmqTraceListenerNode(msmqTraceListenerData);
            ApplicationNode.AddNode(msmqTraceListenerNode);

            Assert.AreEqual(name, msmqTraceListenerNode.Name);
            Assert.AreEqual(messageQueuePath, msmqTraceListenerNode.QueuePath);
            Assert.AreEqual(transactionType, msmqTraceListenerNode.TransactionType);
            Assert.AreEqual(messagePriority, msmqTraceListenerNode.MessagePriority);
            Assert.AreEqual(useDeadLetterQueue, msmqTraceListenerNode.UseDeadLetterQueue);
            Assert.AreEqual(useAuthentication, msmqTraceListenerNode.UseAuthentication);
            Assert.AreEqual(useEncryption, msmqTraceListenerNode.UseEncryption);
            Assert.AreEqual(timeToBeReceived, msmqTraceListenerNode.TimeToBeReceived);
            Assert.AreEqual(timeToReachQueue, msmqTraceListenerNode.TimeToReachQueue);
            Assert.AreEqual(recoverable, msmqTraceListenerNode.Recoverable);
        }
    }
}
