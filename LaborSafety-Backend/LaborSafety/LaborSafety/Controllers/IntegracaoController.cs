using System;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Controllers
{
    public class IntegracaoController : ApiController
    {
        private readonly IIntegracaoNegocio integracaoNegocio;

        public IntegracaoController(IIntegracaoNegocio integracaoNegocio)
        {
            this.integracaoNegocio = integracaoNegocio;
        }

        [HttpPost]
        [Route("api/Integracao/ProcessaCaracteristica")]
        public IHttpActionResult ProcessarDisciplina([FromBody] IntegracaoModelo caracteristica)
        {
            try
            {
                integracaoNegocio.ProcessaDisciplina(caracteristica);

                return Ok(GeradorResponse.GenerateResponseIntegracao(caracteristica.Nome, Constantes.StatusResponseIntegracao.S.ToString()[0], ""));
            }

            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateResponseIntegracaoString("", Constantes.StatusResponseIntegracao.E.ToString()[0],
                    exception.Message),exception);
            }
        }

        [HttpPost]
        [Route("api/Integracao/ProcessaChaveModelo")]
        public IHttpActionResult ProcessaChaveModelo([FromBody] IntegracaoModelo chaveModelo)
        {
            try
            {
                integracaoNegocio.ProcessaAtividadePadrao(chaveModelo);

                return Ok(GeradorResponse.GenerateResponseIntegracao(chaveModelo.Nome, Constantes.StatusResponseIntegracao.S.ToString()[0], ""));
            }

            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateResponseIntegracaoString("", Constantes.StatusResponseIntegracao.E.ToString()[0],
                    exception.Message), exception);
            }
        }

        [HttpPost]
        [Route("api/Integracao/ProcessaPerfilCatalogo")]
        public IHttpActionResult ProcessaPerfilCatalogo([FromBody] IntegracaoModelo perfilCatalogo)
        {
            try
            {
                integracaoNegocio.ProcessaPerfilCatalogo(perfilCatalogo);

                return Ok(GeradorResponse.GenerateResponseIntegracao(perfilCatalogo.Nome, Constantes.StatusResponseIntegracao.S.ToString()[0], ""));
            }

            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateResponseIntegracaoString("", Constantes.StatusResponseIntegracao.E.ToString()[0],
                    exception.Message), exception);
            }
        }
    }
}