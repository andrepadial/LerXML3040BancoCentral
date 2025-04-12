using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Read3040Bacen
{
    [XmlRoot("Inf")]
    public class InformacaoAdicional
    {
        [XmlAttribute("Tp")]
        public string Tp;

        [XmlAttribute("Cd")]
        public string Cd;

        [XmlAttribute("Ident")]
        public string Ident;

        [XmlAttribute("Valor")]
        public string Valor;

        [XmlAttribute("Perc")]
        public string Perc;

        [XmlAttribute("Qtd")]
        public string Qtd;
    }
}
