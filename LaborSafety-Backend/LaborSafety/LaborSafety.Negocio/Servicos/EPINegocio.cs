using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class EPINegocio : IEpiNegocio
    {

        private readonly IEPIPersistencia epiPersistencia;
        private readonly Validador<EPIModelo> validadorEPI;
        public EPINegocio(IEPIPersistencia epiPersistencia, Validador<EPIModelo> validadorEPI)
        {
            this.epiPersistencia = epiPersistencia;
            this.validadorEPI = validadorEPI;
        }

        public EPIModelo MapeamentoEPI(EPI epi)
        {
            EPIModelo epiModelo = new EPIModelo()
            {
                CodEPI = epi.CodEPI,
                Nome = epi.Nome,
                Descricao = epi.Descricao,
                N1 = epi.N1,
                N2 = epi.N2,
                N3 = epi.N3
            };

            return epiModelo;
        }

        public EPIModelo ListarEPIPorID(long id)
        {
            EPI epi = this.epiPersistencia.ListarEPIPorId(id);
            if (epi == null)
            {
                throw new KeyNotFoundException("EPI não encontrado.");
            }
            return MapeamentoEPI(epi);
        }

        public EPIModelo ListarEPIPorNome(string nome)
        {
            EPI epi = this.epiPersistencia.ListarEPIPorNome(nome);
            if (epi == null)
            {
                throw new KeyNotFoundException("EPI não encontrado.");
            }
            return MapeamentoEPI(epi);
        }

        public IEnumerable<EPIModelo> ListarTodosEPIs()
        {
            List<EPIModelo> epiModelo = new List<EPIModelo>();

            IEnumerable<EPI> epi = this.epiPersistencia.ListarTodosEPIs();

            if (epi == null)
                throw new KeyNotFoundException("EPI não encontrado.");

            foreach (EPI contraMedida in epi)
                epiModelo.Add(MapeamentoEPI(contraMedida));


            return epiModelo;
        }

        public EPIModelo ListarEPIPorNivel(string nomeCompleto)
        {
            if (nomeCompleto == null)
                throw new KeyNotFoundException("É necessário informar um nível de EPI para pesquisa.");

            EPI epi = this.epiPersistencia.ListarEPIPorNivel(nomeCompleto);

            if (epi == null)
                throw new KeyNotFoundException("EPI não encontrado.");

            return MapeamentoEPI(epi);
        }

        public List<EPIModelo> ListarTodosEPIPorNivel(string nomeCompleto)
        {
            if (nomeCompleto == null)
                throw new KeyNotFoundException("É necessário informar um nível de EPI para pesquisa.");

            List<EPIModelo> epi = this.epiPersistencia.ListarTodosEPIPorNivel(nomeCompleto);

            if (epi == null)
                throw new KeyNotFoundException("EPI não encontrado.");

            return epi;
        }

        public List<EPIModelo> ListarEPIsPorListaId(List<long> epis)
        {
            List<EPIModelo> epiModelo = new List<EPIModelo>();

            IEnumerable<EPI> listaEpis = this.epiPersistencia.ListarEPIsPorListaId(epis);

            if (listaEpis == null)
            {
                throw new KeyNotFoundException("EPI não encontrado.");
            }

            foreach (EPI ep in listaEpis)
            {
                epiModelo.Add(MapeamentoEPI(ep));
            }

            return epiModelo;
        }
    }
}
