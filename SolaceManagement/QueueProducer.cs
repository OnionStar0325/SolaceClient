#region Copyright & License
/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
#endregion

using System;
using System.Diagnostics;
using System.Text;
using log4net;
using SolaceSystems.Solclient.Messaging;
using SolaceSystems.Solclient.Messaging.SDT;
using static System.Collections.Specialized.BitVector32;

/// <summary>
/// Solace Systems Messaging API tutorial: QueueProducer
/// </summary>

namespace SolaceManagement
{
    /// <summary>
    /// Demonstrates how to use Solace Systems Messaging API for sending and receiving a guaranteed delivery message
    /// </summary>
    public class QueueProducer : IDisposable
    {
        static ILog logger = LogManager.GetLogger(typeof(QueueProducer));

        public event EventHandler<MessageEventArgs> OnMessageReceived;
        private EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);

        ConnectionInfo ConnInfo { get; set; }
        private ISession session;
        private ITransactedSession transactedSession;
        private IQueue queue;
        private IFlow flow;
        private IFlow replyFlow;
        private String repliedMessage;
        const int DefaultReconnectRetries = 3;

        public QueueProducer(String hostName, String vpnName, String queueName, String userName, String password)
        {
            ConnInfo = new ConnectionInfo();
            ConnInfo.HostName = hostName;
            ConnInfo.VPNName = vpnName;
            ConnInfo.QueueName = queueName;
            ConnInfo.UserName = userName;
            ConnInfo.Password = password;
        }

        public QueueProducer(ConnectionInfo connInfo)
        {
            ConnInfo = connInfo;
        }
        
        public void Run(IContext context)
        {
            // Validate parameters
            if (context == null)
            {
                throw new ArgumentException("Solace Systems API context Router must be not null.", "context");
            }
            if (string.IsNullOrWhiteSpace(ConnInfo.HostName))
            {
                throw new ArgumentException("Solace Messaging Router host name must be non-empty.", "host");
            }
            if (string.IsNullOrWhiteSpace(ConnInfo.VPNName))
            {
                throw new InvalidOperationException("VPN name must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(ConnInfo.UserName))
            {
                throw new InvalidOperationException("Client username must be non-empty.");
            }

            // Create session properties
            SessionProperties sessionProps = new SessionProperties()
            {
                Host = ConnInfo.HostName,
                VPNName = ConnInfo.VPNName,
                UserName = ConnInfo.UserName,
                Password = ConnInfo.Password,
                ReconnectRetries = DefaultReconnectRetries
            };

            // Connect to the Solace messaging router
            logger.Info($"Connecting as {ConnInfo.UserName}@{ConnInfo.VPNName} on {ConnInfo.HostName}...");
            session = context.CreateSession(sessionProps, null, null);

            ReturnCode returnCode = session.Connect();
            if (returnCode == ReturnCode.SOLCLIENT_OK)
            {
                logger.Info("Session successfully connected.");
            }
            else
            {
                logger.Info($"Error connecting, return code: {returnCode}");
            }
        }

        public string SendMessage(String content, String replyQueueName)
        {
            return SendMessage(ConnInfo.QueueName, content, replyQueueName);    
        }
        
        public string SendMessage(String queueName, String content, String replyQueueName)
        {
            string result = string.Empty;

            logger.Info($"Attempting to provision the queue '{queueName}'...");

            // Create the message
            using (IMessage message = ContextFactory.Instance.CreateMessage())
            {
                // Message's destination is the queue and the message is persistent
                message.Destination = ContextFactory.Instance.CreateQueue(queueName);
                message.DeliveryMode = MessageDeliveryMode.Persistent;
                // Create the message content as a binary attachment
                //message.BinaryAttachment = Encoding.ASCII.GetBytes(content);
                var stream = SDTUtils.CreateStream(message, 1024);
                stream.AddByteArray(Encoding.UTF8.GetBytes(content));
                // Send the message to the queue on the Solace messaging router
                logger.Info($"Sending message to queue {queueName}...");
                ReturnCode returnCode;
                IQueue replyQueue = null;

                if(string.IsNullOrEmpty(replyQueueName) == false)
                {
                    replyQueue = ContextFactory.Instance.CreateQueue(replyQueueName);
                    message.CorrelationId = Guid.NewGuid().ToString();

                    replyFlow = session.CreateFlow(new FlowProperties()
                    {
                        AckMode = MessageAckMode.ClientAck,
                        Selector = "CorrelationId='" + message.CorrelationId + "'"
                    },
                    replyQueue, null, HandleMessageEvent, HandleFlowEvent);
                    replyFlow.Start();

                }

                returnCode = session.Send(message);

                try
                {
                    if (replyFlow != null)
                    {
                        result = "No Reply";
                        WaitEventWaitHandle.WaitOne(5000);
                        if(string.IsNullOrEmpty(repliedMessage) == false)
                        {
                            result = repliedMessage;
                        }
                    }
                    else
                        {
                            if (returnCode == ReturnCode.SOLCLIENT_OK)
                            {
                                // Delivery not yet confirmed. See ConfirmedPublish.cs
                                result = "Done.";
                                logger.Info(result);
                            }
                            else
                            {
                                result = "$\"Sending failed, return code: {returnCode}\"";
                                logger.Info(result);
                            }
                        }
                }
                finally
                {
                    stream.Close();

                    if(replyFlow != null)
                    {
                        replyFlow.Dispose();
                    }

                    if(replyQueue != null)
                    {
                        replyQueue.Dispose();
                    }
                }
                return result;
            }
        }

        public string RequestMessage(string content)
        {
            return RequestMessage(ConnInfo.QueueName, content);
        }

        public string RequestMessage(String sendQueueName, String content)
        {
            using (IMessage requestMsg = ContextFactory.Instance.CreateMessage())
            {
                requestMsg.Destination = ContextFactory.Instance.CreateTopic(sendQueueName);
                var stream = SDTUtils.CreateStream(requestMsg, 1024);
                stream.AddByteArray(Encoding.UTF8.GetBytes(content));

                IMessage replyMsg = null;
                ReturnCode returnCode = session.SendRequest(requestMsg, out replyMsg, 5000); // 5000ms timeout

                if (returnCode == ReturnCode.SOLCLIENT_OK && replyMsg != null)
                {
                    logger.Info("Request sent successfully.");
                    return Encoding.ASCII.GetString(replyMsg.BinaryAttachment);
                }
                else
                {
                    logger.Info($"Request failed or no reply received, return code: {returnCode}");
                    return $"Request failed or no reply received, return code: {returnCode}";
                }
            }
        }

        private void HandleMessageEvent(object source, MessageEventArgs args)
        {
            // Received a message
            logger.Info("Received message.");

            using (IMessage message = args.Message)
            {
                if (OnMessageReceived != null)
                {
                    OnMessageReceived(this, args);
                }
                // Expecting the message content as a binary attachment
                repliedMessage = SolaceMessageUtil.GetContent(message);
                // logger.Info("{0} - Message content: {1}",ConnInfo.QueueName, msgValue);
                // ACK the message
                replyFlow.Ack(message.ADMessageId);
                // finish the program
                WaitEventWaitHandle.Set();
            }
        }

        public void HandleFlowEvent(object sender, FlowEventArgs args)
        {
            // Received a flow event
            logger.Info($"Received Flow Event '{args.Event}' Type: '{args.ResponseCode.ToString()}' Text: '{args.Info}'");
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (session != null)
                    {
                        session.Dispose();
                        session = null;
                        logger.Info($"Session closed from {ConnInfo.UserName}@{ConnInfo.VPNName} on {ConnInfo.HostName}");
                    }
                    if( transactedSession != null)
                    {
                        transactedSession.Dispose();
                        transactedSession = null;
                        logger.Info($"TrasnsactedSession closed from {ConnInfo.UserName}@{ConnInfo.VPNName} on {ConnInfo.HostName}");
                    }
                    if (queue != null)
                    {
                        queue.Dispose();
                        queue = null;
                        logger.Info($"Queue closed from {ConnInfo.QueueName}");
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

}
