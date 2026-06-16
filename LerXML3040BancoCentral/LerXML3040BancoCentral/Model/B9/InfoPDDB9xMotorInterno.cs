using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model.B9
{
    public class InfoPDDB9xMotorInterno
    {
        public DateTime DatBase { get; set; }

        public string CodColigada { get; set; }
        public string CodSistema { get; set; }
        public Int32 NroControle { get; set; }
        public Int32 IdOperacao { get; set; }
        public string CodModalidade { get; set; }
        public string Ipoc { get; set; }
        public string IpocCalculado { get; set; }
        public string CodigoCliente { get; set; }
        public string CpfCnpj { get; set; }
        public string TpCliente { get; set; }
        public string NumeroContrato { get; set; }
        public int DiasAtraso { get; set; }
        public string Saida { get; set; }
        public string Prejuizo { get; set; }
        public string Vencido { get; set; }
        public string EstagioLegado { get; set; }
        public string CarteiraLegado { get; set; }
        public decimal ValorPrincipal { get; set; }
        public decimal SaldoContabil { get; set; }
        public string CarteiraMotorInterno{ get; set; }
        public string EstagioMotorInterno { get; set; }
        public string AtivoProblematicoMotorInterno { get; set; }
        public decimal ValorProvisaoMotorInterno { get; set; }       

    }
}
