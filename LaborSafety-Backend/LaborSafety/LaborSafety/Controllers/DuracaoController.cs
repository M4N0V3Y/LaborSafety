using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class DuracaoController : ApiController
    {
        private readonly IDuracaoNegocio duracaoNegocio;

        public DuracaoController(IDuracaoNegocio duracaoNegocio)
        {
            this.duracaoNegocio = duracaoNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/Duracao/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            DuracaoModelo duracao;
            try
            {
                duracao = this.duracaoNegocio.ListarDuracaoPorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar duracao de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da duracao de id {id} ocorrida com sucesso", duracao));
        }

        [HttpGet]
        [Route("api/Duracao/FiltrarPorNome")]
        public IHttpActionResult FiltrarPorNome(int indice)
        {
            DuracaoModelo duracao;
            try
            {
                duracao = this.duracaoNegocio.ListarDuracaoPorIndice(indice);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar duracao de indice {indice}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da duracao de indice {indice} ocorrida com sucesso", duracao));
        }

        [HttpGet]
        [Route("api/Duracao/ListarTodasDuracoes")]
        public IHttpActionResult ListarTodasDuracoes()
        {
            IEnumerable<DuracaoModelo> duracao;

            try
            {
                duracao = this.duracaoNegocio.ListarTodasDuracoes();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar durações.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação das durações ocorrida com sucesso", duracao));
        }

    }
}