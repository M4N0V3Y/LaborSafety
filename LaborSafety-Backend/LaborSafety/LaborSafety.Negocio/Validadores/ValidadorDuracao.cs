using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorDuracao : Validador<DuracaoModelo>
    {
        public void ValidaInsercao(DuracaoModelo insercao)
        {
            if (insercao == null || insercao.CodDuracao <= 0 ||
                typeof(long) != insercao.CodDuracao.GetType())
            {
                throw new Exception("Id de duração enviado inválido!");
            }
        }

        public void ValidaEdicao(DuracaoModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
