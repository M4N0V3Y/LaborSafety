using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorRiscoInventarioAmbiente : Validador<RiscoInventarioAmbienteModelo>
    {
        public void ValidaInsercao(RiscoInventarioAmbienteModelo insercao)
        {
            if (insercao == null || insercao.CodRiscoAmbiente <= 0 ||
                typeof(long) != insercao.CodRiscoAmbiente.GetType())
            {
                throw new Exception("Id de risco de inventário de ambiente enviado inválido!");
            }
        }

        public void ValidaEdicao(RiscoInventarioAmbienteModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
