using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorLocalInstalacao : Validador<LocalInstalacaoModelo>
    {
        public void ValidaInsercao(LocalInstalacaoModelo insercao)
        {
            if (insercao == null || insercao.CodLocalInstalacao <= 0 ||
                typeof(long) != insercao.CodLocalInstalacao.GetType())
            {
                throw new Exception("Id de local de instalação enviado inválido!");
            }
        }

        public void ValidaEdicao(LocalInstalacaoModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
