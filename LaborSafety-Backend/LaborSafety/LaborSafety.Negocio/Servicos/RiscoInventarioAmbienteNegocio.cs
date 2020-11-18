using AutoMapper;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class RiscoInventarioAmbienteNegocio : IRiscoInventarioAmbienteNegocio
    {
        private readonly IRiscoInventarioAmbientePersistencia riscoInventarioAmbientePersistencia;
        private readonly Validador<RiscoInventarioAmbienteModelo> validadorRiscoInventarioAmbiente;

        public RiscoInventarioAmbienteNegocio(IRiscoInventarioAmbientePersistencia riscoInventarioAmbientePersistencia, Validador<RiscoInventarioAmbienteModelo> validadorRiscoInventarioAmbiente)
        {
            this.riscoInventarioAmbientePersistencia = riscoInventarioAmbientePersistencia;
            this.validadorRiscoInventarioAmbiente = validadorRiscoInventarioAmbiente;
        }

        public void InserirRiscoInventarioAmbiente(RiscoInventarioAmbienteModelo riscoInventarioAmbienteModelo)
        {
            validadorRiscoInventarioAmbiente.ValidaInsercao(riscoInventarioAmbienteModelo);

            this.riscoInventarioAmbientePersistencia.InserirRiscoInventarioAmbiente(riscoInventarioAmbienteModelo);
        }
    }
}
