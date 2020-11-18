using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class SeveridadeNegocio : ISeveridadeNegocio
    {
        private readonly ISeveridadePersistencia severidadePersistencia;
        public SeveridadeNegocio(ISeveridadePersistencia severidadePersistencia)
        {
            this.severidadePersistencia = severidadePersistencia;
        }

        public SeveridadeModelo MapeamentoSeveridade(SEVERIDADE severidade)
        {
            SeveridadeModelo severidadeModelo = new SeveridadeModelo()
            {
                CodSeveridade = severidade.CodSeveridade,
                Nome = severidade.Nome,
                Indice = (int)severidade.Indice
            };

            return severidadeModelo;
        }

        public SeveridadeModelo ListarSeveridadePorId(long id)
        {
            SEVERIDADE sev = this.severidadePersistencia.ListarSeveridadePorId(id);
            if (sev == null)
            {
                throw new KeyNotFoundException("Severidade não encontrada.");
            }
            return MapeamentoSeveridade(sev);
        }

        public SeveridadeModelo ListarSeveridadePorNome(string nome)
        {
            SEVERIDADE sev = this.severidadePersistencia.ListarSeveridadePorNome(nome);
            if (sev == null)
            {
                throw new KeyNotFoundException("Severidade não encontrada.");
            }
            return MapeamentoSeveridade(sev);
        }

        public IEnumerable<SeveridadeModelo> ListarTodasSeveridades()
        {
            List<SeveridadeModelo> severidadeModelo = new List<SeveridadeModelo>();

            IEnumerable<SEVERIDADE> severidades = this.severidadePersistencia.ListarTodasSeveridades();

            if (severidades == null)
            {
                throw new KeyNotFoundException("Severidade não encontrada.");
            }

            foreach (SEVERIDADE sev in severidades)
            {
                severidadeModelo.Add(MapeamentoSeveridade(sev));
            }

            return severidadeModelo;
        }
    }
}
