using RestSharp;
using RestSharp.Authenticators;
using SolaceRestClient.Json;
using System.Text.Json;

namespace SolaceRestClient
{
    public class SmtpRestClient
    {
        readonly RestClient _client;

        public SmtpRestClient(string baseUri, string userName, string password) {
            var options = new RestClientOptions(baseUri)
            {
                Authenticator = new HttpBasicAuthenticator(userName, password)
            };
            _client = new RestClient(options);
        }

        public Queues GetQueueList(string vpnName)
        {
            vpnName = vpnName.Replace("/", "%2F");
            var request = new RestRequest("monitor/msgVpns/" + vpnName + "/queues?count=1000&select=queueName,accessType,permission");
            return _client.Get<Queues>(request);
        }

        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
