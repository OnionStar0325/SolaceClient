using SolaceSystems.Solclient.Messaging;
using SolaceSystems.Solclient.Messaging.SDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaceManagement
{
    public class SolaceMessageUtil
    {
        public static String GetContent(IMessage message)
        {
            var container = SDTUtils.GetContainer(message);
            if (container is null)
            {
                return Encoding.ASCII.GetString(message.BinaryAttachment);
            }

            if (container is IStreamContainer)
            {
                var streamContainer = container as IStreamContainer;
                StringBuilder sb = new StringBuilder();
                while (streamContainer.HasNext())
                {
                    var bytes = streamContainer.GetByteArray();
                    sb.Append(Encoding.UTF8.GetString(bytes));
                }

                return sb.ToString();
            }

            if (container is IMapContainer)
            {
                var mapContainer = container as IMapContainer;
                StringBuilder sb = new StringBuilder();
                while (mapContainer.HasNext())
                {
                    var map = mapContainer.GetNext();
                    sb.Append(Convert.ToString(map.Value));
                }

                return sb.ToString();
            }

            return Encoding.ASCII.GetString(message.BinaryAttachment);
        }
    }
}
