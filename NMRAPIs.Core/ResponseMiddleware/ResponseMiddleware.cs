using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NMRAPIs.Core.Wrappers;

namespace NMRAPIs.Core.ResponseMiddleware
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate next;

        private readonly bool traceResponse;

        public ResponseMiddleware(RequestDelegate next, bool traceResponse = false)
        {
            this.next = next;
            this.traceResponse = traceResponse;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Ignore())
            {
                await this.next(context);
            }
            else
            {
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    try
                    {
                        await this.next.Invoke(context);

                        if (!context.IsDownload())
                        {
                            if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                            {
                                var body = await FormatResponse(context.Response);
                                await this.HandleSuccessRequestAsync(context, body, (HttpStatusCode)context.Response.StatusCode);
                            }
                            else
                            {
                                await HandleNotSuccessRequestAsync(context, (HttpStatusCode)context.Response.StatusCode);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await this.HandleExceptionAsync(context, ex);
                    }
                    finally
                    {
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                    }
                }
            }
        }

        private static async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return plainBodyText;
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ApiError apiError;
            int code;

            switch (exception)
            {
                case ApiException ex:

                    apiError = new ApiError(ex.Message)
                    {
                        ValidationErrors = ex.Errors
                    };

                    code = ex.StatusCode;
                    context.Response.StatusCode = code;

                    break;
                case UnauthorizedAccessException _:
                    apiError = new ApiError("Unauthorized Access");
                    code = (int)HttpStatusCode.Unauthorized;
                    context.Response.StatusCode = code;

                    break;
                default:
                    var msg = exception.GetBaseException().Message;
                    //var stack = exception.StackTrace;

                    // Pentest: #PT9397_8 remove detailed stack as it will reveal the file system
                    apiError = new ApiError(msg);
                    //{ Details = stack };
                    code = (int)HttpStatusCode.InternalServerError;
                    context.Response.StatusCode = code;

                    break;
            }

            context.Response.ContentType = "application/json";

            var response = new ApiResponse<string>(code, ResponseMessageEnum.Exception.GetDescription(), null, apiError);

            var json = JsonConvert.SerializeObject(response);

            //var ai = new TelemetryClient();
            //ai.TrackException(exception, apiError.ValidationErrors?.ToDictionary(x => x.Field, y => y.Message));

            // Core 3.1 Change
            context.Response.ContentLength = null;
            return context.Response.WriteAsync(json);
        }

        private static Task HandleNotSuccessRequestAsync(HttpContext context, HttpStatusCode code)
        {
            if (context.IsNoContent())
            {
                return Task.CompletedTask;
            }

            context.Response.ContentType = "application/json";

            var apiError = new ApiError("Your request cannot be processed. Something wrong. Please contact support person.");

            var response = new ApiResponse<string>((int)code, ResponseMessageEnum.Failure.GetDescription(), null, apiError);
            context.Response.StatusCode = (int)code;

            var json = JsonConvert.SerializeObject(response);

            // Core 3.1 Change
            context.Response.ContentLength = null;
            return context.Response.WriteAsync(json);
        }

        private Task HandleSuccessRequestAsync(HttpContext context, object body, HttpStatusCode code)
        {
            ApiResponse<object> response;

            context.Response.ContentType = "application/json";

            ApiResponse<object> convertedBody = null;

            try
            {
                convertedBody = bool.TryParse(body.ToString(), out _) ? null : JsonConvert.DeserializeObject<ApiResponse<object>>(body.ToString());
            }
            catch (Exception ex)
            {
                // ignored
            }

            if (convertedBody?.Result == null && (convertedBody?.StatusCode == null || convertedBody.StatusCode == 0))
            {
                var bodyText = !body.ToString().IsValidJson() ? JsonConvert.SerializeObject(body) : body.ToString();

                var bodyContent = JsonConvert.DeserializeObject<object>(bodyText);

                var type = bodyContent?.GetType();

                if (type == typeof(JObject))
                {
                    response = JsonConvert.DeserializeObject<ApiResponse<object>>(bodyText);

                    response = response.StatusCode != (int)code
                        ? new ApiResponse<object>((int)code, ((ResponseMessageEnum)response.StatusCode).GetDescription(), bodyContent)
                        : new ApiResponse<object>((int)code, ResponseMessageEnum.Success.GetDescription(), bodyContent);
                }
                else
                {
                    response = new ApiResponse<object>((int)code, ResponseMessageEnum.Success.GetDescription(), bodyContent);
                }
            }
            else
            {
                response = convertedBody;
            }

            var jsonString = JsonConvert.SerializeObject(response);

            if (!this.traceResponse)
            {
                // Core 3.1 Change
                context.Response.ContentLength = null;
                return context.Response.WriteAsync(jsonString);
            }

            //var ai = new TelemetryClient();
            //ai.TrackTrace($"Response: {jsonString}");

            // Core 3.1 Change
            context.Response.ContentLength = null;
            return context.Response.WriteAsync(jsonString);
        }
    }
}
