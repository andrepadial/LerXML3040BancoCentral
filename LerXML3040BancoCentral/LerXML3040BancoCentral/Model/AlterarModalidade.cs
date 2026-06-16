using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model
{
    public class AlterarModalidade
    {
        public string CpfCnpj {  get; set; }
        public string CodigoCliente { get; set; }
        public string TipoPessoa { get; set; }
        public string NumeroContrato { get; set; }
        public string Ipoc { get; set; }
        public string ModalidadeAtual    { get; set; }
        public string ModalidadeNova { get; set; }
        public string Datbase { get; set; }
        public string IpocNovo { get; set; }

    }
}
