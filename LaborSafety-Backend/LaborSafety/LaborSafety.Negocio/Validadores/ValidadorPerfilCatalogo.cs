using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorPerfilCatalogo : Validador<PerfilCatalogoModelo>
    {
        public void ValidaInsercao(PerfilCatalogoModelo insercao)
        {
            if (insercao == null || insercao.CodPerfilCatalogo <= 0 ||
                typeof(long) != insercao.CodPerfilCatalogo.GetType())
            {
                throw new Exception("Id de perfil catálogo enviado inválido!");
            }
        }

        public void ValidaEdicao(PerfilCatalogoModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
