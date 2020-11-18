using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class RiscoController : ApiController
    {
        private readonly IRiscoNegocio riscoNegocio;

        public RiscoController(IRiscoNegocio riscoNegocio)
        {
            this.riscoNegocio = riscoNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/Risco/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            RiscoModelo risco;
            try
            {
                risco = this.riscoNegocio.ListarRiscoPorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar risco de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da risco de id {id} ocorrida com sucesso", risco));
        }

        [HttpGet]
        [Route("api/Risco/FiltrarPorNome")]
        public IHttpActionResult FiltrarPorNome(string nome)
        {
            RiscoModelo risco;
            try
            {
                risco = this.riscoNegocio.ListarRiscoPorNome(nome);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar risco de nome {nome}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da risco de nome {nome} ocorrida com sucesso", risco));
        }


        [HttpGet]
        [Route("api/Risco/ListarTodosRiscos")]
        public IHttpActionResult ListarTodosRiscos()
        {
            IEnumerable<RiscoModelo> risco;

            try
            {
                risco = this.riscoNegocio.ListarTodosRiscos();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar riscos.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação dos riscos ocorrida com sucesso", risco));
        }

        [HttpGet]
        [Route("api/Risco/ListarTodosTiposRisco")]
        public IHttpActionResult ListarTodosTiposRisco()
        {
            List<TipoRiscoModelo> tiposRisco;

            try
            {
                tiposRisco = this.riscoNegocio.ListarTiposRisco();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar tipos de risco.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação dos tipos de risco ocorrida com sucesso", tiposRisco));
        }

        [HttpGet]
        [Route("api/Risco/ListarPorTipoRisco")]
        public IHttpActionResult ListarPorTipoRisco(long codTipo)
        {
            IEnumerable<RiscoModelo> risco;

            try
            {
                risco = this.riscoNegocio.ListarPorTipoRisco(codTipo);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar riscos.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação dos ricos ocorrida com sucesso", risco));
        }
    }
}