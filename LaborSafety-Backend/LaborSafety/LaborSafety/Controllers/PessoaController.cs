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
    public class PessoaController : ApiController
    {
        private IPessoaNegocio pessoaNegocio;

        public PessoaController(IPessoaNegocio pessoaNegocio)
        {
            this.pessoaNegocio = pessoaNegocio;
        }

        [HttpPut]
        [Route("api/Pessoa/Inserir")]
        public IHttpActionResult Inserir([FromBody] PessoaModelo pessoaModelo)
        {
            try
            {
                this.pessoaNegocio.Inserir(pessoaModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK, $"Inserção de pessoa ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest, "Pessoa não encontrada.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest, "Erro ao inserir pessoa", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/Pessoa/Editar")]
        public IHttpActionResult Editar([FromBody] PessoaModelo pessoaModelo)
        {
            try
            {
                this.pessoaNegocio.Editar(pessoaModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK, $"Edição de pessoa ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Pessoa não encontrada.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar pessoa", exception), exception);
            }
        }

        [HttpDelete]
        [Route("api/Pessoa/Excluir")]
        public IHttpActionResult Excluir(long codPessoa)
        {
            try
            {
                this.pessoaNegocio.Excluir(codPessoa);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Exclusão de pessoa ocorrida com sucesso"));
            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Pessoa de código { codPessoa } não encontrada.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao excluir pessoa", exception), exception);
            }
        }

        [HttpGet]
        [Route("api/Pessoa/ListarPorCodigo")]
        public IHttpActionResult ListarPorCodigo(long codigo)
        {
            PessoaModelo pessoaModelo;

            try
            {
                pessoaModelo = this.pessoaNegocio.ListarPorCodigo(codigo);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar pessoa", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação das pessoas com sucesso", pessoaModelo));
        }

        [HttpPost]
        [Route("api/Pessoa/ListarPorCodigos")]
        public IHttpActionResult ListarPorCodigos([FromBody] List<long> codigo)
        {
            IEnumerable<PessoaModelo> pessoaModelo;

            try
            {
                pessoaModelo = this.pessoaNegocio.ListarPorCodigos(codigo);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar pessoa", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação das pessoas com sucesso", pessoaModelo));
        }

        [HttpGet]
        [Route("api/Pessoa/ListarPorCPF")]
        public IHttpActionResult ListarPorCpf(string cpf)
        {
            PessoaModelo pessoaModelo;

            try
            {
                pessoaModelo = this.pessoaNegocio.ListarPorCpf(cpf);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar pessoa", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação da pessoa ocorrida com sucesso", pessoaModelo));
        }

        [HttpGet]
        [Route("api/Pessoa/Listar")]
        public IHttpActionResult Listar()
        {
            List<PessoaModelo> pessoas = new List<PessoaModelo>();

            try
            {
                pessoas = this.pessoaNegocio.Listar();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar pessoas", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação de todos os tipos de perfis ocorrido com sucesso", pessoas));

        }

    }
}