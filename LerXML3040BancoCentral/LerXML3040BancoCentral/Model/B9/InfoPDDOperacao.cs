using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model.B9
{
    public class InfoPDDOperacao
    {
        public string Empresa {  get; set; }
        public string Sistema { get; set; }
        public string Modalidade { get; set; }
        public string CpfCnpj { get; set; }
        public string NumeroContrato { get; set; }
        public string Carteira { get; set; }
        public string Estagio { get; set; }
        public decimal ValorProvisao { get; set; }

    }
}
