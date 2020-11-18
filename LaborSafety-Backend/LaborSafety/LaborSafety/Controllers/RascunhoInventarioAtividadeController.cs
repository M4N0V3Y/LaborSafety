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
    public class RascunhoInventarioAtividadeController : ApiController
    {
        private readonly IRascunhoInventarioAtividadeNegocio rascunhoInventarioAtividadeNegocio;
        public RascunhoInventarioAtividadeController(IRascunhoInventarioAtividadeNegocio rascunhoInventarioAtividadeNegocio)
        {
            this.rascunhoInventarioAtividadeNegocio = rascunhoInventarioAtividadeNegocio;
        }

        [HttpGet]
        [Route("api/RascunhoInventarioAtividade/FiltrarPorId")]
        public IHttpActionResult GetPorId(long id)
        {
            RascunhoInventarioAtividadeModelo inventarioAtividade;
            try
            {
                inventarioAtividade = this.rascunhoInventarioAtividadeNegocio.ListarInventarioAtividadePorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar inventário de atividade de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da inventário de atividade de id {id} ocorrida com sucesso", inventarioAtividade));
        }

        [HttpPost]
        [Route("api/RascunhoInventarioAtividade/FiltrarPorLi")]
        public IHttpActionResult FiltrarPorLi([FromBody] List<string> li)
        {
            List<RascunhoInventarioAtividadeModelo> rascunhoInventarioAtividadeModelo;
            try
            {
                rascunhoInventarioAtividadeModelo = this.rascunhoInventarioAtividadeNegocio.ListarInventarioAtividadePorLI(li);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar rascunho de inventário de atividade de li {li}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de rascunho de inventário de atividade de li {li} ocorrida com sucesso", rascunhoInventarioAtividadeModelo));
        }

        [HttpPost]
        [Route("api/RascunhoInventarioAtividade/Filtrar")]
        public IHttpActionResult Filtrar([FromBody] FiltroInventarioAtividadeModelo filtroInventarioAtividadeModelo)
        {
            List<RascunhoInventarioAtividadeModelo> inventarioAtividade;
            try
            {
                inventarioAtividade = this.rascunhoInventarioAtividadeNegocio.ListarInventarioAtividade(filtroInventarioAtividadeModelo);

                if (inventarioAtividade.Count == 0)
                    throw new Exception("Não foram encontrados rascunhos de inventários com o(s) filtro(s) selecionado(s)");

            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar rascunho de inventário de atividade.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação do rascunho do inventário de atividade ocorrida com sucesso", inventarioAtividade));
        }

        // PUT: Inventario de ATIVIDADE
        [HttpPut]
        [Route("api/RascunhoInventarioAtividade/Inserir")]
        public IHttpActionResult Inserir([FromBody] RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo)
        {
            try
            {
                this.rascunhoInventarioAtividadeNegocio.InserirRascunhoInventarioAtividade(rascunhoInventarioAtividadeModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de rascunho do inventário de atividade ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Rascunho de inventário de atividade não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir rascunho de inventário de atividade", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/RascunhoInventarioAtividade/Editar")]
        public IHttpActionResult Editar([FromBody] RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo)
        {
            try
            {
                this.rascunhoInventarioAtividadeNegocio.EditarInventarioAtividade(rascunhoInventarioAtividadeModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição do rascunho do inventário ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Rascunho de inventário de atividade não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar rascunho de inventário de atividade", exception), exception);
            }
        }

        [HttpDelete]
        [Route("api/RascunhoInventarioAtividade/Excluir")]
        public IHttpActionResult Excluir(long id)
        {
            try
            {
                this.rascunhoInventarioAtividadeNegocio.ExcluirRascunhoInventarioAtividade(id);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Exclusão do rascunho de inventário ocorrida com sucesso"));
            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Rascunho de inventário de atividade não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar rascunho de inventário de atividade", exception), exception);
            }
        }

    }
}