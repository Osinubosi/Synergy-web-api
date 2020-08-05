using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using NLog;
using Synergy.Service.ApiResponse;
using System.Collections.Generic;
using System.Net;


namespace Synergy_web_api.WebHandler
{
    public class RequestResponseFormatter
    {
        public static Logger Log = LogManager.GetCurrentClassLogger();

        public static ErrorResponse<List<BadRequestErrorResponse>> BadRequestResponse(ModelStateDictionary modelState, string errorTitle, string errorMessageNode, string rootPath)
        {
            var error = new List<BadRequestErrorResponse>();
            string message = "";
            foreach (var item in modelState)
            {
                var key = item.Key;
                foreach (var errors in item.Value.Errors)
                {
                    message += $"{errors.ErrorMessage} ,";
                   
                }

                error.Add(new BadRequestErrorResponse
                {
                    ErrorKey = key,
                    ErrorMessage = message.Remove(message.Length - 1)
                });

                message = string.Empty;
            }
               

            var serializedRequest = JsonConvert.SerializeObject(error);
            Log.Error($"{errorTitle}: {serializedRequest}");
           
            return new ErrorResponse<List<BadRequestErrorResponse>>
            {
                Data = error,
                ResponseCode = ((int) HttpStatusCode.BadRequest).ToString(),
                ResponseMessage = "Invalid Payload, kindly verify your payload",
               // Status = Synergy.Service.Enums.ResponseStatus.BadRequest
                
            };

            


        }

        //public static ErrorResponse UnAuthorizeRequestResponse(string clientId, string clientKey)
        //{
        //    //var serializedRequest = JsonConvert.SerializeObject(modelState);
        //    log.Error($"Client Credentials details {clientId}{clientKey}");

        //    return new ErrorResponse { ResponseCode = ErrorCode.UNAUTHORIZED_CLIENT_CREDENTIALS, ResponseMessage = "Client does not exist!" };//ErrorMessageFormatter.GetErrorMessage(errorMessageNode, rootPath) };
        //}

    }
}
