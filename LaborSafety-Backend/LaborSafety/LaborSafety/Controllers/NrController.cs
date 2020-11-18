using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class NrController : ApiController
    {
        private readonly INrNegocio nrNegocio;

        public NrController(INrNegocio nrNegocio)
        {
            this.nrNegocio = nrNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/Nr/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            NrModelo nr;
            try
            {
                nr = this.nrNegocio.ListarNrPorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar nr de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da nr de id {id} ocorrida com sucesso", nr));
        }

        [HttpGet]
        [Route("api/Nr/ListarNrPorCodigo")]
        public IHttpActionResult ListarNrPorCodigo(string codigo)
        {
            NrModelo nr;
            try
            {
                nr = this.nrNegocio.ListarNRPorCodigo(codigo);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar nr de codigo {codigo}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da nr de codigo {codigo} ocorrida com sucesso", nr));
        }

        [HttpGet]
        [Route("api/Nr/ListarTodasNRs")]
        public IHttpActionResult ListarTodasNRs()
        {
            IEnumerable<NrModelo> nr;

            try
            {
                nr = this.nrNegocio.ListarTodasNRs();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar NRs.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação das NRs ocorrida com sucesso", nr));
        }
    }
}