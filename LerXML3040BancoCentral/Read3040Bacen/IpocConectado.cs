using System.Xml.Serialization;

namespace Read3040Bacen
{
    [XmlRoot("ConIpocs/ipocCon")]
    public class IpocConectado
    {
        [XmlAttribute("ipoc")]
        public string ipoc;
    }
}
