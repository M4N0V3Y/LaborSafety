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
    public class TelaController : ApiController
    {
        private readonly ITelaNegocio telaNegocio;
        
        public TelaController(ITelaNegocio telaNegocio)
        {
            this.telaNegocio = telaNegocio;
        }

        public IHttpActionResult Get()
        {
            IEnumerable<TelaModelo> telas;
            try
            {
                telas = this.telaNegocio.Listar();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar funcionalidades.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação de todas as funcionalidades cadastradas ocorrida com sucesso", telas));
        }


        [HttpGet]
        [Route("api/tela/ListarPorCodigo")]
        public IHttpActionResult ListarPorCodigo(long codigo)
        {
            TelaModelo tela;
            try
            {
                tela = this.telaNegocio.ListarPorCodigo(codigo);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar funcionalidades.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação de todas as funcionalidades cadastradas ocorrida com sucesso", tela));
        }

    }
}