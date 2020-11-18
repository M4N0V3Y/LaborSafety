using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Models.Respostas;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Utils;
using static LaborSafety.Negocio.Servicos.AprNegocio;

namespace LaborSafety.Controllers
{
    public class AprController : ApiController
    {
        private readonly IAprNegocio aprNegocio;

        public AprController(IAprNegocio aprNegocio)
        {
            this.aprNegocio = aprNegocio;
        }

        public class ExceptionEmptyField : Exception
        {
            public ExceptionEmptyField(string mensagem, Exception exception) : base(mensagem, exception)
            { }
        }

        [HttpPut]
        [Route("api/APR/Inserir")]
        public IHttpActionResult Inserir([FromBody] AprModelo apr)
        {
            try
            {
                this.aprNegocio.Inserir(apr);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de APR ocorrida com sucesso"));
            }
            catch (Exception exception)
            {
                throw new ExceptionEmptyField(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
    "Erro ao inserir APR", exception), exception);
            }

        }

        [HttpPut]
        [Route("api/APR/Editar")]
        public IHttpActionResult Editar([FromBody] AprModelo apr)
        {
            try
            {
                this.aprNegocio.EditarApr(apr);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Edição de APR ocorrida com sucesso"));
            }
            catch (Exception exception)
            {
                throw new ExceptionEmptyField(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
    "Erro ao editar APR", exception), exception);
            }

        }

        [HttpPut]
        [Route("api/APR/InserirAprovadores")]
        public IHttpActionResult InserirAprovadores([FromBody] List<AprovadorAprModelo> aprovadores)
        {
            try
            {
                this.aprNegocio.InserirAprovadores(aprovadores);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de aprovadores na APR ocorrida com sucesso"));
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir aprovadores na APR", exception), exception);
            }

        }

        [HttpPut]
        [Route("api/APR/InserirExecutantes")]
        public IHttpActionResult InserirExecutantes([FromBody] List<ExecutanteAprModelo> executantes)
        {
            try
            {
                this.aprNegocio.InserirExecutores(executantes);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de executantes na APR ocorrida com sucesso"));
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir executantes na APR", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/APR/InserirCabecalho")]
        public IHttpActionResult InserirCabecalho([FromBody] AprModelo aprModelo)
        {
            try
            {
                this.aprNegocio.InserirCabecalho(aprModelo);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de cabeçalho de APR ocorrida com sucesso"));
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir cabeçalho de APR", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/APR/InserirPessoa")]
        public IHttpActionResult InserirPessoa([FromBody] PessoaModelo pessoas)
        {
            try
            {
                this.aprNegocio.InserirPessoa(pessoas);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de pessoas ocorrida com sucesso"));
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir pessoas", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/APR/InserirListaPessoa")]
        public IHttpActionResult InserirListaPessoa([FromBody] List<PessoaModelo> pessoas)
        {
            try
            {
                this.aprNegocio.InserirListaPessoa(pessoas);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de pessoas ocorrida com sucesso"));
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir pessoas", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/APR/InserirRisco")]
        public IHttpActionResult InserirRisco([FromBody] List<RiscoAprModelo> riscos)
        {
            try
            {
                this.aprNegocio.InserirRisco(riscos);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de riscos da APR ocorrida com sucesso"));
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir riscos da APR", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/APR/InserirAtividadeOperacao")]
        public IHttpActionResult InserirAtividadeOperacao([FromBody] List<OperacaoAprModelo> atividades)
        {
            try
            {
                this.aprNegocio.InserirAtividadeOperacao(atividades);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                    $"Inserção de atividades da APR ocorrida com sucesso"));
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao inserir atividades da APR", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/APR/EditarRiscos")]
        public IHttpActionResult EditarRiscos([FromBody] List<RiscoAprModelo> riscos)
        {
            try
            {
                this.aprNegocio.EditarRiscos(riscos);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK, $"Edição de riscos ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Riscos não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar riscos", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/APR/EditarAtividades")]
        public IHttpActionResult EditarAtividades([FromBody] List<OperacaoAprModelo> atividades)
        {
            try
            {
                this.aprNegocio.EditarAtividades(atividades);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK, $"Edição de atividades ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Atividade não encontrada.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar atividades", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/APR/EditarExecutantes")]
        public IHttpActionResult EditarExecutantes([FromBody] List<ExecutanteAprModelo> executantes)
        {
            try
            {
                this.aprNegocio.EditarExecutores(executantes);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK, $"Edição de executantes ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Executantes não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar executantes", exception), exception);
            }
        }

        [HttpPut]
        [Route("api/APR/EditarAprovadores")]
        public IHttpActionResult EditarAprovadores([FromBody] List<AprovadorAprModelo> aprovadores)
        {
            try
            {
                this.aprNegocio.EditarAprovadores(aprovadores);
                return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK, $"Edição de aprovadores ocorrida com sucesso"));

            }
            catch (InvalidOperationException exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Aprovadores não encontrado.", exception), exception);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    "Erro ao editar aprovadores", exception), exception);
            }
        }

        [HttpGet]
        [Route("api/APR/PesquisarPorId")]
        public IHttpActionResult PesquisarPorId(long idApr)
        {
            AprModelo listaAPR;
            try
            {
                listaAPR = this.aprNegocio.PesquisarPorId(idApr);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar APR's pelo id {idApr}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de APR's pelo id {idApr} ocorrida com sucesso", listaAPR));
        }

        [HttpGet]
        [Route("api/APR/PesquisarPorNumeroSerie")]
        public IHttpActionResult PesquisarPorNumeroSerie(string numeroSerie)
        {
            AprModelo APR;
            try
            {
                APR = this.aprNegocio.PesquisarPorNumeroSerie(numeroSerie);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar APR pelo número de série {numeroSerie}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da APR pelo número de série {numeroSerie} ocorrida com sucesso", APR));
        }

        [HttpGet]
        [Route("api/APR/ValidarExistenciaOrdemManutencao")]
        public IHttpActionResult ValidarExistenciaOrdemManutencao(string ordemManutencao, long codApr)
        {
            bool ordem;
            try
            {
                ordem = this.aprNegocio.ValidarExistenciaOrdemManutencao(ordemManutencao, codApr);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar APR pela ordem de manutenção {ordemManutencao}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da APR pela ordem de manutenção {ordemManutencao} ocorrida com sucesso", ordem));
        }

        [HttpGet]
        [Route("api/APR/PesquisarPorOrdemManutencao")]
        public IHttpActionResult PesquisarPorOrdemManutencao(string ordemManutencao)
        {
            AprModelo APR;
            try
            {
                APR = this.aprNegocio.PesquisarPorOrdemManutencao(ordemManutencao);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar APR pela ordem de manutenção {ordemManutencao}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação da APR pela ordem de manutenção {ordemManutencao} ocorrida com sucesso", APR));
        }

        [HttpGet]
        [Route("api/APR/PesquisarPorRisco")]
        public IHttpActionResult PesquisarPorRisco(long codRisco)
        {
            List<AprModelo> listaAPR;
            try
            {
                listaAPR = this.aprNegocio.PesquisarPorRisco(codRisco);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar APR's pelo risco {codRisco}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de APR's pelo risco {codRisco} ocorrida com sucesso", listaAPR));
        }

        [HttpGet]
        [Route("api/APR/PesquisarPorLocalInstalacao")]
        public IHttpActionResult PesquisarPorLocalInstalacao(long codLocalInstalacao)
        {
            List<AprModelo> listaAPR;
            try
            {
                listaAPR = this.aprNegocio.PesquisarPorLocalInstalacao(codLocalInstalacao);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar APR's pelo local de instalação {codLocalInstalacao}.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de APR's pelo local de instalação {codLocalInstalacao} ocorrida com sucesso", listaAPR));
        }

        [HttpGet]
        [Route("api/APR/PesquisarPorStatusOk")]
        public IHttpActionResult PesquisarPorStatusOk()
        {
            List<AprModelo> listaAPR;
            try
            {
                listaAPR = this.aprNegocio.PesquisarPorStatusOk();
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar APR's com status OK.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de APR's com status OK ocorrida com sucesso", listaAPR));
        }

        [HttpPost]
        [Route("api/APR/ListarApr")]
        public IHttpActionResult ListarApr(FiltroAprModelo filtroAprModelo)
        {
            List<AprModelo> listaAPR;
            try
            {
                listaAPR = this.aprNegocio.ListarApr(filtroAprModelo);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"Erro ao listar APR's.", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Recuperação de APR's ocorrida com sucesso", listaAPR));
        }

        [HttpPost]
        [Route("api/APR/CalcularRiscoAprTela")]
        public IHttpActionResult CalcularRiscoTotalTela(RiscoTotalAprModelo riscoTotalAprModelo)
        {
            try
            {
                var result = this.aprNegocio.CalcularRiscoAprTela(riscoTotalAprModelo);
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

        [HttpPost]
        [Route("api/APR/CalcularRiscoAprPorAtividadeDisciplinaLI")]
        public IHttpActionResult CalcularRiscoAprPorLi(FiltroUnicoInventarioAtividadeModelo filtro)
        {
            try
            {
                var result = this.aprNegocio.CalcularRiscoAprPorAtividadeDisciplinaLI(filtro);
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
        [Route("api/APR/GerarApr")]
        public IHttpActionResult GerarApr([FromBody] DadosAprModelo dadosAprModelo)
        {
            ResultadoGeracao result;

            try
            {
                if (dadosAprModelo == null)
                    throw new Exception("Os dados da APR não foram informados!");

                if (dadosAprModelo.Operacoes == null)
                    throw new Exception("A APR não contém as informações das operações");

                if (dadosAprModelo.Operacoes.Count == 0)
                    throw new Exception("A APR não contém as informações das operações");

                result = this.aprNegocio.GerarApr(dadosAprModelo, null);

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
        [Route("api/Apr/GeraModelo")]
        public IHttpActionResult GerarModelo()
        {
            byte[] result;
            string resultado;

            try
            {
                string diretorioAtual = ArquivoDiretorioUtils.ObterDiretorioModelo();

                result = File.ReadAllBytes(diretorioAtual +"LayoutApr.xlsx");

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

        [HttpGet]
        [Route("api/APR/GerarAPREmBrancoComNumeroSerie")]
        public IHttpActionResult GerarAPREmBrancoComNumeroSerie()
        {
            ResultadoGeracao result;
            byte[] resultByte;
            string resultado;

            try
            {
                result = this.aprNegocio.GerarAPREmBrancoComNumeroSerie();

                resultByte = File.ReadAllBytes(result.caminhoFinal);

                resultado = Convert.ToBase64String(resultByte);
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.BadRequest,
                    $"", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "", resultado));
        }

        [HttpPost]
        [Route("api/ExportacaoDados/GerarMapaBloqueioAgrupado")]
        public IHttpActionResult GerarMapaBloqueioAgrupado(List<string> listaOrdemManutencao )
        {
            RetornoBloqueioAgrupadoModelo result;

            try
            {
                if (listaOrdemManutencao == null || listaOrdemManutencao.Any() == false)
                    throw new Exception("Os dados de ordem de manutenção, não foram informados!");

                result = this.aprNegocio.GerarMapaBloqueioAgrupado(listaOrdemManutencao);

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
        [Route("api/APR/EscreverLogEmTxt")]
        public IHttpActionResult EscreverLogEmTxt([FromBody] List<long> codApr)
        {
            ArquivoLog result;

            try
            {
                if (codApr == null)
                    throw new Exception("O código da APR não foi informado!");


                result = this.aprNegocio.EscreverLogEmTxt(codApr);
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
        [Route("api/APR/EscreverLogTodasAPRs")]
        public IHttpActionResult EscreverLogTodasAPRs()
        {
            ArquivoLog result;

            try
            {
                result = this.aprNegocio.EscreverLogTodasAPRs();
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