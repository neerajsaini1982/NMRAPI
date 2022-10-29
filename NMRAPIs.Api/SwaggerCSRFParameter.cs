// <copyright file="SwaggerCSRFParameter.cs" company="DSG">
// This source code is owned by DSG and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from DSG. www.vplaybook.com.
// </copyright>

namespace NMRAPIs.Api
{
    using System.Collections.Generic;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// SwaggerCSRFParameter.
    /// </summary>
    public class SwaggerCSRFParameter : IOperationFilter
    {
        //public void Apply(Operation operation, OperationFilterContext context)
        //{
        //    if (operation.Parameters == null)
        //    {
        //        operation.Parameters = new List<IParameter>();
        //    }

        //    operation.Parameters.Add(new NonBodyParameter
        //    {
        //        Name = "X-XSRF-Token",
        //        In = "header",
        //        Type = "string",
        //        Required = false
        //    });
        //}

        /// <summary>
        /// Adds CSRF parameter to the request.
        /// </summary>
        /// <param name="operation">operation.</param>
        /// <param name="context">context.</param>
        // Core 5.0 Change
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "X-XSRF-Token",
                Description = "Access Token",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = "string" },
                Required = false
            });
        }
    }
}
