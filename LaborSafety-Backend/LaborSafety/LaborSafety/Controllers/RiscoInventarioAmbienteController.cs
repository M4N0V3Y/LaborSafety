using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class RiscoInventarioAmbienteController : ApiController
    {
        private readonly IRiscoInventarioAmbienteNegocio riscoInventarioAmbienteNegocio;

        public RiscoInventarioAmbienteController(IRiscoInventarioAmbienteNegocio inventarioRiscoAmbienteNegocio)
        {
            this.riscoInventarioAmbienteNegocio = inventarioRiscoAmbienteNegocio;
        }

        [HttpPut]
        [Route("api/RiscoInventarioAmbiente/Inserir")]
        public IHttpActionResult Inserir([FromBody] RiscoInventarioAmbienteModelo riscoInventarioAmbienteModelo)
        {
            try
            {
                this.riscoInventarioAmbienteNegocio.InserirRiscoInventarioAmbiente(riscoInventarioAmbienteModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção do risco de inventário de ambiente ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Risco do inventário de ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir risco do inventário de ambiente", exception), exception);
            }

        }
    }
}