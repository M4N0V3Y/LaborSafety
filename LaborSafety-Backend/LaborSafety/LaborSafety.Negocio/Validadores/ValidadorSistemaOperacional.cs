using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorSistemaOperacional : Validador<AmbienteModelo>
    {
        public void ValidaInsercao(AmbienteModelo insercao)
        {
            if (insercao == null || insercao.CodAmbiente <= 0 ||
                typeof(long) != insercao.CodAmbiente.GetType())
            {
                throw new Exception("Id de Ambiente enviado inválido!");
            }
        }

        public void ValidaEdicao(AmbienteModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
