using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model
{
    public class InfoAdicional
    {
        public DateTime DatBase {  get; set; }

        public int NumeroControle { get; set; }

        public int IdOperacao { get; set; }

        public int Sequencia { set; get; }

        public string TipoInfo { get; set; }    

    }
}
