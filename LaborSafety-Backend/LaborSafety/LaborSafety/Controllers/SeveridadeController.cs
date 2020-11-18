using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class SeveridadeController : ApiController
    {
        private readonly ISeveridadeNegocio severidadeNegocio;

        public SeveridadeController(ISeveridadeNegocio severidadeNegocio)
        {
            this.severidadeNegocio = severidadeNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/Severidade/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            SeveridadeModelo severidade;
            try
            {
                severidade = this.severidadeNegocio.ListarSeveridadePorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar severidade de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da severidade de id {id} ocorrida com sucesso", severidade));
        }

        [HttpGet]
        [Route("api/Severidade/FiltrarPorNome")]
        public IHttpActionResult FiltrarPorNome(string nome)
        {
            SeveridadeModelo severidade;
            try
            {
                severidade = this.severidadeNegocio.ListarSeveridadePorNome(nome);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar severidade de nome {nome}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da severidade de nome {nome} ocorrida com sucesso", severidade));
        }

        [HttpGet]
        [Route("api/Severidade/ListarTodasSeveridades")]
        public IHttpActionResult ListarTodasSeveridades()
        {
            IEnumerable<SeveridadeModelo> severidade;

            try
            {
                severidade = this.severidadeNegocio.ListarTodasSeveridades();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar severidades.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação das severidades ocorrida com sucesso", severidade));
        }
    }
}