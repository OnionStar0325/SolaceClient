using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaceRestClient.Json
{
    public class Collection
    {
    }

    public class Datum
    {
        public string queueName { get; set; }
        public string accessType { get; set; }
        public string permission {  get; set; }
    }

    public class Link
    {
    }

    public class Meta
    {
        public int count { get; set; }
        public Request request { get; set; }
        public int responseCode { get; set; }
    }

    public class Request
    {
        public string method { get; set; }
        public string uri { get; set; }
    }

    public class Queues
    {
        public List<Datum> data { get; set; }
        public List<Collection> collections { get; set; }
        public List<Link> links { get; set; }
        public Meta meta { get; set; }
    }


}
