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
using SolaceSystems.Solclient.Messaging;
using System.Threading;
using log4net;

/// <summary>
/// Solace Systems Messaging API tutorial: QueueConsumer
/// </summary>

namespace SolaceManagement
{
    /// <summary>
    /// Demonstrates how to use Solace Systems Messaging API for sending and receiving a guaranteed delivery message
    /// </summary>
    public class QueueConsumer : IDisposable
    {
        static ILog logger = LogManager.GetLogger(typeof(QueueConsumer));

        public event EventHandler<MessageEventArgs> OnMessageReceived;

        public ConnectionInfo ConnInfo { get; set; }
        const int DefaultReconnectRetries = 3;

        private ISession Session = null;
        private IQueue Queue = null;
        private IFlow Flow = null;
        private IContext context = null;
        private EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);

        public QueueConsumer(String hostName, String vpnName, String queueName, String userName, String password)
        {
            ConnInfo = new ConnectionInfo();
            ConnInfo.HostName = hostName;
            ConnInfo.VPNName = vpnName;
            ConnInfo.QueueName = queueName;
            ConnInfo.UserName = userName;
            ConnInfo.Password = password;
        }

        public QueueConsumer(ConnectionInfo connInfo)
        {
            ConnInfo = connInfo;
        }

        public void Run(IContext context)
        {
            this.context = context;
            // Validate parameterss
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
            if (string.IsNullOrWhiteSpace(ConnInfo.QueueName))
            {
                throw new InvalidOperationException("Solace QueueName must be non-empty.");
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
            Session = context.CreateSession(sessionProps, null, null);
            ReturnCode returnCode = Session.Connect();
            if (returnCode == ReturnCode.SOLCLIENT_OK)
            {
                logger.Info("Session successfully connected.");
                
                // Create the queue
                Queue = ContextFactory.Instance.CreateQueue(ConnInfo.QueueName);

                // Create and start flow to the newly provisioned queue
                // NOTICE HandleMessageEvent as the message event handler 
                // and HandleFlowEvent as the flow event handler
                Flow = Session.CreateFlow(new FlowProperties()
                {
                    AckMode = MessageAckMode.ClientAck
                },
                Queue, null, HandleMessageEvent, HandleFlowEvent);
                Flow.Start();
                logger.Info($"Waiting for a message in the queue '{ConnInfo.QueueName}'...");
                //WaitEventWaitHandle.WaitOne();
            }
            else
            {
                logger.Info($"Error connecting, return code: {returnCode}");
            }
        }

        /// <summary>
        /// This event handler is invoked by Solace Systems Messaging API when a message arrives
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private void HandleMessageEvent(object source, MessageEventArgs args)
        {
            // Received a message
            logger.Info("Received message.");
            String msgValue;
            using (IMessage message = args.Message)
            {
                if (OnMessageReceived != null)
                {
                    OnMessageReceived(this, args);
                }
                // Expecting the message content as a binary attachment
                msgValue = Encoding.ASCII.GetString(message.BinaryAttachment);
                // logger.Info("{0} - Message content: {1}",ConnInfo.QueueName, msgValue);
                // ACK the message
                Flow.Ack(message.ADMessageId);
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
                    if (Session != null)
                    {
                        Session.Dispose();
                        Session = null;
                        logger.Info($"Session closed from {ConnInfo.UserName}@{ConnInfo.VPNName} on {ConnInfo.HostName}");
                    }
                    if (Queue != null)
                    {
                        Queue.Dispose();
                        Queue = null;
                        logger.Info($"Queue closed from {ConnInfo.QueueName}");
                    }
                    if (Flow != null)
                    {
                        Flow.Dispose();
                        Flow = null;
                        logger.Info($"Flow closed from {ConnInfo.QueueName}");
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
