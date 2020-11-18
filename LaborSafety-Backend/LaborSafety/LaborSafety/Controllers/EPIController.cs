using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class EPIController : ApiController
    {
        private readonly IEpiNegocio EPINegocio;

        public EPIController(IEpiNegocio EPINegocio)
        {
            this.EPINegocio = EPINegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/EPI/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            EPIModelo EPI;
            try
            {
                EPI = this.EPINegocio.ListarEPIPorID(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar EPI de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da EPI de id {id} ocorrida com sucesso", EPI));
        }

        [HttpGet]
        [Route("api/EPI/FiltrarPorNome")]
        public IHttpActionResult FiltrarPorNome(string nome)
        {
            EPIModelo EPI;
            try
            {
                EPI = this.EPINegocio.ListarEPIPorNome(nome);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar EPI de nome {nome}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da EPI de nome {nome} ocorrida com sucesso", EPI));
        }

        [HttpGet]
        [Route("api/EPI/ListarTodosEPIs")]
        public IHttpActionResult ListarTodosEPIs()
        {
            IEnumerable<EPIModelo> EPI;

            try
            {
                EPI = this.EPINegocio.ListarTodosEPIs();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar EPIs.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação das EPIs ocorrida com sucesso", EPI));
        }

        [HttpGet]
        [Route("api/EPI/ListarTodosEPIPorNivel")]
        public IHttpActionResult ListarTodosEPIPorNivel(string nome)
        {
            List<EPIModelo> EPI;

            try
            {
                EPI = this.EPINegocio.ListarTodosEPIPorNivel(nome);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar EPIs.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação das EPIs ocorrida com sucesso", EPI));
        }

        [HttpPost]
        [Route("api/EPI/ListarEPIsPorListaId")]
        public IHttpActionResult ListarEPIsPorListaId(List<long> epis)
        {
            List<EPIModelo> epi;

            try
            {
                epi = this.EPINegocio.ListarEPIsPorListaId(epis);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar EPIs.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação dos EPIs ocorrida com sucesso", epi));
        }
    }
}