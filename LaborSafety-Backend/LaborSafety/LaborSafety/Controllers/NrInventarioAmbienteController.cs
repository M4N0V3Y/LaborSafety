using System;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class NrInventarioAmbienteController : ApiController
    {
        private readonly INrInventarioAmbienteNegocio nrInventariosAmbienteNegocio;

        public NrInventarioAmbienteController(INrInventarioAmbienteNegocio nrInventariosAmbienteNegocio)
        {
            this.nrInventariosAmbienteNegocio = nrInventariosAmbienteNegocio;
        }

        [HttpPut]
        [Route("api/NrInventarioAmbiente/Inserir")]
        public IHttpActionResult Inserir([FromBody] NrInventarioAmbienteModelo nrInventarioAmbienteModelo)
        {
            try
            {
                this.nrInventariosAmbienteNegocio.InserirNrInventarioAmbiente(nrInventarioAmbienteModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de NR do inventário de ambiente ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "NR do Inventário de Ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir NRo do Inventário de Ambiente", exception), exception);
            }

        }
    }
}