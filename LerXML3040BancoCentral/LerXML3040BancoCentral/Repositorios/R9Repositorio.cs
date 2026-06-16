using Dapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using LerXML3040BancoCentral.Model;
using LerXML3040BancoCentral.Model.R9;
using LerXML3040BancoCentral.Seguranca;
using System.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using Read3040Bacen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;


namespace LerXML3040BancoCentral.Repositorios
{
    public class R9Repositorio
    {
        public string conexaoBD { get; set; }
        public string server { get; set; }
        public string caminhoDat { get; set; }


        public R9Repositorio()
        {
            setarConexao();
        }

        private void setarConexao()
        {
            Credencial credencial = new Credencial();
            credencial = obterCredencial();
            setarServer();

            this.conexaoBD = String.Concat("Server=", this.server, ";",
                "Database=AB_GESTAORC;",
                "User Id=", credencial.UserName, ";",
                "Password=", credencial.Password, ";",
                "Encrypt=false;",
                "TrustServerCertificate=true"
                );

            //this.conexaoBD = String.Concat("Server=", this.server, ";",
            //   "Database=AB_GESTAORC;",
            //   "User Id=USER_R9;",
            //   "Password=P8Ui45M@10;",
            //   "Encrypt=false;",
            //   "TrustServerCertificate=true"
            //   );


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

            return Descriptografar.ObterCredenciais(this.caminhoDat).Where(c => c.CodSistema == "B9").FirstOrDefault();

        }

        public List<InfoR9> listarOperacoesComProvisao(DateTime dataVigencia)
        {
            var conexao = new SqlConnection(this.conexaoBD);
            List<InfoR9> operacoes = new List<InfoR9>();

            try
            {

                string comandoSQL = " SELECT IE.CodColigada, IE.CodSistema, O.NRO_CONTROLE AS NroControle, O.ID_OPERACAO AS IdOperacao, IE.CONTRATO_ORIGEM AS NumeroContrato, " +
                " IE.CPF_CNPJ AS CpfCnpj, IE.TIPO_PESSOA AS CodPessoa, IE.CodModalidade, IE.CODCARTEIRA AS Carteira, IE.CODESTAGIO AS Estagio, DiasAtraso, " +
                " O.INDVENCIDO AS Vencido, O.INDSAIDA AS Saida, IE.VALOR_EL AS ValorProvisao, IE.CODPROBLEMATICO AS AtivoProblematico,  " +
                " ('60889128' + O.CODMODALIDADE + O.TIPO_PESSOA + (CASE WHEN CAST(O.TIPO_PESSOA AS INT) IN (2,4,6) THEN SUBSTRING(O.CPF_CNPJ, 1, 8) ELSE O.CPF_CNPJ END) + IE.CONTRATO_ORIGEM) AS Ipoc " +
                " FROM IMPORTACAO_ESTATISTICO IE INNER JOIN OPERACAO O WITH(NOLOCK)  " +
                " ON IE.DATA_BASE = O.DATA_BASE AND IE.CODCOLIGADA = O.CODCOLIGADA AND IE.CODSISTEMA = O.CODSISTEMA AND IE.CONTRATO_ORIGEM = O.CONTRATO_ORIGEM AND IE.CODMODALIDADE = O.CODMODALIDADE " +
                " WHERE IE.DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";


                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                operacoes = conexao.Query<InfoR9>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro na listagem de operações");
            }
            finally
            {
                conexao.Close();
            }

            return operacoes;
        }

        public List<ResumoIntegracaoB9> listarIntegracaoB9R9(DateTime dataVigencia)
        {
            List<ResumoIntegracaoB9> lista = new List<ResumoIntegracaoB9>();
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                string comandoSQL = " SELECT      CODCOLIGADA AS Coligada, CodSistema AS CodigoSistema, COUNT(*) AS Quantidade ";
                comandoSQL +=" FROM OPERACAO WITH(NOLOCK) ";
                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " GROUP BY    CODCOLIGADA, CODSISTEMA ";
                comandoSQL += " ORDER BY CODCOLIGADA, CODSISTEMA ";


                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                lista = conexao.Query<ResumoIntegracaoB9>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();

            }
            catch
            {
                throw new Exception("Erro na listagem de operações");
            }
            finally
            {
                conexao.Close();
            }

            return lista;
        }

        public List<ResumoIntegracaoB9> listarIntegracaoMotorInternoR9(DateTime dataVigencia)
        {
            List<ResumoIntegracaoB9> lista = new List<ResumoIntegracaoB9>();
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                string comandoSQL = " SELECT      CODCOLIGADA AS Coligada, CodSistema AS CodigoSistema, COUNT(*) AS Quantidade ";
                comandoSQL +=" FROM IMPORTACAO_ESTATISTICO WITH(NOLOCK) ";
                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " GROUP BY    CODCOLIGADA, CODSISTEMA ";
                comandoSQL += " ORDER BY CODCOLIGADA, CODSISTEMA ";


                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                lista = conexao.Query<ResumoIntegracaoB9>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();

            }
            catch
            {
                throw new Exception("Erro na listagem de operações");
            }
            finally
            {
                conexao.Close();
            }

            return lista;
        }

        public List<InfoContratoIntegracaoMotor> listarContratoR9IntegracaoMotor(DateTime dataVigencia, string codigoSistema, string numeroContrato)
        {
            List<InfoContratoIntegracaoMotor> lista = new List<InfoContratoIntegracaoMotor>();
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                string comandoSQL = " SELECT CodColigada, CodSistema, CONTRATO_ORIGEM AS NumeroContrato, "+
                    " CPF_CNPJ AS CpfCnpj, TIPO_PESSOA AS CodPessoa, CodModalidade, CODCARTEIRA AS Carteira, CODESTAGIO AS Estagio, " +
                    " VALOR_EL AS ValorProvisao, CODPROBLEMATICO AS AtivoProblematico " +
                    " FROM IMPORTACAO_ESTATISTICO IE ";

                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODSISTEMA = '" + codigoSistema + "' ";
                comandoSQL += " AND CONTRATO_ORIGEM = '" + numeroContrato + "' ";

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                lista = conexao.Query<InfoContratoIntegracaoMotor>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();

            }
            catch
            {
                throw new Exception("Erro na listagem de operações");
            }
            finally
            {
                conexao.Close();
            }

            return lista;
        }

        public List<InfoContratoIntegracaoMotor> listarR9IntegracaoMotorInterno(DateTime dataVigencia)
        {
            List<InfoContratoIntegracaoMotor> lista = new List<InfoContratoIntegracaoMotor>();
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                string comandoSQL = " SELECT CodColigada, CodSistema, CONTRATO_ORIGEM AS NumeroContrato, "+
                    " CPF_CNPJ AS CpfCnpj, TIPO_PESSOA AS CodPessoa, CodModalidade, CODCARTEIRA AS Carteira, CODESTAGIO AS Estagio, " +
                    " VALOR_EL AS ValorProvisao, CODPROBLEMATICO AS AtivoProblematico " +
                    " FROM IMPORTACAO_ESTATISTICO IE ";

                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";                

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                lista = conexao.Query<InfoContratoIntegracaoMotor>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();

            }
            catch
            {
                throw new Exception("Erro na listagem de operações");
            }
            finally
            {
                conexao.Close();
            }

            return lista;
        }

        public List<ResumoIntegracaoMotorR9> listarResumoIntegracaoMotorR9(DateTime datavigencia)
        {
            List<ResumoIntegracaoMotorR9> lista = new List<ResumoIntegracaoMotorR9>();
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                string comandoSQL = " SELECT      CodColigada AS Empresa, (S.CODSISTEMA + ' - ' + S.DESCSISTEMA) AS Sistema, SUM(IE.VALOR_EL) AS TotalProvisao " +
                " FROM IMPORTACAO_ESTATISTICO IE INNER JOIN CAD_SISTEMA S ON IE.CODSISTEMA = S.CODSISTEMA " +
                " WHERE DATA_BASE = '" + datavigencia.ToString("yyyy-MM-dd") + "' " +
                " GROUP BY    CodColigada, (S.CODSISTEMA + ' - ' + S.DESCSISTEMA) " +
                " ORDER BY    CodColigada, (S.CODSISTEMA + ' - ' + S.DESCSISTEMA) ";
                                
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                lista = conexao.Query<ResumoIntegracaoMotorR9>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();

            }
            catch
            {
                throw new Exception("Erro na listagem de operações");
            }
            finally
            {
                conexao.Close();
            }

            return lista;
        }

        public bool atualizarControleProcessamento(DateTime dataVigencia, string codEmpresa, string codSistema)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL = " UPDATE CONTROLE_IMPORTACAO SET STATUS_MOVIMENTO = 'P' ";                
                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODCOLIGADA = " + codEmpresa;
                comandoSQL += " AND CODSISTEMA = '" + codSistema + "' ";

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    );

                sucesso = true;

            }
            catch
            {
                sucesso = false;
            }
            finally
            {
                conexao.Close();
            }


            return sucesso;

        }

        public bool ajustarContratosErradosColigada600MotorInterno(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL = " UPDATE I " +
                " SET CONTRATO_ORIGEM = REPLICATE('0', 10 - LEN(CONTRATO_ORIGEM)) + RTRIM(LTRIM(CONTRATO_ORIGEM)) " +
                " FROM IMPORTACAO_ESTATISTICO I " +
                " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND CODCOLIGADA = 600 ";

                comandoSQL += " UPDATE I " +
                " SET CONTRATO_ORIGEM = REPLICATE('0', 10 - LEN(CONTRATO_ORIGEM)) + RTRIM(LTRIM(CONTRATO_ORIGEM)) " +
                " FROM IMPORTACAO_ESTATISTICO_ALOCACAO I " +
                " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND CODCOLIGADA = 600 ";

                comandoSQL += " UPDATE I " +
                " SET CONTRATO_ORIGEM = REPLICATE('0', 10 - LEN(CONTRATO_ORIGEM)) + RTRIM(LTRIM(CONTRATO_ORIGEM)) " +
                " FROM IMPORTACAO_ESTATISTICO_ALOCACAO_LEGADO I " +
                " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND CODCOLIGADA = 600 ";

                comandoSQL += " UPDATE I " +
                " SET CONTRATO_ORIGEM = REPLICATE('0', 10 - LEN(CONTRATO_ORIGEM)) + RTRIM(LTRIM(CONTRATO_ORIGEM)) " +
                " FROM IMPORTACAO_ESTATISTICO_LEGADO I " +
                " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND CODCOLIGADA = 600 ";

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    );

                sucesso = true;

            }
            catch
            {
                sucesso = false;
            }
            finally
            {
                conexao.Close();
            }


            return sucesso;

        }

        public bool ajustarColigada600ImportacaoEstatistico(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL = " UPDATE IMPORTACAO_ESTATISTICO SET CODCOLIGADA = 600 ";
                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODSISTEMA = 301 ";

                comandoSQL = " UPDATE IMPORTACAO_ESTATISTICO_ALOCACAO SET CODCOLIGADA = 600 ";
                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODSISTEMA = 301 ";

                comandoSQL = " UPDATE CONTROLE_IMPORTACAO SET CODCOLIGADA = 600, STATUS_MOVIMENTO = 'P' ";
                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODSISTEMA = 301 ";

                comandoSQL = " UPDATE ATIVO_PROBLEMATICO SET CODCOLIGADA = 600 ";
                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODSISTEMA = 301 ";

                comandoSQL = " UPDATE CARTEIRA_RISCO SET CODCOLIGADA = 600 ";
                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODSISTEMA = 301 ";

                comandoSQL = " UPDATE ESTAGIO_RISCO SET CODCOLIGADA = 600 ";
                comandoSQL += " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODSISTEMA = 301 ";


                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    );

                sucesso = true;

            }
            catch
            {
                sucesso = false;
            }
            finally
            {
                conexao.Close();
            }


            return sucesso;
        }

        public bool ajustarCodPessoa(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL = " UPDATE CLIENTES_INTEGRACAOB9 ";
                comandoSQL += " SET CODPESSOA = CONVERT(VARCHAR(1), CAST(CODPESSOA AS INT)) ";                
                comandoSQL += " WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND LEN(CODPESSOA) > 1 ";


                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    );

                sucesso = true;

            }
            catch
            {
                sucesso = false;
            }
            finally
            {
                conexao.Close();
            }


            return sucesso;
        }

        public List<ImportacaoEstatistico> listarImportacaoEstatistico(DateTime dataVigencia)
        {
            List<ImportacaoEstatistico> lista = new List<ImportacaoEstatistico>();
            var conexao = new SqlConnection(this.conexaoBD);
            string comandoSQL = String.Empty;

            try
            {

                comandoSQL += " SELECT C.DATA_BASE AS DataVigencia, ";
                comandoSQL += " C.CodColigada AS CodigoColigada, ";
                comandoSQL += " C.CodSistema + ' - ' + S.DESCSISTEMA AS Sistema, ";
                comandoSQL += " C.NRO_CONTROLE AS NumeroControle, ";
                comandoSQL += " C.ID_OPERACAO AS IdOperacao, ";
                comandoSQL += " C.CodModalidade AS CodigoModalidade, ";
                comandoSQL += " IPOC_3040 AS Ipoc, ";
                comandoSQL += " ('60889128' + C.CODMODALIDADE + C.TIPO_PESSOA +  (CASE WHEN CAST(C.TIPO_PESSOA AS INT) IN(2, 4, 6) ";
                comandoSQL += " THEN SUBSTRING(C.CPF_CNPJ, 1, 8) ELSE C.CPF_CNPJ END) +C.CONTRATO_ORIGEM) AS IpocCalculado, ";
                comandoSQL += " CASE WHEN CAST(C.TIPO_PESSOA AS INT) IN(2, 4, 6) THEN SUBSTRING(C.CPF_CNPJ, 1, 8) ELSE C.CPF_CNPJ END AS CodigoCliente, ";
                comandoSQL += " C.CPF_CNPJ AS CpfCnpj, ";
                comandoSQL += " C.TIPO_PESSOA AS TipoCliente, ";
                comandoSQL += " C.CONTRATO_ORIGEM AS NumeroContrato, ";
                comandoSQL += " C.DiasAtraso, ";
                comandoSQL += " INDSAIDA AS SaidaOperacao, ";
                comandoSQL += " 'N' AS Prejuizo, ";
                comandoSQL += " INDVENCIDO AS OperacaoVencida, ";
                comandoSQL += " C.VALOR_PRINCIPAL_EAD AS ValorPrincipal, ";
                comandoSQL += " C.VALOR_PRINCIPAL_EAD AS SaldoContabil, ";
                comandoSQL += " IE.CODCARTEIRA AS ClassificacaoCarteiraMotorInterno, ";
                comandoSQL += " IE.CODESTAGIO AS ClassificacaoEstagioMotorInterno, ";
                comandoSQL += " IE.CODPROBLEMATICO AS ClassificacaoAtivoProblematicoMotorInterno, ";
                comandoSQL += " IE.VALOR_EL AS ValorProvisaoMotorInterno ";
                comandoSQL += " FROM AB_GESTAORC.dbo.OPERACAO C WITH(NOLOCK) INNER JOIN AB_GESTAORC.dbo.IMPORTACAO_ESTATISTICO IE ";
                comandoSQL += " ON IE.DATA_BASE = C.DATA_BASE ";
                comandoSQL += " AND IE.CODCOLIGADA = C.CODCOLIGADA ";
                comandoSQL += " AND IE.CODSISTEMA = C.CODSISTEMA ";
                comandoSQL += " AND IE.CPF_CNPJ = C.CPF_CNPJ ";
                comandoSQL += " AND IE.CONTRATO_ORIGEM = C.CONTRATO_ORIGEM ";
                comandoSQL += " AND IE.CODMODALIDADE = C.CODMODALIDADE ";
                comandoSQL += " AND IE.TIPO_PESSOA = C.TIPO_PESSOA ";
                comandoSQL += " INNER JOIN AB_GESTAORC.dbo.CAD_SISTEMA S ";
                comandoSQL += " ON C.CODSISTEMA = S.CODSISTEMA ";
                comandoSQL += " WHERE C.DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND C.CODCOLIGADA IN (1) ";
                comandoSQL += " ORDER BY   C.CODCOLIGADA, (C.CodSistema + ' - ' + S.DESCSISTEMA), C.NRO_CONTROLE, C.ID_OPERACAO ";

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                lista = conexao.Query<ImportacaoEstatistico>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();

            }
            catch
            {
                throw new Exception("Erro na listagem de operações");
            }
            finally
            {
                conexao.Close();
            }

            return lista;
        }

        public List<ListarMotivoAlocacaoEstagio> listarMotivoAlocacaoEstagioMotorInterno(DateTime dataVigencia)
        {
            List<ListarMotivoAlocacaoEstagio> lista = new List<ListarMotivoAlocacaoEstagio>();
            var conexao = new SqlConnection(this.conexaoBD);
            string comandoSQL = String.Empty;

            try
            {

                comandoSQL = " SELECT C.DATA_BASE AS DataVigencia, " +
            " C.CodColigada AS CodigoColigada, " + 
            " C.CodSistema + ' - ' + S.DESCSISTEMA AS Sistema, " +            
            " C.CodModalidade AS CodigoModalidade, " + 
            " IPOC_3040 AS Ipoc, " +
            
            " (SUBSTRING(CO.CNPJIF, 1, 8) + " +
                " C.CODMODALIDADE + " +
                " C.TIPO_PESSOA +  (CASE WHEN CAST(C.TIPO_PESSOA AS INT) IN(2, 4, 6) THEN SUBSTRING(C.CPF_CNPJ, 1, 8) ELSE C.CPF_CNPJ END) + " +
                " C.CONTRATO_ORIGEM " +
            " ) AS IpocCalculado, " +

            " CASE WHEN CAST(C.TIPO_PESSOA AS INT) IN(2, 4, 6) THEN SUBSTRING(C.CPF_CNPJ, 1, 8) ELSE C.CPF_CNPJ END AS CodigoCliente, " +
            " C.CPF_CNPJ AS CpfCnpj, " +
            " C.TIPO_PESSOA AS TipoCliente, " +
            " C.CONTRATO_ORIGEM AS NumeroContrato, " +
            " C.DiasAtraso, " +
            " INDSAIDA AS SaidaOperacao, " +
            " 'N' AS Prejuizo, " +
            " INDVENCIDO AS OperacaoVencida, " +
            " C.VALOR_PRINCIPAL_EAD AS ValorPrincipal, " +
            " C.VALOR_PRINCIPAL_EAD AS SaldoContabil, " +
            " IE.CODCARTEIRA AS ClassificacaoCarteiraMotorInterno, "+
            " IE.CODESTAGIO AS ClassificacaoEstagioMotorInterno, " +
            " IE.CODPROBLEMATICO AS ClassificacaoAtivoProblematicoMotorInterno, " +
            " IE.VALOR_EL AS ValorProvisaoMotorInterno, " +
            " IE.CODESTAGIO + ISNULL(IEA.MOTIVO_ALOCACAO_ESTAGIO, '') AS CodigoMotivoAlocacaoEstagio " +
            " FROM AB_GESTAORC.dbo.OPERACAO C WITH(NOLOCK) INNER JOIN AB_GESTAORC.dbo.IMPORTACAO_ESTATISTICO IE WITH(NOLOCK) " +
            " ON IE.DATA_BASE = C.DATA_BASE " +
                " AND IE.CODCOLIGADA = C.CODCOLIGADA " +
                " AND IE.CODSISTEMA = C.CODSISTEMA " +
                " AND IE.CPF_CNPJ = C.CPF_CNPJ " +
                " AND IE.CONTRATO_ORIGEM = C.CONTRATO_ORIGEM " +
                " AND IE.CODMODALIDADE = C.CODMODALIDADE " +
                " AND IE.TIPO_PESSOA = C.TIPO_PESSOA " +
            " INNER JOIN AB_GESTAORC.dbo.CAD_SISTEMA S WITH(NOLOCK) " +
                " ON C.CODSISTEMA = S.CODSISTEMA " +
            " INNER JOIN AB_GESTAORC.dbo.CAD_COLIGADA CO WITH(NOLOCK) " +
                " ON IE.CODCOLIGADA = CO.CODCOLIGADA " +
            " LEFT OUTER JOIN AB_GESTAORC.dbo.IMPORTACAO_ESTATISTICO_ALOCACAO IEA WITH(NOLOCK) " +
                " ON IE.CODCOLIGADA = IEA.CODCOLIGADA " +
                " AND IE.CODSISTEMA = IEA.CODSISTEMA " +
                " AND IE.CPF_CNPJ = IEA.CPF_CNPJ " +
                " AND IE.CONTRATO_ORIGEM = IEA.CONTRATO_ORIGEM " +
                " AND IE.CODMODALIDADE = IEA.CODMODALIDADE " +
                " AND IE.ID_CONTROLE_IMPORTACAO = IEA.ID_CONTROLE_IMPORTACAO " +
                " WHERE C.DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND C.CODCOLIGADA = 1 ";                

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                lista = conexao.Query<ListarMotivoAlocacaoEstagio>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();

            }
            catch
            {
                throw new Exception("Erro na listagem de operações");
            }
            finally
            {
                conexao.Close();
            }

            return lista;
        }


    }
}
