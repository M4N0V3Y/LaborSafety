using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaborSafety.Dominio.Modelos.SAP
{
    [XmlRoot(ElementName = "Ordens", Namespace = "http://com.br/INT_LaborSafety_PI_ORDEM")]
    public class APRSAPModelo
    {
        [XmlElement(ElementName = "Itens")]
        public List<APRItemSAPModelo> Itens { get; set; }
    }

    public class APRItemSAPModelo
    {
        [XmlElement(ElementName = "Numero_Ordem")]
        public string Numero_Ordem { get; set; }

        [XmlElement(ElementName = "Descricao")]
        public string Descricao_Ordem { get; set; }

        [XmlElement(ElementName = "Local_Instalacao")]
        public string Local_Instalacao { get; set; }

        [XmlElement(ElementName = "Operacao")]
        public string Operacao { get; set; }

        [XmlElement(ElementName = "Descricao_Operacao")]
        public string Descricao_Operacao { get; set; }

        [XmlElement(ElementName = "Centro_Trabalho_Operacao")]
        public string Centro_Trabalho { get; set; }

        [XmlElement(ElementName = "Valor_Carct_Centro_Trabalho_Operacao")]
        public string Valor_Centro_Trabalho { get; set; }

        [XmlElement(ElementName = "Chave_Modelo_Operacao")]
        public string Chave_Modelo_Operacao { get; set; }

        [XmlElement(ElementName = "Local_Instalacao_Operacao")]
        public string Local_Instalacao_Operacao { get; set; }

        [XmlElement(ElementName = "Status_Ordem")]
        public string Status_Ordem { get; set; }
    }
}
