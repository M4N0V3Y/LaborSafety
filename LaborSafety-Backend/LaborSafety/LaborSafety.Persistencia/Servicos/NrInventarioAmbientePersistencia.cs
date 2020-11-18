using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using System.Data.Entity;

namespace LaborSafety.Persistencia.Servicos
{
    public class NrInventarioAmbientePersistencia : INrInventarioAmbientePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public NrInventarioAmbientePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public void InserirNrInventarioAmbiente(NrInventarioAmbienteModelo nrInventarioAmbienteModelo)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        var nrInventario = new NR_INVENTARIO_AMBIENTE()
                        {
                            CodInventarioAmbiente = nrInventarioAmbienteModelo.CodInventarioAmbiente,
                            CodNR = nrInventarioAmbienteModelo.CodNR,
                            Ativo = true
                        };

                        entities.NR_INVENTARIO_AMBIENTE.Add(nrInventario);
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
