using System;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos.Exportacao;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces.Exportacao;
using static LaborSafety.Negocio.Servicos.Exportacao.ExportacaoDadosNegocio;

namespace LaborSafety.Controllers.Exportacao
{
    public class ExportacaoDadosController : ApiController
    {
        private readonly IExportacaoDadosNegocio exportacaoDadosNegocio;

        public ExportacaoDadosController(IExportacaoDadosNegocio exportacaoDadosNegocio)
        {
            this.exportacaoDadosNegocio = exportacaoDadosNegocio;
        }

        [HttpPut]
        [Route("api/ExportacaoDados/ExportarDadosAmbiente")]
        public IHttpActionResult ExportarDadosAmbiente([FromBody] DadosExportacaoAmbienteModelo dadosExportacaoModelo)
        {
            ResultadoExportacao result;

            try
            {
                if (dadosExportacaoModelo == null)
                    throw new Exception("Os dados da exportação não foram informados!");

                result = this.exportacaoDadosNegocio.GerarArquivoAmbiente(dadosExportacaoModelo);

            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "", result));
        }

        [HttpPut]
        [Route("api/ExportacaoDados/ExportarDadosAtividade")]
        public IHttpActionResult ExportarDadosAtividade([FromBody] DadosExportacaoAtividadeModelo dadosExportacaoModelo)
        {
            ResultadoExportacao result;

            try
            {
                if (dadosExportacaoModelo == null)
                    throw new Exception("Os dados da exportação não foram informados!");

                result = this.exportacaoDadosNegocio.GerarArquivoAtividade(dadosExportacaoModelo);

            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "", result));
        }

        [HttpPut]
        [Route("api/ExportacaoDados/ExportarDadosApr")]
        public IHttpActionResult ExportarDadosApr([FromBody] DadosExportacaoAprModelo dadosExportacaoModelo)
        {
            ResultadoExportacao result;

            try
            {
                if (dadosExportacaoModelo == null)
                    throw new Exception("Os dados da exportação não foram informados!");

                result = this.exportacaoDadosNegocio.GerarArquivoAPR(dadosExportacaoModelo);

            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "", result));
        }
        [HttpPut]
        [Route("api/ExportacaoDados/agruparAprPdf")]
        public IHttpActionResult GerarEAgruparPdf([FromUri] string numeroSerie)
        {
            string result = "";

            try
            {
                if (string.IsNullOrEmpty(numeroSerie))
                    throw new Exception("Os dados da exportação não foram informados!");

                result = this.exportacaoDadosNegocio.AgruparAprPdf(numeroSerie);

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