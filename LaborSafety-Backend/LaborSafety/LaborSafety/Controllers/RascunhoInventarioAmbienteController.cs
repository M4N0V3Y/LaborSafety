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
    public class RascunhoInventarioAmbienteController : ApiController
    {
        private readonly IRascunhoInventarioAmbienteNegocio rascunhoInventarioAmbienteNegocio;
        public RascunhoInventarioAmbienteController(IRascunhoInventarioAmbienteNegocio rascunhoInventarioAmbienteNegocio)
        {
            this.rascunhoInventarioAmbienteNegocio = rascunhoInventarioAmbienteNegocio;
        }

        [HttpGet]
        [Route("api/RascunhoInventarioAmbiente/FiltrarPorId")]
        public IHttpActionResult GetPorId(long id)
        {
            RascunhoInventarioAmbienteModelo rascunhoInventarioAmbiente;
            try
            {
                rascunhoInventarioAmbiente = this.rascunhoInventarioAmbienteNegocio.ListarRascunhoInventarioAmbientePorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar rascunho inventário de ambiente de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de rascunho de inventário de ambiente de id {id} ocorrida com sucesso", rascunhoInventarioAmbiente));
        }

        [HttpGet]
        [Route("api/RascunhoInventarioAmbiente/FiltrarPorLi")]
        public IHttpActionResult GetPorLi(long li)
        {
            RascunhoInventarioAmbienteModelo rascunhoInventarioAmbiente;
            try
            {
                rascunhoInventarioAmbiente = this.rascunhoInventarioAmbienteNegocio.ListarRascunhoInventarioAmbientePorLI(li);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar rascunho de inventário de atividade de li {li}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de rascunho de inventário de atividade de li {li} ocorrida com sucesso", rascunhoInventarioAmbiente));
        }

        [HttpPost]
        [Route("api/RascunhoInventarioAmbiente/Filtrar")]
        public IHttpActionResult Filtrar([FromBody] FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo)
        {
            IEnumerable<RascunhoInventarioAmbienteModelo> rascunhoInventarioAmbiente;
            try
            {
                rascunhoInventarioAmbiente = this.rascunhoInventarioAmbienteNegocio.ListarRascunhoInventarioAmbiente(filtroInventarioAmbienteModelo);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar rascunho de inventário de ambiente!", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de rascunho de inventário de ambiente ocorrida com sucesso", rascunhoInventarioAmbiente));
        }

        // PUT: Inventario de Ambiente
        [HttpPut]
        [Route("api/RascunhoInventarioAmbiente/Inserir")]
        public IHttpActionResult Inserir([FromBody] RascunhoInventarioAmbienteModelo rascunhInventarioAmbienteModelo)
        {
            try
            {
                var resultado = this.rascunhoInventarioAmbienteNegocio.InserirRascunhoInventarioAmbiente(rascunhInventarioAmbienteModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de rascunho de inventário de ambiente ocorrida com sucesso", resultado));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Rascunho de inventário de ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir rascunho de inventário de ambiente", exception), exception);
            }

        }

        [HttpPut]
        [Route("api/RascunhoInventarioAmbiente/Editar")]
        public IHttpActionResult Editar([FromBody] RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo)
        {
            try
            {
                var resultado = this.rascunhoInventarioAmbienteNegocio.EditarRascunhoInventarioAmbiente(rascunhoInventarioAmbienteModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição do rascunho do inventário ocorrida com sucesso", resultado));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Rascunho de inventário de ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar rascunho de inventário de ambiente", exception), exception);
            }
        }


        [HttpDelete]
        [Route("api/RascunhoInventarioAmbiente/Excluir")]
        public IHttpActionResult Excluir(long id)
        {
            try
            {
                this.rascunhoInventarioAmbienteNegocio.ExcluirRascunhoInventarioAmbiente(id);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Exclusão do rascunho de inventário ocorrida com sucesso"));
            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Rascunho de inventário de ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar rascunho de inventário de ambiente", exception), exception);
            }
        }


    }
}