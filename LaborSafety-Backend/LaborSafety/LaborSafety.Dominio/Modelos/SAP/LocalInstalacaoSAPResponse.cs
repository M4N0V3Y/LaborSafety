using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaborSafety.Dominio.Modelos.SAP
{
    [XmlRoot(ElementName = "LocalInstalacaoSAPResponse", Namespace = "http://com.br/INT_LaborSafety_PI_LOCAL_INSTALACAO_RESPONSE")]
    public class LocalInstalacaoSAPResponse
    {
        [XmlElement(ElementName = "Responses")]
        public List<LocalInstalacaoItemSAPResponse> Itens { get; set; }
    }

    public class LocalInstalacaoItemSAPResponse
    {
        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "Descricao")]
        public string Descricao { get; set; }
    }
}
