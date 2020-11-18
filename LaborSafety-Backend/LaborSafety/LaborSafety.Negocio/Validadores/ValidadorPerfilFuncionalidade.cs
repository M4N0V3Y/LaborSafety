using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorPerfilFuncionalidade : Validador<PerfilFuncionalidadeModelo>
    {
        public void ValidaInsercao(PerfilFuncionalidadeModelo insercao)
        {
            if (insercao == null || insercao.CodPerfil <= 0 ||
                typeof(long) != insercao.CodPerfil.GetType())
            {
                throw new Exception("Id de perfil enviado inválido!");
            }

            if (insercao.Funcionalidades == null)
            {
                insercao.Funcionalidades = new List<FuncionalidadeModelo>();
            }
            else if (typeof(List<AdministracaoPerfilModelo>) != insercao.Funcionalidades.GetType())
            {
                throw new Exception("Lista de funcionalidades enviada é inválida!");
            }
        }

        public void ValidaEdicao(PerfilFuncionalidadeModelo edicao)
        {
            throw new NotImplementedException();
        }
    }
}
