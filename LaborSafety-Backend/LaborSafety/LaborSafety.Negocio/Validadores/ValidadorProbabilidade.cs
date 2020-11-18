using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorProbabilidade : Validador<ProbabilidadeModelo>
    {
        public void ValidaInsercao(ProbabilidadeModelo insercao)
        {
            if (insercao == null || insercao.CodProbabilidade <= 0 ||
                typeof(long) != insercao.CodProbabilidade.GetType())
            {
                throw new Exception("Id de probabilidade enviado inválido!");
            }
        }

        public void ValidaEdicao(ProbabilidadeModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
