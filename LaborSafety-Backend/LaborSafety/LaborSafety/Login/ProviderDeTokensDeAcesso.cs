using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using LaborSafety.Models.Respostas;

namespace LaborSafety.Login
{
    public class ProviderDeTokensDeAcesso : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            try
            {

                if (Login.Validate(context.UserName, context.Password))
                {
                    context.Validated(identity);
                }
                else
                {
                    await WriteResponseBody(HttpStatusCode.Unauthorized, new System.Exception("Usuário ou senha inválido."), context, "Acesso Inválido");
                }
            }
            catch (Exception ex)
            {
                await WriteResponseBody(HttpStatusCode.InternalServerError, ex, context, "Acesso Inválido");
            }
        }

        private async Task WriteResponseBody(HttpStatusCode statusCode, Exception ex, OAuthGrantResourceOwnerCredentialsContext context, string message)
        {
            var errosString = GeradorResponse.GenerateErrorResponseString((int)statusCode,
                 message, ex);
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            context.SetError(errosString);
        }
    }
}