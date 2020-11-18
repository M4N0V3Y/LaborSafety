using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;
using static LaborSafety.Negocio.Servicos.InventarioAtividadeNegocio;
using LaborSafety.Persistencia;
using System.Configuration;
using LaborSafety.Utils;

namespace LaborSafety.Controllers
{
    public class InventarioAtividadeController : ApiController
    {
        private readonly IInventariosAtividadeNegocio inventariosAtividadeNegocio;

        public InventarioAtividadeController(IInventariosAtividadeNegocio inventariosAtividadeNegocio)
        {
            this.inventariosAtividadeNegocio = inventariosAtividadeNegocio;
        }

        [HttpGet]
        [Route("api/InventarioAtividade/FiltrarPorId")]
        public IHttpActionResult GetPorId(long id)
        {
            InventarioAtividadeModelo inventarioAtividade;
            try
            {
                inventarioAtividade = this.inventariosAtividadeNegocio.ListarInventarioAtividadePorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar inventário de atividade de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da inventário de atividade de id {id} ocorrida com sucesso", inventarioAtividade));
        }

        [HttpGet]
        [Route("api/InventarioAtividade/ListarCodAprPorInventarioTela")]
        public IHttpActionResult ListarCodAprPorInventarioTela(long codInventario)
        {
            long codAPR;
            try
            {
                codAPR = this.inventariosAtividadeNegocio.ListarCodAprPorInventarioTela(codInventario);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar inventário de atividade de id {codInventario}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da inventário de atividade de id {codInventario} ocorrida com sucesso", codAPR));
        }

        [HttpPost]
        [Route("api/InventarioAtividade/Filtrar")]
        public IHttpActionResult Filtrar([FromBody] FiltroInventarioAtividadeModelo filtroInventarioAtividadeModelo)
        {
            List<InventarioAtividadeModelo> inventarioAtividade;
            try
            {
                inventarioAtividade = this.inventariosAtividadeNegocio.ListarInventarioAtividade(filtroInventarioAtividadeModelo);

                if (inventarioAtividade.Count == 0)
                    throw new Exception("Não foram encontrados inventários com o(s) filtro(s) selecionado(s)");

            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar inventário de atividade.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação do inventário de atividade ocorrida com sucesso", inventarioAtividade));
        }

        // PUT: Inventario de ATIVIDADE
        [HttpPut]
        [Route("api/InventarioAtividade/Inserir")]
        public IHttpActionResult Inserir([FromBody] InventarioAtividadeModelo inventarioAtividadeModelo)
        {
            try
            {
                this.inventariosAtividadeNegocio.InserirInventarioAtividade(inventarioAtividadeModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção do inventário de atividade ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Inventário de Atividade não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir Inventário de Atividade", exception), exception);
            }
        }

        // PUT edição: Inventario de ATIVIDADE
        [HttpPut]
        [Route("api/InventarioAtividade/Editar")]
        public IHttpActionResult Editar([FromBody] InventarioAtividadeModelo inventarioAtividadeModelo)
        {
            try
            {
                this.inventariosAtividadeNegocio.EditarInventarioAtividade(inventarioAtividadeModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição do inventário ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Inventário de Atividade não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar Inventário de Atividade", exception), exception);
            }
        }
        [HttpPut]
        [Route("api/InventarioAtividade/EditarLocalInstalacao")]
        public IHttpActionResult EditarLocalInstalacao(long idInventario, long idLi)
        {
            try
            {
                this.inventariosAtividadeNegocio.EditarLocalInstalacaoInventarioAtividade(idInventario, idLi);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição das funcionalidades ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Local de instalação da Atividade não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar Local de instalação da Atividade", exception), exception);
            }

        }
        [HttpPut]
        [Route("api/InventarioAtividade/EditarRisco")]
        public IHttpActionResult EditarRisco(long idInventario, long idRisco)
        {
            try
            {
                this.inventariosAtividadeNegocio.EditarRiscoInventarioAtividade(idInventario, idRisco);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição das funcionalidades ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Risco da atividade não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar risco da atividade", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/InventarioAtividade/EditarResponsavel")]
        public IHttpActionResult EditarResponsavel(long idInventario, long idResponsavel)
        {
            try
            {
                this.inventariosAtividadeNegocio.EditarResponsavelInventarioAtividade(idInventario, idResponsavel);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição de responsável ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Responsável da atividade não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar responsável da atividade", exception), exception);
            }

        }

        [HttpPost]
        [Route("api/InventarioAtividade/Desativar")]
        public IHttpActionResult DesativarInventario(InventarioAtividadeDelecaoComLogModelo inventarioAtividadeDelecao)
        {
            try
            {
                this.inventariosAtividadeNegocio.DesativarInventario(inventarioAtividadeDelecao, null);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Exclusão do inventário ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Inventário não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao excluir o inventário", exception), exception);
            }
        }

        [HttpPost]
        [Route("api/InventarioAtividade/Excluir")]
        public IHttpActionResult Excluir(long id)
        {
            try
            {
                this.inventariosAtividadeNegocio.ExcluirInventarioAtividade(id);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Exclusão do inventário ocorrida com sucesso"));
            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Inventário de Atividade não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar Inventário de Atividade", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/InventarioAtividade/ImportarPlanilha")]
        public IHttpActionResult ImportarPlanilha([FromBody] ArquivoModelo arquivo)
        {
            ResultadoImportacao result;

            try
            {
                if (arquivo == null || String.IsNullOrEmpty(arquivo.arquivoBase64))
                    throw new Exception("O arquivo não foi informado!");

                string diretorioAtual = ArquivoDiretorioUtils.ObterDiretorioArquivosImportados();

                //string dataHora = DateTime.Now.ToString();
                //dataHora = dataHora.Replace("/", "_").Replace(":", "").Replace(" ", "");

                //Directory.CreateDirectory(diretorioAtual + @"\InventarioAtividade");

                //Cria o arquivo localmente
                File.WriteAllBytes(diretorioAtual + @"\LayoutInventarioAtividade.xlsx", Convert.FromBase64String(arquivo.arquivoBase64));

                result = this.inventariosAtividadeNegocio.ImportarPlanilha(diretorioAtual + @"\LayoutInventarioAtividade.xlsx", arquivo.EightIDUsuarioModificador);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "", result));
        }

        [HttpPost]
        [Route("api/InventarioAtividade/CalcularRiscoTotalTela")]
        public IHttpActionResult CalcularRiscoTotalTela(RiscoTotalAtividadeModelo riscoTotalAtividadeModelo)
        {
            try
            {
                var result = this.inventariosAtividadeNegocio.CalcularRiscoTotalTela(riscoTotalAtividadeModelo);

                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Risco calculado com sucesso", result));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Risco não calculado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao tentar calcular o risco geral", exception), exception);
            }
        }

        [HttpGet]
        [Route("api/InventarioAtividade/GeraModelo")]
        public IHttpActionResult GerarModelo()
        {
            byte[] result;
            string resultado;

            try
            {
                string diretorioAtual = ArquivoDiretorioUtils.ObterDiretorioModelo();

                result = File.ReadAllBytes(diretorioAtual + @"\LayoutInventarioAtividade.xlsx");

                resultado = Convert.ToBase64String(result);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "", resultado));
        }

        [HttpPut]
        [Route("api/InventarioAtividade/EscreverLogEmTxt")]
        public IHttpActionResult EscreverLogEmTxt([FromBody] List<long> codInventariosAtividade)
        {
            ArquivoLog result;

            try
            {
                if (codInventariosAtividade == null)
                    throw new Exception("O código do inventário não foi informado!");

                string diretorioAtual = ConfigurationManager.AppSettings["caminhoLogInventario"];

                result = this.inventariosAtividadeNegocio.EscreverLogEmTxt(codInventariosAtividade);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "", result));
        }

        [HttpGet]
        [Route("api/InventarioAtividade/EscreverLogTodosInventarios")]
        public IHttpActionResult EscreverLogTodosInventarios()
        {
            ArquivoLog result;

            try
            {
                result = this.inventariosAtividadeNegocio.EscreverLogTodosInventarios();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "", result));
        }
    }
}