using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class BloqueioController : ApiController
    {
        private readonly IBloqueioNegocio bloqueioNegocio;

        public BloqueioController(IBloqueioNegocio bloqueioNegocio)
        {
            this.bloqueioNegocio = bloqueioNegocio;
        }

        [HttpPut]
        [Route("api/Bloqueio/GerarBloqueioAPR")]
        public IHttpActionResult GerarApr([FromBody] DadosMapaBloqueioAprModelo dados)
        {
            List<string> result;

            try
            {
                if (dados == null)
                    throw new Exception("Os dados de bloqueio da APR não foram informados!");

                result = this.bloqueioNegocio.ListarBloqueioPorListaLIPorArea(dados);

            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "", result));
        }
    }
}