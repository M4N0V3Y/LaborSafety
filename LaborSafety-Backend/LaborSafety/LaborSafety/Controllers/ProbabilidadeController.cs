using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class ProbabilidadeController : ApiController
    {
        private readonly IProbabilidadeNegocio probabilidadeNegocio;

        public ProbabilidadeController(IProbabilidadeNegocio probabilidadeNegocio)
        {
            this.probabilidadeNegocio = probabilidadeNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/Probabilidade/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            ProbabilidadeModelo probabilidade;
            try
            {
                probabilidade = this.probabilidadeNegocio.ListarProbabilidadePorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar probabilidade de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da probabilidade de id {id} ocorrida com sucesso", probabilidade));
        }

        [HttpGet]
        [Route("api/Probabilidade/FiltrarPorNome")]
        public IHttpActionResult FiltrarPorNome(string nome)
        {
            ProbabilidadeModelo probabilidade;
            try
            {
                probabilidade = this.probabilidadeNegocio.ListarProbabilidadePorNome(nome);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar probabilidade de nome {nome}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da probabilidade de nome {nome} ocorrida com sucesso", probabilidade));
        }

        [HttpGet]
        [Route("api/Probabilidade/ListarTodasProbabilidades")]
        public IHttpActionResult ListarTodasProbabilidades()
        {
            IEnumerable<ProbabilidadeModelo> peso;

            try
            {
                peso = this.probabilidadeNegocio.ListarTodasProbabilidades();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar probabilidades.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação das probabilidades ocorrida com sucesso", peso));
        }
    }
}