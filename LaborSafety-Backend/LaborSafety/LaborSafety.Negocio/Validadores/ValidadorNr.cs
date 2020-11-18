using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;


namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorNr : Validador<NrModelo>
    {
        public void ValidaInsercao(NrModelo insercao)
        {
            if (insercao == null || insercao.CodNR <= 0 ||
                typeof(long) != insercao.CodNR.GetType())
            {
                throw new Exception("Id de nr enviado inválido!");
            }
        }

        public void ValidaEdicao(NrModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
