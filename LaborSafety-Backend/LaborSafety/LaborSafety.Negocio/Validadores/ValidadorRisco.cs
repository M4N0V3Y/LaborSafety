using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorRisco : Validador<RiscoModelo>
    {
        public void ValidaInsercao(RiscoModelo insercao)
        {
            if (insercao == null || insercao.CodRisco <= 0 ||
                typeof(long) != insercao.CodRisco.GetType())
            {
                throw new Exception("Id de risco enviado inválido!");
            }
        }

        public void ValidaEdicao(RiscoModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
