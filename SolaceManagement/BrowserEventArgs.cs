using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaceManagement
{
    public class BrowserEventArgs : EventArgs
    {
        public IMessage Message { get; private set; }
        public BrowserEventArgs(IMessage message)
        {
            this.Message = message;
        }
    }
}
