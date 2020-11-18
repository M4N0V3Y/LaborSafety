using System.Collections.Generic;
using System.Xml.Serialization;

namespace LaborSafety.Dominio.Modelos.SAP
{
    [XmlRoot(ElementName = "APRSAPResponse", Namespace = "http://com.br/INT_LaborSafety_PI_ORDEM_RESPONSE")]
    public class APRSAPResponse
    {
        [XmlElement(ElementName = "Responses")]
        public List<APRItemSAPResponse> Itens { get; set; }
    }

    public class APRItemSAPResponse
    {
        [XmlElement(ElementName = "VRG")]
        public string VRG { get; set; }

        [XmlElement(ElementName = "Numero_Serie")]
        public string Numero_Serie { get; set; }

        [XmlElement(ElementName = "Endreco_Rede")]
        public string Endereco_Rede { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "Descricao")]
        public string Descricao { get; set; }
    }
}
