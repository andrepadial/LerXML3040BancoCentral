using System.Xml.Serialization;

namespace Read3040Bacen
{
    [XmlRoot("ContInstFinRes4966")]
    public class ContabilizacaoInstrumentoFinanceiro
    {
        [XmlAttribute("ClasAtFin")]
        public string ClasAtFin;

        [XmlAttribute("EstInstFin")]
        public string EstInstFin;

        [XmlAttribute("QtdInst")]
        public string QtdInst;

        [XmlAttribute("VlrContBr")]
        public string VlrContBr;

        [XmlAttribute("VlrPerdaAcum")]
        public string VlrPerdaAcum;

        [XmlAttribute("VlrJusto")]
        public string VlrJusto;

        [XmlAttribute("TJE")]
        public string TJE;

        [XmlAttribute("RendMes")]
        public string RendMes;

        [XmlAttribute("PdEst1")]
        public string PdEst1;

        [XmlAttribute("CartProvMin")]
        public string CartProvMin;

        [XmlAttribute("TratRisc")]
        public string TratRisc;

        [XmlElement("Estagio")]
        public List<Estagio> estagios;

        [XmlElement("Perda")]
        public List<Perda> perdas;
    }
}
