using System.Xml.Serialization;

namespace Read3040Bacen
{
    [XmlRoot("Perda")]
    public class Perda
    {
        [XmlAttribute("MotPerda")]
        public string MotPerda;

        [XmlAttribute("VlrPerda")]
        public string VlrPerda;
        
    }
}
