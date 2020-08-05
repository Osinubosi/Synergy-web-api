using System;

namespace Synergy.Repository.Models
{
    public  class ClientResponseLog
    {
        public int ResponseId { get; set; }
        public string RequestUniqueRefernceId { get; set; }
        public string Payload { get; set; }
        public DateTime DateLogged { get; set; }
        public int HttpStatusCode { get; set; }
        public string SynergyStatusCode { get; set; }
    }
}
