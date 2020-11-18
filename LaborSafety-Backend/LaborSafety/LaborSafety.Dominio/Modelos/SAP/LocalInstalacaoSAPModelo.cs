using System.Collections.Generic;
using System.Xml.Serialization;

namespace LaborSafety.Dominio.Modelos.SAP
{
    [XmlRoot(ElementName = "LocaisInstalacao", Namespace = "http://com.br/INT_LaborSafety_PI_LOCAL_INSTALACAO")]
    public class LocalInstalacaoSAPModelo
    {
        [XmlElement(ElementName = "Itens")]
        public List<LocalInstalacaoItemSAPModelo> Itens { get; set; }
    }

    public class LocalInstalacaoItemSAPModelo
    {
        [XmlElement(ElementName = "Local_Instalacao")]
        public string Local_Instalacao { get; set; }

        [XmlElement(ElementName = "Desc_Loc_Instalacao")]
        public string Descricao_Local_Instalacao { get; set; }

        [XmlElement(ElementName = "Perfil_Catalogo")]
        public string Perfil_Catalogo { get; set; }

        [XmlElement(ElementName = "Descricao_Perfil_Catalogo")]
        public string Descricao_Perfil_Catalogo { get; set; }

        [XmlElement(ElementName = "Classe_Local")]
        public string Classe_Local_Instalacao { get; set; }

        [XmlElement(ElementName = "Descricao_Classe")]
        public string Descricao_Classe { get; set; }

        [XmlElement(ElementName = "Caracteristica_Classe")]
        public string Caracteristica_Classe { get; set; }

        [XmlElement(ElementName = "Descricao_Caracteristica")]
        public string Descricao_Caracteristica { get; set; }

        [XmlElement(ElementName = "Valor_Caracteristica")]
        public string Valor_Caracteristica { get; set; }

        [XmlElement(ElementName = "Status_Local_Instalacao")]
        public string Status_Local_Instalacao { get; set; }

    }
}
