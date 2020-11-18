using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaborSafety.Dominio.Modelos.SAP
{
    [XmlRoot(ElementName = "DisciplinaResponse", Namespace = "http://com.br/INT_FromERP_toAPR_PT")]
    public class DisciplinaSAPResponse
    {
        [XmlElement(ElementName = "Responses")]
        public List<DisciplinaItemSAPResponse> Itens { get; set; }
    }

    public class DisciplinaItemSAPResponse
    {
        [XmlElement(ElementName = "Caracteristica")]
        public string Caracteristica { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "Descricao")]
        public string Descricao { get; set; }
    }
}
