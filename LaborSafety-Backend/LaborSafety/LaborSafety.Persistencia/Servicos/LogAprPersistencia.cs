using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;
using System.Data.Entity;

namespace LaborSafety.Persistencia.Servicos
{
    public class LogAprPersistencia : ILogAprPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;

        public LogAprPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public LOG_APR Inserir(AprModelo apr, long codAprInserido,
           DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            LOG_APR logApr = new LOG_APR();

            logApr.CodLogTipoOperacao = (long)Constantes.TipoOperacaoLog.INSERCAO;
            logApr.CodApr = codAprInserido;
            logApr.CodUsuarioModificador = apr.EightIDUsuarioModificador;
            logApr.DataAlteracao = DateTime.Now;

            entities.LOG_APR.Add(logApr);
            entities.SaveChanges();

            return logApr;
        }

        public LOG_APR Editar(APR aprExistente, AprModelo aprModelo, APR novaApr, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            try
            {
                if (aprExistente == null)
                    throw new Exception("APR não encontrada na base de dados");

                if (aprExistente.NumeroSerie != novaApr.NumeroSerie)
                    throw new Exception("O número de série informado não é igual ao existente na base de dados.");

                if (aprExistente.OrdemManutencao != novaApr.OrdemManutencao)
                    throw new Exception("A ordem de manutenção informada não é igual a existente na base de dados.");

                //Verifica se já existe LOG
                var logExistente = entities.LOG_APR.Where(x => x.CodApr == aprModelo.CodAPR).FirstOrDefault();

                #region Insere os dados do log de inventário

                LOG_APR logApr = new LOG_APR();

                //Armazena os riscos antigos
                string codigoRiscosAntigos = string.Empty;
                foreach (var risco in aprExistente.RISCO_APR)
                    codigoRiscosAntigos += codigoRiscosAntigos.Length == 0 ? risco.CodRisco.ToString() : "," + risco.CodRisco;

                logApr.CodRiscoAprAntigo = codigoRiscosAntigos;

                //Armazena os riscos novos
                string codigoRiscosNovos = string.Empty;
                foreach (var risco in novaApr.RISCO_APR)
                    codigoRiscosNovos += codigoRiscosNovos.Length == 0 ? risco.CodRisco.ToString() : "," + risco.CodRisco;

                logApr.CodRiscoAprNovo = codigoRiscosNovos;

                //Armazena os aprovadores antigos
                string codigoAprovadoresAntigos = string.Empty;
                foreach (var risco in aprExistente.APROVADOR_APR)
                    codigoAprovadoresAntigos += codigoAprovadoresAntigos.Length == 0 ? risco.CodAprovadorAPR.ToString() : "," + risco.CodAprovadorAPR;

                logApr.CodAprovadorAprAntigo = codigoAprovadoresAntigos;

                //Armazena os aprovadores novos
                string codigoAprovadoresNovos = string.Empty;
                foreach (var risco in novaApr.APROVADOR_APR)
                    codigoAprovadoresNovos += codigoAprovadoresNovos.Length == 0 ? risco.CodAprovadorAPR.ToString() : "," + risco.CodAprovadorAPR;

                logApr.CodAprovadorAprNovo = codigoAprovadoresNovos;

                //Armazena os executantes antigos
                string codigoExecutantesAntigos = string.Empty;
                foreach (var executante in aprExistente.EXECUTANTE_APR)
                    codigoExecutantesAntigos += codigoExecutantesAntigos.Length == 0 ? executante.CodExecutanteAPR.ToString() : "," + executante.CodExecutanteAPR;

                logApr.CodExecutanteAprAntigo = codigoExecutantesAntigos;

                //Armazena os executantes novos
                string codigoExecutanteNovos = string.Empty;
                foreach (var executante in novaApr.EXECUTANTE_APR)
                    codigoExecutanteNovos += codigoExecutanteNovos.Length == 0 ? executante.CodExecutanteAPR.ToString() : "," + executante.CodExecutanteAPR;

                logApr.CodExecutanteAprNovo = codigoExecutanteNovos;

                logApr.CodLogTipoOperacao = (long)Constantes.TipoOperacaoLog.EDICAO;

                logApr.CodApr = novaApr.CodAPR;

                logApr.CodUsuarioModificador = aprModelo.EightIDUsuarioModificador;

                logApr.DataAlteracao = DateTime.Now;

                logApr.CodStatusAprAntigo = aprExistente.CodStatusAPR;
                logApr.CodStatusAprNovo = novaApr.CodStatusAPR;

                logApr.DataAprovacaoAntiga = aprExistente.DataAprovacao;
                logApr.DataAprovacaoNova = DateTime.Now;

                logApr.DataEncerramentoAntiga = aprExistente.DataEncerramento;
                logApr.DataEncerramentoNova = DateTime.Now;

                logApr.DataInicioAntiga = aprExistente.DataInicio;
                logApr.DataInicioNova = DateTime.Now;

                logApr.DescricaoAntiga = aprExistente.Descricao;
                logApr.DescricaoNova = novaApr.Descricao;

                logApr.NumeroSerie = novaApr.NumeroSerie;

                logApr.OrdemManutencao = novaApr.OrdemManutencao;

                logApr.RiscoGeralAntigo = aprExistente.RiscoGeral;
                logApr.RiscoGeralNovo = novaApr.RiscoGeral;

                logApr.RiscoGeralAntigo = aprExistente.RiscoGeral;
                logApr.RiscoGeralNovo = novaApr.RiscoGeral;

                entities.LOG_APR.Add(logApr);
                entities.SaveChanges();

                #endregion

                #region Insere os dados do log de operacao da apr

                ////Armazena as operacoes
                //List<LOG_OPERACAO_APR> logOperacoes = new List<LOG_OPERACAO_APR>();

                //foreach (var op in novaApr.OPERACAO_APR)
                //{
                //    LOG_OPERACAO_APR logOperacao = new LOG_OPERACAO_APR();

                //    logOperacao.CodOperacaoApr = op.CodOperacaoAPR;
                //    logOperacao.CodStatusApr = op.CodStatusAPR;
                //    logOperacao.Codigo = op.Codigo;
                //    logOperacao.Descricao = op.Descricao;
                //    logOperacao.CodLI = op.CodLI;
                //    logOperacao.CodDisciplina = op.CodDisciplina;
                //    logOperacao.CodAtvPadrao = op.CodAtvPadrao;
                //    logOperacao.CodLogApr = logApr.CodLogApr;

                //    logOperacoes.Add(logOperacao);
                //}

                //logApr.LOG_OPERACAO_APR = logOperacoes;
                //entities.SaveChanges();

                #endregion

                return logApr;
            }
            catch (Exception e)
            {
                throw;
            }
            

        }

        public void InserirLogOperacaoApr(APR aprAntiga, long codLogApr, DB_LaborSafetyEntities entities = null)
        {

            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            try
            {
                //Armazena as operacoes
                //List<LOG_OPERACAO_APR> logOperacoes = new List<LOG_OPERACAO_APR>();

                foreach (var op in aprAntiga.OPERACAO_APR)
                {
                    if(op.Ativo)
                    {
                        LOG_OPERACAO_APR logOperacao = new LOG_OPERACAO_APR();

                        logOperacao.CodOperacaoApr = op.CodOperacaoAPR;
                        logOperacao.CodStatusApr = op.CodStatusAPR;
                        logOperacao.Codigo = op.Codigo;
                        logOperacao.Descricao = op.Descricao;
                        logOperacao.CodLI = op.CodLI;
                        logOperacao.CodDisciplina = op.CodDisciplina;
                        logOperacao.CodAtvPadrao = op.CodAtvPadrao;
                        logOperacao.CodLogApr = codLogApr;

                        entities.LOG_OPERACAO_APR.Add(logOperacao);
                    }
                }
                entities.SaveChanges();
                //entities.LOG_OPERACAO_APR.Add(logOperacoes);
                //entities.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public void AtualizarLogApr(AprModelo apr, long codApr)
        {
            // atualizar log com novo cod da apr
        }

        public LOG_APR Excluir(AprDelecaoComLogModelo aprDelecaoComLogModelo, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            #region Insere os dados do log de inventário

            LOG_APR logApr = new LOG_APR();

            logApr.CodLogTipoOperacao = (long)Constantes.TipoOperacaoLog.DELECAO;
            logApr.CodApr = aprDelecaoComLogModelo.CodApr;
            logApr.CodUsuarioModificador = aprDelecaoComLogModelo.EightIDUsuarioModificador;
            logApr.DataAlteracao = DateTime.Now;

            entities.LOG_APR.Add(logApr);
            entities.SaveChanges();

            #endregion

            return logApr;
        }

        public List<LOG_APR> ListarLogApr(long codApr, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            //Busca os dados do inventario antigo (anterior à edição)
            var aprExistente = entities.APR
                    .Include(x => x.RISCO_APR)
                    .Include(x => x.OPERACAO_APR).Where(invAmb => invAmb.CodAPR == codApr).FirstOrDefault();

            if (aprExistente == null)
                throw new Exception("APR não encontrada na base de dados");

            var logAprExistente = entities.LOG_APR.Where(x => x.CodApr == aprExistente.CodAPR).ToList();

            if (logAprExistente == null)
                throw new Exception("Log da APR não encontrado na base de dados");

            List<LOG_APR> logApr = new List<LOG_APR>();

            foreach (var item in logAprExistente)
            {
                logApr.Add(item);
            }

            return logApr;
        }

        public List<LOG_APR> BuscarTodos(DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var logs = entities.LOG_APR.ToList();

            return logs;
        }

        public List<LOG_APR> BuscarPorInventario(long codApr, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var logs = entities.LOG_APR.Where(x => x.CodApr == codApr).ToList();


            if (logs.Count == 0)
                throw new Exception("Não existe(m) log(s) para a APR informada!");

            return logs;

        }

        public List<LOG_OPERACAO_APR> ListarLogOperacaoApr(long codApr, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var aprExistente = entities.APR
                .Include(x => x.OPERACAO_APR)
                .Where(invAmb => invAmb.CodAPR == codApr).FirstOrDefault();

            if (aprExistente == null)
                throw new Exception("APR não encontrada na base de dados");

            var logAprExistente = entities.LOG_OPERACAO_APR.
                Include(x => x.LOG_APR)
                .Where(x => x.CodLogApr == x.LOG_APR.CodLogApr && x.LOG_APR.CodApr == aprExistente.CodAPR).ToList();

            if (logAprExistente == null)
                throw new Exception("Log da APR e/ou operações não encontrado na base de dados");

            return logAprExistente;
        }

    }
}
