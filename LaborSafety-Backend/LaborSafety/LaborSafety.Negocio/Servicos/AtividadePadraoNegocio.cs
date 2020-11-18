using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class AtividadePadraoNegocio : IAtividadePadraoNegocio
    {
        private readonly IAtividadePadraoPersistencia atividadePersistencia;
        public AtividadePadraoNegocio(IAtividadePadraoPersistencia atividadePersistencia)
        {
            this.atividadePersistencia = atividadePersistencia;
        }

        public AtividadePadraoModelo MapeamentoAtividade(ATIVIDADE_PADRAO ativ)
        {
            AtividadePadraoModelo atividade = new AtividadePadraoModelo()
            {
                CodAtividadePadrao = ativ.CodAtividadePadrao,
                Nome = ativ.Nome,
                Descricao = ativ.Descricao
            };

            return atividade;
        }

        public AtividadePadraoModelo ListarAtividadePorId(long id)
        {
            ATIVIDADE_PADRAO sis = this.atividadePersistencia.ListarAtividadePorId(id);
            if (sis == null)
            {
                throw new KeyNotFoundException("Atividade não encontrada.");
            }
            return MapeamentoAtividade(sis);
        }

        public AtividadePadraoModelo ListarAtividadePorNome(string nome)
        {
            ATIVIDADE_PADRAO sis = this.atividadePersistencia.ListarAtividadePorNome(nome);
            if (sis == null)
            {
                throw new KeyNotFoundException("Atividade não encontrada.");
            }
            return MapeamentoAtividade(sis);
        }

        public IEnumerable<AtividadePadraoModelo> ListarTodasAtividades()
        {
            List<AtividadePadraoModelo> atividadePadraoModelo = new List<AtividadePadraoModelo>();

            IEnumerable<ATIVIDADE_PADRAO> atividades = this.atividadePersistencia.ListarTodasAtividades();

            if (atividades == null)
            {
                throw new KeyNotFoundException("Atividade não encontrada.");
            }

            foreach (ATIVIDADE_PADRAO atv in atividades)
            {
                atividadePadraoModelo.Add(MapeamentoAtividade(atv));
            }

            return atividadePadraoModelo;
        }
    }
}
