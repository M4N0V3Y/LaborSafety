using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaborSafety.Dominio.Modelos.SAP
{
    [XmlRoot(ElementName = "Atividades", Namespace = "http://com.br/INT_LaborSafety_PI_ATV_PADRAO")]
    public class AtividadePadraoSAPModelo
    {
        [XmlElement(ElementName = "Itens")]
        public List<AtividadePadraoItemSAPModelo> Itens { get; set; }
    }

    public class AtividadePadraoItemSAPModelo
    {
        [XmlElement(ElementName = "Chave_Modelo")]
        public string Chave_Modelo { get; set; }

        [XmlElement(ElementName = "Texto_Chave_Modelo")]
        public string Texto_Chave_Modelo { get; set; }
    }

}
