using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Utils;
using static LaborSafety.Negocio.Servicos.InventarioAmbienteNegocio;

namespace LaborSafety.Controllers
{
    public class InventarioAmbienteController : ApiController
    {
        private readonly IInventariosAmbienteNegocio inventariosAmbienteNegocio;

        public InventarioAmbienteController(IInventariosAmbienteNegocio inventariosAmbienteNegocio)
        {
            this.inventariosAmbienteNegocio = inventariosAmbienteNegocio;
        }

        // GET: Inventario de ambiente por id
        [HttpGet]
        public IHttpActionResult Get(long id)
        {
            InventarioAmbienteModelo inventarioAmbiente;
            try
            {
                inventarioAmbiente = this.inventariosAmbienteNegocio.ListarInventarioAmbientePorId(id);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar inventário de ambiente de id {id}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da inventário de ambiente de id {id} ocorrida com sucesso", inventarioAmbiente));
        }

        [HttpGet]
        [Route("api/InventarioAmbiente/ListarCodAprPorInventarioTela")]
        public IHttpActionResult ListarCodAprPorInventarioTela(long codInventario)
        {
            long codAPR;
            try
            {
                codAPR = this.inventariosAmbienteNegocio.ListarCodAprPorInventarioTela(codInventario);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar inventário de ambiente de id {codInventario}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da inventário de ambiente de id {codInventario} ocorrida com sucesso", codAPR));
        }

        [HttpPost]
        [Route("api/InventarioAmbiente/Filtrar")]
        public IHttpActionResult Filtrar([FromBody] FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo)
        {
            IEnumerable<InventarioAmbienteModelo> inventarioAmbiente;
            try
            {
                inventarioAmbiente = this.inventariosAmbienteNegocio.ListarInventarioAmbiente(filtroInventarioAmbienteModelo);
                                    
                if (!inventarioAmbiente.Any())
                {
                    throw new Exception("Não foram encontrados inventários com o(s) filtro(s) selecionado(s)");
                }
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar inventário de ambiente!", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da inventário de ambiente ocorrida com sucesso", inventarioAmbiente));
        }
        

        // PUT: Inventario de Ambiente
        [HttpPut]
        [Route("api/InventarioAmbiente/Inserir")]
        public IHttpActionResult Inserir([FromBody] InventarioAmbienteModelo inventarioAmbienteModelo)
        {
            try
            {
                RetornoInsercao resultado = new RetornoInsercao();
                resultado = this.inventariosAmbienteNegocio.InserirInventarioAmbiente(inventarioAmbienteModelo);

                if (resultado.status == true)
                    return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                        $"Inserção do inventário de ambiente ocorrida com sucesso", resultado));
                else
                    return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.BadRequest,
                    $"Erro ao inserir inventário de ambiente", resultado));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.OK,
                    "Inventário de Ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.OK,
                    "Erro ao inserir Inventário de Ambiente", exception), exception);
            }

        }

        // PUT edição: Inventario de Ambiente
        [HttpPut]
        [Route("api/InventarioAmbiente/Editar")]
        public IHttpActionResult Editar([FromBody] InventarioAmbienteModelo inventarioAmbienteModelo)
        {
            try
            {
                this.inventariosAmbienteNegocio.EditarInventarioAmbiente(inventarioAmbienteModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição do inventário de ambiente ocorrida com sucesso"));
            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Inventário de Ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar Inventário de Ambiente", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/InventarioAmbiente/EditarNR")]
        public IHttpActionResult EditarNR(long idInventario, long idNr)
        {
            try
            {
                this.inventariosAmbienteNegocio.EditarNrInventarioAmbiente(idInventario, idNr);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição da Nr do inventário de ambiente ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "NR do inventário de ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar NR do inventário de ambiente", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/InventarioAmbiente/EditarRisco")]
        public IHttpActionResult EditarRisco(long idInventario, long idRisco)
        {
            try
            {
                this.inventariosAmbienteNegocio.EditarRiscoInventarioAmbiente(idInventario, idRisco);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição do risco do inventário de ambiente ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Risco do inventário de ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar risco do inventário de ambiente", exception), exception);
            }
        }

        [HttpPost]
        [Route("api/InventarioAmbiente/Desativar")]
        public IHttpActionResult DesativarInventario(InventarioAmbienteDelecaoComLogModelo inventarioAmbienteDelecaoComLogModelo)
        {
            try
            {
                this.inventariosAmbienteNegocio.DesativarInventario(inventarioAmbienteDelecaoComLogModelo, null);
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
        [Route("api/InventarioAmbiente/Excluir")]
        public IHttpActionResult Excluir(long id)
        {
            try
            {
                this.inventariosAmbienteNegocio.ExcluirInventarioAmbiente(id);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Exclusão do inventário de ambiente ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Inventário de Ambiente não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar Inventário de Ambiente", exception), exception);
            }
        }

        // GET: Inventario de ambiente por id
        [HttpPut]
        [Route("api/InventarioAmbiente/ImportarPlanilha")]
        public IHttpActionResult ImportarPlanilha([FromBody] ArquivoModelo arquivo)
        {
            ResultadoImportacao result;

            try
            {
                if(arquivo == null || String.IsNullOrEmpty(arquivo.arquivoBase64))
                    throw new Exception("O arquivo não foi informado!");

                string diretorioAtual = ArquivoDiretorioUtils.ObterDiretorioArquivosImportados();

                //Cria o arquivo localmente
                File.WriteAllBytes(diretorioAtual + @"\LayoutInventarioAmbiente.xlsx", Convert.FromBase64String(arquivo.arquivoBase64));

                result = this.inventariosAmbienteNegocio.ImportarPlanilha(diretorioAtual + @"\LayoutInventarioAmbiente.xlsx", arquivo.EightIDUsuarioModificador);
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
        [Route("api/InventarioAmbiente/CalcularRiscoTotalLista")]
        public IHttpActionResult CalcularRiscoTotalLista(int codProbabilidade, int codSeveridade)
        {
            try
            {
                var result = this.inventariosAmbienteNegocio.CalcularRiscoTotalLista(codProbabilidade, codSeveridade);
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

        [HttpPut]
        [Route("api/InventarioAmbiente/CalcularRiscoTotalTela")]
        public IHttpActionResult CalcularRiscoTotalTela(RiscoTotalAmbienteModelo riscoTotalAmbienteModelo)
        {
            try
            {
                var result = this.inventariosAmbienteNegocio.CalcularRiscoTotalTela(riscoTotalAmbienteModelo);
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
        [Route("api/InventarioAmbiente/GeraModelo")]
        public IHttpActionResult GerarModelo()
        {
            byte[] result;
            string resultado;

            try
            {
                string diretorioAtual = ArquivoDiretorioUtils.ObterDiretorioModelo();

                result = File.ReadAllBytes(diretorioAtual + @"\LayoutInventarioAmbiente.xlsx");

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
        [Route("api/InventarioAmbiente/EscreverLogEmTxt")]
        public IHttpActionResult EscreverLogEmTxt([FromBody] List<long> codInventariosAmbiente)
        {
            ArquivoLog result;

            try
            {
                if (codInventariosAmbiente == null)
                    throw new Exception("O código do inventário não foi informado!");

                result = this.inventariosAmbienteNegocio.EscreverLogEmTxt(codInventariosAmbiente);
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
        [Route("api/InventarioAmbiente/EscreverLogTodosInventarios")]
        public IHttpActionResult EscreverLogTodosInventarios()
        {
            ArquivoLog result;

            try
            {
                result = this.inventariosAmbienteNegocio.EscreverLogTodosInventarios();
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