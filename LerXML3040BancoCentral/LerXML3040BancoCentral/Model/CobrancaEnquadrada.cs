using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model
{
    public class CobrancaEnquadrada
    {
        public string Cpf {  get; set; }

        public string Modalidade { get; set; }
        public string Contrato { get; set; }
        public string Saldo { get; set; }
        public string Provisao { get; set; }
        public string DataContrato { get; set; }

        public string DataAtraso { get; set; }

        public string Rating { get; set; }

        public string DiasAtraso { get; set; }
        public string Prejuizo { get; set; }

    }
}
