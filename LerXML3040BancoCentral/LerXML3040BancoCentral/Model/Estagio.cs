using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model
{
    public class Estagio
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public List<MotivoAlocacaoEstagio> motivosAlocaoEstagio = new List<MotivoAlocacaoEstagio>();
    }
}
