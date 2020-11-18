using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class FuncionalidadesController : ApiController
    {
        private readonly IFuncionalidadesNegocio funcionalidadeNegocio;

        public FuncionalidadesController(IFuncionalidadesNegocio funcionalidadeNegocio)
        {
            this.funcionalidadeNegocio = funcionalidadeNegocio;
        }
        // GET: Funcionalidades
        public IHttpActionResult Get()
        {
            IEnumerable<FuncionalidadeModelo> funcionalidades;
            try
            {
                funcionalidades = this.funcionalidadeNegocio.ListarTodasFuncionalidades();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar funcionalidades.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação de todas as funcionalidades cadastradas ocorrida com sucesso", funcionalidades));
        }

        // GET: Funcionalidades por id
        public IHttpActionResult Get(long id)
        {
            FuncionalidadeModelo funcionalidade;
            try
            {
                funcionalidade = this.funcionalidadeNegocio.ListarFuncionalidadePorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar funcionalidade de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da funcionalidade de id {id} ocorrida com sucesso", funcionalidade));
        }

        // GET {id}
        [Route("api/funcionalidades/c8id/{c8id}")]
        public IHttpActionResult Get(String c8id)
        {
            FuncionalidadesPor8IdModelo funcionalidadesPor8IdModelo;
            try
            {
                funcionalidadesPor8IdModelo = this.funcionalidadeNegocio.ListarFuncionalidadesPor8ID(c8id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao recuperar funcionalidade por 8id.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de funcionalidades por 8id ocorrida com sucesso", funcionalidadesPor8IdModelo));
        }
    }
}
