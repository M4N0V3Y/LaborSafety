using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;

namespace LaborSafety.Controllers
{
    public class PerfilCatalogoController : ApiController
    {
        private readonly IPerfilCatalogoNegocio perfilCatalogoNegocio;

        public PerfilCatalogoController(IPerfilCatalogoNegocio perfilCatalogoNegocio)
        {
            this.perfilCatalogoNegocio = perfilCatalogoNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        [Route("api/PerfilCatalogo/FiltrarPorId")]
        public IHttpActionResult FiltrarPorId(long id)
        {
            PerfilCatalogoModelo perfilCatalogo;
            try
            {
                perfilCatalogo = this.perfilCatalogoNegocio.ListarPerfilCatalogoPorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar perfil de catálogo de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da perfil de catálogo de id {id} ocorrida com sucesso", perfilCatalogo));
        }

        [HttpGet]
        [Route("api/PerfilCatalogo/FiltrarPorNome")]
        public IHttpActionResult FiltrarPorNome(string nome)
        {
            PerfilCatalogoModelo perfilCatalogo;
            try
            {
                perfilCatalogo = this.perfilCatalogoNegocio.ListarPerfilCatalogoPorNome(nome);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar perfil de catálogo de nome {nome}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da perfil de catálogo de nome {nome} ocorrida com sucesso", perfilCatalogo));
        }

        [HttpGet]
        [Route("api/PerfilCatalogo/ListarTodosPCs")]
        public IHttpActionResult ListarTodosPCs()
        {
            IEnumerable<PerfilCatalogoModelo> perfilCatalogo;

            try
            {
                perfilCatalogo = this.perfilCatalogoNegocio.ListarTodosPCs();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar perfis de catálogo.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação dos perfis de catálogo ocorrida com sucesso", perfilCatalogo));
        }
    }
}