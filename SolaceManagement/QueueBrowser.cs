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
    public class QueueBrowser : IDisposable
    {
        static ILog logger = LogManager.GetLogger(typeof(QueueBrowser));

        public ConnectionInfo ConnInfo { get; set; }
        const int DefaultReconnectRetries = 3;

        private ISession Session = null;
        private IQueue Queue = null;
        private IBrowser Browser = null;
        private IContext context = null;

        public event EventHandler<BrowserEventArgs> OnMessageReceived;

        public QueueBrowser(String hostName, String vpnName, String queueName, String userName, String password)
        {
            ConnInfo = new ConnectionInfo();
            ConnInfo.HostName = hostName;
            ConnInfo.VPNName = vpnName;
            ConnInfo.QueueName = queueName;
            ConnInfo.UserName = userName;
            ConnInfo.Password = password;
        }

        public QueueBrowser(ConnectionInfo connInfo)
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
                Browser = Session.CreateBrowser(Queue, new BrowserProperties() {WaitTimeout = 5000, TransportWindowSize = 255 });
                
                logger.Info($"Waiting for a message in the queue '{ConnInfo.QueueName}'...");

                new Thread(new ThreadStart(GetNext)).Start();
            }
            else
            {
                logger.Info($"Error connecting, return code: {returnCode}");
            }
        }

        public void GetNext()
        {
            IMessage message = null;
            while (Browser != null)
            {
                message = Browser.GetNext(1000);
                if (OnMessageReceived != null && message != null)
                {
                    var args = new BrowserEventArgs(message);
                    OnMessageReceived(this, args);
                }
            }
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
                    if (Browser != null)
                    {
                        Browser.Dispose();
                        Browser = null;
                        logger.Info($"Browser closed from {ConnInfo.QueueName}");
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
