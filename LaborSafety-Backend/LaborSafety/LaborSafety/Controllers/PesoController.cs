using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class PesoController : ApiController
    {
        private readonly IPesoNegocio pesoNegocio;

        public PesoController(IPesoNegocio pesoNegocio)
        {
            this.pesoNegocio = pesoNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/Peso/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            PesoModelo peso;
            try
            {
                peso = this.pesoNegocio.ListarPesoPorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar peso de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da peso de id {id} ocorrida com sucesso", peso));
        }

        [HttpGet]
        [Route("api/Peso/FiltrarPorIndice")]
        public IHttpActionResult FiltrarPorNome(int indice)
        {
            PesoModelo peso;
            try
            {
                peso = this.pesoNegocio.ListarPesoPorIndice(indice);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar peso de nome {indice}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da peso de nome {indice} ocorrida com sucesso", peso));
        }

        [HttpGet]
        [Route("api/Peso/ListarTodosPesos")]
        public IHttpActionResult ListarTodosPesos()
        {
            IEnumerable<PesoModelo> peso;

            try
            {
                peso = this.pesoNegocio.ListarTodosPesos();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar pesos.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação dos pesos ocorrida com sucesso", peso));
        }
    }
}