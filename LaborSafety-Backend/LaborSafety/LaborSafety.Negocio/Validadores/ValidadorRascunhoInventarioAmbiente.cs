using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorRascunhoInventarioAmbiente : Validador<RascunhoInventarioAmbienteModelo>
    {
        public void ValidaInsercao(RascunhoInventarioAmbienteModelo insercao)
        {
            if (insercao == null)
            {
                throw new Exception("Id de inventario de ambiente enviado inválido!");
            }
        }

        public void ValidaEdicao(RascunhoInventarioAmbienteModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
