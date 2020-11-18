using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class RiscoInventarioAmbientePersistencia : IRiscoInventarioAmbientePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public RiscoInventarioAmbientePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public void InserirRiscoInventarioAmbiente(RiscoInventarioAmbienteModelo riscoInventarioAmbienteModelo)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        var inventario = new RISCO_INVENTARIO_AMBIENTE()
                        {
                            CodInventarioAmbiente = riscoInventarioAmbienteModelo.CodInventarioAmbiente,
                            CodRiscoAmbiente = riscoInventarioAmbienteModelo.CodRiscoAmbiente,
                            CodSeveridade = riscoInventarioAmbienteModelo.CodSeveridade,
                            CodProbabilidade = riscoInventarioAmbienteModelo.CodProbabilidade,
                            FonteGeradora = riscoInventarioAmbienteModelo.FonteGeradora,
                            ProcedimentosAplicaveis = riscoInventarioAmbienteModelo.ProcedimentosAplicaveis,
                            ContraMedidas = riscoInventarioAmbienteModelo.ContraMedidas,
                            Ativo = true
                        };

                        entities.RISCO_INVENTARIO_AMBIENTE.Add(inventario);
                        entities.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        throw exception;
                    }
                }
            }
        }
    }
}
