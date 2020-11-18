using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IDisciplinaNegocio
    {
        void Inserir(DisciplinaModelo modelo);
        void Editar(DisciplinaModelo modelo);
        DisciplinaModelo ListarDisciplinaPorId(long id);
        DisciplinaModelo ListarDisciplinaPorNome(string nome);
        IEnumerable<DisciplinaModelo> ListarTodasDisciplinas(); 
    }
}
