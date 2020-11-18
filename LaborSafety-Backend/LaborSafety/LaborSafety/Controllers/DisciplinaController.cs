using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class DisciplinaController : ApiController
    {
        private readonly IDisciplinaNegocio disciplinaNegocio;

        public DisciplinaController(IDisciplinaNegocio disciplinaNegocio)
        {
            this.disciplinaNegocio = disciplinaNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/Disciplina/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            DisciplinaModelo disciplina;
            try
            {
                disciplina = this.disciplinaNegocio.ListarDisciplinaPorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar disciplina de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da disciplina de id {id} ocorrida com sucesso", disciplina));
        }

        [HttpGet]
        [Route("api/Disciplina/FiltrarPorNome")]
        public IHttpActionResult FiltrarPorNome(string nome)
        {
            DisciplinaModelo disciplina;
            try
            {
                disciplina = this.disciplinaNegocio.ListarDisciplinaPorNome(nome);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar disciplina de nome {nome}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da disciplina de nome {nome} ocorrida com sucesso", disciplina));
        }

        [HttpGet]
        [Route("api/Disciplina/ListarTodasDisciplinas")]
        public IHttpActionResult ListarTodasDisciplinas()
        {
            IEnumerable<DisciplinaModelo> disciplina;

            try
            {
                disciplina = this.disciplinaNegocio.ListarTodasDisciplinas();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar disciplinas.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação das disciplinas ocorrida com sucesso", disciplina));
        }
    }
}