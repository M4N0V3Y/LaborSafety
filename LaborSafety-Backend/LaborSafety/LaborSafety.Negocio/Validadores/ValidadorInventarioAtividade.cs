using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorInventarioAtividade : Validador<InventarioAtividadeModelo>
    {
        public void ValidaInsercao(InventarioAtividadeModelo insercao)
        {
            if (insercao == null ||  typeof(long) != insercao.CodInventarioAtividade.GetType())
            {
                throw new Exception("Id de inventário de atividade enviado inválido!");
            }
        }

        public void ValidaEdicao(InventarioAtividadeModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}