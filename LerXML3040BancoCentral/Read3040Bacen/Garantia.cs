using System.Xml.Serialization;

namespace Read3040Bacen
{
    [XmlRoot("Gar")]
    public class Garantia
    {
        [XmlAttribute("Tp")]
        public string Tp;

        [XmlAttribute("Ident")]
        public string Ident;

        [XmlAttribute("PercGar")]
        public string PercGar;

        [XmlAttribute("VlrOrig")]
        public string VlrOrig;

        [XmlAttribute("VlrData")]
        public string VlrData;

        [XmlAttribute("DtReav")]
        public string DtReav;
    }
}
