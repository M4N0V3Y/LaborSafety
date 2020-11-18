using System.Collections.Generic;
using System.Linq;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class DuracaoPersistencia : IDuracaoPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public DuracaoPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public DURACAO ListarDuracaoPorId(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                DURACAO duracao = entities.DURACAO
                    .Where(dur => dur.CodDuracao == id).FirstOrDefault();
                return duracao;
            }
        }

        public DURACAO ListarDuracaoPorIndice(int indice)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                DURACAO duracao = entities.DURACAO
                    .Where(dur => dur.Indice == indice).FirstOrDefault();
                return duracao;
            }
        }

        public DURACAO ListarDuracaoPorNome(string nome)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                DURACAO duracao = entities.DURACAO
                    .Where(dur => dur.Nome == nome).FirstOrDefault();
                return duracao;
            }
        }

        public IEnumerable<DURACAO> ListarTodasDuracoes()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.DURACAO
                    .ToList();

                return resultado;
            }
        }

        public List<DURACAO> ListarTodasDuracoesExportacao(List<long> duracaoes)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.DURACAO.Where(p => duracaoes.Contains(p.CodDuracao)).ToList();

                return resultado;
            }
        }
    }
}
