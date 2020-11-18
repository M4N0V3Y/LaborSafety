using System;
using System.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using LaborSafety.Login;
using LaborSafety.Negocio.Servicos;

[assembly: OwinStartup(typeof(LaborSafety.Startup))]

namespace LaborSafety
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // ativando cors
            app.UseCors(CorsOptions.AllowAll);
            AtivarGeracaoTokenAcesso(app);
            CacheFolhaAprNegocio.InicializarCacheFolhas();
        }

        private void AtivarGeracaoTokenAcesso(IAppBuilder app)
        {
            var opcoesConfiguracaoToken = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(Convert.ToInt32(ConfigurationManager.AppSettings["DuracaoTokenHoras"])),
                Provider = new ProviderDeTokensDeAcesso()
            };
            app.UseOAuthAuthorizationServer(opcoesConfiguracaoToken);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}