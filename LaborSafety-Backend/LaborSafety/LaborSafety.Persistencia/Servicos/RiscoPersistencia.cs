
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class RiscoPersistencia : IRiscoPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public RiscoPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public RISCO ListarRiscoPorId(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                RISCO risco = entities.RISCO
                    .Where(risc => risc.CodRisco == id).FirstOrDefault();
                return risco;
            }
        }

        public RISCO ListarRiscoPorNome(string nome)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                RISCO risco = entities.RISCO
                    .Where(risc => risc.Nome == nome).FirstOrDefault();
                return risco;
            }
        }

        public IEnumerable<RISCO> ListarTodosRiscos()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.RISCO
                    .ToList();

                return resultado;
            }
        }

        public List<TIPO_RISCO> ListarTiposRisco()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.TIPO_RISCO.ToList();
                return resultado;
            }
        }

        public IEnumerable<RISCO> ListarPorTipoRisco(long codTipo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.RISCO
                    .Where(risc => risc.CodTipoRisco == codTipo)
                    .ToList();

                return resultado;
            }
        }

        public RISCO ListarRiscoPorNomeETipo(string nome, long? tipo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                RISCO risco;
                if (tipo == null)
                {
                    risco = entities.RISCO
                        .Where(risc => risc.Nome == nome && risc.CodTipoRisco != (long)Constantes.TipoRisco.INV_ATV).FirstOrDefault();
                }
                else
                {
                    risco = entities.RISCO
                        .Where(risc => risc.Nome == nome && risc.CodTipoRisco == tipo).FirstOrDefault();
                }

                return risco;
            }
        }
    }
}
