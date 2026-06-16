using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model
{
    public class TabelaVencimentosBacen
    {
        public string CodigoVencimento { get; set; }

        public string Vencimento { get; set; }

        public int De { get; set; }

        public int Ate { get; set; }
        public string TipoVencimento { get; set; }
    }
}
