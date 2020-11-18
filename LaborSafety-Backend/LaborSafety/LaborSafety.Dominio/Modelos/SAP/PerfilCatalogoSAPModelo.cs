using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaborSafety.Dominio.Modelos.SAP
{
    [XmlRoot(ElementName = "Perfis", Namespace = "http://com.br/INT_LaborSafety_PI_PERFIL_CATALOGO")]
    public class PerfilCatalogoSAPModelo
    {
        [XmlElement(ElementName = "Itens")]
        public List<PerfilCatalogoItemSAPModelo> Itens { get; set; }
    }

    public class PerfilCatalogoItemSAPModelo
    {
        [XmlElement(ElementName = "Perfil_Catalogo")]
        public string PerfilCatalogo { get; set; }

        [XmlElement(ElementName = "Descricao")]
        public string Descricao { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }
    }
}
