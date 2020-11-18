using System.Collections.Generic;
using System.Xml.Serialization;

namespace LaborSafety.Dominio.Modelos.SAP
{
    [XmlRoot(ElementName = "Disciplinas", Namespace = "http://com.br/INT_LaborSafety_PI_DISCIPLINA")]
    public class DisciplinaSAPModelo
    {
        [XmlElement(ElementName = "Itens")]
        public List<DisciplinaItemSAPModelo> Itens { get; set; }
    }

    public class DisciplinaItemSAPModelo
    {
        [XmlElement(ElementName = "C")]
        public string Caracteristica { get; set; }

        [XmlElement(ElementName = "Valor_Caracteristica")]
        public string ValorCaracteristica { get; set; }
    }
}
