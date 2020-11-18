using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorSeveridade : Validador<SeveridadeModelo>
    {
        public void ValidaInsercao(SeveridadeModelo insercao)
        {
            if (insercao == null || insercao.CodSeveridade <= 0 ||
                typeof(long) != insercao.CodSeveridade.GetType())
            {
                throw new Exception("Id de severidade enviado inválido!");
            }
        }

        public void ValidaEdicao(SeveridadeModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
