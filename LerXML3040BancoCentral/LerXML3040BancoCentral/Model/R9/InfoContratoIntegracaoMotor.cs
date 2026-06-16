using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Model.R9
{
    public class InfoContratoIntegracaoMotor
    {
        public int CodColigada { get; set; }
        public string CodSistema { get; set; }        
        public string NumeroContrato { get; set; }
        public string CpfCnpj { get; set; }
        public string CodPessoa { get; set; }
        public string CodModalidade { get; set; }
        public string Carteira { get; set; }
        public string Estagio { get; set; }        
        public decimal ValorProvisao { get; set; }        
        public string AtivoProblematico { get; set; }
    }
}
