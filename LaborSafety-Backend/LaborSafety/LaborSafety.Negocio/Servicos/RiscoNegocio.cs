using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class RiscoNegocio : IRiscoNegocio
    {
        private readonly IRiscoPersistencia riscoPersistencia;
        public RiscoNegocio(IRiscoPersistencia riscoPersistencia)
        {
            this.riscoPersistencia = riscoPersistencia;
        }

        public RiscoModelo MapeamentoRisco(RISCO risco)
        {
            RiscoModelo riscoModelo = new RiscoModelo()
            {
                CodRisco = risco.CodRisco,
                CodTipoRisco = risco.CodTipoRisco,
                Nome = risco.Nome
            };

            return riscoModelo;
        }

        public TipoRiscoModelo MapeamentoTipoRisco(TIPO_RISCO tipoRisco)
        {
            TipoRiscoModelo tipoRiscoModelo = new TipoRiscoModelo()
            { 
                CodTipoRisco = tipoRisco.CodTipoRisco,
                Nome = tipoRisco.Nome,
            };

            return tipoRiscoModelo;
        }

        public RiscoModelo ListarRiscoPorId(long id)
        {
            RISCO ris = this.riscoPersistencia.ListarRiscoPorId(id);
            if (ris == null)
            {
                throw new KeyNotFoundException("Risco não encontrado.");
            }
            return MapeamentoRisco(ris);
        }

        public RiscoModelo ListarRiscoPorNome(string nome)
        {
            RISCO ris = this.riscoPersistencia.ListarRiscoPorNome(nome);
            if (ris == null)
            {
                throw new KeyNotFoundException("Risco não encontrado.");
            }
            return MapeamentoRisco(ris);
        }

        public IEnumerable<RiscoModelo> ListarTodosRiscos()
        {
            List<RiscoModelo> riscoModelo = new List<RiscoModelo>();

            IEnumerable<RISCO> riscos = this.riscoPersistencia.ListarTodosRiscos();

            if (riscos == null)
            {
                throw new KeyNotFoundException("Riscos não encontrados.");
            }

            foreach (RISCO risco in riscos)
            {
                riscoModelo.Add(MapeamentoRisco(risco));
            }

            return riscoModelo;
        }

        public IEnumerable<RiscoModelo> ListarPorTipoRisco(long codTipo)
        {
            List<RiscoModelo> riscoModelo = new List<RiscoModelo>();

            IEnumerable<RISCO> riscos = this.riscoPersistencia.ListarPorTipoRisco(codTipo);

            if (riscos == null)
            {
                throw new KeyNotFoundException("Riscos não encontrados.");
            }

            foreach (RISCO risco in riscos)
            {
                riscoModelo.Add(MapeamentoRisco(risco));
            }

            return riscoModelo;
        }

        public List<TipoRiscoModelo> ListarTiposRisco()
        {
            List<TipoRiscoModelo> tipoRisco = new List<TipoRiscoModelo>();

            IEnumerable<TIPO_RISCO> listTipoRiscos = this.riscoPersistencia.ListarTiposRisco();

            if (listTipoRiscos == null)
            {
                throw new KeyNotFoundException("Tipos de riscos não encontrados.");
            }

            foreach (TIPO_RISCO tpRisco in listTipoRiscos)
            {
                tipoRisco.Add(MapeamentoTipoRisco(tpRisco));
            }

            return tipoRisco;

        }
    }
}
