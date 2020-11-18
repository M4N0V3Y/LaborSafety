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
    public class NrInventarioAmbienteNegocio : INrInventarioAmbienteNegocio
    {
        private readonly INrInventarioAmbientePersistencia nrInventarioAmbientePersistencia;
        private readonly Validador<NrInventarioAmbienteModelo> validadorNrInventarioAmbiente;

        public NrInventarioAmbienteNegocio(INrInventarioAmbientePersistencia nrInventarioAmbientePersistencia, Validador<NrInventarioAmbienteModelo> validadorNrInventarioAmbiente)
        {
            this.nrInventarioAmbientePersistencia = nrInventarioAmbientePersistencia;
            this.validadorNrInventarioAmbiente = validadorNrInventarioAmbiente;
        }

        public void InserirNrInventarioAmbiente(NrInventarioAmbienteModelo nrInventarioAmbienteModelo)
        {
            validadorNrInventarioAmbiente.ValidaInsercao(nrInventarioAmbienteModelo);

            this.nrInventarioAmbientePersistencia.InserirNrInventarioAmbiente(nrInventarioAmbienteModelo);
        }
    }
}
