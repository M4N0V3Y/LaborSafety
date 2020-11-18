using Microsoft.Owin;
using Owin;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace EntregaEColeta.API.App_Start
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class OwinExceptionHandlerMiddleware
    {
        private readonly AppFunc _next;

        public OwinExceptionHandlerMiddleware(AppFunc next)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            try
            {
                await _next(environment);
            }
            catch (Exception ex)
            {
                try
                {

                    var owinContext = new OwinContext(environment);

                    HandleException(owinContext.Request, ex);

                    return;
                }
                catch (Exception)
                {
                    // If there's a Exception while generating the error page, re-throw the original exception.
                }
                throw;
            }
        }

        private async Task HandleException(dynamic request, Exception ex)
        {
            var pathLog = $"{AppDomain.CurrentDomain.BaseDirectory}Logs";
            var logNameTodayFull = $@"{pathLog}\logEntregasEColetas{DateTime.Today.ToString("yyyyMMdd")}.log";

            if (!Directory.Exists(pathLog))
                Directory.CreateDirectory(pathLog);

            if (File.Exists(logNameTodayFull))
                File.Create(logNameTodayFull);

            Log.Logger = new LoggerConfiguration()
                            .Enrich.WithEnvironmentUserName()
                            .Enrich.WithMachineName()
                            .WriteTo.File(logNameTodayFull,
                                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                            .CreateLogger();
            Log.Logger.Error(ex, ex.Message);
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //context.Response.ReasonPhrase = "Internal Server Error";
            //context.Response.ContentType = "application/json";
            //context.Response.Write(JsonConvert.SerializeObject(errorDataModel));
        }

    }

    public static class OwinExceptionHandlerMiddlewareAppBuilderExtensions
    {
        public static void UseOwinExceptionHandler(this IAppBuilder app)
        {
            app.Use<OwinExceptionHandlerMiddleware>();
        }
    }

    public class ContentNegotiatedExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var errorDataModel = new
            {
                StatusCode = 500,
                errorMessage = "Erro interno.",
                errorDetails = "Erro interno.",
            };

            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, errorDataModel);
            context.Result = new ResponseMessageResult(response);
        }
    }
}