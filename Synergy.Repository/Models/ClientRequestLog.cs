using System;

namespace Synergy.Repository.Models
{
    public class ClientRequestLog
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string Payload { get; set; }
        public string Url { get; set; }
        public DateTime DateLogged { get; set; }
        public string RequestReferencId { get; set; }
        public string RequestUniqueRefernceId { get; set; }
        public string RequestMethod { get; set; }
    }
}
