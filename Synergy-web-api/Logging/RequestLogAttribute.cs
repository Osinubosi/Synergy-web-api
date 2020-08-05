
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Synergy.Service.Interfaces;
using Synergy.Service.ViewModel;
using System;
using Synergy.Service.Implementations;

namespace Synergy_web_api.Logging
{
    public class RequestLogAttribute : ActionFilterAttribute
    {
        private string modelArgument;
        private static Logger log = LogManager.GetCurrentClassLogger();
        private ILoggingService loggingService;
        private string _refernceId;
        public RequestLogAttribute(string parameterName)
        {
            modelArgument = parameterName;
            loggingService = new LoggingService();

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string payload = "";
            try
            {
                string httpMethod = context.HttpContext.Request.Method;
                string url = context.HttpContext.Request.Path;
                _refernceId = DateTime.UtcNow.Ticks.ToString();

                if (context.ActionArguments.Count > 0)
                {
                    var arguement = context.ActionArguments[modelArgument];
                    if (arguement != null)
                        payload = JsonConvert.SerializeObject(arguement);

                    loggingService.LogRequestData(new RequestLoggingViewModel
                    {
                        ClientId = context.HttpContext.Request.Headers["ClientId"].ToString(),
                        Payload = payload,
                        RequestMethod = httpMethod,
                        RequestReferencId = "",
                        RequestUniqueRefernceId = _refernceId,
                        Url = url

                    });
                }
            }
            catch (Exception ex)
            {

                log.Error(ex.Message, "RequestLogAttribute OnActionExecuting");
            }
            base.OnActionExecuting(context);
        }

        public  override void OnActionExecuted(ActionExecutedContext context)
        {
            string payload = "";
            string cadawadaStatusCode = "";
            try
            {
                if (context.Exception != null)
                    // ReSharper disable once RedundantAssignment
                    payload = context.Exception.ToString();
                else
                {
                    var result = context.Result;
                    if (result != null)
                    {

                        if (result is ObjectResult actionResult)
                        {

                            payload = JsonConvert.SerializeObject(actionResult.Value);
                            var res = JObject.Parse(payload);
                            cadawadaStatusCode = res["ResponseCode"]?.ToString();
                            //cadawadaStatusCode = (int)payload["ResponseCode"].ToString();
                        }
                    }

                    loggingService.LogResponseData(new ResponseLoggingViewModel
                    {
                        Payload = payload,
                        RequestUniqueRefernceId = _refernceId,
                        StatusCode = context.HttpContext.Response.StatusCode,
                        CadawadaStatusCode = cadawadaStatusCode,

                    });
                }

            }
            catch (Exception ex)
            {

                log.Error(ex.Message, "RequestLogAttribute OnActionExecuting");

            }

            base.OnActionExecuted(context);
        }

    }
}
