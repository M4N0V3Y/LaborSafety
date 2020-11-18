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
    public class LogInventarioAtividadePersistencia : ILogInventarioAtividadePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;

        public LogInventarioAtividadePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public LOG_INVENTARIO_ATIVIDADE Editar(InventarioAtividadeModelo novoInventario, List<LOCAL_INSTALACAO> locaisInstalacao,
            DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            //Busca os dados do inventario antigo (anterior à edição)
            var inventarioAtividadeExistente = entities.INVENTARIO_ATIVIDADE
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE)
                .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                .Where(invAmb => invAmb.CodInventarioAtividade == novoInventario.CodInventarioAtividade && invAmb.Ativo).FirstOrDefault();

            if (inventarioAtividadeExistente == null)
                throw new Exception("Inventário não encontrado na base de dados");

            //Verifica se já existe LOG
            var logExistente = entities.LOG_INVENTARIO_ATIVIDADE.Where(x => x.CodInventarioAtividade == novoInventario.CodInventarioAtividade).FirstOrDefault();

            #region Insere os dados do log de inventário

            LOG_INVENTARIO_ATIVIDADE logInventario = new LOG_INVENTARIO_ATIVIDADE();

            logInventario.CodLogTipoOperacao = (long)Constantes.TipoOperacaoLog.EDICAO;

            //Armazena os LI's antigos
            string codigoLIsAntigos = string.Empty;
            foreach (var li in inventarioAtividadeExistente.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                codigoLIsAntigos += codigoLIsAntigos.Length == 0 ? li.CodLocalInstalacao.ToString() : "," + li.CodLocalInstalacao;

            logInventario.CodLIsAntigos = codigoLIsAntigos;

            //Armazena os LI's novos
            string codigoLIsNovos = string.Empty;
            foreach (var li in locaisInstalacao)
                codigoLIsNovos += codigoLIsNovos.Length == 0 ? li.CodLocalInstalacao.ToString() : "," + li.CodLocalInstalacao;

            logInventario.CodLIsNovos = codigoLIsNovos;

            var ultimoLog = entities.LOG_INVENTARIO_ATIVIDADE.Where(x => x.CodInventarioAtividade == novoInventario.CodInventarioAtividade).OrderByDescending(u => u.CodInventariosAntigos).FirstOrDefault();

            if (ultimoLog == null)
                logInventario.CodInventariosAntigos = $"{inventarioAtividadeExistente.CodInventarioAtividade}";

            else
            {
                if (string.IsNullOrEmpty(ultimoLog.CodInventariosAntigos))
                    logInventario.CodInventariosAntigos = $"{inventarioAtividadeExistente.CodInventarioAtividade}";

                else
                    logInventario.CodInventariosAntigos = $"{ultimoLog.CodInventariosAntigos},{inventarioAtividadeExistente.CodInventarioAtividade}";
            }

            logInventario.CodInventarioAtividade = novoInventario.CodInventarioAtividade;
            logInventario.CodUsuarioModificador = novoInventario.EightIDUsuarioModificador;

            logInventario.CodPesoAntigo = inventarioAtividadeExistente.CodPeso;
            logInventario.CodPesoNovo = novoInventario.CodPeso;
            logInventario.CodPerfilCatalogoAntigo = inventarioAtividadeExistente.CodPerfilCatalogo;
            logInventario.CodPerfilCatalogoNovo = novoInventario.CodPerfilCatalogo;
            logInventario.CodDuracaoAntiga = inventarioAtividadeExistente.CodDuracao;
            logInventario.CodDuracaoNovo = novoInventario.CodDuracao;
            logInventario.CodAtividadeAntiga = inventarioAtividadeExistente.CodAtividade;
            logInventario.CodAtividadeNova = novoInventario.CodAtividade;
            logInventario.CodDisciplinaAntiga = inventarioAtividadeExistente.CodDisciplina;
            logInventario.CodDisciplinaNova = novoInventario.CodDisciplina;

            logInventario.DescricaoAntiga = inventarioAtividadeExistente.Descricao;
            logInventario.DescricaoNova = novoInventario.Descricao;
            logInventario.ObsGeralAntiga = inventarioAtividadeExistente.ObservacaoGeral;
            logInventario.ObsGeralNova = novoInventario.ObservacaoGeral;
            logInventario.RiscoGeralAntigo = inventarioAtividadeExistente.RiscoGeral;
            logInventario.RiscoGeralNovo = novoInventario.RiscoGeral;
            logInventario.DataAlteracao = DateTime.Now;

            entities.LOG_INVENTARIO_ATIVIDADE.Add(logInventario);
            entities.SaveChanges();

            #endregion

            #region Insere os dados do log de risco do inventário

            //Armazena os riscos
            List<LOG_RISCO_INVENTARIO_ATIVIDADE> logRiscos = new List<LOG_RISCO_INVENTARIO_ATIVIDADE>();

            foreach (var risco in inventarioAtividadeExistente.RISCO_INVENTARIO_ATIVIDADE)
            {
                LOG_RISCO_INVENTARIO_ATIVIDADE logRisco = new LOG_RISCO_INVENTARIO_ATIVIDADE();
                string codigosEPIs = string.Empty;

                //Busca todos os EPI's
                if (risco.EPI_RISCO_INVENTARIO_ATIVIDADE.Count > 0)
                {
                    foreach (var epi in risco.EPI_RISCO_INVENTARIO_ATIVIDADE)
                        codigosEPIs += codigosEPIs.Length == 0 ? epi.CodEPI.ToString() : "," + epi.CodEPI;

                    logRisco.CodigosEPIs = codigosEPIs;
                }
                logRisco.ProcedimentoAplicavel = risco.ProcedimentoAplicavel;
                logRisco.ContraMedidas = risco.ContraMedidas;
                logRisco.FonteGeradora = risco.FonteGeradora;
                logRisco.CodRiscoAtividade = risco.CodRisco;
                logRisco.CodSeveridade = risco.CodSeveridade;

                logRisco.CodLogInventarioAtividade = logInventario.CodLogInventarioAtividade;

                entities.LOG_RISCO_INVENTARIO_ATIVIDADE.Add(logRisco);
                entities.SaveChanges();

                logRiscos.Add(logRisco);
            }

            logInventario.LOG_RISCO_INVENTARIO_ATIVIDADE = logRiscos;
            entities.SaveChanges();

            #endregion

            return logInventario;
        }


        public void AtualizarCodInventarioLog(long ?codInventarioAntigo, long codInventarioAtual,
           DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var logAntigo = entities.LOG_INVENTARIO_ATIVIDADE.Where(x => x.CodInventarioAtividade == codInventarioAntigo).ToList();

            foreach (var itemLog in logAntigo)
            {
                itemLog.CodInventarioAtividade = codInventarioAtual;
            }

            entities.SaveChanges();
        }


        public LOG_INVENTARIO_ATIVIDADE Inserir(InventarioAtividadeModelo novoInventario, long codInventarioInserido,
    DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            #region Insere os dados do log de inventário

            LOG_INVENTARIO_ATIVIDADE logInventario = new LOG_INVENTARIO_ATIVIDADE();

            logInventario.CodLogTipoOperacao = (long)Constantes.TipoOperacaoLog.INSERCAO;
            logInventario.CodInventarioAtividade = codInventarioInserido;
            logInventario.CodUsuarioModificador = novoInventario.EightIDUsuarioModificador;
            logInventario.DataAlteracao = DateTime.Now;

            entities.LOG_INVENTARIO_ATIVIDADE.Add(logInventario);
            entities.SaveChanges();

            #endregion

            return logInventario;
        }


        public LOG_INVENTARIO_ATIVIDADE Excluir(InventarioAtividadeDelecaoComLogModelo inventarioAtividadeDelecaoComLogModelo, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            #region Insere os dados do log de inventário

            LOG_INVENTARIO_ATIVIDADE logInventario = new LOG_INVENTARIO_ATIVIDADE();

            logInventario.CodLogTipoOperacao = (long)Constantes.TipoOperacaoLog.DELECAO;
            logInventario.CodInventarioAtividade = inventarioAtividadeDelecaoComLogModelo.CodInventarioAtividade;
            logInventario.CodUsuarioModificador = inventarioAtividadeDelecaoComLogModelo.EightIDUsuarioModificador;
            logInventario.DataAlteracao = DateTime.Now;

            entities.LOG_INVENTARIO_ATIVIDADE.Add(logInventario);
            entities.SaveChanges();

            #endregion

            return logInventario;
        }


        public List<LOG_INVENTARIO_ATIVIDADE> ListarLogInventario(long codInventarioAtividade, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            //Busca os dados do inventario antigo (anterior à edição)
            var inventarioAtividadeExistente = entities.INVENTARIO_ATIVIDADE
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE)
                .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                .Where(invAmb => invAmb.CodInventarioAtividade == codInventarioAtividade).FirstOrDefault();

            if (inventarioAtividadeExistente == null)
                throw new Exception("Inventário não encontrado na base de dados");

            var logInventarioAtividadeExistente = entities.LOG_INVENTARIO_ATIVIDADE.Where(x => x.CodInventarioAtividade == inventarioAtividadeExistente.CodInventarioAtividade).ToList();

            if (logInventarioAtividadeExistente == null)
                throw new Exception("Log do inventário não encontrado na base de dados");

            List<LOG_INVENTARIO_ATIVIDADE> logInventario = new List<LOG_INVENTARIO_ATIVIDADE>();

            foreach (var item in logInventarioAtividadeExistente)
            {
                logInventario.Add(item);
            }

            return logInventario;
        }


        public List<LOG_INVENTARIO_ATIVIDADE> BuscarTodos(DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var logs = entities.LOG_INVENTARIO_ATIVIDADE.ToList();

            return logs;
        }

        public List<LOG_INVENTARIO_ATIVIDADE> BuscarPorInventario(long codInventario, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var logs = entities.LOG_INVENTARIO_ATIVIDADE.Where(x => x.CodInventarioAtividade == codInventario).ToList();


            if (logs.Count == 0)
                throw new Exception("Não existe(m) log(s) para o inventário informado!");

            return logs;

        }

        public List<LOG_RISCO_INVENTARIO_ATIVIDADE> ListarLogRiscosInventario(long codInventarioAtividade, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var inventarioAtividadeExistente = entities.INVENTARIO_ATIVIDADE
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE)
                .Include(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(sev => sev.SEVERIDADE))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                .Include(x => x.RISCO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_INVENTARIO_ATIVIDADE))
                .Where(invAmb => invAmb.CodInventarioAtividade == codInventarioAtividade).FirstOrDefault();

            if (inventarioAtividadeExistente == null)
                throw new Exception("Inventário não encontrado na base de dados");

            var logInventarioAtividadeExistente = entities.LOG_RISCO_INVENTARIO_ATIVIDADE.
                Include(x => x.LOG_INVENTARIO_ATIVIDADE)
                .Where(x => x.CodLogInventarioAtividade == x.LOG_INVENTARIO_ATIVIDADE.CodLogInventarioAtividade && x.LOG_INVENTARIO_ATIVIDADE.CodInventarioAtividade == inventarioAtividadeExistente.CodInventarioAtividade).ToList();

            if (logInventarioAtividadeExistente == null)
                throw new Exception("Log do inventário e/ou riscos não encontrado na base de dados");

            return logInventarioAtividadeExistente;
        }

    }
}
