using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class SistemaOperacionalController : ApiController
    {
        private readonly IAmbienteNegocio sistemaOperacionalNegocio;

        public SistemaOperacionalController(IAmbienteNegocio sistemaOperacionalNegocio)
        {
            this.sistemaOperacionalNegocio = sistemaOperacionalNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/SistemaOperacional/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            AmbienteModelo sistemaOperacional;
            try
            {
                sistemaOperacional = this.sistemaOperacionalNegocio.ListarSistemaOperacionalPorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar ambiente de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da ambiente de id {id} ocorrida com sucesso", sistemaOperacional));
        }

        [HttpGet]
        [Route("api/SistemaOperacional/FiltrarPorNome")]
        public IHttpActionResult FiltrarPorNome(string nome)
        {
            AmbienteModelo sistemaOperacional;
            try
            {
                sistemaOperacional = this.sistemaOperacionalNegocio.ListarSistemaOperacionalPorNome(nome);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar ambiente de nome {nome}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da ambiente de nome {nome} ocorrida com sucesso", sistemaOperacional));
        }

        [HttpGet]
        [Route("api/SistemaOperacional/ListarTodosSOs")]
        public IHttpActionResult ListarTodosSOs()
        {
            IEnumerable<AmbienteModelo> sistema;

            try
            {
                sistema = this.sistemaOperacionalNegocio.ListarTodosSOs();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar ambiente.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação dos ambientes ocorrida com sucesso", sistema));
        }

        [HttpPut]
        [Route("api/SistemaOperacional/Inserir")]
        public IHttpActionResult Inserir([FromBody] AmbienteModelo ambienteModelo)
        {
            try
            {
                var resultado = this.sistemaOperacionalNegocio.Inserir(ambienteModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de ambiente ocorrida com sucesso", resultado));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir ambiente!", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir ambiente.", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/SistemaOperacional/Editar")]
        public IHttpActionResult Editar([FromBody] AmbienteModelo ambienteModelo)
        {
            try
            {
                var resultado = this.sistemaOperacionalNegocio.Editar(ambienteModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição do ambiente ocorrida com sucesso", resultado));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar ambiente", exception), exception);
            }
        }

        [HttpDelete]
        [Route("api/SistemaOperacional/Desativar")]
        public IHttpActionResult DesativarAmbiente(long codAmbienteExistente)
        {
            try
            {
                this.sistemaOperacionalNegocio.DesativarAmbiente(codAmbienteExistente);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Exclusão do ambiente ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao excluir o ambiente", exception), exception);
            }
        }
    }
}