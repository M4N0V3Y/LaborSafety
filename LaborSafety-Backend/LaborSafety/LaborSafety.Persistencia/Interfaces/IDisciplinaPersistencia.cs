using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IDisciplinaPersistencia
    {
        void Inserir(DisciplinaModelo modelo);
        void InsereOuAtualiza(DisciplinaModelo modelo);
        void Editar(DisciplinaModelo modelo);
        DISCIPLINA ListarDisciplinaPorId(long id, DB_APRPTEntities entities = null);
        DISCIPLINA ListarDisciplinaPorNome(string nome, DB_APRPTEntities entities = null);
        IEnumerable<DISCIPLINA> ListarTodasDisciplinas();
    }
}
