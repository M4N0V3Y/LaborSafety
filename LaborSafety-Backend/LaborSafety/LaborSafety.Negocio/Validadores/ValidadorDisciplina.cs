using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorDisciplina : Validador<DisciplinaModelo>
    {
        public void ValidaInsercao(DisciplinaModelo insercao)
        {
            if (insercao == null || insercao.CodDisciplina <= 0 || typeof(long) != insercao.CodDisciplina.GetType())
            {
                throw new Exception("Id de disciplina enviado inválido!");
            }
        }

        public void ValidaEdicao(DisciplinaModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
