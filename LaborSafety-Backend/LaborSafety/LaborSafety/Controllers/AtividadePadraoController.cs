using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class AtividadePadraoController : ApiController
    {
        private readonly IAtividadePadraoNegocio atividadePadraoNegocio;

        public AtividadePadraoController(IAtividadePadraoNegocio atividadePadraoNegocio)
        {
            this.atividadePadraoNegocio = atividadePadraoNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/AtividadePadrao/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            AtividadePadraoModelo atividade;
            try
            {
                atividade = this.atividadePadraoNegocio.ListarAtividadePorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar atividade de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da atividade de id {id} ocorrida com sucesso", atividade));
        }

        [HttpGet]
        [Route("api/AtividadePadrao/FiltrarPorNome")]
        public IHttpActionResult FiltrarPorNome(string nome)
        {
            AtividadePadraoModelo atividade;
            try
            {
                atividade = this.atividadePadraoNegocio.ListarAtividadePorNome(nome);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar atividade de nome {nome}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da atividade de nome {nome} ocorrida com sucesso", atividade));
        }

        [HttpGet]
        [Route("api/AtividadePadrao/ListarTodasAtividades")]
        public IHttpActionResult ListarTodasAtividades()
        {
            IEnumerable<AtividadePadraoModelo> atividade;

            try
            {
                atividade = this.atividadePadraoNegocio.ListarTodasAtividades();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar atividades.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação das atividades ocorrida com sucesso", atividade));
        }
    }
}