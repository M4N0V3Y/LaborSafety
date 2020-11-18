using System.Collections.Generic;
using System.Linq;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class PesoPersistencia : IPesoPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public PesoPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public PESO ListarPesoPorId(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                PESO peso = entities.PESO
                    .Where(pes => pes.CodPeso == id && pes.CodPeso != (long)Constantes.PesoFisico.SEM_PESO).FirstOrDefault();
                return peso;
            }
        }

        public PESO ListarPesoPorIndice(int indice)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                PESO peso = entities.PESO
                    .Where(pes => pes.Indice == indice && pes.CodPeso != (long)Constantes.PesoFisico.SEM_PESO).FirstOrDefault();
                return peso;
            }
        }

        public PESO ListarPesoPorNome(string nome, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            PESO peso = entities.PESO
                .Where(pes => pes.Nome.ToUpper() == nome.ToUpper() && pes.CodPeso != (long)Constantes.PesoFisico.SEM_PESO).FirstOrDefault();
            return peso;

        }

        public IEnumerable<PESO> ListarTodosPesos()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.PESO
                    .Where(pes => pes.CodPeso != (long)Constantes.PesoFisico.SEM_PESO)
                    .ToList();

                return resultado;
            }
        }

        public List<PESO> ListarTodosPesosExportacao(List<long> pesos)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.PESO.Where(p => pesos.Contains(p.CodPeso) && p.CodPeso != (long)Constantes.PesoFisico.SEM_PESO).ToList();

                return resultado;
            }
        }
    }
}
