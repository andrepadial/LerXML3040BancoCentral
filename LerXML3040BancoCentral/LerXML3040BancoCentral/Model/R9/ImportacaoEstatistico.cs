using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model.R9
{
    public class ImportacaoEstatistico
    {
        public DateTime DataVigencia { get; set; }
        public int CodigoColigada { get; set; }
        public string Sistema {  get; set; }
        public int NumeroControle { get; set; }
        public int IdOperacao { get; set; }
        public string CodigoModalidade { get; set; }
        public string Ipoc {  get; set; }
        public string IpocCalculado { get; set; }
        public string CodigoCliente { get; set; }
        public string CpfCnpj { get; set; }

        public string TipoCliente { get; set; }
        public string NumeroContrato { get; set; }
        public int DiasAtraso { get; set; }
        public string SaidaOperacao { get; set; }
        public string Prejuizo { get; set; }
        public string OperacaoVencida {  get; set; }
        public decimal ValorPrincipal { get; set; }
        public decimal SaldoContabil { get; set; }
        public string ClassificacaoCarteiraMotorInterno { get; set; }
        public string ClassificacaoEstagioMotorInterno { get; set; }
        public string ClassificacaoAtivoProblematicoMotorInterno { get; set; }
        public decimal ValorProvisaoMotorInterno { get; set; }



    }
}
