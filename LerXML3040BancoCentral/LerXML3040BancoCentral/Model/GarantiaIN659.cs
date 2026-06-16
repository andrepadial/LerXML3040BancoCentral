using DocumentFormat.OpenXml.Office.CoverPageProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model
{
    public class GarantiaIN659
    {
        public string DataVigencia { set; get; }
        public string CodigoColigada { set; get; }
        public string TipoCliente { set; get; }
        public string Sistema { set; get; }

        public string CpfCnpj { set; get; }
        public string Ipoc { set; get; }
        public string NumeroContrato { set; get; }
        public string CodigoModalidade { set; get; }
        public string CodigoGarantia { set; get; }
        public string CpfCnpjGarantidor { set; get; }
        public string TipoPessoaGarantidor { set; get; }
        public string PercentualGarantidor { set; get; }
        public string ValorOriginalGarantia { set; get; }

        public string ValorAtualGarantia { set; get; }
        public DateTime DataUltimaAvaliacao { set; get; }
        public string SituacaoGarantia { set; get; }
        public string IdentificacaoGarantia { set; get; }
        public string TipoValorGarantia { set; get; }
        public string PercentualReavaliado { set; get; }
        public string Compartilhamento { set; get; }

        public string IpocCalculado { set; get; }
    }
}
