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
    public class EPIRiscoInventarioAmbienteNegocio : IEPIRiscoInventarioAmbienteNegocio
    {
        private readonly IEPIRiscoInventarioAmbientePersistencia epiPersistencia;
        public EPIRiscoInventarioAmbienteNegocio(IEPIRiscoInventarioAmbientePersistencia epiPersistencia,
            Validador<EPIRiscoInventarioAmbienteModelo> validadorEPI)
        {
            this.epiPersistencia = epiPersistencia;
        }

        public List<EPIRiscoInventarioAmbienteModelo> ListarEPIsPorRisco(long codRisco)
        {
            List<EPI_RISCO_INVENTARIO_AMBIENTE> epis = epiPersistencia.ListarEPIPorRisco(codRisco);

            if (epis == null)
                throw new KeyNotFoundException($@" EPI(s) não encontrado(s) para o risco {codRisco}.");

            List<EPIRiscoInventarioAmbienteModelo> result = new List<EPIRiscoInventarioAmbienteModelo>();

            foreach (var epi in epis)
            {
                EPIRiscoInventarioAmbienteModelo ePIRiscoInventarioAmbienteModelo = new EPIRiscoInventarioAmbienteModelo();
                ePIRiscoInventarioAmbienteModelo.CodEpiRiscoInventarioAmbiente = epi.CodEpiRiscoInventarioAmbiente;
                ePIRiscoInventarioAmbienteModelo.CodRiscoInventarioAmbiente = epi.CodRiscoInventarioAmbiente;
                ePIRiscoInventarioAmbienteModelo.CodEPI = epi.CodEPI;

                result.Add(ePIRiscoInventarioAmbienteModelo);
            }

            return result;
        }
    }
}
