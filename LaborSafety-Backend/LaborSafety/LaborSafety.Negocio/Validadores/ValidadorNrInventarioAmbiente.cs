using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorNrInventarioAmbiente : Validador<NrInventarioAmbienteModelo>
    {
        public void ValidaInsercao(NrInventarioAmbienteModelo insercao)
        {
            if (insercao == null || insercao.CodNRInventarioAmbiente <= 0 ||
                typeof(long) != insercao.CodNRInventarioAmbiente.GetType())
            {
                throw new Exception("Id de NR de inventario de ambiente enviado inválido!");
            }
        }

        public void ValidaEdicao(NrInventarioAmbienteModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
