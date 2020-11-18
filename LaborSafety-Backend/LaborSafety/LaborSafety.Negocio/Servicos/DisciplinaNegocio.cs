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
    public class DisciplinaNegocio : IDisciplinaNegocio
    {
        private readonly IDisciplinaPersistencia disciplinaPersistencia;
        public DisciplinaNegocio(IDisciplinaPersistencia disciplinaPersistencia)
        {
            this.disciplinaPersistencia = disciplinaPersistencia;
        }

        public void Inserir(DisciplinaModelo modelo)
        {
            this.disciplinaPersistencia.Inserir(modelo);
        }

        public void Editar(DisciplinaModelo modelo)
        {
            this.disciplinaPersistencia.Editar(modelo);
        }

        public DisciplinaModelo MapeamentoDisciplina(DISCIPLINA disciplina)
        {
            DisciplinaModelo disciplinaModelo = new DisciplinaModelo()
            {
                CodDisciplina = disciplina.CodDisciplina,
                Nome = disciplina.Nome,
                Descricao = disciplina.Descricao
            };

            return disciplinaModelo;
        }

        public DisciplinaModelo ListarDisciplinaPorId(long id)
        {
            DISCIPLINA sis = this.disciplinaPersistencia.ListarDisciplinaPorId(id);
            if (sis == null)
            {
                throw new KeyNotFoundException("Disciplina não encontrada.");
            }
            return MapeamentoDisciplina(sis);
        }

        public DisciplinaModelo ListarDisciplinaPorNome(string nome)
        {
            DISCIPLINA sis = this.disciplinaPersistencia.ListarDisciplinaPorNome(nome);
            if (sis == null)
            {
                throw new KeyNotFoundException("Disciplina não encontrada.");
            }
            return MapeamentoDisciplina(sis);
        }

        public IEnumerable<DisciplinaModelo> ListarTodasDisciplinas()
        {
            List<DisciplinaModelo> disciplinaModelo = new List<DisciplinaModelo>();

            IEnumerable<DISCIPLINA> disciplinas = this.disciplinaPersistencia.ListarTodasDisciplinas();

            if (disciplinas == null)
            {
                throw new KeyNotFoundException("Disciplina não encontrada.");
            }

            foreach (DISCIPLINA disciplina in disciplinas)
            {
                disciplinaModelo.Add(MapeamentoDisciplina(disciplina));
            }

            return disciplinaModelo;
        }
    }
}
