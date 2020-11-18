using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using Serilog;
using LaborSafety.Models.Respostas;

namespace LaborSafety.App_Start
{
    public class AprExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public bool HabilitaGravaLog
        {
            get
            {
                try
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["HabilitaGravaLog"] != null)
                    {
                        return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["HabilitaGravaLog"]);
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }
        public override void OnException(HttpActionExecutedContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            if (context.Exception is UnauthorizedAccessException)
                statusCode = HttpStatusCode.Unauthorized;

            HandleException(context);
            VerifyLogExpire();
        }

        private async Task HandleException(HttpActionExecutedContext context)
        {
            try
            {
                var ex = context.Exception;
                if (HabilitaGravaLog)
                {
                    try
                    {
                        var pathLog = $"{AppDomain.CurrentDomain.BaseDirectory}Logs";
                        var logNameFull = $@"{pathLog}\logLaborSafety.log";

                        if (!Directory.Exists(pathLog))
                            Directory.CreateDirectory(pathLog);

                        var code = Guid.NewGuid().ToString();
                        Log.Logger = new LoggerConfiguration()
                                        .Enrich.WithEnvironmentUserName()
                                        .Enrich.WithMachineName()
                                        .Enrich.WithThreadId()
                                        .WriteTo.File(logNameFull, rollingInterval: RollingInterval.Day)
                                        .CreateLogger();
                        Log.Logger.Error(ex.InnerException, $"Error Code: {code}");
                        Log.CloseAndFlush();
                    }
                    catch { }
                }

                //Chaveia o tipo de exceção para controle na INTEGRAÇÃO
                if (context.Request.RequestUri.ToString().Contains("Integracao"))
                {
                    var messageResponse = JsonConvert.DeserializeObject<ResponseIntegracao>(ex.Message);
                    context.Response = new HttpResponseMessage();
                    context.Response.Content = new StringContent(ex.Message);
                }
                else
                {
                    var messageResponse = JsonConvert.DeserializeObject<ResponseError>(ex.Message);
                    context.Response = new HttpResponseMessage((HttpStatusCode)messageResponse.StatusCode);
                    context.Response.Content = new StringContent(ex.Message);
                }
            }
            catch { }
        }

        private async Task VerifyLogExpire()
        {
            var pathLog = $"{AppDomain.CurrentDomain.BaseDirectory}Logs";

            var dateMouth = DateTime.Today.AddMonths(-1);
            var filesDel = Directory.GetFiles(pathLog, "*", SearchOption.TopDirectoryOnly);

            if (filesDel != null && filesDel.Length > 0)
            {
                foreach (string file in filesDel)
                {
                    if (File.Exists(file))
                    {
                        var creatTime = Directory.GetCreationTime(file).Date;
                        if (creatTime < dateMouth)
                            File.Delete(file);
                    }
                }
            }
        }
    }
}
