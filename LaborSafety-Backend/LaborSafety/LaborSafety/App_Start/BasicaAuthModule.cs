using System;
using System.Text;
using System.Web;
using LaborSafety.Utils.Security;

namespace EntregaEColeta.API.App_Start
{
    public class BasicAuthHttpModule : IHttpModule
    {
        void IHttpModule.Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(OnAuthenticateRequest);
        }

        void OnAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            if (!request.CurrentExecutionFilePathExtension.Contains("asmx"))
            {
                return;
            }

            string header = request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(header) && header.Trim().ToUpper().StartsWith("BASIC"))  //if has header
            {
                string encodedUserPass = header.Substring(6).Trim();  //remove the "Basic"
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string userPass = encoding.GetString(Convert.FromBase64String(encodedUserPass));
                string[] credentials = userPass.Split(':');
                string username = credentials[0];
                string password = credentials.Length > 1? credentials[1] : string.Empty;

                if (!ADSettings.AuthAD(username, password))
                {
                    HttpContext.Current.Response.StatusCode = 401;
                    HttpContext.Current.Response.End();
                }
            }
            else
            {
                //send request header for the 1st round
                HttpContext context = HttpContext.Current;
                context.Response.StatusCode = 401;
                context.Response.AddHeader("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", string.Empty));
            }
        }

        void IHttpModule.Dispose()
        {
        }
    }
}