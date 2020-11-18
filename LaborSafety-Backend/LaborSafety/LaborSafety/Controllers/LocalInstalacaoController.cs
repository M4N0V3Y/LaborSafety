using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class LocalInstalacaoController : ApiController
    {
        private readonly ILocalInstalacaoNegocio localInstalacaoNegocio;

        public LocalInstalacaoController(ILocalInstalacaoNegocio localInstalacaoNegocio)
        {
            this.localInstalacaoNegocio = localInstalacaoNegocio;
        }

        [HttpGet]
        [Route("api/LocalInstalacao/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            LocalInstalacaoModelo localInstalacao;
            try
            {
                localInstalacao = this.localInstalacaoNegocio.ListarLocalInstalacaoPorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar local de instalação de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da local de instalação de id {id} ocorrida com sucesso", localInstalacao));
        }

        [HttpGet]
        [Route("api/LocalInstalacao/FiltrarPorNome")]
        public IHttpActionResult FiltrarPorNome(string nome)
        {
            LocalInstalacaoModelo localInstalacao;
            try
            {
                localInstalacao = this.localInstalacaoNegocio.ListarLocalInstalacaoPorNome(nome);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar local de instalação de nome {nome}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da local de instalação de nome {nome} ocorrida com sucesso", localInstalacao));
        }

        [HttpPost]
        [Route("api/LocalInstalacao/ListarPorNivel")]
        public IHttpActionResult Filtrar([FromBody] LocalInstalacaoFiltroModelo filtro)
        {
            List<LocalInstalacaoModelo> locais = new List<LocalInstalacaoModelo>();

            try
            {
                locais = this.localInstalacaoNegocio.ListarLIPorNivel(filtro);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar locais de instalação.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da locais de instalação ocorrida com sucesso", locais));
        }
    }
}