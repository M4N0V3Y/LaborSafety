using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class OperacaoAprController : ApiController
    {
        private IOperacaoAprNegocio operacaoAprNegocio;

        public OperacaoAprController(IOperacaoAprNegocio operacaoAprNegocio)
        {
            this.operacaoAprNegocio = operacaoAprNegocio;
        }

        [HttpPost]
        [Route("api/OperacaoApr/ListarPorId")]
        public IHttpActionResult ListarPorCodigos([FromBody] List<long> codigo)
        {
            IEnumerable<OperacaoAprModelo> operacaoAprModelos;

            try
            {
                operacaoAprModelos = this.operacaoAprNegocio.ListarPorCodigos(codigo);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar pessoa", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação das pessoas com sucesso", operacaoAprModelos));
        }
    }
}