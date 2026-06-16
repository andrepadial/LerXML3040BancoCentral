using Dapper;
using LerXML3040BancoCentral.Model;
using LerXML3040BancoCentral.Model.B9;
using LerXML3040BancoCentral.Model.CC;
using LerXML3040BancoCentral.Seguranca;
using System.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using Read3040Bacen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace LerXML3040BancoCentral.Repositorios
{
    public class CCRepositorio
    {
        public string conexaoBD { get; set; }
        public string server { get; set; }
        public string caminhoDat { get; set; }


        private void setarConexao()
        {
            Credencial credencial = new Credencial();
            credencial = obterCredencial();
            setarServer();

            this.conexaoBD = String.Concat("Server=", this.server, ";",
                "Database=AB_OPERACOES_IFRS9;",
                "User Id=", credencial.UserName, ";",
                "Password=", credencial.Password, ";",
                "Encrypt=false;",
                "TrustServerCertificate=true"
                );


            //Server=MyServerName;Database=MyDbName;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true

        }

        private void setarServer()
        {
#if (DEBUG)
            this.server = "HOMOG";
#else
            this.server = "PROD";
#endif

        }        

        private Credencial obterCredencial()
        {
#if(DEBUG)
            this.caminhoDat = String.Concat(ConfigurationManager.AppSettings["diretorioDat"], @"\HMG\cactrlj.dat");
#else
            this.caminhoDat = String.Concat(ConfigurationManager.AppSettings["diretorioDat"], @"\PRD\cactrlj.dat");            
#endif

            return Descriptografar.ObterCredenciais(this.caminhoDat).Where(c => c.CodSistema == "CC").FirstOrDefault();

        }


        public CCRepositorio()
        {
            setarConexao();
        }


        public List<VersaoSistemaContaCorrente> getVersoes()
        {
            List<VersaoSistemaContaCorrente> lista = new List<VersaoSistemaContaCorrente>();
            string comandoSQL = " SELECT Sequencial, Versao, NomeScript, CodUsuario, DataAAtu FROM AB_CONTACORRENTE.dbo.VERSAO_SISTEMA_CC ORDER BY SEQUENCIAL ";
            
            var conexao = new SqlConnection(this.conexaoBD);            

            try
            {
                conexao.Open();
                lista = conexao.Query<VersaoSistemaContaCorrente>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();
            }
            catch
            {
                throw new Exception("Erro ao consultar informações");
            }
            finally
            {
                conexao.Close();
            }


            return lista;
        }

    }
}
