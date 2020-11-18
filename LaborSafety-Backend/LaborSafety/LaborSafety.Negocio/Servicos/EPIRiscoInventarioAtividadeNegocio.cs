using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class EPIRiscoInventarioAtividadeNegocio : IEPIRiscoInventarioAtividadeNegocio
    {
        private readonly IEPIRiscoInventarioAtividadePersistencia epiPersistencia;
        public EPIRiscoInventarioAtividadeNegocio(IEPIRiscoInventarioAtividadePersistencia epiPersistencia,
            Validador<EPIRiscoInventarioAtividadeModelo> validadorEPI)
        {
            this.epiPersistencia = epiPersistencia;
        }

        public List<EPIRiscoInventarioAtividadeModelo> ListarEPIsPorRisco(long codRisco)
        {
            List<EPI_RISCO_INVENTARIO_ATIVIDADE> epis = epiPersistencia.ListarEPIPorRisco(codRisco);

            if (epis == null)
                throw new KeyNotFoundException($@" EPI(s) não encontrado(s) para o risco {codRisco}.");

            List<EPIRiscoInventarioAtividadeModelo> result = new List<EPIRiscoInventarioAtividadeModelo>();

            foreach (var epi in epis)
            {
                EPIRiscoInventarioAtividadeModelo epiRiscoInventarioAtividadeModelo = new EPIRiscoInventarioAtividadeModelo();
                epiRiscoInventarioAtividadeModelo.CodEpiRiscoInventarioAtividade = epi.CodRiscoInventarioAtividade;
                epiRiscoInventarioAtividadeModelo.CodRiscoInventarioAtividade = epi.CodRiscoInventarioAtividade;
                epiRiscoInventarioAtividadeModelo.CodEPI = epi.CodEPI;

                result.Add(epiRiscoInventarioAtividadeModelo);
            }

            return result;
        }
    }
}
