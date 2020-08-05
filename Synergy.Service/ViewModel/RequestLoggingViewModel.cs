using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.Service.ViewModel
{
  public  class RequestLoggingViewModel
    {
        public string ClientId { get; set; }
        public string Payload { get; set; }
        public string Url { get; set; }
        public string RequestReferencId { get; set; }
        public string RequestUniqueRefernceId { get; set; }
        public string RequestMethod { get; set; }
    }
     public class ResponseLoggingViewModel
    {
        public string RequestUniqueRefernceId { get; set; }
        public string Payload { get; set; }
        public int StatusCode { get; set; }
        public string CadawadaStatusCode { get; set; }
    }
}
