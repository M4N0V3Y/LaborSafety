using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorInventarioAmbiente : Validador<InventarioAmbienteModelo>
    {
        public void ValidaInsercao(InventarioAmbienteModelo insercao)
        {
            if (insercao == null || typeof(long) != insercao.CodInventarioAmbiente.GetType())
            {
                throw new Exception("Id de inventario de ambiente enviado inválido!");
            }
        }

        public void ValidaEdicao(InventarioAmbienteModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
