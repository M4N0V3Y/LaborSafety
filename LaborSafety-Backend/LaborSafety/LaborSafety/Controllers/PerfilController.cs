using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class PerfilController : ApiController
    {
        private IPerfisNegocio perfilNegocio;

        public PerfilController(IPerfisNegocio perfilNegocio)
        {
            this.perfilNegocio = perfilNegocio;
        }

        [HttpGet]
        [Route("api/Perfil/ListarListaTelasEFuncionalidadesPorPerfil")]
        public IHttpActionResult ListarTelaEFuncionalidadesPorPerfil(long codPerfil)
        {
            IEnumerable<TelaModelo> listaTelas = new List<TelaModelo>();
            try
            {
                listaTelas = this.perfilNegocio.ListarListaTelasEFuncionalidadesPorPerfil(codPerfil);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar informações das telas.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação de todos as informações das telas ocorrida com sucesso.", listaTelas));
        }


        [HttpGet]
        [Route("api/Perfil/ListarTelaEFuncionalidadesPorPerfilETela")]
        public IHttpActionResult ListarTelaEFuncionalidadesPorPerfilETela(long codPerfil, long codTela)
        {
            TelaModelo telaModelo = new TelaModelo();
            try
            {
                telaModelo = this.perfilNegocio.ListarTelaEFuncionalidadesPorPerfilETela(codPerfil, codTela);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar as informações da tela.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação de dados da tela ocorrida com sucesso", telaModelo));
        }



        // GET: Perfis
        [HttpGet]
        public IHttpActionResult Get()
        {
            IEnumerable<PerfilModelo> listaPerfis = new List<PerfilModelo>();
            try
            {
                listaPerfis = this.perfilNegocio.ListarTodosOsTiposPerfis();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao listar tipos de perfis.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Recuperação de todos os tipos de perfis ocorrido com sucesso", listaPerfis));
        }


        // GET {id}
        [HttpGet]
        public IHttpActionResult Get(long id)
        {
            PerfilModelo perfil;
            try
            {
                perfil = this.perfilNegocio.ListarPerfilPorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao recuperar perfis relacionados ao usuário.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação do perfil ocorrida com sucesso", perfil));
        }

        // GET {id}
        [HttpPost]
        [Route("api/Perfil/BuscarPor8ID")]
        public IHttpActionResult BuscarPor8ID(Usuario8IDModelo usuario)
        {
            IEnumerable<PerfilModelo> listaPerfisPor8id = new List<PerfilModelo>();
            try
            {
                listaPerfisPor8id = this.perfilNegocio.ObterPerfisPor8ID(usuario.Usuario8ID);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao recuperar perfis relacionados ao usuário.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de perfis associados ao usuário ocorrida com sucesso", listaPerfisPor8id));
        }

        [HttpPost]
        [Route("api/Perfil/AssociarFuncionalidades")]
        public IHttpActionResult AssociarFuncionalidades(PerfilFuncionalidadeModelo perfilFuncionalidadeModelo)
        {
            try
            {
                bool result;
                result = this.perfilNegocio.Insercao(perfilFuncionalidadeModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção das funcionalidades ocorrida com sucesso", result));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.NotFound,
                    "Perfil ou funcionalidade não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir funcionalidades para o perfil.", exception), exception);
            }

        }
    }
}
