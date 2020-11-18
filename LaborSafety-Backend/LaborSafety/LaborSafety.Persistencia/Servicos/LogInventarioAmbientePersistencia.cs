using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class LogInventarioAmbientePersistencia : ILogInventarioAmbientePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;

        public LogInventarioAmbientePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public LOG_INVENTARIO_AMBIENTE Editar(List<LOCAL_INSTALACAO> novosLIs, InventarioAmbienteModelo novoInventario,
            DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            //Busca os dados do inventario antigo (anterior à edição)
            var inventarioAmbienteExistente = entities.INVENTARIO_AMBIENTE
                .Include(x => x.NR_INVENTARIO_AMBIENTE)
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE)
                .Include(x => x.LOCAL_INSTALACAO)
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(sev => sev.SEVERIDADE))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(prob => prob.PROBABILIDADE))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_INVENTARIO_AMBIENTE))
                .Where(invAmb => invAmb.CodInventarioAmbiente == novoInventario.CodInventarioAmbiente && invAmb.Ativo).FirstOrDefault();

            if (inventarioAmbienteExistente == null)
                throw new Exception("Inventário não encontrado na base de dados");

            //Verifica se já existe LOG
            var logExistente = entities.LOG_INVENTARIO_AMBIENTE.Where(x => x.CodInventario == novoInventario.CodInventarioAmbiente).FirstOrDefault();

            #region Insere os dados do log de inventário

            LOG_INVENTARIO_AMBIENTE logInventario = new LOG_INVENTARIO_AMBIENTE();

            logInventario.CodAmbienteAntigo = inventarioAmbienteExistente.CodAmbiente;
            logInventario.CodAmbienteNovo = novoInventario.CodAmbiente;

            logInventario.CodLogTipoOperacao = (long)Constantes.TipoOperacaoLog.EDICAO;

            //Armazena os LI's antigos
            string codigoLIsAntigos = string.Empty;
            foreach (var li in inventarioAmbienteExistente.LOCAL_INSTALACAO)
                codigoLIsAntigos += codigoLIsAntigos.Length == 0 ? li.CodLocalInstalacao.ToString() : "," + li.CodLocalInstalacao;

            logInventario.CodigosLIsAntigos = codigoLIsAntigos;

            //Armazena os LI's novos
            string codigoLIsNovos = string.Empty;
            foreach (var li in novoInventario.LOCAL_INSTALACAO_MODELO)
                codigoLIsNovos += codigoLIsNovos.Length == 0 ? li.CodLocalInstalacao.ToString() : "," + li.CodLocalInstalacao;

            logInventario.CodigosLIsNovos = codigoLIsNovos;

            //Armazena as NR's antigas
            string codigoNRsAntigas = string.Empty;
            foreach (var nr in inventarioAmbienteExistente.NR_INVENTARIO_AMBIENTE)
                codigoNRsAntigas += codigoNRsAntigas.Length == 0 ? nr.CodNR.ToString() : "," + nr.CodNR;

            logInventario.CodigosNRsAntigas = codigoNRsAntigas;

            //Armazena as NR's novas
            string codigoNRsNovas = string.Empty;
            foreach (var nr in novoInventario.NR_INVENTARIO_AMBIENTE)
                codigoNRsNovas += codigoNRsNovas.Length == 0 ? nr.CodNR.ToString() : "," + nr.CodNR;

            logInventario.CodigosNRsNovas = codigoNRsNovas;

            var ultimoLog = entities.LOG_INVENTARIO_AMBIENTE.Where(x => x.CodInventario == novoInventario.CodInventarioAmbiente)
                .OrderByDescending(u => u.CodInventariosAntigos).FirstOrDefault();

            if (ultimoLog == null)
                logInventario.CodInventariosAntigos = $"{inventarioAmbienteExistente.CodInventarioAmbiente}";

            else
            {
                if (string.IsNullOrEmpty(ultimoLog.CodInventariosAntigos))
                    logInventario.CodInventariosAntigos = $"{inventarioAmbienteExistente.CodInventarioAmbiente}";

                else
                    logInventario.CodInventariosAntigos = $"{ultimoLog.CodInventariosAntigos},{inventarioAmbienteExistente.CodInventarioAmbiente}";
            }

            logInventario.CodInventario = novoInventario.CodInventarioAmbiente;
            logInventario.CodUsuarioModificador = novoInventario.EightIDUsuarioModificador;
            logInventario.DescricaoAntiga = inventarioAmbienteExistente.Descricao;
            logInventario.DescricaoNova = novoInventario.Descricao;
            logInventario.ObsGeralAntiga = inventarioAmbienteExistente.ObservacaoGeral;
            logInventario.ObsGeralNova = novoInventario.ObservacaoGeral;
            logInventario.RiscoGeralAntigo = inventarioAmbienteExistente.RiscoGeral;
            logInventario.RiscoGeralNovo = novoInventario.RiscoGeral;
            logInventario.DataAlteracao = DateTime.Now;

            entities.LOG_INVENTARIO_AMBIENTE.Add(logInventario);
            entities.SaveChanges();

            #endregion

            #region Insere os dados do log de risco do inventário

            //Armazena os riscos
            List<LOG_RISCO_INVENTARIO_AMBIENTE> logRiscos = new List<LOG_RISCO_INVENTARIO_AMBIENTE>();

            foreach (var risco in inventarioAmbienteExistente.RISCO_INVENTARIO_AMBIENTE)
            {
                LOG_RISCO_INVENTARIO_AMBIENTE logRisco = new LOG_RISCO_INVENTARIO_AMBIENTE();
                string codigosEPIs = string.Empty;

                //Busca todos os EPI's
                if (risco.EPI_RISCO_INVENTARIO_AMBIENTE.Count > 0)
                {
                    foreach (var epi in risco.EPI_RISCO_INVENTARIO_AMBIENTE)
                        codigosEPIs += codigosEPIs.Length == 0 ? epi.CodEPI.ToString() : "," + epi.CodEPI;

                    logRisco.CodigosEPIs = codigosEPIs;
                }
                logRisco.CodProbabilidade = risco.CodProbabilidade;
                logRisco.ProcedimentosAplicaveis = risco.ProcedimentosAplicaveis;
                logRisco.ContraMedidas = risco.ContraMedidas;
                logRisco.FonteGeradora = risco.FonteGeradora;
                logRisco.CodRisco = risco.CodRiscoAmbiente;
                logRisco.CodSeveridade = risco.CodSeveridade;

                logRisco.CodLogInventarioAmbiente = logInventario.CodLogInventarioAmbiente;

                entities.LOG_RISCO_INVENTARIO_AMBIENTE.Add(logRisco);
                entities.SaveChanges();

                logRiscos.Add(logRisco);
            }

            logInventario.LOG_RISCO_INVENTARIO_AMBIENTE = logRiscos;
            entities.SaveChanges();

            #endregion

            return logInventario;
        }

        public void AtualizarCodInventarioLog(long ?codInventarioAntigo, long codInventarioAtual,
           DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            if (codInventarioAntigo == 0)
                codInventarioAntigo = null;

            var logAntigo = entities.LOG_INVENTARIO_AMBIENTE.Where(x => x.CodInventario == codInventarioAntigo).ToList();

            foreach (var itemLog in logAntigo)
            {
                itemLog.CodInventario = codInventarioAtual;
            }

            entities.SaveChanges();
        }

        public LOG_INVENTARIO_AMBIENTE Inserir(InventarioAmbienteModelo novoInventario, long codInventarioInserido,
            DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            entities.Configuration.AutoDetectChangesEnabled = false;

            #region Insere os dados do log de inventário

            LOG_INVENTARIO_AMBIENTE logInventario = new LOG_INVENTARIO_AMBIENTE();

            logInventario.CodLogTipoOperacao = (long)Constantes.TipoOperacaoLog.INSERCAO;
            logInventario.CodInventario = codInventarioInserido;
            logInventario.CodUsuarioModificador = novoInventario.EightIDUsuarioModificador;
            logInventario.DataAlteracao = DateTime.Now;

            entities.LOG_INVENTARIO_AMBIENTE.Add(logInventario);

            entities.ChangeTracker.DetectChanges();
            entities.SaveChanges();

            entities.Configuration.AutoDetectChangesEnabled = true;

            #endregion

            return logInventario;
        }

        public LOG_INVENTARIO_AMBIENTE Excluir(InventarioAmbienteDelecaoComLogModelo inventarioAmbienteDelecaoComLogModelo, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            #region Insere os dados do log de inventário

            LOG_INVENTARIO_AMBIENTE logInventario = new LOG_INVENTARIO_AMBIENTE();

            logInventario.CodLogTipoOperacao = (long)Constantes.TipoOperacaoLog.DELECAO;
            logInventario.CodInventario = inventarioAmbienteDelecaoComLogModelo.CodInventarioAmbiente;
            logInventario.CodUsuarioModificador = inventarioAmbienteDelecaoComLogModelo.EightIDUsuarioModificador;
            logInventario.DataAlteracao = DateTime.Now;

            entities.LOG_INVENTARIO_AMBIENTE.Add(logInventario);
            entities.SaveChanges();

            #endregion

            return logInventario;
        }

        public List<LOG_INVENTARIO_AMBIENTE> ListarLogInventario(long codInventarioAmbiente, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            //Busca os dados do inventario antigo (anterior à edição)
            var inventarioAmbienteExistente = entities.INVENTARIO_AMBIENTE
                .Include(x => x.NR_INVENTARIO_AMBIENTE)
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE)
                .Include(x => x.LOCAL_INSTALACAO)
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(sev => sev.SEVERIDADE))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(prob => prob.PROBABILIDADE))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_INVENTARIO_AMBIENTE))
                .Where(invAmb => invAmb.CodInventarioAmbiente == codInventarioAmbiente).FirstOrDefault();

            if (inventarioAmbienteExistente == null)
                throw new Exception("Inventário não encontrado na base de dados");

            var logInventarioAmbienteExistente = entities.LOG_INVENTARIO_AMBIENTE.Where(x => x.CodInventario == inventarioAmbienteExistente.CodInventarioAmbiente).ToList();

            if(logInventarioAmbienteExistente == null)
                throw new Exception("Log do inventário não encontrado na base de dados");

            List<LOG_INVENTARIO_AMBIENTE> logInventario = new List<LOG_INVENTARIO_AMBIENTE>();

            foreach (var item in logInventarioAmbienteExistente)
            {
                    logInventario.Add(item);
            }
            
            return logInventario;
        }

        public List<LOG_INVENTARIO_AMBIENTE> BuscarTodos(DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var logs = entities.LOG_INVENTARIO_AMBIENTE.ToList();

            return logs;
        }

        public List<LOG_INVENTARIO_AMBIENTE> BuscarPorInventario(long codInventario, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var logs = entities.LOG_INVENTARIO_AMBIENTE.Where(x => x.CodInventario == codInventario).ToList();


            if (logs.Count == 0)
                throw new Exception("Não existe(m) log(s) para o inventário informado!");

            return logs;

        }

        public List<LOG_RISCO_INVENTARIO_AMBIENTE> ListarLogRiscosInventario(long codInventarioAmbiente, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var inventarioAmbienteExistente = entities.INVENTARIO_AMBIENTE
                .Include(x => x.NR_INVENTARIO_AMBIENTE)
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE)
                .Include(x => x.LOCAL_INSTALACAO)
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(sev => sev.SEVERIDADE))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(prob => prob.PROBABILIDADE))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_INVENTARIO_AMBIENTE))
                .Where(invAmb => invAmb.CodInventarioAmbiente == codInventarioAmbiente).FirstOrDefault();

            if (inventarioAmbienteExistente == null)
                throw new Exception("Inventário não encontrado na base de dados");

            var logInventarioAmbienteExistente = entities.LOG_RISCO_INVENTARIO_AMBIENTE.
                Include(x => x.LOG_INVENTARIO_AMBIENTE)
                .Where(x => x.CodLogInventarioAmbiente == x.LOG_INVENTARIO_AMBIENTE.CodLogInventarioAmbiente && x.LOG_INVENTARIO_AMBIENTE.CodInventario == inventarioAmbienteExistente.CodInventarioAmbiente).ToList();

            if (logInventarioAmbienteExistente == null)
                throw new Exception("Log do inventário e/ou riscos não encontrado na base de dados");

            return logInventarioAmbienteExistente;
        }
    }
}
