using System;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Validadores.Interface;

namespace LaborSafety.Negocio.Validadores
{
    public class ValidadorAtividadePadrao : Validador<AtividadePadraoModelo>
    {
        public void ValidaInsercao(AtividadePadraoModelo insercao)
        {
            if (insercao == null || insercao.CodAtividadePadrao <= 0 ||
                typeof(long) != insercao.CodAtividadePadrao.GetType())
            {
                throw new Exception("Id de atividade enviado inválido!");
            }
        }

        public void ValidaEdicao(AtividadePadraoModelo edicao)
        {
            if (edicao == null)
            {
                throw new Exception("Existem campos obrigatórios sem preenchimento!");
            }
        }
    }
}
