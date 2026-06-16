using Read3040Bacen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LerXML3040BancoCentral.Model._3040
{

    [XmlRoot("Cli")]
    public class Cliente3040
    {
        [XmlAttribute("Cd")]
        public string Cd;

        [XmlAttribute("Tp")]
        public string Tp;

        [XmlAttribute("Autorzc")]
        public string Autorzc;

        [XmlAttribute("PorteCli")]
        public string PorteCli;

        [XmlAttribute("TpCtrl")]
        public string TpCtrl;

        [XmlAttribute("IniRelactCli")]
        public string IniRelactCli;

        [XmlAttribute("FatAnual")]
        public string FatAnual;

        [XmlAttribute("CongEcon")]
        public string CongEcon;

        [XmlAttribute("ClassCli")]
        public string ClassCli;

        [XmlAttribute("NomeCli")]
        public string NomeCli;

        [XmlAttribute("TpIdentExt")]
        public string TpIdentExt;

        [XmlAttribute("CodExt")]
        public string CodExt;

        [XmlAttribute("IdLiderBR")]
        public string IdLiderBR;

        [XmlAttribute("IdPais")]
        public string IdPais;

        [XmlElement("Op")]
        public List<Operacao3040> operacoes;
    }
}
