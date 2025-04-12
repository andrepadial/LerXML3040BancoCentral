using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Read3040Bacen
{
    [XmlRoot("Sicor")]
    public class Sicor
    {
        [XmlAttribute("RefBacen")]
        public string RefBacen;

        [XmlAttribute("Ordem")]
        public string Ordem;

        [XmlAttribute("VlrSaldoTot")]
        public string VlrSaldoTot;

        [XmlAttribute("VlrSaldoVinc")]
        public string VlrSaldoVinc;

        [XmlAttribute("Situacao")]
        public string Situacao;

        [XmlAttribute("TpBonusRebate")]
        public string TpBonusRebate;

        [XmlAttribute("VlrBonusRebate")]
        public string VlrBonusRebate;

        [XmlAttribute("DtBonusRebate")]
        public string DtBonusRebate;

    }
}
