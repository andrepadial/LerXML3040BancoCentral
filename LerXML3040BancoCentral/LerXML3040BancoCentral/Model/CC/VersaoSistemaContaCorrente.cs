using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model.CC
{
    public class VersaoSistemaContaCorrente
    {
        public int Sequencial { get; set; }

        public string Versao {  get; set; }

        public string NomeScript { get; set; }
        public string CodUsuario { get; set; }

        public string DataAAtu { get; set; }


    }
}
