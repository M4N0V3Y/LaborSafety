using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorContraMedida : Validador<EPIModelo>
    {
        public void ValidaInsercao(EPIModelo insercao)
        {
            if (insercao == null || insercao.CodEPI <= 0 ||
                typeof(long) != insercao.CodEPI.GetType())
            {
                throw new Exception("Id de contra medida enviado inválido!");
            }
        }

        public void ValidaEdicao(EPIModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
