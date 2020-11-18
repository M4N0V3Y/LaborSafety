using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class LogPerfilFuncionalidadePersistencia : ILogPerfilFuncionalidadePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;

        public LogPerfilFuncionalidadePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public LOG_PERFIL_FUNCIONALIDADE Inserir (LOG_PERFIL_FUNCIONALIDADE modelo, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            LOG_PERFIL_FUNCIONALIDADE logPerfil = new LOG_PERFIL_FUNCIONALIDADE();

            logPerfil.CodigosFuncionalidadesAnteriores = modelo.CodigosFuncionalidadesAnteriores;
            logPerfil.CodigosNovasFuncionalidades = modelo.CodigosNovasFuncionalidades;
            logPerfil.CodPerfil = modelo.CodPerfil;
            logPerfil.CodTela = modelo.CodTela;
            logPerfil.CodUsuarioModificador = modelo.CodUsuarioModificador;
            logPerfil.DataAlteracao = DateTime.Now;

            entities.LOG_PERFIL_FUNCIONALIDADE.Add(logPerfil);
            entities.SaveChanges();

            return logPerfil;
        }

        public LOG_PERFIL_FUNCIONALIDADE BuscaEInsereFuncionalidades (string eightIDUsuarioModificador, long codPerfil, long codTela,
            List<FUNCIONALIDADE> novasFuncionalidades, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            //Busca as funcionalidades antigas
            var perfilFuncionalidadeExitente = entities.PERFIL_FUNCIONALIDADE.Where(x => x.CodPerfil == codPerfil && x.CodTela == codTela).ToList();

            //Se não houverem registros antigos, não insere o log
            if (perfilFuncionalidadeExitente == null)
                return null;

            //Armazena os códigos das funcionalidades
            string codFuncionalidadesAntigas = "";
            string codNovasFuncionalidades = "";

            List<FUNCIONALIDADE> funcionalidadesAntigas = new List<FUNCIONALIDADE>();
            foreach (var item in perfilFuncionalidadeExitente)
            {
                funcionalidadesAntigas.Add(item.FUNCIONALIDADE);
                codFuncionalidadesAntigas += codFuncionalidadesAntigas.Length == 0 ? item.FUNCIONALIDADE.CodFuncionalidade.ToString() 
                    : "," + item.FUNCIONALIDADE.CodFuncionalidade.ToString();
                    //string.Join(",", item.FUNCIONALIDADE.CodFuncionalidade);
            }

            foreach (var item in novasFuncionalidades)
            {
                codNovasFuncionalidades += codNovasFuncionalidades.Length == 0 ? item.CodFuncionalidade.ToString()
                    : "," + item.CodFuncionalidade.ToString();
            }

            //Insere a diferença
            if (codFuncionalidadesAntigas != codNovasFuncionalidades)
            {

                LOG_PERFIL_FUNCIONALIDADE log = new LOG_PERFIL_FUNCIONALIDADE();
                log.CodUsuarioModificador = eightIDUsuarioModificador;
                log.CodPerfil = codPerfil;
                log.CodTela = codTela;
                log.CodigosFuncionalidadesAnteriores = codFuncionalidadesAntigas;
                log.CodigosNovasFuncionalidades = codNovasFuncionalidades;
                log.DataAlteracao = DateTime.Now;

                var result = this.Inserir(log, entities);
                return result;
            }
            else
                return null;
           
        }

        public List<LOG_PERFIL_FUNCIONALIDADE> BuscarTodos(DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var logs = entities.LOG_PERFIL_FUNCIONALIDADE.ToList();

            return logs;
        }

        public List<LOG_PERFIL_FUNCIONALIDADE> BuscarPorPerfil(long codPerfil, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var logs = entities.LOG_PERFIL_FUNCIONALIDADE.Where(x => x.CodPerfil == codPerfil).ToList();

            if (logs.Count == 0)
                throw new Exception("Não existe(m) log(s) para o inventário informado!");

            return logs;
        }

    }
}
