using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaborSafety.Dominio.Modelos.SAP
{
    [XmlRoot(ElementName = "PerfilCatalogoSAPResponse", Namespace = "http://com.br/INT_LaborSafety_PI_PERFIL_CATALOGO_RESPONSE")]
    public class PerfilCatalogoSAPResponse
    {
        [XmlElement(ElementName = "Responses")]
        public List<PerfilCatalogoItemSAPResponse> Itens { get; set; }
    }

    public class PerfilCatalogoItemSAPResponse
    {
        [XmlElement(ElementName = "Perfil_Do_Catalogo")]
        public string Perfil_Do_Catalogo { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "Descricao")]
        public string Descricao { get; set; }
    }
}
