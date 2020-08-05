using Synergy.Service.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.Service.ApiResponse
{
  public  class ErrorResponse<T> where T : class
    {
        public T Data { get; set; }
       // public ResponseStatus Status { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}
