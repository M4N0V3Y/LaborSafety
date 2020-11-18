using System.Collections.Generic;
using System.Xml.Serialization;

namespace LaborSafety.Dominio.Modelos.SAP
{
    [XmlRoot(ElementName = "AtividadePadraoResponse", Namespace = "http://com.br/INT_FromERP_toAPR_PT")]
    public class AtividadePadraoSAPResponse
    {
        [XmlElement(ElementName = "Responses")]
        public List<AtividadePadraoItemSAPResponse> Itens { get; set; }
    }

    public class AtividadePadraoItemSAPResponse
    {
        [XmlElement(ElementName = "Chave_Modelo")]
        public string Chave_Modelo { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "Descricao")]
        public string Descricao { get; set; }
    }
}
