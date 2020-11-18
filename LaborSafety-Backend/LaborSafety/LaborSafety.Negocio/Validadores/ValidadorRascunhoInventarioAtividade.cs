using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorRascunhoInventarioAtividade : Validador<RascunhoInventarioAtividadeModelo>
    {
        public void ValidaInsercao(RascunhoInventarioAtividadeModelo insercao)
        {
            if (insercao == null)
            {
                throw new Exception("Id de rascunho inventário de atividade enviado inválido!");
            }
        }

        public void ValidaEdicao(RascunhoInventarioAtividadeModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
