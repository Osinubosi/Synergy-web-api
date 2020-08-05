using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Synergy_web_api.WebHandler
{
    public class HeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null && !descriptor.ControllerName.StartsWith("Weather"))
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "X-Auth-Token",
                    In = ParameterLocation.Header,
                    Description = "Access Token",
                    Schema = new OpenApiSchema() { Type = "Bearer" },
                    Required = false,
                });

                //operation.Parameters.Add(new OpenApiParameter()
                //{
                //    Name = "nonce",
                //    In = ParameterLocation.Query,
                //    Description = "The random value",
                //    Required = true
                //});

                //operation.Parameters.Add(new OpenApiParameter()
                //{
                //    Name = "sign",
                //    In = ParameterLocation.Query,
                //    Description = "The signature",
                //    Required = true
                //});
            }

        }
    }
}

