using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class NrNegocio : INrNegocio
    {
        private readonly INrPersistencia nrPersistencia;
        public NrNegocio(INrPersistencia nrPersistencia)
        {
            this.nrPersistencia = nrPersistencia;
        }

        public NrModelo MapeamentoNr(NR nr)
        {
            NrModelo nrModelo = new NrModelo()
            {
                CodNR = nr.CodNR,
                Codigo = nr.Codigo,
                Nome = nr.Nome,
                Descricao = nr.Descricao
            };

            return nrModelo;
        }

        public NrModelo ListarNrPorId(long id)
        {
            NR pes = this.nrPersistencia.ListarNrPorId(id);
            if (pes == null)
            {
                throw new KeyNotFoundException("NR não encontrada.");
            }
            return MapeamentoNr(pes);
        }

        public NrModelo ListarNRPorCodigo(string codigo)
        {
            NR pes = this.nrPersistencia.ListarNRPorCodigo(codigo);
            if (pes == null)
            {
                throw new KeyNotFoundException("NR não encontrada.");
            }
            return MapeamentoNr(pes);
        }
        public IEnumerable<NrModelo> ListarTodasNRs()
        {
            List<NrModelo> nrModelo = new List<NrModelo>();

            IEnumerable<NR> NRs = this.nrPersistencia.ListarTodasNRs();

            if (NRs == null)
            {
                throw new KeyNotFoundException("Nenhuma NR encontrada.");
            }

            foreach (NR nr in NRs)
            {
                nrModelo.Add(MapeamentoNr(nr));
            }

            return nrModelo;
        }
    }
}
