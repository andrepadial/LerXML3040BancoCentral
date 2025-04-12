using System.Xml.Serialization;

namespace Read3040Bacen
{
    [XmlRoot("Estagio")]
    public class Estagio
    {
        [XmlAttribute("Motivo")]
        public string Motivo;

        [XmlAttribute("DtAlocacao")]
        public string DtAlocacao;

        
    }
}
