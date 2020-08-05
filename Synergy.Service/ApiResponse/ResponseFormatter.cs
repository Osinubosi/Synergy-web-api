using Synergy.Service.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.Service.ApiResponse
{
  public class Response<T> where T : class
    {
        public SuccessResponse<T> SuccessData { get; set; }
        public ResponseStatus Status { get; set; }
        public ErrorResponse<T> ErrorData { get; set; }
       
    }
}
