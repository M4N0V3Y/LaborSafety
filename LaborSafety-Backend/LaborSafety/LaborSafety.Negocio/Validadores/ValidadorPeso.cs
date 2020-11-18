using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorPeso: Validador<PesoModelo>
    {
        public void ValidaInsercao(PesoModelo insercao)
        {
            if (insercao == null || insercao.CodPeso <= 0 ||
                typeof(long) != insercao.CodPeso.GetType())
            {
                throw new Exception("Id de peso enviado inválido!");
            }
        }

        public void ValidaEdicao(PesoModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
