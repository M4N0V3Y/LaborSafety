using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class AprModelo
    {
        public long CodAPR { get; set; }
        public long CodStatusAPR { get; set; }
        public string NumeroSerie { get; set; }
        public string OrdemManutencao { get; set; }
        public string Descricao { get; set; }
        public int RiscoGeral { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataEncerramento { get; set; }
        public List<RiscoAprModelo> RISCO_APR { get; set; }
        //public List<FolhaAnexoAprModelo> FOLHA_ANEXO_APR { get; set; }
        public List<OperacaoAprModelo> OPERACAO_APR { get; set; }
        public List<AprovadorAprModelo> APROVADOR_APR { get; set; }
        public List<ExecutanteAprModelo> EXECUTANTE_APR { get; set; }
        public List<PessoaModelo> PESSOA { get; set; }
        public bool Ativo { get; set; }
        public bool? AprEditavel { get; set; }
        public string EightIDUsuarioModificador { get; set; }
    }
}
