using System.Xml.Serialization;

namespace Read3040Bacen
{
    [XmlRoot("Agreg")]
    public class Agregacao
    {
        [XmlAttribute("NatuOp")]
        public string NatuOp;

        [XmlAttribute("Mod")]
        public string Mod;

        [XmlAttribute("OrigemRec")]
        public string OrigemRec;

        [XmlAttribute("VincME")]
        public string VincME;

        [XmlAttribute("ClassOp")]
        public string ClassOp;

        [XmlAttribute("FaixaVlr")]
        public string FaixaVlr;

        [XmlAttribute("PrzProvm")]
        public string PrzProvm;

        [XmlAttribute("Localiz")]
        public string Localiz;

        [XmlAttribute("TpCli")]
        public string TpCli;

        [XmlAttribute("TpCtrl")]
        public string TpCtrl;

        [XmlAttribute("DesempOp")]
        public string DesempOp;

        [XmlAttribute("CaracEspecial")]
        public string CaracEspecial;

        [XmlAttribute("ProvConsttd")]
        public string ProvConsttd;

        [XmlAttribute("QtdOp")]
        public string QtdOp;

        [XmlAttribute("QtdCli")]
        public string QtdCli;

        [XmlElement("Venc")]
        public List<Vencimento> vencimentos;
    }
}
