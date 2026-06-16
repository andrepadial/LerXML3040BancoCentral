using Dapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Math;
using LerXML3040BancoCentral.Model;
using LerXML3040BancoCentral.Model.B9;
using LerXML3040BancoCentral.Model.R9;
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
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;


namespace LerXML3040BancoCentral.Repositorios
{
    public class B9Repositorio
    {

       

        public string conexaoBD { get; set; }
        public string server { get; set; }
        public string caminhoDat { get; set; }


        public B9Repositorio()
        {
            setarConexao();
        }

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


            //this.conexaoBD = String.Concat("Server=", this.server, ";",
            //    "Database=AB_OPERACOES_IFRS9;",
            //    "User Id=USER_R9;",
            //    "Password=P8Ui45M@10;",
            //    "Encrypt=false;",
            //    "TrustServerCertificate=true"
            //    );






        }

        private void setarServer()
        {
#if (DEBUG)
            this.server = "HOMOG";
            //this.server = "COREDEVSQL01,1501";
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

        public bool tratarContratosPJ541DiasAtraso(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_AJUSTAR_CONTRATOS_PJ_541_DIAS_ATRASO",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarIPOC(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_AJUSTAR_IPOC",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarContratosPrejuizoSemClassificacao4966(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_CONTRATOS_PREJUIZO_SEM_CLASSIFICACAO_4966",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarDuplicadosRegra164(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_DUPLICADOS_REGRA_164",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarGrupoModalidade18(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_GRUPO_MODALIDADE_18_QTDINST",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarQuantidadeInstrumentoGrupoModalidade18(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL = " UPDATE O " +
                            " SET QTDINST = 1000 " +
                            " FROM OPERACOES O ";
                comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND (QTDINST IS NULL OR QTDINST = 0) ";
                comandoSQL += " AND O.CODGRPMODALIDADE = '18' ";

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

        public bool setarNuloQuantidadeInstrumentoGrupoModalidadeDiferente18(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL = " UPDATE O " +
                            " SET QTDINST = NULL " +
                            " FROM OPERACOES O ";
                comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";                
                comandoSQL += " AND O.CODGRPMODALIDADE != '18' ";

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

        public bool tratarIndexador(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_INDEXADOR_OPERACOES",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarInformacaoAdicional1001(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_INFOADICIONAL_1001",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarLocalidade(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_LOCALIDADE",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarLocalidadeV2(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL = " UPDATE O " +
                " SET CEP = '01418100', CODAGENCIAORI = '00019', CODLOCALIDADE = '10058' ";                
                comandoSQL += " FROM OPERACOES O ";
                comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND (CEP IS NULL OR LEN(RTRIM(LTRIM(CEP))) = 0) ";

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

        public bool tratarInformacaoAdicional1701(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_OPERACOES_INFADICIONAL_1701",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarAutorizacaoBacen(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_PERMITEBC_CLIENTE",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarPorteCliente(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_PORTE_CLIENTE",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        private bool tratarPorteClientePF(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL = " UPDATE C " +
                " SET CODPORTE = CASE WHEN (FATURAMENTO IS NULL OR FATURAMENTO <= 1.00) THEN '1' " +
                                        " WHEN FATURAMENTO <= 1508.00 THEN '2' " +
                                        " WHEN FATURAMENTO BETWEEN 1508.00 AND (1508.00 * 2) THEN '3' " +
                                        " WHEN FATURAMENTO BETWEEN (1508.00 * 2) AND (1508.00 * 3) THEN '4' " +
                                        " WHEN FATURAMENTO BETWEEN (1508.00 * 3) AND (1508.00 * 5) THEN '5' " +
                                        " WHEN FATURAMENTO BETWEEN (1508.00 * 5) AND (1508.00 * 10) THEN '6' " +
                                        " WHEN FATURAMENTO BETWEEN (1508.00 * 10) AND (1508.00 * 20) THEN '7' " +
                                    " ELSE '8' END " +
                " FROM    CLIENTES C " +
                " WHERE DATBASE = '"+ dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND CODPESSOA IN ('1', '3', '5') " +
                " AND (CODPORTE IS NULL OR CODPORTE NOT IN ('0', '1', '2', '3', '4', '5', '6', '7', '8')) ";

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

        private bool tratarPorteClientePJ(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL = " UPDATE C " +
        " SET CODPORTE = CASE WHEN(FATURAMENTO IS NULL) THEN '0' " +
                                " WHEN FATURAMENTO <= 360000 THEN '1' " +
                                " WHEN FATURAMENTO <= 480000 THEN '2' " +
                                " WHEN FATURAMENTO <= 300000000 THEN '3' " +
                            " ELSE '4' END " +
        " FROM    CLIENTES C " +
        " WHERE DATBASE = '"+ dataVigencia.ToString("yyyy-MM-dd") + "' " +
        " AND CODPESSOA IN ('2', '4', '6') " +
        " AND (CODPORTE IS NULL OR CODPORTE NOT IN ('0', '1', '2', '3')) " +
        " AND CODPESSOA IN ('1', '3', '5') " +
        " AND (CODPORTE IS NULL OR CODPORTE NOT IN ('0', '1', '2', '3', '4', '5', '6', '7', '8')) ";

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
        public bool tratarPorteClienteV2(DateTime dataVigencia)
        {
            return (tratarPorteClientePF(dataVigencia) && tratarPorteClientePJ(dataVigencia));
        }

        public bool tratarRegraQ01(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_REGRA_Q01",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarValoresNegativos(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_VALORES_NEGATIVOS",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarInfoAdicionalLCILCA(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_INFOADICIONAL_LCI_LCA",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarRegraQ03(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_REGRA_Q03",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool tratarRegra181(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_TRATAR_REGRA_181",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public bool manterContratosPJEmPrejuizo(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DATAVIGENCIA", dataVigencia);
                parameters.Add("@CODCOLIGADAENT", "001");
                parameters.Add("@CONTRATOOP", null);

                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                var retorno = conexao.Query("SOF_B9_MANTER_OPERACOES_PJ_PREJUIZO",
                    parameters,
                    null,
                    true,
                    null,
                    commandType: CommandType.StoredProcedure
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

        public List<InfoAdicional> excluirInfoAdicional02(DateTime dataVigencia, string numeroContrato)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);
            string comandoSQL = String.Empty;
            List<InfoAdicional> infoAdicional = new List<InfoAdicional>();

            try
            {
                comandoSQL = "SELECT O.DatBase, O.NROCONTROLE AS NumeroControle, O.IdOperacao, I.Sequencia, i.CODTIPCESSAO as TipoInfo FROM OPERACOES O WITH(NOLOCK) INNER JOIN INFADICIONAISOPE I ON O.DATBASE = I.DATBASE AND O.CODCOLIGADA = I.CODCOLIGADA AND O.NROCONTROLE = I.NROCONTROLE AND O.IDOPERACAO = I.IDOPERACAO ";
                comandoSQL += "WHERE O.DATBASE = '" + dataVigencia.ToString("yyyyMMdd") + "' ";
                comandoSQL += "AND O.CONTRATO_ORIGINAL = '" + numeroContrato + "' ";
                //comandoSQL += "AND I.CODTIPCESSAO LIKE '02%'";


                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                infoAdicional = conexao.Query<InfoAdicional>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();

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

            return infoAdicional;
        }
        public bool ajustarContratosParaWO(DateTime dataVigencia, string codSistema, string numeroContrato)
        {
            bool sucesso = false;            
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL += " IF OBJECT_ID('tempdb..#Contratos') IS NOT NULL DROP TABLE #Contratos ";
                comandoSQL += " IF OBJECT_ID('tempdb..#Vencimentos') IS NOT NULL DROP TABLE #Vencimentos ";
                comandoSQL += " SELECT * into		#Contratos FROM OPERACOES WITH(NOLOCK) WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "'   " +
                    "AND CONTRATO IN('" + numeroContrato + "')    " +
                    "AND CODSISTEMA IN('" + codSistema + "')";

                comandoSQL += " SELECT O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO, SUM(V.VLRVENCTO) AS TotalVencimentos ";
                comandoSQL += " INTO	#Vencimentos FROM VENCIMENTOSOPE V INNER JOIN #Contratos O ON ";
                comandoSQL += " V.DATBASE = O.DATBASE AND V.CODCOLIGADA = O.CODCOLIGADA AND V.NROCONTROLE = O.NROCONTROLE AND V.IDOPERACAO = O.IDOPERACAO ";
                comandoSQL += " WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' GROUP BY    O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO ";

                comandoSQL += " DELETE  V FROM    VENCIMENTOSOPE V INNER JOIN #Contratos O ON V.DATBASE = O.DATBASE AND V.NROCONTROLE = O.NROCONTROLE  AND V.IDOPERACAO = O.IDOPERACAO WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";

                comandoSQL += " UPDATE O SET INDPREJUIZO = 'S' ";
                comandoSQL += " FROM OPERACOES O INNER JOIN #Contratos C ON  O.DATBASE = C.DATBASE AND O.CODCOLIGADA = C.CODCOLIGADA AND O.NROCONTROLE = C.NROCONTROLE  AND O.IDOPERACAO = C.IDOPERACAO ";
                comandoSQL += " AND O.CODSISTEMA = C.CODSISTEMA AND O.CNPJCPFCLI = C.CNPJCPFCLI AND O.DVCNPJCLI = C.DVCNPJCLI AND O.IPOC = C.IPOC ";
                comandoSQL += " AND O.CONTRATO = C.IPOC AND O.CODMODALIDADE = O.CODMODALIDADE ";
                comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";

                comandoSQL += " INSERT INTO VENCIMENTOSOPE ";
                comandoSQL += " SELECT  V.DATBASE, V.CODCOLIGADA, V.NROCONTROLE, V.IDOPERACAO, VENC.CODVENCTO, V.TotalVencimentos AS VLRVENCTO ";
                comandoSQL += " FROM    #Vencimentos V INNER JOIN #Contratos O ";
                comandoSQL += " ON V.DATBASE = O.DATBASE ";
                comandoSQL += " AND V.CODCOLIGADA = O.CODCOLIGADA AND V.NROCONTROLE = O.NROCONTROLE AND V.IDOPERACAO = O.IDOPERACAO ";
                comandoSQL += " INNER JOIN(SELECT CODVENCTO, FAIXA1, FAIXA2 FROM INTER_VENCIMENTOS WHERE   DATVIGENCIA = (SELECT MAX(DATVIGENCIA) FROM INTER_VENCIMENTOS) AND INDVENCTO = 'P') AS VENC ON O.DIASATRASO BETWEEN FAIXA1 AND FAIXA2 ";


                comandoSQL += " DROP TABLE #Contratos ";
                comandoSQL += " DROP TABLE #Vencimentos ";

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
        public bool ajustarContratosParaWOQG(DateTime dataVigencia, string codSistema, string numeroContrato)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL += " IF OBJECT_ID('tempdb..#Contratos') IS NOT NULL DROP TABLE #Contratos ";
                comandoSQL += " IF OBJECT_ID('tempdb..#Vencimentos') IS NOT NULL DROP TABLE #Vencimentos ";
                comandoSQL += " SELECT * into		#Contratos FROM OPERACOES WITH(NOLOCK) WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "'   ";

                comandoSQL += " AND CODSISTEMA IN('" + codSistema + "')";
                comandoSQL += " AND CONTRATO IN (";

                comandoSQL += "'0010067720',";
                comandoSQL += "'0000002376',";
                comandoSQL += "'0000002400',";
                comandoSQL += "'0000004623',";
                comandoSQL += "'0000006159',";
                comandoSQL += "'0000007969',";
                comandoSQL += "'0000008232',";
                comandoSQL += "'0000008259',";
                comandoSQL += "'0000009930',";
                comandoSQL += "'0000011293',";
                comandoSQL += "'0000011955',";
                comandoSQL += "'0000012030',";
                comandoSQL += "'0000012818',";
                comandoSQL += "'0000013745',";
                comandoSQL += "'0000014322',";
                comandoSQL += "'0000015128',";
                comandoSQL += "'0000016272',";
                comandoSQL += "'0000016566',";
                comandoSQL += "'0000104754',";
                comandoSQL += "'0000117795',";
                comandoSQL += "'0000119119',";
                comandoSQL += "'0000119283',";
                comandoSQL += "'0000119984',";
                comandoSQL += "'0000123232',";
                comandoSQL += "'0000125804',";
                comandoSQL += "'0000125995',";
                comandoSQL += "'0000126028',";
                comandoSQL += "'0000126576',";
                comandoSQL += "'0000127237',";
                comandoSQL += "'0000155977',";
                comandoSQL += "'0000158810',";
                comandoSQL += "'0000159000',";
                comandoSQL += "'0000159689',";
                comandoSQL += "'0001004874',";
                comandoSQL += "'0001006869',";
                comandoSQL += "'0001007091',";
                comandoSQL += "'0001508697',";
                comandoSQL += "'0001691203',";
                comandoSQL += "'0002222193',";
                comandoSQL += "'0002401560',";
                comandoSQL += "'0002898138',";
                comandoSQL += "'0002911142',";
                comandoSQL += "'0003092285',";
                comandoSQL += "'0003121633',";
                comandoSQL += "'0003149350',";
                comandoSQL += "'0003241612',";
                comandoSQL += "'0003498141',";
                comandoSQL += "'0003526480',";
                comandoSQL += "'0003692568',";
                comandoSQL += "'0003735933',";
                comandoSQL += "'0003749209',";
                comandoSQL += "'0003788654',";
                comandoSQL += "'0003828443',";
                comandoSQL += "'0004407298',";
                comandoSQL += "'0004457759',";
                comandoSQL += "'0004562595',";
                comandoSQL += "'0004608277',";
                comandoSQL += "'0004795018',";
                comandoSQL += "'0004811803',";
                comandoSQL += "'0004812672',";
                comandoSQL += "'0004844841',";
                comandoSQL += "'0004854340',";
                comandoSQL += "'0004859911',";
                comandoSQL += "'0004965380',";
                comandoSQL += "'0005235547',";
                comandoSQL += "'0005235555',";
                comandoSQL += "'0005813320',";
                comandoSQL += "'0005874671',";
                comandoSQL += "'0007090619',";
                comandoSQL += "'0007248514',";
                comandoSQL += "'0008219470',";
                comandoSQL += "'0008323234',";
                comandoSQL += "'0008531791',";
                comandoSQL += "'0008536572',";
                comandoSQL += "'0008571750',";
                comandoSQL += "'0008598993',";
                comandoSQL += "'0008958426',";
                comandoSQL += "'0008963349',";
                comandoSQL += "'0008981550',";
                comandoSQL += "'0008991628',";
                comandoSQL += "'0009049284',";
                comandoSQL += "'0009059638',";
                comandoSQL += "'0009085922',";
                comandoSQL += "'0009135113',";
                comandoSQL += "'0009135180',";
                comandoSQL += "'0009138775',";
                comandoSQL += "'0009647734',";
                comandoSQL += "'0009826795',";
                comandoSQL += "'0009835352',";
                comandoSQL += "'0009899067',";
                comandoSQL += "'0009947380',";
                comandoSQL += "'0010234618',";
                comandoSQL += "'0010267850',";
                comandoSQL += "'0010284658',";
                comandoSQL += "'0010355628',";
                comandoSQL += "'0010721012',";
                comandoSQL += "'0011271991',";
                comandoSQL += "'000287611200026081111',";
                comandoSQL += "'000287611200026081129',";
                comandoSQL += "'FNF/20010804',";
                comandoSQL += "'PII25957-9R01',";
                comandoSQL += "'0000004122',";
                comandoSQL += "'PII26377-7',";
                comandoSQL += "'PMC019721-4',";
                comandoSQL += "'PII25721-6',";
                comandoSQL += "'0000011117',";
                comandoSQL += "'CNF09565',";
                comandoSQL += "'CCE20397-1R01',";
                comandoSQL += "'PMT33306-6',";
                comandoSQL += "'PMT25807-5R01',";
                comandoSQL += "'FIN31311-6',";
                comandoSQL += "'FIN31304-1',";
                comandoSQL += "'PII23578-4',";
                comandoSQL += "'PII30097-4',";
                comandoSQL += "'PII24243-1',";
                comandoSQL += "'0005857858',";
                comandoSQL += "'0010424700',";
                comandoSQL += "'0005982261',";
                comandoSQL += "'0010267850',";
                comandoSQL += "'0000002400',";
                comandoSQL += "'0003469206',";
                comandoSQL += "'0003432345',";
                comandoSQL += "'0000125952',";
                comandoSQL += "'0000126576',";
                comandoSQL += "'0000118085'";


                comandoSQL += " ) ";


                comandoSQL += " SELECT O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO, SUM(V.VLRVENCTO) AS TotalVencimentos ";
                comandoSQL += " INTO	#Vencimentos FROM VENCIMENTOSOPE V INNER JOIN #Contratos O ON ";
                comandoSQL += " V.DATBASE = O.DATBASE AND V.CODCOLIGADA = O.CODCOLIGADA AND V.NROCONTROLE = O.NROCONTROLE AND V.IDOPERACAO = O.IDOPERACAO ";
                comandoSQL += " WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' GROUP BY    O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO ";

                comandoSQL += " DELETE  V FROM    VENCIMENTOSOPE V INNER JOIN #Contratos O ON V.DATBASE = O.DATBASE AND V.NROCONTROLE = O.NROCONTROLE  AND V.IDOPERACAO = O.IDOPERACAO WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";

                comandoSQL += " UPDATE O SET INDPREJUIZO = 'S' ";
                comandoSQL += " FROM OPERACOES O INNER JOIN #Contratos C ON  O.DATBASE = C.DATBASE AND O.CODCOLIGADA = C.CODCOLIGADA AND O.NROCONTROLE = C.NROCONTROLE  AND O.IDOPERACAO = C.IDOPERACAO ";
                comandoSQL += " AND O.CODSISTEMA = C.CODSISTEMA AND O.CNPJCPFCLI = C.CNPJCPFCLI AND O.DVCNPJCLI = C.DVCNPJCLI AND O.IPOC = C.IPOC ";
                comandoSQL += " AND O.CONTRATO = C.IPOC AND O.CODMODALIDADE = O.CODMODALIDADE ";
                comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";


                comandoSQL += " INSERT INTO VENCIMENTOSOPE ";
                comandoSQL += " SELECT  V.DATBASE, V.CODCOLIGADA, V.NROCONTROLE, V.IDOPERACAO, VENC.CODVENCTO, V.TotalVencimentos AS VLRVENCTO ";
                comandoSQL += " FROM    #Vencimentos V INNER JOIN #Contratos O ";
                comandoSQL += " ON V.DATBASE = O.DATBASE ";
                comandoSQL += " AND V.CODCOLIGADA = O.CODCOLIGADA AND V.NROCONTROLE = O.NROCONTROLE AND V.IDOPERACAO = O.IDOPERACAO ";
                comandoSQL += " INNER JOIN(SELECT CODVENCTO, FAIXA1, FAIXA2 FROM INTER_VENCIMENTOS WHERE   DATVIGENCIA = (SELECT MAX(DATVIGENCIA) FROM INTER_VENCIMENTOS) AND INDVENCTO = 'P') AS VENC ON O.DIASATRASO BETWEEN FAIXA1 AND FAIXA2 ";


                comandoSQL += " DROP TABLE #Contratos ";
                comandoSQL += " DROP TABLE #Vencimentos ";

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
        public bool ajustarContratosWOParaAtivo(DateTime dataVigencia, string codSistema, string numeroContrato)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL += " IF OBJECT_ID('tempdb..#Contratos') IS NOT NULL DROP TABLE #Contratos ";
                comandoSQL += " IF OBJECT_ID('tempdb..#Vencimentos') IS NOT NULL DROP TABLE #Vencimentos ";
                comandoSQL += " SELECT * into		#Contratos FROM OPERACOES WITH(NOLOCK) WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "'   " +
                    "AND CONTRATO IN('" + numeroContrato + "')    " +
                    "AND CODSISTEMA IN('" + codSistema + "')";

                comandoSQL += " SELECT O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO, SUM(V.VLRVENCTO) AS TotalVencimentos ";
                comandoSQL += " INTO	#Vencimentos FROM VENCIMENTOSOPE V INNER JOIN #Contratos O ON ";
                comandoSQL += " V.DATBASE = O.DATBASE AND V.CODCOLIGADA = O.CODCOLIGADA AND V.NROCONTROLE = O.NROCONTROLE AND V.IDOPERACAO = O.IDOPERACAO ";
                comandoSQL += " WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' GROUP BY    O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO ";

                comandoSQL += " DELETE  V FROM    VENCIMENTOSOPE V INNER JOIN #Contratos O ON V.DATBASE = O.DATBASE AND V.NROCONTROLE = O.NROCONTROLE  AND V.IDOPERACAO = O.IDOPERACAO WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";

                comandoSQL += " INSERT INTO VENCIMENTOSOPE ";
                comandoSQL += " SELECT  V.DATBASE, V.CODCOLIGADA, V.NROCONTROLE, V.IDOPERACAO, CASE WHEN VENC.CODVENCTO IS NULL THEN '199' ELSE VENC.CODVENCTO END AS CODVENCTO, V.TotalVencimentos AS VLRVENCTO ";
                comandoSQL += " FROM    #Vencimentos V INNER JOIN #Contratos O ";
                comandoSQL += " ON V.DATBASE = O.DATBASE ";
                comandoSQL += " AND V.CODCOLIGADA = O.CODCOLIGADA AND V.NROCONTROLE = O.NROCONTROLE AND V.IDOPERACAO = O.IDOPERACAO ";
                comandoSQL += " LEFT JOIN(SELECT CODVENCTO, FAIXA1, FAIXA2 FROM INTER_VENCIMENTOS WHERE   DATVIGENCIA = (SELECT MAX(DATVIGENCIA) FROM INTER_VENCIMENTOS) AND INDVENCTO = 'V') AS VENC ON O.DIASATRASO BETWEEN FAIXA1 AND FAIXA2 ";


                comandoSQL += " DROP TABLE #Contratos ";
                comandoSQL += " DROP TABLE #Vencimentos ";

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
        public bool ajustarContratosWOParaAtivoQG(DateTime dataVigencia, string codSistema, string numeroContrato)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL += " IF OBJECT_ID('tempdb..#Contratos') IS NOT NULL DROP TABLE #Contratos ";
                comandoSQL += " IF OBJECT_ID('tempdb..#Vencimentos') IS NOT NULL DROP TABLE #Vencimentos ";
                comandoSQL += " SELECT * into		#Contratos FROM OPERACOES WITH(NOLOCK) WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "'   ";                                
                comandoSQL += " AND CODSISTEMA IN('" + codSistema + "')";
                comandoSQL += " AND CONTRATO IN (";

                comandoSQL += "'CAP36766-1R01',";
                comandoSQL += "'PII42252-0',";
                comandoSQL += "'PII24008-9',";
                comandoSQL += "'PII21699-1R01',";
                comandoSQL += "'FGF25212-4',";
                comandoSQL += "'PMT34548-4',";
                comandoSQL += "'PII22646-0',";
                comandoSQL += "'PMT31174-9',";
                comandoSQL += "'PMT40075-8',";
                comandoSQL += "'PMT37899-0',";
                comandoSQL += "'PII21948-1',";
                comandoSQL += "'PMT47432-3',";
                comandoSQL += "'PAF08578-3',";
                comandoSQL += "'PMT34406-3',";
                comandoSQL += "'PII27871-8',";
                comandoSQL += "'PAF08698-0R01',";
                comandoSQL += "'PII24717-7',";
                comandoSQL += "'PII25676-4',";
                comandoSQL += "'PAF08609-6R01',";
                comandoSQL += "'PMT37248-7',";
                comandoSQL += "'PMT34901-3',";
                comandoSQL += "'PMT31816-8',";
                comandoSQL += "'PII22066-9',";
                comandoSQL += "'PMT26500-2',";
                comandoSQL += "'CNF38478',";
                comandoSQL += "'PAF07112-8R01',";
                comandoSQL += "'PAF08949-7R01',";
                comandoSQL += "'PII28905-4',";
                comandoSQL += "'PII34367-8',";
                comandoSQL += "'PII21483-6R0001',";
                comandoSQL += "'FGF20383-9R01',";
                comandoSQL += "'FGF38820-1',";
                comandoSQL += "'PAF06812-6H01',";
                comandoSQL += "'PII43154-7',";
                comandoSQL += "'PII39841-7',";
                comandoSQL += "'PMT25642-4',";
                comandoSQL += "'CNF45509',";
                comandoSQL += "'PII28032-3R01',";
                comandoSQL += "'PMT32020-1',";
                comandoSQL += "'PII24806-8R01',";
                comandoSQL += "'PII33770-3',";
                comandoSQL += "'PII24677-3H01',";
                comandoSQL += "'PII28977-4R01',";
                comandoSQL += "'PMT34655-7',";
                comandoSQL += "'PII23754-0',";
                comandoSQL += "'PII21336-7',";
                comandoSQL += "'FGF15792-0',";
                comandoSQL += "'PII28518-5',";
                comandoSQL += "'PII34396-7',";
                comandoSQL += "'PMT45685-1',";
                comandoSQL += "'PAF06864-8',";
                comandoSQL += "'CNF41891',";
                comandoSQL += "'CR96826',";
                comandoSQL += "'PMT34086-3',";
                comandoSQL += "'PAF06480-1',";
                comandoSQL += "'PMT39296-4',";
                comandoSQL += "'PII38148-8',";
                comandoSQL += "'FGF35968-4',";
                comandoSQL += "'PAF09329-9',";
                comandoSQL += "'PII22010-4R01',";
                comandoSQL += "'PII24376-1',";
                comandoSQL += "'PMT31924-9',";
                comandoSQL += "'CAP41437-0',";
                comandoSQL += "'PII21256-7',";
                comandoSQL += "'PMT47182-4',";
                comandoSQL += "'PII26208-3R01',";
                comandoSQL += "'PAF06254-0',";
                comandoSQL += "'CAP28961-6R01',";
                comandoSQL += "'PII43522-6',";
                comandoSQL += "'PAF09901-4H01',";
                comandoSQL += "'PII33462-6',";
                comandoSQL += "'FGF20284-9',";
                comandoSQL += "'PII27908-0',";
                comandoSQL += "'PII25780-2',";
                comandoSQL += "'PMT40080-6',";
                comandoSQL += "'CNF0025395-0',";
                comandoSQL += "'PII23634-3',";
                comandoSQL += "'CNF41737',";
                comandoSQL += "'PII22839-1',";
                comandoSQL += "'PII40983-4',";
                comandoSQL += "'PMT32737-5',";
                comandoSQL += "'PII25498-2',";
                comandoSQL += "'PII24779-8',";
                comandoSQL += "'PII27434-3',";
                comandoSQL += "'PMT31186-4',";
                comandoSQL += "'CNF0012161839',";
                comandoSQL += "'PII32788-9',";
                comandoSQL += "'CNF43228',";
                comandoSQL += "'PII23699-9',";
                comandoSQL += "'FGF36990-5',";
                comandoSQL += "'PII21985-3',";
                comandoSQL += "'PII28935-1',";
                comandoSQL += "'PII22619-7',";
                comandoSQL += "'PII22931-4',";
                comandoSQL += "'PAF08834-9',";
                comandoSQL += "'PII29928-6H01',";
                comandoSQL += "'CAP28739-8R01',";
                comandoSQL += "'PII34373-4',";
                comandoSQL += "'PII43114-1',";
                comandoSQL += "'PII26803-1R01',";
                comandoSQL += "'PII30814-2',";
                comandoSQL += "'PMT36107-5',";
                comandoSQL += "'PMT39435-8',";
                comandoSQL += "'PAF08455-2R01',";
                comandoSQL += "'CNF97093',";
                comandoSQL += "'CNF38637',";
                comandoSQL += "'PII29817-1',";
                comandoSQL += "'PII29299-1',";
                comandoSQL += "'PII26530-0',";
                comandoSQL += "'PAF06513-0',";
                comandoSQL += "'PMT27636-6',";
                comandoSQL += "'PAF09426-2',";
                comandoSQL += "'PII27508-7',";
                comandoSQL += "'FGF20287-3',";
                comandoSQL += "'PII41394-1',";
                comandoSQL += "'PAF09352-9',";
                comandoSQL += "'PMT20478-9',";
                comandoSQL += "'PII32177-2',";
                comandoSQL += "'PII27868-6',";
                comandoSQL += "'PII38908-7',";
                comandoSQL += "'PAF06482-7',";
                comandoSQL += "'FAT025445-2',";
                comandoSQL += "'PII24744-0H01',";
                comandoSQL += "'PII26402-1R01',";
                comandoSQL += "'PII26752-1',";
                comandoSQL += "'PMT33056-7',";
                comandoSQL += "'FGF4702-1',";
                comandoSQL += "'CNF025660-6',";
                comandoSQL += "'PMT33697-1',";
                comandoSQL += "'PII27988-2',";
                comandoSQL += "'PII21675-0',";
                comandoSQL += "'PMT37621-4',";
                comandoSQL += "'PMT41017-9',";
                comandoSQL += "'PII34621-7',";
                comandoSQL += "'PMT31862-1',";
                comandoSQL += "'PII25981-7',";
                comandoSQL += "'CNF44606-1',";
                comandoSQL += "'CNF41689',";
                comandoSQL += "'PMT45435-0',";
                comandoSQL += "'PMT39987-1',";
                comandoSQL += "'PII28197-7',";
                comandoSQL += "'PII30107-1',";
                comandoSQL += "'PII28018-4',";
                comandoSQL += "'PAF07221-7H02',";
                comandoSQL += "'PII34230-5',";
                comandoSQL += "'PII42069-0',";
                comandoSQL += "'PII37611-5',";
                comandoSQL += "'PMT33348-9',";
                comandoSQL += "'PII36325-3',";
                comandoSQL += "'PMT30952-1 - A',";
                comandoSQL += "'CAP38272-4R01',";
                comandoSQL += "'PMT39196-6',";
                comandoSQL += "'CNF44950',";
                comandoSQL += "'PII37126-4',";
                comandoSQL += "'PII42738-1',";
                comandoSQL += "'PMT38398-0',";
                comandoSQL += "'PMT36997-2',";
                comandoSQL += "'PII32458-7',";
                comandoSQL += "'PMTGCH96863-A',";
                comandoSQL += "'PMT30780-5',";
                comandoSQL += "'PMT36139-9',";
                comandoSQL += "'PMT39646-1',";
                comandoSQL += "'PAF06436-4',";
                comandoSQL += "'PMT36129-0',";
                comandoSQL += "'PII23979-5',";
                comandoSQL += "'CNF0000126826',";
                comandoSQL += "'PMT38584-4',";
                comandoSQL += "'CNF37488',";
                comandoSQL += "'PII40964-4',";
                comandoSQL += "'PII28259-5',";
                comandoSQL += "'PMT34540-9R01',";
                comandoSQL += "'PMT37592-8',";
                comandoSQL += "'PMT37596-1',";
                comandoSQL += "'PII28597-0',";
                comandoSQL += "'PII46956-6',";
                comandoSQL += "'PII27236-3',";
                comandoSQL += "'PMT25508-9',";
                comandoSQL += "'FGF19868-6',";
                comandoSQL += "'FGF19453-3',";
                comandoSQL += "'PMT36246-1',";
                comandoSQL += "'PII30243-2',";
                comandoSQL += "'FGF20498-7',";
                comandoSQL += "'PII30133-5',";
                comandoSQL += "'PII26508-8',";
                comandoSQL += "'PII21957-2',";
                comandoSQL += "'PII42979-2',";
                comandoSQL += "'PII29016-7',";
                comandoSQL += "'PII26063-1',";
                comandoSQL += "'PII29991-2',";
                comandoSQL += "'PII24830-6R01',";
                comandoSQL += "'PII27598-9',";
                comandoSQL += "'PII41480-8',";
                comandoSQL += "'PII26997-4',";
                comandoSQL += "'FGF20575-2',";
                comandoSQL += "'PMT40511-1',";
                comandoSQL += "'PAF09088-1H03',";
                comandoSQL += "'CNF47438',";
                comandoSQL += "'PMT46343-3',";
                comandoSQL += "'PII29071-1',";
                comandoSQL += "'PII47546-3',";
                comandoSQL += "'PMT39974-7',";
                comandoSQL += "'PII24759-0',";
                comandoSQL += "'PII33820-6',";
                comandoSQL += "'PII32240-6',";
                comandoSQL += "'PII38914-3',";
                comandoSQL += "'PII33034-2',";
                comandoSQL += "'PII32620-1',";
                comandoSQL += "'PMT31494-1',";
                comandoSQL += "'PII26121-6',";
                comandoSQL += "'PII28968-3',";
                comandoSQL += "'AGR32105-2R01',";
                comandoSQL += "'PII29091-9',";
                comandoSQL += "'PII44230-3',";
                comandoSQL += "'PMT28262-7R01',";
                comandoSQL += "'PII34197-9R01',";
                comandoSQL += "'PII29586-1',";
                comandoSQL += "'PMT41570-7',";
                comandoSQL += "'PMT37056-3',";
                comandoSQL += "'PMT33586-5',";
                comandoSQL += "'PII32908-2',";
                comandoSQL += "'PII36189-4',";
                comandoSQL += "'PII24815-9',";
                comandoSQL += "'PII26276-1',";
                comandoSQL += "'FGF37601-6',";
                comandoSQL += "'PMT21833-3',";
                comandoSQL += "'CNF0011642108',";
                comandoSQL += "'PII41514-5',";
                comandoSQL += "'PII29359-2',";
                comandoSQL += "'PII22671-6R01',";
                comandoSQL += "'GNC25208-4',";
                comandoSQL += "'PMT26986-7R01',";
                comandoSQL += "'PMT30642-7',";
                comandoSQL += "'PII27715-8',";
                comandoSQL += "'PII32355-4',";
                comandoSQL += "'PMT39947-4',";
                comandoSQL += "'PII27511-9',";
                comandoSQL += "'PII24843-0',";
                comandoSQL += "'PMT047393-8',";
                comandoSQL += "'PAF08567-6H01',";
                comandoSQL += "'PII22018-0',";
                comandoSQL += "'PII24110-1',";
                comandoSQL += "'PMT38282-3',";
                comandoSQL += "'PMT38169-4',";
                comandoSQL += "'PII33812-3',";
                comandoSQL += "'PII21603-0',";
                comandoSQL += "'PII27246-2',";
                comandoSQL += "'PAF06989-5',";
                comandoSQL += "'PII33819-1',";
                comandoSQL += "'PMT32092-1',";
                comandoSQL += "'PII23565-1',";
                comandoSQL += "'PII27408-9',";
                comandoSQL += "'PII27624-1',";
                comandoSQL += "'PII21276-5',";
                comandoSQL += "'PMT30158-4R01',";
                comandoSQL += "'PAF06566-0',";
                comandoSQL += "'PAF06299-7R01',";
                comandoSQL += "'PII28411-0',";
                comandoSQL += "'PII27323-8R01',";
                comandoSQL += "'PMT33040-9',";
                comandoSQL += "'PII25565-9R01',";
                comandoSQL += "'PII25026-0',";
                comandoSQL += "'PII32531-0',";
                comandoSQL += "'PII23921-4R001',";
                comandoSQL += "'PMT34597-1R01',";
                comandoSQL += "'PAF06946-4',";
                comandoSQL += "'PMT36163-7',";
                comandoSQL += "'PMT33145-8',";
                comandoSQL += "'PMT38897-2',";
                comandoSQL += "'CAP31734-1R03',";
                comandoSQL += "'PMT39889-9',";
                comandoSQL += "'PII22389-6H01',";
                comandoSQL += "'PII24710-0',";
                comandoSQL += "'PII25907-3',";
                comandoSQL += "'PMT37303-8',";
                comandoSQL += "'FGF19240-3H01',";
                comandoSQL += "'PMT23499-2R03',";
                comandoSQL += "'PMT33057-5',";
                comandoSQL += "'PII24819-1',";
                comandoSQL += "'CNF0125072',";
                comandoSQL += "'PII25683-9',";
                comandoSQL += "'PII26760-3',";
                comandoSQL += "'PII24946-2',";
                comandoSQL += "'CNF38738',";
                comandoSQL += "'CNF0012139426',";
                comandoSQL += "'PMT28802-1',";
                comandoSQL += "'CNF46357',";
                comandoSQL += "'PII42771-1',";
                comandoSQL += "'PII39441-4',";
                comandoSQL += "'PII26187-0',";
                comandoSQL += "'PII26191-0R01',";
                comandoSQL += "'PII24789-7',";
                comandoSQL += "'PMT31623-6',";
                comandoSQL += "'PII27537-6',";
                comandoSQL += "'PII26769-7',";
                comandoSQL += "'PII29968-2',";
                comandoSQL += "'PII29345-1',";
                comandoSQL += "'PII23226-8',";
                comandoSQL += "'CNF0031215-1',";
                comandoSQL += "'PII27541-6',";
                comandoSQL += "'CNF41128',";
                comandoSQL += "'PII24696-3',";
                comandoSQL += "'PII34057-4',";
                comandoSQL += "'PMT40428-0',";
                comandoSQL += "'PMT043364-2',";
                comandoSQL += "'PII29958-3',";
                comandoSQL += "'PII26499-0',";
                comandoSQL += "'PII28433-4H01',";
                comandoSQL += "'PMT44067-1',";
                comandoSQL += "'PII25962-7R01',";
                comandoSQL += "'PII41357-0',";
                comandoSQL += "'PII39596-9',";
                comandoSQL += "'PMT29178-6',";
                comandoSQL += "'PII27458-4',";
                comandoSQL += "'PII34361-9',";
                comandoSQL += "'PII24897-8',";
                comandoSQL += "'PAF06861-3H03',";
                comandoSQL += "'PII35980-7',";
                comandoSQL += "'PII21884-7',";
                comandoSQL += "'FGF38563-8',";
                comandoSQL += "'PII25044-1',";
                comandoSQL += "'PII29187-7',";
                comandoSQL += "'PII26791-9',";
                comandoSQL += "'PII31020-2',";
                comandoSQL += "'PII30001-3',";
                comandoSQL += "'PII43526-9',";
                comandoSQL += "'PII29425-1',";
                comandoSQL += "'PII38647-1',";
                comandoSQL += "'PII21233-4',";
                comandoSQL += "'PII24229-1',";
                comandoSQL += "'PII34914-7',";
                comandoSQL += "'PII23742-4',";
                comandoSQL += "'PII28178-7',";
                comandoSQL += "'PMT37820-2',";
                comandoSQL += "'PII29875-9R01',";
                comandoSQL += "'PII28021-6R01',";
                comandoSQL += "'FGF42709-2',";
                comandoSQL += "'PII28415-2',";
                comandoSQL += "'PMT31706-1',";
                comandoSQL += "'PMT33143-1',";
                comandoSQL += "'PII26205-9',";
                comandoSQL += "'PII21872-1',";
                comandoSQL += "'PII25408-1H01',";
                comandoSQL += "'PMT33645-9',";
                comandoSQL += "'PMT34464-1',";
                comandoSQL += "'CNF41705',";
                comandoSQL += "'PMT42664-8',";
                comandoSQL += "'PMT40002-0',";
                comandoSQL += "'PMT37670-1',";
                comandoSQL += "'PAF06590-8R01.',";
                comandoSQL += "'FGF36981-4',";
                comandoSQL += "'PII38433-2',";
                comandoSQL += "'FGF39324-2',";
                comandoSQL += "'PII41382-6',";
                comandoSQL += "'PMT30879-8',";
                comandoSQL += "'PII33423-8',";
                comandoSQL += "'PII28380-7',";
                comandoSQL += "'PII24178-1',";
                comandoSQL += "'PII21507-4',";
                comandoSQL += "'CNF46748',";
                comandoSQL += "'PMT32724-1',";
                comandoSQL += "'FGF15337-3R01',";
                comandoSQL += "'PII21445-6',";
                comandoSQL += "'PII24383-5',";
                comandoSQL += "'PII26159-9R01',";
                comandoSQL += "'PII29234-5',";
                comandoSQL += "'PII27632-3',";
                comandoSQL += "'PMT33418-0',";
                comandoSQL += "'PII26560-7',";
                comandoSQL += "'PII24113-5',";
                comandoSQL += "'PII29310-2',";
                comandoSQL += "'PII32397-7R01',";
                comandoSQL += "'CNF0010283546',";
                comandoSQL += "'PMT38863-2',";
                comandoSQL += "'PMT37911-0',";
                comandoSQL += "'CNF42728',";
                comandoSQL += "'PMT26812-2',";
                comandoSQL += "'PII38910-1',";
                comandoSQL += "'PMT38694-1',";
                comandoSQL += "'PII21724-4',";
                comandoSQL += "'CNFPMT21092-4',";
                comandoSQL += "'PII29677-9',";
                comandoSQL += "'PII28428-6',";
                comandoSQL += "'PII22856-5',";
                comandoSQL += "'PMT30684-0',";
                comandoSQL += "'PII30091-5',";
                comandoSQL += "'PII22342-2',";
                comandoSQL += "'PII38834-3',";
                comandoSQL += "'PII26202-4',";
                comandoSQL += "'PII28363-3',";
                comandoSQL += "'PMT33983-3',";
                comandoSQL += "'PMT38149-6',";
                comandoSQL += "'PII30529-8',";
                comandoSQL += "'PII28638-1',";
                comandoSQL += "'PII29039-0',";
                comandoSQL += "'PII34807-4',";
                comandoSQL += "'PII29637-2',";
                comandoSQL += "'PAF09315-7R01',";
                comandoSQL += "'PMT38988-0',";
                comandoSQL += "'PII21287-2R01',";
                comandoSQL += "'PII30044-4',";
                comandoSQL += "'PAF07239-1H01',";
                comandoSQL += "'PII28785-1R01.',";
                comandoSQL += "'PII27986-6',";
                comandoSQL += "'PII24177-2',";
                comandoSQL += "'PII27523-4',";
                comandoSQL += "'PMT36747-1',";
                comandoSQL += "'PII25938-9',";
                comandoSQL += "'PMT34201-6',";
                comandoSQL += "'PII34297-7',";
                comandoSQL += "'PII29285-9',";
                comandoSQL += "'PII27965-0',";
                comandoSQL += "'PII34447-8',";
                comandoSQL += "'PII32704-3R01',";
                comandoSQL += "'CNF0016742-3R',";
                comandoSQL += "'CNF42338',";
                comandoSQL += "'PMT38344-1',";
                comandoSQL += "'PMT32665-8',";
                comandoSQL += "'PII43517-8',";
                comandoSQL += "'PII24447-0R01',";
                comandoSQL += "'PMT37613-1R01',";
                comandoSQL += "'PII28170-1',";
                comandoSQL += "'CNF41554',";
                comandoSQL += "'PMT36968-3',";
                comandoSQL += "'PMT40662-3',";
                comandoSQL += "'CNFCNF20924-1',";
                comandoSQL += "'PII27472-3',";
                comandoSQL += "'PII26824-8',";
                comandoSQL += "'PII41485-9',";
                comandoSQL += "'CNF0011591333',";
                comandoSQL += "'FAT021180-7',";
                comandoSQL += "'PII28176-1',";
                comandoSQL += "'PII26286-0',";
                comandoSQL += "'PII28856-0',";
                comandoSQL += "'PII29929-4',";
                comandoSQL += "'PII39133-7',";
                comandoSQL += "'PMT043374-1',";
                comandoSQL += "'PII31064-1',";
                comandoSQL += "'PMT34365-1',";
                comandoSQL += "'PMT34215-8',";
                comandoSQL += "'PMT38439-1',";
                comandoSQL += "'PMT37898-1',";
                comandoSQL += "'PII34290-0',";
                comandoSQL += "'PII29332-7',";
                comandoSQL += "'FGF42647-4',";
                comandoSQL += "'PII33308-2',";
                comandoSQL += "'CNF31441-1',";
                comandoSQL += "'PII24091-3',";
                comandoSQL += "'PMT37734-6',";
                comandoSQL += "'PII29440-8',";
                comandoSQL += "'PII41014-4',";
                comandoSQL += "'PII30946-4',";
                comandoSQL += "'PII35959-3R01',";
                comandoSQL += "'PMT41957-9',";
                comandoSQL += "'PII21845-9',";
                comandoSQL += "'PMT21851-5',";
                comandoSQL += "'PII38805-4',";
                comandoSQL += "'PII39606-5',";
                comandoSQL += "'PMT39754-2',";
                comandoSQL += "'CNF0005653152',";
                comandoSQL += "'CNF0011807883',";
                comandoSQL += "'PMT42266-1',";
                comandoSQL += "'PII21651-9',";
                comandoSQL += "'PMT34362-7',";
                comandoSQL += "'PMT39812-8',";
                comandoSQL += "'PII27306-4',";
                comandoSQL += "'PII38727-1',";
                comandoSQL += "'PMT39776-7',";
                comandoSQL += "'PII32918-1',";
                comandoSQL += "'PII42087-1',";
                comandoSQL += "'PII31358-0',";
                comandoSQL += "'PII39176-8',";
                comandoSQL += "'PII32728-4',";
                comandoSQL += "'PMT48587-7',";
                comandoSQL += "'PII34376-9R01',";
                comandoSQL += "'PMT37467-3',";
                comandoSQL += "'CNF96893R01',";
                comandoSQL += "'PII22494-2',";
                comandoSQL += "'CNF43830',";
                comandoSQL += "'PII28162-9',";
                comandoSQL += "'PII30965-4',";
                comandoSQL += "'CNF40370',";
                comandoSQL += "'PII34659-0',";
                comandoSQL += "'PMT33047-6R01',";
                comandoSQL += "'PMT34903-0',";
                comandoSQL += "'CNF43222',";
                comandoSQL += "'CNF43227',";
                comandoSQL += "'PMT31208-6',";
                comandoSQL += "'PII22885-4',";
                comandoSQL += "'PMT36559-0',";
                comandoSQL += "'PII29035-7',";
                comandoSQL += "'PMT37913-6',";
                comandoSQL += "'PMT41647-5',";
                comandoSQL += "'CNF43692',";
                comandoSQL += "'PII34391-6R01',";
                comandoSQL += "'PII38673-5',";
                comandoSQL += "'PMT33302-3',";
                comandoSQL += "'PMT29368-3R01',";
                comandoSQL += "'PII33033-4R01',";
                comandoSQL += "'PII29214-7',";
                comandoSQL += "'PMT18870-1',";
                comandoSQL += "'PII22419-1',";
                comandoSQL += "'PII27887-6',";
                comandoSQL += "'PII40800-9',";
                comandoSQL += "'PAF07030-1R01',";
                comandoSQL += "'PMT39244-2',";
                comandoSQL += "'PII42094-6',";
                comandoSQL += "'PII32189-8',";
                comandoSQL += "'PMT26707-6',";
                comandoSQL += "'FGF42189-6',";
                comandoSQL += "'PII29127-2',";
                comandoSQL += "'PII24128-5',";
                comandoSQL += "'PII27817-2',";
                comandoSQL += "'PII30978-8',";
                comandoSQL += "'PII29323-6',";
                comandoSQL += "'PMT26846-2',";
                comandoSQL += "'CNF43556',";
                comandoSQL += "'PII28195-1',";
                comandoSQL += "'PII29162-8',";
                comandoSQL += "'FGF42361-9',";
                comandoSQL += "'PII27158-0',";
                comandoSQL += "'PMT34336-2R01',";
                comandoSQL += "'PII33729-1',";
                comandoSQL += "'PII28194-2',";
                comandoSQL += "'PMT38339-3',";
                comandoSQL += "'PMT31068-4',";
                comandoSQL += "'PII21243-3',";
                comandoSQL += "'PII30523-9',";
                comandoSQL += "'PII44154-6',";
                comandoSQL += "'PMT39808-8',";
                comandoSQL += "'CAP31914-0',";
                comandoSQL += "'PII25876-1',";
                comandoSQL += "'PII38531-4',";
                comandoSQL += "'PII33419-8',";
                comandoSQL += "'PMT40614-4',";
                comandoSQL += "'PMT40354-6',";
                comandoSQL += "'PII40635-1',";
                comandoSQL += "'PMT36463-1',";
                comandoSQL += "'PII22277-2',";
                comandoSQL += "'PMT33051-6',";
                comandoSQL += "'PII32927-2',";
                comandoSQL += "'PMT40149-1',";
                comandoSQL += "'PII38742-8',";
                comandoSQL += "'PII38693-3',";
                comandoSQL += "'PII29655-4',";
                comandoSQL += "'PII38851-7',";
                comandoSQL += "'PII23184-8',";
                comandoSQL += "'PII28125-7',";
                comandoSQL += "'PMT44424-3',";
                comandoSQL += "'PII32878-8',";
                comandoSQL += "'PII29456-6',";
                comandoSQL += "'CNF39174',";
                comandoSQL += "'FAT025366-1',";
                comandoSQL += "'PII32794-5',";
                comandoSQL += "'PII27727-3',";
                comandoSQL += "'PII26228-1',";
                comandoSQL += "'PMT27617-6',";
                comandoSQL += "'PMT27877-7',";
                comandoSQL += "'PMT36839-6',";
                comandoSQL += "'CNF42464',";
                comandoSQL += "'PMT34774-5',";
                comandoSQL += "'PII40895-1',";
                comandoSQL += "'PAF07348-1R02',";
                comandoSQL += "'PII24758-1',";
                comandoSQL += "'PII34040-8',";
                comandoSQL += "'PMT37655-4',";
                comandoSQL += "'CNF43893',";
                comandoSQL += "'CNF47096',";
                comandoSQL += "'PII23508-1R02',";
                comandoSQL += "'PII27802-2',";
                comandoSQL += "'PII38852-5',";
                comandoSQL += "'CNF42700',";
                comandoSQL += "'FAT024932-1',";
                comandoSQL += "'PII43521-8',";
                comandoSQL += "'PII28270-0R001',";
                comandoSQL += "'PII29233-7',";
                comandoSQL += "'CNF37969',";
                comandoSQL += "'PMT36691-9',";
                comandoSQL += "'PMT29579-7',";
                comandoSQL += "'PII29932-6R01',";
                comandoSQL += "'PII38721-1',";
                comandoSQL += "'PMT29944-1',";
                comandoSQL += "'PII28223-9',";
                comandoSQL += "'PII28717-3R01',";
                comandoSQL += "'PII27604-2',";
                comandoSQL += "'PII27814-8',";
                comandoSQL += "'PII28766-1R01',";
                comandoSQL += "'PII39030-4R01',";
                comandoSQL += "'PMT42692-9',";
                comandoSQL += "'PII26529-4',";
                comandoSQL += "'PII30130-1',";
                comandoSQL += "'PMT39233-5',";
                comandoSQL += "'PII43516-0',";
                comandoSQL += "'PII25633-3',";
                comandoSQL += "'PII29319-6',";
                comandoSQL += "'PII21915-0',";
                comandoSQL += "'PMT20398-9',";
                comandoSQL += "'PMT42103-4',";
                comandoSQL += "'PII27562-2',";
                comandoSQL += "'PII27410-2',";
                comandoSQL += "'PII29566-3',";
                comandoSQL += "'PII41851-1',";
                comandoSQL += "'PMT34029-3',";
                comandoSQL += "'CNF42516',";
                comandoSQL += "'PMT36549-1',";
                comandoSQL += "'PII28333-6',";
                comandoSQL += "'PMT41312-2',";
                comandoSQL += "'PAF09078-1',";
                comandoSQL += "'PII22238-4',";
                comandoSQL += "'PII29276-8',";
                comandoSQL += "'PII26788-7R01',";
                comandoSQL += "'PII41029-4',";
                comandoSQL += "'PII36011-7',";
                comandoSQL += "'PII23668-3',";
                comandoSQL += "'PII21245-0R01',";
                comandoSQL += "'PII24753-1',";
                comandoSQL += "'PMT36034-0',";
                comandoSQL += "'PII34068-1',";
                comandoSQL += "'PII27407-1',";
                comandoSQL += "'PII30530-3',";
                comandoSQL += "'PII23122-7',";
                comandoSQL += "'PII23899-5',";
                comandoSQL += "'CNF47375',";
                comandoSQL += "'CNF29144-6',";
                comandoSQL += "'PMT33608-7',";
                comandoSQL += "'CNF0000006150',";
                comandoSQL += "'PII32674-9',";
                comandoSQL += "'FGF20760-9',";
                comandoSQL += "'FGF20354-0R01',";
                comandoSQL += "'PII39307-9',";
                comandoSQL += "'CNF41340',";
                comandoSQL += "'PII34466-8',";
                comandoSQL += "'PII38681-8',";
                comandoSQL += "'CNF42746',";
                comandoSQL += "'PMT37387-3',";
                comandoSQL += "'PMT37612-3',";
                comandoSQL += "'PMT33940-2',";
                comandoSQL += "'PII42390-8',";
                comandoSQL += "'PII32809-2',";
                comandoSQL += "'PMT36142-1',";
                comandoSQL += "'PII40995-0',";
                comandoSQL += "'PII40949-7',";
                comandoSQL += "'CNF39137',";
                comandoSQL += "'CNF40291',";
                comandoSQL += "'PMT043121-5',";
                comandoSQL += "'PAF09549-3R02',";
                comandoSQL += "'PII21807-9',";
                comandoSQL += "'PMT28489-9',";
                comandoSQL += "'PII33227-4',";
                comandoSQL += "'PMT33236-5',";
                comandoSQL += "'PMT40779-8',";
                comandoSQL += "'PMT39207-1',";
                comandoSQL += "'PMT28048-1R01',";
                comandoSQL += "'PII38991-1',";
                comandoSQL += "'PII24226-7',";
                comandoSQL += "'PII47137-0',";
                comandoSQL += "'PII34126-7',";
                comandoSQL += "'PMT41356-1',";
                comandoSQL += "'PII39634-6',";
                comandoSQL += "'PII42389-2',";
                comandoSQL += "'PII22141-8',";
                comandoSQL += "'PII29366-7',";
                comandoSQL += "'CNF34248-0',";
                comandoSQL += "'PII24478-5',";
                comandoSQL += "'CAP36226-3R01',";
                comandoSQL += "'CNF45065',";
                comandoSQL += "'PII41255-5',";
                comandoSQL += "'PAF07334-9',";
                comandoSQL += "'CR96798',";
                comandoSQL += "'PMT36779-4',";
                comandoSQL += "'CAP36407-0',";
                comandoSQL += "'PMT42320-4',";
                comandoSQL += "'PMT27909-8R01',";
                comandoSQL += "'PII21330-8',";
                comandoSQL += "'PII24527-0',";
                comandoSQL += "'PII38882-2',";
                comandoSQL += "'PMT16595-7',";
                comandoSQL += "'PMT38082-7',";
                comandoSQL += "'PAF06851-4',";
                comandoSQL += "'PII28421-9',";
                comandoSQL += "'PII41531-9',";
                comandoSQL += "'PMT27103-3',";
                comandoSQL += "'PAF08960-1',";
                comandoSQL += "'CNF45013',";
                comandoSQL += "'PMT44380-7',";
                comandoSQL += "'PII42275-2',";
                comandoSQL += "'PII33154-9',";
                comandoSQL += "'PII29704-9',";
                comandoSQL += "'PII38813-7',";
                comandoSQL += "'PMT40347-1',";
                comandoSQL += "'PII43525-1',";
                comandoSQL += "'PMT27102-5R01',";
                comandoSQL += "'PII39815-2',";
                comandoSQL += "'PMT40502-1',";
                comandoSQL += "'CAP18786-1R03',";
                comandoSQL += "'FGF41681-2',";
                comandoSQL += "'PMT25010-1R01',";
                comandoSQL += "'PII26950-1',";
                comandoSQL += "'PII26515-2',";
                comandoSQL += "'PMT31978-7',";
                comandoSQL += "'PII27514-3',";
                comandoSQL += "'PMT27808-1R01',";
                comandoSQL += "'PMT28105-9',";
                comandoSQL += "'PII26029-3',";
                comandoSQL += "'CAP40244-9',";
                comandoSQL += "'CCE29769-4',";
                comandoSQL += "'CNF38098',";
                comandoSQL += "'PII22191-3R01',";
                comandoSQL += "'PMT47447-3',";
                comandoSQL += "'PMT47406-9',";
                comandoSQL += "'PII28200-6',";
                comandoSQL += "'PII23196-3R01',";
                comandoSQL += "'PII40790-2',";
                comandoSQL += "'PMT32329-0',";
                comandoSQL += "'CAP34938-8R01',";
                comandoSQL += "'CCE37657-1R01',";
                comandoSQL += "'PMT32457-9',";
                comandoSQL += "'PII27494-8',";
                comandoSQL += "'PII27846-1',";
                comandoSQL += "'PII28005-1',";
                comandoSQL += "'PMT42398-3',";
                comandoSQL += "'PAF06323-2R01',";
                comandoSQL += "'PMT29311-1',";
                comandoSQL += "'PAF08424-7H01',";
                comandoSQL += "'PII42432-8',";
                comandoSQL += "'PMT39462-1',";
                comandoSQL += "'PII33551-7',";
                comandoSQL += "'PII29300-3R01',";
                comandoSQL += "'PII29255-1',";
                comandoSQL += "'PMT41536-0R01',";
                comandoSQL += "'PII27491-3',";
                comandoSQL += "'CNF0012233368',";
                comandoSQL += "'CNF44795-1',";
                comandoSQL += "'PMT34481-5',";
                comandoSQL += "'PII23352-1R02',";
                comandoSQL += "'PMT28595-3',";
                comandoSQL += "'PII30096-6',";
                comandoSQL += "'PII28789-3'";
                comandoSQL += " ) ";

                comandoSQL += " SELECT O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO, SUM(V.VLRVENCTO) AS TotalVencimentos ";
                comandoSQL += " INTO	#Vencimentos FROM VENCIMENTOSOPE V INNER JOIN #Contratos O ON ";
                comandoSQL += " V.DATBASE = O.DATBASE AND V.CODCOLIGADA = O.CODCOLIGADA AND V.NROCONTROLE = O.NROCONTROLE AND V.IDOPERACAO = O.IDOPERACAO ";
                comandoSQL += " WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' GROUP BY    O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO ";

                comandoSQL += " DELETE  V FROM    VENCIMENTOSOPE V INNER JOIN #Contratos O ON V.DATBASE = O.DATBASE AND V.NROCONTROLE = O.NROCONTROLE  AND V.IDOPERACAO = O.IDOPERACAO WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";


                comandoSQL += " INSERT INTO VENCIMENTOSOPE ";
                comandoSQL += " SELECT  V.DATBASE, V.CODCOLIGADA, V.NROCONTROLE, V.IDOPERACAO, CASE WHEN VENC.CODVENCTO IS NULL THEN '199' ELSE VENC.CODVENCTO END AS CODVENCTO, V.TotalVencimentos AS VLRVENCTO ";
                comandoSQL += " FROM    #Vencimentos V INNER JOIN #Contratos O ";
                comandoSQL += " ON V.DATBASE = O.DATBASE ";
                comandoSQL += " AND V.CODCOLIGADA = O.CODCOLIGADA AND V.NROCONTROLE = O.NROCONTROLE AND V.IDOPERACAO = O.IDOPERACAO ";
                comandoSQL += " LEFT JOIN(SELECT CODVENCTO, FAIXA1, FAIXA2 FROM INTER_VENCIMENTOS WHERE   DATVIGENCIA = (SELECT MAX(DATVIGENCIA) FROM INTER_VENCIMENTOS) AND INDVENCTO = 'V') AS VENC ON O.DIASATRASO BETWEEN FAIXA1 AND FAIXA2 ";                

                comandoSQL += " DROP TABLE #Contratos ";
                comandoSQL += " DROP TABLE #Vencimentos ";

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
        public bool atualizarIndPrejuizoParaWOQG(DateTime dataVigencia, string codSistema, string numeroContrato)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL += " UPDATE OPERACOES SET INDPREJUIZO = 'S' ";
                comandoSQL += " WHERE DATBASE = '"+ dataVigencia.ToString("yyyy-MM-dd") + "' ";                
                comandoSQL += " AND CODSISTEMA ='" + codSistema + "' ";
                comandoSQL += " AND CONTRATO IN (";
                comandoSQL += "'0010067720',";
                comandoSQL += "'0000002376',";
                comandoSQL += "'0000002400',";
                comandoSQL += "'0000004623',";
                comandoSQL += "'0000006159',";
                comandoSQL += "'0000007969',";
                comandoSQL += "'0000008232',";
                comandoSQL += "'0000008259',";
                comandoSQL += "'0000009930',";
                comandoSQL += "'0000011293',";
                comandoSQL += "'0000011955',";
                comandoSQL += "'0000012030',";
                comandoSQL += "'0000012818',";
                comandoSQL += "'0000013745',";
                comandoSQL += "'0000014322',";
                comandoSQL += "'0000015128',";
                comandoSQL += "'0000016272',";
                comandoSQL += "'0000016566',";
                comandoSQL += "'0000104754',";
                comandoSQL += "'0000117795',";
                comandoSQL += "'0000119119',";
                comandoSQL += "'0000119283',";
                comandoSQL += "'0000119984',";
                comandoSQL += "'0000123232',";
                comandoSQL += "'0000125804',";
                comandoSQL += "'0000125995',";
                comandoSQL += "'0000126028',";
                comandoSQL += "'0000126576',";
                comandoSQL += "'0000127237',";
                comandoSQL += "'0000155977',";
                comandoSQL += "'0000158810',";
                comandoSQL += "'0000159000',";
                comandoSQL += "'0000159689',";
                comandoSQL += "'0001004874',";
                comandoSQL += "'0001006869',";
                comandoSQL += "'0001007091',";
                comandoSQL += "'0001508697',";
                comandoSQL += "'0001691203',";
                comandoSQL += "'0002222193',";
                comandoSQL += "'0002401560',";
                comandoSQL += "'0002898138',";
                comandoSQL += "'0002911142',";
                comandoSQL += "'0003092285',";
                comandoSQL += "'0003121633',";
                comandoSQL += "'0003149350',";
                comandoSQL += "'0003241612',";
                comandoSQL += "'0003498141',";
                comandoSQL += "'0003526480',";
                comandoSQL += "'0003692568',";
                comandoSQL += "'0003735933',";
                comandoSQL += "'0003749209',";
                comandoSQL += "'0003788654',";
                comandoSQL += "'0003828443',";
                comandoSQL += "'0004407298',";
                comandoSQL += "'0004457759',";
                comandoSQL += "'0004562595',";
                comandoSQL += "'0004608277',";
                comandoSQL += "'0004795018',";
                comandoSQL += "'0004811803',";
                comandoSQL += "'0004812672',";
                comandoSQL += "'0004844841',";
                comandoSQL += "'0004854340',";
                comandoSQL += "'0004859911',";
                comandoSQL += "'0004965380',";
                comandoSQL += "'0005235547',";
                comandoSQL += "'0005235555',";
                comandoSQL += "'0005813320',";
                comandoSQL += "'0005874671',";
                comandoSQL += "'0007090619',";
                comandoSQL += "'0007248514',";
                comandoSQL += "'0008219470',";
                comandoSQL += "'0008323234',";
                comandoSQL += "'0008531791',";
                comandoSQL += "'0008536572',";
                comandoSQL += "'0008571750',";
                comandoSQL += "'0008598993',";
                comandoSQL += "'0008958426',";
                comandoSQL += "'0008963349',";
                comandoSQL += "'0008981550',";
                comandoSQL += "'0008991628',";
                comandoSQL += "'0009049284',";
                comandoSQL += "'0009059638',";
                comandoSQL += "'0009085922',";
                comandoSQL += "'0009135113',";
                comandoSQL += "'0009135180',";
                comandoSQL += "'0009138775',";
                comandoSQL += "'0009647734',";
                comandoSQL += "'0009826795',";
                comandoSQL += "'0009835352',";
                comandoSQL += "'0009899067',";
                comandoSQL += "'0009947380',";
                comandoSQL += "'0010234618',";
                comandoSQL += "'0010267850',";
                comandoSQL += "'0010284658',";
                comandoSQL += "'0010355628',";
                comandoSQL += "'0010721012',";
                comandoSQL += "'0011271991',";
                comandoSQL += "'000287611200026081111',";
                comandoSQL += "'000287611200026081129',";
                comandoSQL += "'FNF/20010804',";
                comandoSQL += "'FNJ201806/03',";
                comandoSQL += "'FNJ202006/003',";
                comandoSQL += "'FIBRA_300310_00',";
                comandoSQL += "'FNJ2/20030206',";
                comandoSQL += "'PII25957-9R01',";
                comandoSQL += "'0000004122',";
                comandoSQL += "'PII26377-7',";
                comandoSQL += "'PMC019721-4',";
                comandoSQL += "'PII25721-6',";
                comandoSQL += "'0000011117',";
                comandoSQL += "'CNF09565',";
                comandoSQL += "'CCE20397-1R01',";
                comandoSQL += "'PMT33306-6',";
                comandoSQL += "'PMT25807-5R01',";
                comandoSQL += "'FIN31311-6',";
                comandoSQL += "'FIN31304-1',";
                comandoSQL += "'PII23578-4',";
                comandoSQL += "'PII30097-4',";
                comandoSQL += "'PII24243-1',";
                comandoSQL += "'0005857858',";
                comandoSQL += "'0010424700',";
                comandoSQL += "'0005982261',";
                comandoSQL += "'0010267850',";
                comandoSQL += "'0000002400',";
                comandoSQL += "'0003469206',";
                comandoSQL += "'0003432345',";
                comandoSQL += "'0000125952',";
                comandoSQL += "'0000126576',";
                comandoSQL += "'0000118085'";





                comandoSQL += " ) ";

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
        public bool atualizarIndPrejuizoParaWO(DateTime dataVigencia, string codSistema, string numeroContrato)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL += " UPDATE OPERACOES SET INDPREJUIZO = 'S' ";
                comandoSQL += " WHERE DATBASE = '"+ dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODSISTEMA ='" + codSistema + "' ";
                comandoSQL += " AND CONTRATO = '" + numeroContrato + "' ";                

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
        public bool atualizarIndPrejuizoParaAtivo(DateTime dataVigencia, string codSistema, string numeroContrato)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL += " UPDATE OPERACOES SET INDPREJUIZO = 'N' ";
                comandoSQL += " WHERE DATBASE = '"+ dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODSISTEMA ='" + codSistema + "' ";
                comandoSQL += " AND CONTRATO = '" + numeroContrato + "' ";

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

        public bool atualizarIndPrejuizoParaAtivoQG(DateTime dataVigencia, string codSistema, string numeroContrato)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL += " UPDATE OPERACOES SET INDPREJUIZO = 'N' ";
                comandoSQL += " WHERE DATBASE = '"+ dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND CODSISTEMA ='" + codSistema + "' ";
                comandoSQL += " AND CONTRATO IN (";
                comandoSQL += "'CAP36766-1R01',";
                comandoSQL += "'PII42252-0',";
                comandoSQL += "'PII24008-9',";
                comandoSQL += "'PII21699-1R01',";
                comandoSQL += "'FGF25212-4',";
                comandoSQL += "'PMT34548-4',";
                comandoSQL += "'PII22646-0',";
                comandoSQL += "'PMT31174-9',";
                comandoSQL += "'PMT40075-8',";
                comandoSQL += "'PMT37899-0',";
                comandoSQL += "'PII21948-1',";
                comandoSQL += "'PMT47432-3',";
                comandoSQL += "'PAF08578-3',";
                comandoSQL += "'PMT34406-3',";
                comandoSQL += "'PII27871-8',";
                comandoSQL += "'PAF08698-0R01',";
                comandoSQL += "'PII24717-7',";
                comandoSQL += "'PII25676-4',";
                comandoSQL += "'PAF08609-6R01',";
                comandoSQL += "'PMT37248-7',";
                comandoSQL += "'PMT34901-3',";
                comandoSQL += "'PMT31816-8',";
                comandoSQL += "'PII22066-9',";
                comandoSQL += "'PMT26500-2',";
                comandoSQL += "'CNF38478',";
                comandoSQL += "'PAF07112-8R01',";
                comandoSQL += "'PAF08949-7R01',";
                comandoSQL += "'PII28905-4',";
                comandoSQL += "'PII34367-8',";
                comandoSQL += "'PII21483-6R0001',";
                comandoSQL += "'FGF20383-9R01',";
                comandoSQL += "'FGF38820-1',";
                comandoSQL += "'PAF06812-6H01',";
                comandoSQL += "'PII43154-7',";
                comandoSQL += "'PII39841-7',";
                comandoSQL += "'PMT25642-4',";
                comandoSQL += "'CNF45509',";
                comandoSQL += "'PII28032-3R01',";
                comandoSQL += "'PMT32020-1',";
                comandoSQL += "'PII24806-8R01',";
                comandoSQL += "'PII33770-3',";
                comandoSQL += "'PII24677-3H01',";
                comandoSQL += "'PII28977-4R01',";
                comandoSQL += "'PMT34655-7',";
                comandoSQL += "'PII23754-0',";
                comandoSQL += "'PII21336-7',";
                comandoSQL += "'FGF15792-0',";
                comandoSQL += "'PII28518-5',";
                comandoSQL += "'PII34396-7',";
                comandoSQL += "'PMT45685-1',";
                comandoSQL += "'PAF06864-8',";
                comandoSQL += "'CNF41891',";
                comandoSQL += "'CR96826',";
                comandoSQL += "'PMT34086-3',";
                comandoSQL += "'PAF06480-1',";
                comandoSQL += "'PMT39296-4',";
                comandoSQL += "'PII38148-8',";
                comandoSQL += "'FGF35968-4',";
                comandoSQL += "'PAF09329-9',";
                comandoSQL += "'PII22010-4R01',";
                comandoSQL += "'PII24376-1',";
                comandoSQL += "'PMT31924-9',";
                comandoSQL += "'CAP41437-0',";
                comandoSQL += "'PII21256-7',";
                comandoSQL += "'PMT47182-4',";
                comandoSQL += "'PII26208-3R01',";
                comandoSQL += "'PAF06254-0',";
                comandoSQL += "'CAP28961-6R01',";
                comandoSQL += "'PII43522-6',";
                comandoSQL += "'PAF09901-4H01',";
                comandoSQL += "'PII33462-6',";
                comandoSQL += "'FGF20284-9',";
                comandoSQL += "'PII27908-0',";
                comandoSQL += "'PII25780-2',";
                comandoSQL += "'PMT40080-6',";
                comandoSQL += "'CNF0025395-0',";
                comandoSQL += "'PII23634-3',";
                comandoSQL += "'CNF41737',";
                comandoSQL += "'PII22839-1',";
                comandoSQL += "'PII40983-4',";
                comandoSQL += "'PMT32737-5',";
                comandoSQL += "'PII25498-2',";
                comandoSQL += "'PII24779-8',";
                comandoSQL += "'PII27434-3',";
                comandoSQL += "'PMT31186-4',";
                comandoSQL += "'CNF0012161839',";
                comandoSQL += "'PII32788-9',";
                comandoSQL += "'CNF43228',";
                comandoSQL += "'PII23699-9',";
                comandoSQL += "'FGF36990-5',";
                comandoSQL += "'PII21985-3',";
                comandoSQL += "'PII28935-1',";
                comandoSQL += "'PII22619-7',";
                comandoSQL += "'PII22931-4',";
                comandoSQL += "'PAF08834-9',";
                comandoSQL += "'PII29928-6H01',";
                comandoSQL += "'CAP28739-8R01',";
                comandoSQL += "'PII34373-4',";
                comandoSQL += "'PII43114-1',";
                comandoSQL += "'PII26803-1R01',";
                comandoSQL += "'PII30814-2',";
                comandoSQL += "'PMT36107-5',";
                comandoSQL += "'PMT39435-8',";
                comandoSQL += "'PAF08455-2R01',";
                comandoSQL += "'CNF97093',";
                comandoSQL += "'CNF38637',";
                comandoSQL += "'PII29817-1',";
                comandoSQL += "'PII29299-1',";
                comandoSQL += "'PII26530-0',";
                comandoSQL += "'PAF06513-0',";
                comandoSQL += "'PMT27636-6',";
                comandoSQL += "'PAF09426-2',";
                comandoSQL += "'PII27508-7',";
                comandoSQL += "'FGF20287-3',";
                comandoSQL += "'PII41394-1',";
                comandoSQL += "'PAF09352-9',";
                comandoSQL += "'PMT20478-9',";
                comandoSQL += "'PII32177-2',";
                comandoSQL += "'PII27868-6',";
                comandoSQL += "'PII38908-7',";
                comandoSQL += "'PAF06482-7',";
                comandoSQL += "'FAT025445-2',";
                comandoSQL += "'PII24744-0H01',";
                comandoSQL += "'PII26402-1R01',";
                comandoSQL += "'PII26752-1',";
                comandoSQL += "'PMT33056-7',";
                comandoSQL += "'FGF4702-1',";
                comandoSQL += "'CNF025660-6',";
                comandoSQL += "'PMT33697-1',";
                comandoSQL += "'PII27988-2',";
                comandoSQL += "'PII21675-0',";
                comandoSQL += "'PMT37621-4',";
                comandoSQL += "'PMT41017-9',";
                comandoSQL += "'PII34621-7',";
                comandoSQL += "'PMT31862-1',";
                comandoSQL += "'PII25981-7',";
                comandoSQL += "'CNF44606-1',";
                comandoSQL += "'CNF41689',";
                comandoSQL += "'PMT45435-0',";
                comandoSQL += "'PMT39987-1',";
                comandoSQL += "'PII28197-7',";
                comandoSQL += "'PII30107-1',";
                comandoSQL += "'PII28018-4',";
                comandoSQL += "'PAF07221-7H02',";
                comandoSQL += "'PII34230-5',";
                comandoSQL += "'PII42069-0',";
                comandoSQL += "'PII37611-5',";
                comandoSQL += "'PMT33348-9',";
                comandoSQL += "'PII36325-3',";
                comandoSQL += "'PMT30952-1 - A',";
                comandoSQL += "'CAP38272-4R01',";
                comandoSQL += "'PMT39196-6',";
                comandoSQL += "'CNF44950',";
                comandoSQL += "'PII37126-4',";
                comandoSQL += "'PII42738-1',";
                comandoSQL += "'PMT38398-0',";
                comandoSQL += "'PMT36997-2',";
                comandoSQL += "'PII32458-7',";
                comandoSQL += "'PMTGCH96863-A',";
                comandoSQL += "'PMT30780-5',";
                comandoSQL += "'PMT36139-9',";
                comandoSQL += "'PMT39646-1',";
                comandoSQL += "'PAF06436-4',";
                comandoSQL += "'PMT36129-0',";
                comandoSQL += "'PII23979-5',";
                comandoSQL += "'CNF0000126826',";
                comandoSQL += "'PMT38584-4',";
                comandoSQL += "'CNF37488',";
                comandoSQL += "'PII40964-4',";
                comandoSQL += "'PII28259-5',";
                comandoSQL += "'PMT34540-9R01',";
                comandoSQL += "'PMT37592-8',";
                comandoSQL += "'PMT37596-1',";
                comandoSQL += "'PII28597-0',";
                comandoSQL += "'PII46956-6',";
                comandoSQL += "'PII27236-3',";
                comandoSQL += "'PMT25508-9',";
                comandoSQL += "'FGF19868-6',";
                comandoSQL += "'FGF19453-3',";
                comandoSQL += "'PMT36246-1',";
                comandoSQL += "'PII30243-2',";
                comandoSQL += "'FGF20498-7',";
                comandoSQL += "'PII30133-5',";
                comandoSQL += "'PII26508-8',";
                comandoSQL += "'PII21957-2',";
                comandoSQL += "'PII42979-2',";
                comandoSQL += "'PII29016-7',";
                comandoSQL += "'PII26063-1',";
                comandoSQL += "'PII29991-2',";
                comandoSQL += "'PII24830-6R01',";
                comandoSQL += "'PII27598-9',";
                comandoSQL += "'PII41480-8',";
                comandoSQL += "'PII26997-4',";
                comandoSQL += "'FGF20575-2',";
                comandoSQL += "'PMT40511-1',";
                comandoSQL += "'PAF09088-1H03',";
                comandoSQL += "'CNF47438',";
                comandoSQL += "'PMT46343-3',";
                comandoSQL += "'PII29071-1',";
                comandoSQL += "'PII47546-3',";
                comandoSQL += "'PMT39974-7',";
                comandoSQL += "'PII24759-0',";
                comandoSQL += "'PII33820-6',";
                comandoSQL += "'PII32240-6',";
                comandoSQL += "'PII38914-3',";
                comandoSQL += "'PII33034-2',";
                comandoSQL += "'PII32620-1',";
                comandoSQL += "'PMT31494-1',";
                comandoSQL += "'PII26121-6',";
                comandoSQL += "'PII28968-3',";
                comandoSQL += "'AGR32105-2R01',";
                comandoSQL += "'PII29091-9',";
                comandoSQL += "'PII44230-3',";
                comandoSQL += "'PMT28262-7R01',";
                comandoSQL += "'PII34197-9R01',";
                comandoSQL += "'PII29586-1',";
                comandoSQL += "'PMT41570-7',";
                comandoSQL += "'PMT37056-3',";
                comandoSQL += "'PMT33586-5',";
                comandoSQL += "'PII32908-2',";
                comandoSQL += "'PII36189-4',";
                comandoSQL += "'PII24815-9',";
                comandoSQL += "'PII26276-1',";
                comandoSQL += "'FGF37601-6',";
                comandoSQL += "'PMT21833-3',";
                comandoSQL += "'CNF0011642108',";
                comandoSQL += "'PII41514-5',";
                comandoSQL += "'PII29359-2',";
                comandoSQL += "'PII22671-6R01',";
                comandoSQL += "'GNC25208-4',";
                comandoSQL += "'PMT26986-7R01',";
                comandoSQL += "'PMT30642-7',";
                comandoSQL += "'PII27715-8',";
                comandoSQL += "'PII32355-4',";
                comandoSQL += "'PMT39947-4',";
                comandoSQL += "'PII27511-9',";
                comandoSQL += "'PII24843-0',";
                comandoSQL += "'PMT047393-8',";
                comandoSQL += "'PAF08567-6H01',";
                comandoSQL += "'PII22018-0',";
                comandoSQL += "'PII24110-1',";
                comandoSQL += "'PMT38282-3',";
                comandoSQL += "'PMT38169-4',";
                comandoSQL += "'PII33812-3',";
                comandoSQL += "'PII21603-0',";
                comandoSQL += "'PII27246-2',";
                comandoSQL += "'PAF06989-5',";
                comandoSQL += "'PII33819-1',";
                comandoSQL += "'PMT32092-1',";
                comandoSQL += "'PII23565-1',";
                comandoSQL += "'PII27408-9',";
                comandoSQL += "'PII27624-1',";
                comandoSQL += "'PII21276-5',";
                comandoSQL += "'PMT30158-4R01',";
                comandoSQL += "'PAF06566-0',";
                comandoSQL += "'PAF06299-7R01',";
                comandoSQL += "'PII28411-0',";
                comandoSQL += "'PII27323-8R01',";
                comandoSQL += "'PMT33040-9',";
                comandoSQL += "'PII25565-9R01',";
                comandoSQL += "'PII25026-0',";
                comandoSQL += "'PII32531-0',";
                comandoSQL += "'PII23921-4R001',";
                comandoSQL += "'PMT34597-1R01',";
                comandoSQL += "'PAF06946-4',";
                comandoSQL += "'PMT36163-7',";
                comandoSQL += "'PMT33145-8',";
                comandoSQL += "'PMT38897-2',";
                comandoSQL += "'CAP31734-1R03',";
                comandoSQL += "'PMT39889-9',";
                comandoSQL += "'PII22389-6H01',";
                comandoSQL += "'PII24710-0',";
                comandoSQL += "'PII25907-3',";
                comandoSQL += "'PMT37303-8',";
                comandoSQL += "'FGF19240-3H01',";
                comandoSQL += "'PMT23499-2R03',";
                comandoSQL += "'PMT33057-5',";
                comandoSQL += "'PII24819-1',";
                comandoSQL += "'CNF0125072',";
                comandoSQL += "'PII25683-9',";
                comandoSQL += "'PII26760-3',";
                comandoSQL += "'PII24946-2',";
                comandoSQL += "'CNF38738',";
                comandoSQL += "'CNF0012139426',";
                comandoSQL += "'PMT28802-1',";
                comandoSQL += "'CNF46357',";
                comandoSQL += "'PII42771-1',";
                comandoSQL += "'PII39441-4',";
                comandoSQL += "'PII26187-0',";
                comandoSQL += "'PII26191-0R01',";
                comandoSQL += "'PII24789-7',";
                comandoSQL += "'PMT31623-6',";
                comandoSQL += "'PII27537-6',";
                comandoSQL += "'PII26769-7',";
                comandoSQL += "'PII29968-2',";
                comandoSQL += "'PII29345-1',";
                comandoSQL += "'PII23226-8',";
                comandoSQL += "'CNF0031215-1',";
                comandoSQL += "'PII27541-6',";
                comandoSQL += "'CNF41128',";
                comandoSQL += "'PII24696-3',";
                comandoSQL += "'PII34057-4',";
                comandoSQL += "'PMT40428-0',";
                comandoSQL += "'PMT043364-2',";
                comandoSQL += "'PII29958-3',";
                comandoSQL += "'PII26499-0',";
                comandoSQL += "'PII28433-4H01',";
                comandoSQL += "'PMT44067-1',";
                comandoSQL += "'PII25962-7R01',";
                comandoSQL += "'PII41357-0',";
                comandoSQL += "'PII39596-9',";
                comandoSQL += "'PMT29178-6',";
                comandoSQL += "'PII27458-4',";
                comandoSQL += "'PII34361-9',";
                comandoSQL += "'PII24897-8',";
                comandoSQL += "'PAF06861-3H03',";
                comandoSQL += "'PII35980-7',";
                comandoSQL += "'PII21884-7',";
                comandoSQL += "'FGF38563-8',";
                comandoSQL += "'PII25044-1',";
                comandoSQL += "'PII29187-7',";
                comandoSQL += "'PII26791-9',";
                comandoSQL += "'PII31020-2',";
                comandoSQL += "'PII30001-3',";
                comandoSQL += "'PII43526-9',";
                comandoSQL += "'PII29425-1',";
                comandoSQL += "'PII38647-1',";
                comandoSQL += "'PII21233-4',";
                comandoSQL += "'PII24229-1',";
                comandoSQL += "'PII34914-7',";
                comandoSQL += "'PII23742-4',";
                comandoSQL += "'PII28178-7',";
                comandoSQL += "'PMT37820-2',";
                comandoSQL += "'PII29875-9R01',";
                comandoSQL += "'PII28021-6R01',";
                comandoSQL += "'FGF42709-2',";
                comandoSQL += "'PII28415-2',";
                comandoSQL += "'PMT31706-1',";
                comandoSQL += "'PMT33143-1',";
                comandoSQL += "'PII26205-9',";
                comandoSQL += "'PII21872-1',";
                comandoSQL += "'PII25408-1H01',";
                comandoSQL += "'PMT33645-9',";
                comandoSQL += "'PMT34464-1',";
                comandoSQL += "'CNF41705',";
                comandoSQL += "'PMT42664-8',";
                comandoSQL += "'PMT40002-0',";
                comandoSQL += "'PMT37670-1',";
                comandoSQL += "'PAF06590-8R01.',";
                comandoSQL += "'FGF36981-4',";
                comandoSQL += "'PII38433-2',";
                comandoSQL += "'FGF39324-2',";
                comandoSQL += "'PII41382-6',";
                comandoSQL += "'PMT30879-8',";
                comandoSQL += "'PII33423-8',";
                comandoSQL += "'PII28380-7',";
                comandoSQL += "'PII24178-1',";
                comandoSQL += "'PII21507-4',";
                comandoSQL += "'CNF46748',";
                comandoSQL += "'PMT32724-1',";
                comandoSQL += "'FGF15337-3R01',";
                comandoSQL += "'PII21445-6',";
                comandoSQL += "'PII24383-5',";
                comandoSQL += "'PII26159-9R01',";
                comandoSQL += "'PII29234-5',";
                comandoSQL += "'PII27632-3',";
                comandoSQL += "'PMT33418-0',";
                comandoSQL += "'PII26560-7',";
                comandoSQL += "'PII24113-5',";
                comandoSQL += "'PII29310-2',";
                comandoSQL += "'PII32397-7R01',";
                comandoSQL += "'CNF0010283546',";
                comandoSQL += "'PMT38863-2',";
                comandoSQL += "'PMT37911-0',";
                comandoSQL += "'CNF42728',";
                comandoSQL += "'PMT26812-2',";
                comandoSQL += "'PII38910-1',";
                comandoSQL += "'PMT38694-1',";
                comandoSQL += "'PII21724-4',";
                comandoSQL += "'CNFPMT21092-4',";
                comandoSQL += "'PII29677-9',";
                comandoSQL += "'PII28428-6',";
                comandoSQL += "'PII22856-5',";
                comandoSQL += "'PMT30684-0',";
                comandoSQL += "'PII30091-5',";
                comandoSQL += "'PII22342-2',";
                comandoSQL += "'PII38834-3',";
                comandoSQL += "'PII26202-4',";
                comandoSQL += "'PII28363-3',";
                comandoSQL += "'PMT33983-3',";
                comandoSQL += "'PMT38149-6',";
                comandoSQL += "'PII30529-8',";
                comandoSQL += "'PII28638-1',";
                comandoSQL += "'PII29039-0',";
                comandoSQL += "'PII34807-4',";
                comandoSQL += "'PII29637-2',";
                comandoSQL += "'PAF09315-7R01',";
                comandoSQL += "'PMT38988-0',";
                comandoSQL += "'PII21287-2R01',";
                comandoSQL += "'PII30044-4',";
                comandoSQL += "'PAF07239-1H01',";
                comandoSQL += "'PII28785-1R01.',";
                comandoSQL += "'PII27986-6',";
                comandoSQL += "'PII24177-2',";
                comandoSQL += "'PII27523-4',";
                comandoSQL += "'PMT36747-1',";
                comandoSQL += "'PII25938-9',";
                comandoSQL += "'PMT34201-6',";
                comandoSQL += "'PII34297-7',";
                comandoSQL += "'PII29285-9',";
                comandoSQL += "'PII27965-0',";
                comandoSQL += "'PII34447-8',";
                comandoSQL += "'PII32704-3R01',";
                comandoSQL += "'CNF0016742-3R',";
                comandoSQL += "'CNF42338',";
                comandoSQL += "'PMT38344-1',";
                comandoSQL += "'PMT32665-8',";
                comandoSQL += "'PII43517-8',";
                comandoSQL += "'PII24447-0R01',";
                comandoSQL += "'PMT37613-1R01',";
                comandoSQL += "'PII28170-1',";
                comandoSQL += "'CNF41554',";
                comandoSQL += "'PMT36968-3',";
                comandoSQL += "'PMT40662-3',";
                comandoSQL += "'CNFCNF20924-1',";
                comandoSQL += "'PII27472-3',";
                comandoSQL += "'PII26824-8',";
                comandoSQL += "'PII41485-9',";
                comandoSQL += "'CNF0011591333',";
                comandoSQL += "'FAT021180-7',";
                comandoSQL += "'PII28176-1',";
                comandoSQL += "'PII26286-0',";
                comandoSQL += "'PII28856-0',";
                comandoSQL += "'PII29929-4',";
                comandoSQL += "'PII39133-7',";
                comandoSQL += "'PMT043374-1',";
                comandoSQL += "'PII31064-1',";
                comandoSQL += "'PMT34365-1',";
                comandoSQL += "'PMT34215-8',";
                comandoSQL += "'PMT38439-1',";
                comandoSQL += "'PMT37898-1',";
                comandoSQL += "'PII34290-0',";
                comandoSQL += "'PII29332-7',";
                comandoSQL += "'FGF42647-4',";
                comandoSQL += "'PII33308-2',";
                comandoSQL += "'CNF31441-1',";
                comandoSQL += "'PII24091-3',";
                comandoSQL += "'PMT37734-6',";
                comandoSQL += "'PII29440-8',";
                comandoSQL += "'PII41014-4',";
                comandoSQL += "'PII30946-4',";
                comandoSQL += "'PII35959-3R01',";
                comandoSQL += "'PMT41957-9',";
                comandoSQL += "'PII21845-9',";
                comandoSQL += "'PMT21851-5',";
                comandoSQL += "'PII38805-4',";
                comandoSQL += "'PII39606-5',";
                comandoSQL += "'PMT39754-2',";
                comandoSQL += "'CNF0005653152',";
                comandoSQL += "'CNF0011807883',";
                comandoSQL += "'PMT42266-1',";
                comandoSQL += "'PII21651-9',";
                comandoSQL += "'PMT34362-7',";
                comandoSQL += "'PMT39812-8',";
                comandoSQL += "'PII27306-4',";
                comandoSQL += "'PII38727-1',";
                comandoSQL += "'PMT39776-7',";
                comandoSQL += "'PII32918-1',";
                comandoSQL += "'PII42087-1',";
                comandoSQL += "'PII31358-0',";
                comandoSQL += "'PII39176-8',";
                comandoSQL += "'PII32728-4',";
                comandoSQL += "'PMT48587-7',";
                comandoSQL += "'PII34376-9R01',";
                comandoSQL += "'PMT37467-3',";
                comandoSQL += "'CNF96893R01',";
                comandoSQL += "'PII22494-2',";
                comandoSQL += "'CNF43830',";
                comandoSQL += "'PII28162-9',";
                comandoSQL += "'PII30965-4',";
                comandoSQL += "'CNF40370',";
                comandoSQL += "'PII34659-0',";
                comandoSQL += "'PMT33047-6R01',";
                comandoSQL += "'PMT34903-0',";
                comandoSQL += "'CNF43222',";
                comandoSQL += "'CNF43227',";
                comandoSQL += "'PMT31208-6',";
                comandoSQL += "'PII22885-4',";
                comandoSQL += "'PMT36559-0',";
                comandoSQL += "'PII29035-7',";
                comandoSQL += "'PMT37913-6',";
                comandoSQL += "'PMT41647-5',";
                comandoSQL += "'CNF43692',";
                comandoSQL += "'PII34391-6R01',";
                comandoSQL += "'PII38673-5',";
                comandoSQL += "'PMT33302-3',";
                comandoSQL += "'PMT29368-3R01',";
                comandoSQL += "'PII33033-4R01',";
                comandoSQL += "'PII29214-7',";
                comandoSQL += "'PMT18870-1',";
                comandoSQL += "'PII22419-1',";
                comandoSQL += "'PII27887-6',";
                comandoSQL += "'PII40800-9',";
                comandoSQL += "'PAF07030-1R01',";
                comandoSQL += "'PMT39244-2',";
                comandoSQL += "'PII42094-6',";
                comandoSQL += "'PII32189-8',";
                comandoSQL += "'PMT26707-6',";
                comandoSQL += "'FGF42189-6',";
                comandoSQL += "'PII29127-2',";
                comandoSQL += "'PII24128-5',";
                comandoSQL += "'PII27817-2',";
                comandoSQL += "'PII30978-8',";
                comandoSQL += "'PII29323-6',";
                comandoSQL += "'PMT26846-2',";
                comandoSQL += "'CNF43556',";
                comandoSQL += "'PII28195-1',";
                comandoSQL += "'PII29162-8',";
                comandoSQL += "'FGF42361-9',";
                comandoSQL += "'PII27158-0',";
                comandoSQL += "'PMT34336-2R01',";
                comandoSQL += "'PII33729-1',";
                comandoSQL += "'PII28194-2',";
                comandoSQL += "'PMT38339-3',";
                comandoSQL += "'PMT31068-4',";
                comandoSQL += "'PII21243-3',";
                comandoSQL += "'PII30523-9',";
                comandoSQL += "'PII44154-6',";
                comandoSQL += "'PMT39808-8',";
                comandoSQL += "'CAP31914-0',";
                comandoSQL += "'PII25876-1',";
                comandoSQL += "'PII38531-4',";
                comandoSQL += "'PII33419-8',";
                comandoSQL += "'PMT40614-4',";
                comandoSQL += "'PMT40354-6',";
                comandoSQL += "'PII40635-1',";
                comandoSQL += "'PMT36463-1',";
                comandoSQL += "'PII22277-2',";
                comandoSQL += "'PMT33051-6',";
                comandoSQL += "'PII32927-2',";
                comandoSQL += "'PMT40149-1',";
                comandoSQL += "'PII38742-8',";
                comandoSQL += "'PII38693-3',";
                comandoSQL += "'PII29655-4',";
                comandoSQL += "'PII38851-7',";
                comandoSQL += "'PII23184-8',";
                comandoSQL += "'PII28125-7',";
                comandoSQL += "'PMT44424-3',";
                comandoSQL += "'PII32878-8',";
                comandoSQL += "'PII29456-6',";
                comandoSQL += "'CNF39174',";
                comandoSQL += "'FAT025366-1',";
                comandoSQL += "'PII32794-5',";
                comandoSQL += "'PII27727-3',";
                comandoSQL += "'PII26228-1',";
                comandoSQL += "'PMT27617-6',";
                comandoSQL += "'PMT27877-7',";
                comandoSQL += "'PMT36839-6',";
                comandoSQL += "'CNF42464',";
                comandoSQL += "'PMT34774-5',";
                comandoSQL += "'PII40895-1',";
                comandoSQL += "'PAF07348-1R02',";
                comandoSQL += "'PII24758-1',";
                comandoSQL += "'PII34040-8',";
                comandoSQL += "'PMT37655-4',";
                comandoSQL += "'CNF43893',";
                comandoSQL += "'CNF47096',";
                comandoSQL += "'PII23508-1R02',";
                comandoSQL += "'PII27802-2',";
                comandoSQL += "'PII38852-5',";
                comandoSQL += "'CNF42700',";
                comandoSQL += "'FAT024932-1',";
                comandoSQL += "'PII43521-8',";
                comandoSQL += "'PII28270-0R001',";
                comandoSQL += "'PII29233-7',";
                comandoSQL += "'CNF37969',";
                comandoSQL += "'PMT36691-9',";
                comandoSQL += "'PMT29579-7',";
                comandoSQL += "'PII29932-6R01',";
                comandoSQL += "'PII38721-1',";
                comandoSQL += "'PMT29944-1',";
                comandoSQL += "'PII28223-9',";
                comandoSQL += "'PII28717-3R01',";
                comandoSQL += "'PII27604-2',";
                comandoSQL += "'PII27814-8',";
                comandoSQL += "'PII28766-1R01',";
                comandoSQL += "'PII39030-4R01',";
                comandoSQL += "'PMT42692-9',";
                comandoSQL += "'PII26529-4',";
                comandoSQL += "'PII30130-1',";
                comandoSQL += "'PMT39233-5',";
                comandoSQL += "'PII43516-0',";
                comandoSQL += "'PII25633-3',";
                comandoSQL += "'PII29319-6',";
                comandoSQL += "'PII21915-0',";
                comandoSQL += "'PMT20398-9',";
                comandoSQL += "'PMT42103-4',";
                comandoSQL += "'PII27562-2',";
                comandoSQL += "'PII27410-2',";
                comandoSQL += "'PII29566-3',";
                comandoSQL += "'PII41851-1',";
                comandoSQL += "'PMT34029-3',";
                comandoSQL += "'CNF42516',";
                comandoSQL += "'PMT36549-1',";
                comandoSQL += "'PII28333-6',";
                comandoSQL += "'PMT41312-2',";
                comandoSQL += "'PAF09078-1',";
                comandoSQL += "'PII22238-4',";
                comandoSQL += "'PII29276-8',";
                comandoSQL += "'PII26788-7R01',";
                comandoSQL += "'PII41029-4',";
                comandoSQL += "'PII36011-7',";
                comandoSQL += "'PII23668-3',";
                comandoSQL += "'PII21245-0R01',";
                comandoSQL += "'PII24753-1',";
                comandoSQL += "'PMT36034-0',";
                comandoSQL += "'PII34068-1',";
                comandoSQL += "'PII27407-1',";
                comandoSQL += "'PII30530-3',";
                comandoSQL += "'PII23122-7',";
                comandoSQL += "'PII23899-5',";
                comandoSQL += "'CNF47375',";
                comandoSQL += "'CNF29144-6',";
                comandoSQL += "'PMT33608-7',";
                comandoSQL += "'CNF0000006150',";
                comandoSQL += "'PII32674-9',";
                comandoSQL += "'FGF20760-9',";
                comandoSQL += "'FGF20354-0R01',";
                comandoSQL += "'PII39307-9',";
                comandoSQL += "'CNF41340',";
                comandoSQL += "'PII34466-8',";
                comandoSQL += "'PII38681-8',";
                comandoSQL += "'CNF42746',";
                comandoSQL += "'PMT37387-3',";
                comandoSQL += "'PMT37612-3',";
                comandoSQL += "'PMT33940-2',";
                comandoSQL += "'PII42390-8',";
                comandoSQL += "'PII32809-2',";
                comandoSQL += "'PMT36142-1',";
                comandoSQL += "'PII40995-0',";
                comandoSQL += "'PII40949-7',";
                comandoSQL += "'CNF39137',";
                comandoSQL += "'CNF40291',";
                comandoSQL += "'PMT043121-5',";
                comandoSQL += "'PAF09549-3R02',";
                comandoSQL += "'PII21807-9',";
                comandoSQL += "'PMT28489-9',";
                comandoSQL += "'PII33227-4',";
                comandoSQL += "'PMT33236-5',";
                comandoSQL += "'PMT40779-8',";
                comandoSQL += "'PMT39207-1',";
                comandoSQL += "'PMT28048-1R01',";
                comandoSQL += "'PII38991-1',";
                comandoSQL += "'PII24226-7',";
                comandoSQL += "'PII47137-0',";
                comandoSQL += "'PII34126-7',";
                comandoSQL += "'PMT41356-1',";
                comandoSQL += "'PII39634-6',";
                comandoSQL += "'PII42389-2',";
                comandoSQL += "'PII22141-8',";
                comandoSQL += "'PII29366-7',";
                comandoSQL += "'CNF34248-0',";
                comandoSQL += "'PII24478-5',";
                comandoSQL += "'CAP36226-3R01',";
                comandoSQL += "'CNF45065',";
                comandoSQL += "'PII41255-5',";
                comandoSQL += "'PAF07334-9',";
                comandoSQL += "'CR96798',";
                comandoSQL += "'PMT36779-4',";
                comandoSQL += "'CAP36407-0',";
                comandoSQL += "'PMT42320-4',";
                comandoSQL += "'PMT27909-8R01',";
                comandoSQL += "'PII21330-8',";
                comandoSQL += "'PII24527-0',";
                comandoSQL += "'PII38882-2',";
                comandoSQL += "'PMT16595-7',";
                comandoSQL += "'PMT38082-7',";
                comandoSQL += "'PAF06851-4',";
                comandoSQL += "'PII28421-9',";
                comandoSQL += "'PII41531-9',";
                comandoSQL += "'PMT27103-3',";
                comandoSQL += "'PAF08960-1',";
                comandoSQL += "'CNF45013',";
                comandoSQL += "'PMT44380-7',";
                comandoSQL += "'PII42275-2',";
                comandoSQL += "'PII33154-9',";
                comandoSQL += "'PII29704-9',";
                comandoSQL += "'PII38813-7',";
                comandoSQL += "'PMT40347-1',";
                comandoSQL += "'PII43525-1',";
                comandoSQL += "'PMT27102-5R01',";
                comandoSQL += "'PII39815-2',";
                comandoSQL += "'PMT40502-1',";
                comandoSQL += "'CAP18786-1R03',";
                comandoSQL += "'FGF41681-2',";
                comandoSQL += "'PMT25010-1R01',";
                comandoSQL += "'PII26950-1',";
                comandoSQL += "'PII26515-2',";
                comandoSQL += "'PMT31978-7',";
                comandoSQL += "'PII27514-3',";
                comandoSQL += "'PMT27808-1R01',";
                comandoSQL += "'PMT28105-9',";
                comandoSQL += "'PII26029-3',";
                comandoSQL += "'CAP40244-9',";
                comandoSQL += "'CCE29769-4',";
                comandoSQL += "'CNF38098',";
                comandoSQL += "'PII22191-3R01',";
                comandoSQL += "'PMT47447-3',";
                comandoSQL += "'PMT47406-9',";
                comandoSQL += "'PII28200-6',";
                comandoSQL += "'PII23196-3R01',";
                comandoSQL += "'PII40790-2',";
                comandoSQL += "'PMT32329-0',";
                comandoSQL += "'CAP34938-8R01',";
                comandoSQL += "'CCE37657-1R01',";
                comandoSQL += "'PMT32457-9',";
                comandoSQL += "'PII27494-8',";
                comandoSQL += "'PII27846-1',";
                comandoSQL += "'PII28005-1',";
                comandoSQL += "'PMT42398-3',";
                comandoSQL += "'PAF06323-2R01',";
                comandoSQL += "'PMT29311-1',";
                comandoSQL += "'PAF08424-7H01',";
                comandoSQL += "'PII42432-8',";
                comandoSQL += "'PMT39462-1',";
                comandoSQL += "'PII33551-7',";
                comandoSQL += "'PII29300-3R01',";
                comandoSQL += "'PII29255-1',";
                comandoSQL += "'PMT41536-0R01',";
                comandoSQL += "'PII27491-3',";
                comandoSQL += "'CNF0012233368',";
                comandoSQL += "'CNF44795-1',";
                comandoSQL += "'PMT34481-5',";
                comandoSQL += "'PII23352-1R02',";
                comandoSQL += "'PMT28595-3',";
                comandoSQL += "'PII30096-6',";
                comandoSQL += "'PII28789-3'";
                comandoSQL += " ) ";

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

        public bool ajustarCarteira(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;                
                comandoSQL += " UPDATE  O ";
                comandoSQL += " SET CARTPROVMIN =   CASE    WHEN DIASATRASO <= 60 THEN 'C1' ";
                comandoSQL += " WHEN DIASATRASO BETWEEN 61 AND 89 THEN 'C2'";
                comandoSQL += " ELSE 'C3' END ";                
                comandoSQL += " FROM OPERACOES O ";
                comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND (CARTPROVMIN IS NULL OR LEN(RTRIM(LTRIM(CARTPROVMIN))) = 0)";

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

        public bool ajustarCarteiraV2 (DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);
            string comandoSQL = String.Empty;

            try
            {
                comandoSQL = " UPDATE O " +
                " SET CARTPROVMIN = CASE WHEN INDPREJUIZO = 'S' THEN 'C4' " +
                            " ELSE " +
                                " CASE    WHEN DIASATRASO <= 60 THEN 'C1' " +
                                        " WHEN DIASATRASO BETWEEN 61 AND 89 THEN 'C2' " +
                                " ELSE 'C3' END " +
                                " END " +
                " FROM OPERACOES O ";
                
                comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND (CARTPROVMIN IS NULL OR LEN(RTRIM(LTRIM(CARTPROVMIN))) = 0) ";

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

        public bool ajustarEstagio(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL += " UPDATE  O ";
                comandoSQL += " SET ESTINSTFIN =   CASE    WHEN DIASATRASO <= 60 THEN '1' ";
                comandoSQL += " WHEN DIASATRASO BETWEEN 61 AND 89 THEN '2'";
                comandoSQL += " ELSE '3' END ";
                comandoSQL += " FROM OPERACOES O ";
                comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND (ESTINSTFIN IS NULL OR LEN(RTRIM(LTRIM(ESTINSTFIN))) = 0)";

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

        public bool ajustarValorContabil(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;                
                comandoSQL += " IF OBJECT_ID('tempdb..#Vencimentos') IS NOT NULL DROP TABLE #Vencimentos ";                
                comandoSQL += " SELECT O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO, SUM(V.VLRVENCTO) AS TotalVencimentos ";
                comandoSQL += " INTO	#Vencimentos FROM VENCIMENTOSOPE V INNER JOIN OPERACOES O WITH(NOLOCK) ON ";
                comandoSQL += " V.DATBASE = O.DATBASE AND V.CODCOLIGADA = O.CODCOLIGADA AND V.NROCONTROLE = O.NROCONTROLE AND V.IDOPERACAO = O.IDOPERACAO ";
                comandoSQL += " WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND (O.VLRCONTBR IS NULL OR VLRCONTBR = 0.00) ";
                comandoSQL += " GROUP BY O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO ";
                

                comandoSQL += " UPDATE O SET VLRCONTBR = C.TotalVencimentos ";
                comandoSQL += " FROM OPERACOES O INNER JOIN #Vencimentos C ON O.DATBASE = C.DATBASE AND O.CODCOLIGADA = C.CODCOLIGADA AND O.NROCONTROLE = C.NROCONTROLE  AND O.IDOPERACAO = C.IDOPERACAO ";                
                comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";

                comandoSQL += " DROP TABLE #Vencimentos ";

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

        public bool ajustarValorJusto(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL += " IF OBJECT_ID('tempdb..#Vencimentos') IS NOT NULL DROP TABLE #Vencimentos ";
                comandoSQL += " SELECT O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO, SUM(V.VLRVENCTO) AS TotalVencimentos ";
                comandoSQL += " INTO	#Vencimentos FROM VENCIMENTOSOPE V INNER JOIN OPERACOES O WITH(NOLOCK) ON ";
                comandoSQL += " V.DATBASE = O.DATBASE AND V.CODCOLIGADA = O.CODCOLIGADA AND V.NROCONTROLE = O.NROCONTROLE AND V.IDOPERACAO = O.IDOPERACAO ";
                comandoSQL += " WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND (O.VLRJUSTO IS NULL OR VLRJUSTO = 0.00) ";
                comandoSQL += " GROUP BY O.CODCOLIGADA, O.DATBASE, O.NROCONTROLE, O.IDOPERACAO ";


                comandoSQL += " UPDATE O SET VLRJUSTO = C.TotalVencimentos ";
                comandoSQL += " FROM OPERACOES O INNER JOIN #Vencimentos C ON O.DATBASE = C.DATBASE AND O.CODCOLIGADA = C.CODCOLIGADA AND O.NROCONTROLE = C.NROCONTROLE  AND O.IDOPERACAO = C.IDOPERACAO ";
                comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";

                comandoSQL += " DROP TABLE #Vencimentos ";

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


        public List<ResultAgrupadoVigencia> listarResultadoVigencia(DateTime dataVigencia)
        {
            List<ResultAgrupadoVigencia> lista = new List<ResultAgrupadoVigencia>();
            string comandoSQL = "SELECT      FORMAT(DATBASE, 'dd/MM/yyyy') AS DataVigencia, CODCOLIGADA AS CodigoColigada, CODSISTEMA AS CodigoSistema, COUNT (*) AS Quantidade FROM        OPERACOES WITH(NOLOCK) ";
            comandoSQL += " WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
            comandoSQL += " GROUP BY    DATBASE, CODCOLIGADA, CODSISTEMA ";
            comandoSQL += " ORDER BY CODCOLIGADA, CODSISTEMA ";

            var conexao = new SqlConnection(this.conexaoBD);

            //MessageBox.Show(comandoSQL);
            //MessageBox.Show(this.conexaoBD.ToString());

            try
            {
                conexao.Open();
                lista = conexao.Query<ResultAgrupadoVigencia>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();
            }
            catch
            {
                throw new Exception ("Erro ao consultar informações");
            }
            finally
            {
                conexao.Close();
            }


            return lista;
        }

        public List<InfoPDDVigencia> listarResultadoPDDVigencia(DateTime dataVigencia)
        {
            List<InfoPDDVigencia> lista = new List<InfoPDDVigencia>();
            

            var conexao = new SqlConnection(this.conexaoBD);

            string comandoSQL = " SELECT P.CODCARTEIRA AS Carteira, P.CODESTAGIO AS Estagio, (P.CODMODALIDADE + ' - ' + M.DESMODALIDADE) AS Modalidade, " +
            " COUNT(*) AS Quantidade, SUM(VALOR_EL) AS TotalProvisao " +
            " FROM GRC_PERDA P INNER JOIN MODALIDADES M ON P.CODMODALIDADE = M.CODMODALIDADE " +
            " AND M.DATVIGENCIA = (SELECT MAX(DATVIGENCIA) FROM MODALIDADES) " +
            " WHERE P.DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
            " GROUP BY   P.CODCARTEIRA, P.CODESTAGIO, (P.CODMODALIDADE + ' - ' + M.DESMODALIDADE) " +
            " ORDER BY (P.CODMODALIDADE +' - ' + M.DESMODALIDADE), P.CODCARTEIRA, P.CODESTAGIO ";

            try
            {
                conexao.Open();
                lista = conexao.Query<InfoPDDVigencia>(comandoSQL,
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


        public List<InfoPDDColigada> listarResultadoPDDEmpresa(DateTime dataVigencia)
        {
            List<InfoPDDColigada> lista = new List<InfoPDDColigada>();


            var conexao = new SqlConnection(this.conexaoBD);

            string comandoSQL = "SELECT CodColigada AS Empresa, COUNT(*) AS Quantidade, SUM(VALOR_EL) AS TotalProvisao " +
                " FROM GRC_PERDA WITH(NOLOCK) " +
                " WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " GROUP BY CODCOLIGADA ORDER BY CODCOLIGADA ";

            try
            {
                conexao.Open();
                lista = conexao.Query<InfoPDDColigada>(comandoSQL,
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


        public List<InfoPDDOperacao> listarResultadoPDDOperacao(DateTime dataVigencia)
        {
            List<InfoPDDOperacao> lista = new List<InfoPDDOperacao>();


            var conexao = new SqlConnection(this.conexaoBD);

            string comandoSQL = " SELECT CODCOLIGADA AS Empresa, (P.CODSISTEMA + ' - ' + S.NOMSISTEMA) AS Sistema, " +
                " (P.CODMODALIDADE + ' - ' + M.DESMODALIDADE) AS Modalidade, P.CPF_CNPJ AS CpfCnpj, " +
                " P.CONTRATO_ORIGEM AS NumeroContrato, P.CODCARTEIRA AS Carteira, P.CODESTAGIO AS Estagio," +
                " P.VALOR_EL AS ValorProvisao " +
                " FROM GRC_PERDA P WITH(NOLOCK) INNER JOIN SISTEMAS S ON P.CODSISTEMA = S.CODSISTEMA" +
                " INNER JOIN MODALIDADES M ON P.CODMODALIDADE = M.CODMODALIDADE AND M.DATVIGENCIA = (SELECT MAX(DATVIGENCIA) FROM MODALIDADES) " +
                " WHERE P.DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " ORDER BY P.CODCOLIGADA, P.CODSISTEMA, P.CONTRATO_ORIGEM ";


            try
            {
                conexao.Open();
                lista = conexao.Query<InfoPDDOperacao>(comandoSQL,
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

        public List<InfoPDDB9xMotorInterno> listarResultadoPDDMotorInterno(DateTime dataVigencia)
        {
            List<InfoPDDB9xMotorInterno> lista = new List<InfoPDDB9xMotorInterno>();


            var conexao = new SqlConnection(this.conexaoBD);

            string comandoSQL = " IF OBJECT_ID('tempdb..#Contratos') IS NOT NULL DROP TABLE #Contratos ";
            comandoSQL += " IF OBJECT_ID('tempdb..#Vencimentos') IS NOT NULL DROP TABLE #Vencimentos ";
            comandoSQL += " SELECT\t\tO.DatBase,\r\n\t\t\tO.CodColigada ,\r\n\t\t\tO.CodSistema,\r\n\t\t\tO.NroControle,\r\n\t\t\tO.IdOperacao,\r\n\t\t\tO.CodModalidade,\t\t\t\r\n\t\t\tO.Ipoc,\t\t\t\t\t\t\r\n\t\t\t(SUBSTRING(CO.CNPJEMPR, 1, 8) + O.CODMODALIDADE + O.CODPESSOA + \r\n\t\t\t(CASE WHEN CAST(O.CODPESSOA AS INT) IN (2,4,6) THEN SUBSTRING(OGC.CPF_CNPJ, 1, 8) ELSE OGC.CPF_CNPJ END) +\r\n\t\t\tO.CONTRATO) AS IpocCalculado,\r\n\t\t\tCASE WHEN CAST(O.CODPESSOA AS INT) IN (2,4,6) THEN SUBSTRING(OGC.CPF_CNPJ, 1, 8) ELSE OGC.CPF_CNPJ END AS CodigoCliente, \r\n\t\t\tRTRIM(LTRIM(OGC.CPF_CNPJ)) AS CpfCnpj,\r\n\t\t\tO.CODPESSOA AS TpCliente,\r\n\t\t\tRTRIM(LTRIM(O.CONTRATO)) AS NumeroContrato,\r\n\t\t\tO.DiasAtraso,\r\n\t\t\tO.INDSAIDA AS Saida,\r\n\t\t\tO.INDPREJUIZO AS Prejuizo,\r\n\t\t\tO.INDVENCIDO AS Vencido,\r\n\t\t\tO.ESTINSTFIN AS EstagioLegado,\r\n\t\t\tO.CARTPROVMIN AS CarteiraLegado,\r\n\t\t\tO.VLRPRINCIPAL AS ValorPrincipal,\r\n\t\t\tCAST(0 AS NUMERIC(21,2)) AS SaldoContabil,\r\n\t\t\tIE.CODCARTEIRA AS CarteiraMotorInterno,\r\n\t\t\tIE.CODESTAGIO AS EstagioMotorInterno,\r\n\t\t\tIE.CODPROBLEMATICO AS AtivoProblematicoMotorInterno,\r\n\t\t\tIE.VALOR_EL AS ValorProvisaoMotorInterno\t\t\t\t\t\t\r\ninto\t\t#Contratos\r\nFROM\t\tAB_OPERACOES_IFRS9.dbo.OPERACOES O WITH(NOLOCK) INNER JOIN AB_GESTAORC.dbo.OPERACAO OGC WITH(NOLOCK)\r\n\t\t\t\tON\tO.DATBASE = OGC.DATA_BASE\r\n\t\t\t\tAND O.CODCOLIGADA = OGC.CODCOLIGADA\r\n\t\t\t\tAND O.NROCONTROLE = OGC.NRO_CONTROLE\r\n\t\t\t\tAND O.IDOPERACAO = OGC.ID_OPERACAO\r\n\t\t\t\tAND O.CODSISTEMA = OGC.CODSISTEMA\r\n\t\t\t\tAND O.CONTRATO = OGC.CONTRATO_ORIGEM\r\n\t\t\t\tAND O.CODMODALIDADE = OGC.CODMODALIDADE\r\n\t\t\t\tAND (RTRIM(LTRIM(O.CNPJCPFCLI)) + RTRIM(LTRIM(ISNULL(O.DVCNPJCLI,'')))) = RTRIM(LTRIM(OGC.CPF_CNPJ))\t\t\t\r\n\t\t\tINNER JOIN AB_GESTAORC.dbo.IMPORTACAO_ESTATISTICO IE WITH(NOLOCK)\r\n\t\t\t\t\tON\tOGC.DATA_BASE = IE.DATA_BASE\r\n\t\t\t\t\tAND OGC.CODCOLIGADA = IE.CODCOLIGADA\r\n\t\t\t\t\tAND OGC.CODSISTEMA = IE.CODSISTEMA\r\n\t\t\t\t\tAND OGC.CPF_CNPJ = IE.CPF_CNPJ\r\n\t\t\t\t\tAND OGC.CONTRATO_ORIGEM = IE.CONTRATO_ORIGEM\r\n\t\t\t\t\tAND OGC.CODMODALIDADE = IE.CODMODALIDADE\r\n\t\t\t\t\tAND OGC.TIPO_PESSOA = IE.TIPO_PESSOA\t\r\n\t\t\tINNER JOIN AB_OPERACOES_IFRS9.dbo.COLIGADAS CO\r\n\t\t\t\t\tON\tCO.DATBASE = O.DATBASE\r\n\t\t\t\t\tAND CO.CODCOLIGADA = O.CODCOLIGADA ";
            comandoSQL += " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";

            comandoSQL += " SELECT\t\tO.NROCONTROLE, O.IDOPERACAO, O.CodColigada, O.CODSISTEMA, O.CODMODALIDADE, O.NumeroContrato, SUM(V.VLRVENCTO) TotalVencimentos\r\nINTO\t\t#Vencimentos\r\nFROM\t\t#Contratos O INNER JOIN AB_OPERACOES_IFRS9.dbo.VENCIMENTOSOPE V WITH(NOLOCK)\r\n\t\t\t\tON\tO.DATBASE = V.DATBASE\r\n\t\t\t\tAND\tO.CODCOLIGADA = V.CODCOLIGADA\r\n\t\t\t\tAND O.NROCONTROLE = V.NROCONTROLE\r\n\t\t\t\tAND O.IDOPERACAO = V.IDOPERACAO ";
            comandoSQL += " WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
            comandoSQL += " GROUP BY\tO.NROCONTROLE, O.IDOPERACAO, O.CODCOLIGADA, O.CODSISTEMA, O.CODMODALIDADE, O.NumeroContrato ";

            comandoSQL += " UPDATE\tO\r\nSET\t\tSaldoContabil = V.TotalVencimentos\r\nFROM\t#Contratos O INNER JOIN #Vencimentos V\r\n\t\t\tON\tO.NROCONTROLE = V.NROCONTROLE\r\n\t\t\tAND O.IDOPERACAO = V.IDOPERACAO\r\n\t\t\tAND O.CODCOLIGADA = V.CODCOLIGADA\r\n\t\t\tAND O.CODSISTEMA = V.CODSISTEMA\r\n\t\t\tAND O.CODMODALIDADE = V.CODMODALIDADE\r\n\t\t\tAND O.NumeroContrato = V.NumeroContrato ";

            comandoSQL += " SELECT\t\t*\r\nFROM\t\t#Contratos\r\nORDER BY\tCODCOLIGADA,\r\n\t\t\tCODSISTEMA,\r\n\t\t\tNROCONTROLE,\r\n\t\t\tIDOPERACAO ";

            comandoSQL += " DROP TABLE #Contratos ";
            comandoSQL += " DROP TABLE #Vencimentos ";            

            try
            {
                conexao.Open();
                lista = conexao.Query<InfoPDDB9xMotorInterno>(comandoSQL,
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

        public List<InfoPDDB9xMotorInterno> listarResultadoPDDMotorInternoV2(DateTime dataVigencia)
        {
            List<InfoPDDB9xMotorInterno> lista = new List<InfoPDDB9xMotorInterno>();


            var conexao = new SqlConnection(this.conexaoBD);

            string comandoSQL = String.Empty;

            comandoSQL += " IF OBJECT_ID('tempdb..#Contratos') IS NOT NULL DROP TABLE #Contratos " +
" IF OBJECT_ID('tempdb..#Vencimentos') IS NOT NULL DROP TABLE #Vencimentos " +

" SELECT      DATA_BASE AS DatBase, CodColigada, CodSistema, NRO_CONTROLE AS NroControle, ID_OPERACAO AS IdOperacao, CodModalidade, " +
              " IPOC_3040 AS Ipoc, " +
            " ('60889128' + CODMODALIDADE + TIPO_PESSOA +  (CASE WHEN CAST(TIPO_PESSOA AS INT) IN(2, 4, 6) THEN SUBSTRING(CPF_CNPJ, 1, 8) ELSE CPF_CNPJ END) +CONTRATO_ORIGEM) AS IpocCalculado, " +
            " CASE WHEN CAST(TIPO_PESSOA AS INT) IN(2, 4, 6) THEN SUBSTRING(CPF_CNPJ, 1, 8) ELSE CPF_CNPJ END AS CodigoCliente, " +
            " CPF_CNPJ AS CpfCnpj, TIPO_PESSOA AS TpCliente, CONTRATO_ORIGEM AS NumeroContrato, DiasAtraso, INDSAIDA AS Saida, " +
            " 'N' AS Prejuizo, INDVENCIDO AS Vencido, CODCARTEIRA AS CarteiraLegado, CODESTAGIO AS EstagioLegado, " +
            " VALOR_PRINCIPAL_EAD AS ValorPrincipal, CAST(0 AS NUMERIC(21, 2)) AS SaldoContabil, CAST('' AS VARCHAR(5)) AS CarteiraMotorInterno, " +
            " CAST('' AS VARCHAR(5)) AS EstagioMotorInterno, CAST('' AS VARCHAR(5)) AS AtivoProblematicoMotorInterno, CAST(0 AS NUMERIC(21, 2)) AS ValorProvisaoMotorInterno " +
" INTO		#Contratos " +
" FROM        AB_GESTAORC.dbo.OPERACAO WITH(NOLOCK) " +
" WHERE DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
" AND CODCOLIGADA = 1 " +


" SELECT O.NROCONTROLE, O.IDOPERACAO, O.CodColigada, SUM(V.VLRVENCTO) TotalVencimentos " +
" INTO		#Vencimentos " +
" FROM		#Contratos O INNER JOIN AB_OPERACOES_IFRS9.dbo.VENCIMENTOSOPE V WITH(NOLOCK)  " +
                "  ON O.DATBASE = V.DATBASE AND O.CODCOLIGADA = V.CODCOLIGADA AND O.NROCONTROLE = V.NROCONTROLE AND O.IDOPERACAO = V.IDOPERACAO " +
" WHERE V.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
" GROUP BY    O.NROCONTROLE, O.IDOPERACAO, O.CODCOLIGADA " +

" UPDATE O SET SaldoContabil = V.TotalVencimentos " +
" FROM #Contratos O INNER JOIN #Vencimentos V " +
    " ON O.NROCONTROLE = V.NROCONTROLE " +
    " AND O.IDOPERACAO = V.IDOPERACAO " +
    " AND O.CODCOLIGADA = V.CODCOLIGADA " +

" UPDATE O " +
" SET CarteiraMotorInterno = IE.CODCARTEIRA, " +
        " EstagioMotorInterno = IE.CODESTAGIO, " +
        " ValorProvisaoMotorInterno = IE.VALOR_EL, " +
        " AtivoProblematicoMotorInterno = IE.CODPROBLEMATICO " +
" FROM	#Contratos O INNER JOIN AB_GESTAORC.dbo.IMPORTACAO_ESTATISTICO IE " +
            " ON  O.DatBase = IE.DATA_BASE AND O.CodColigada = IE.CODCOLIGADA AND O.CodSistema = IE.CODSISTEMA " +
            " AND O.CpfCnpj = IE.CPF_CNPJ " +
            " AND O.NumeroContrato = IE.CONTRATO_ORIGEM " +
            " AND O.CodModalidade = IE.CODMODALIDADE " +
            " AND O.TPCLIENTE = IE.TIPO_PESSOA " +
" WHERE IE.DATA_BASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +

" SELECT C.* FROM AB_GESTAORC.dbo.INDICIOS_102025 I INNER JOIN #Contratos C ON C.IPOC = I.IPOC ORDER BY CODSISTEMA, NroControle, IdOperacao ";
            
comandoSQL += " DROP TABLE #Contratos ";
comandoSQL += " DROP TABLE #Vencimentos ";

            try
            {
                conexao.Open();
                lista = conexao.Query<InfoPDDB9xMotorInterno>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar informações: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }


            return lista;
        }

        public List<InfoPDDB9xMotorInterno> listarIndiciosBacen102025()
        {
            List<InfoPDDB9xMotorInterno> lista = new List<InfoPDDB9xMotorInterno>();
            var conexao = new SqlConnection(this.conexaoBD);
            string comandoSQL = String.Empty;

            comandoSQL += " SELECT\t\tC.DATA_BASE AS DatBase,\r\n\t\t\tC.CodColigada,\r\n\t\t\tC.CodSistema,\r\n\t\t\tC.NRO_CONTROLE AS NroControle,\r\n\t\t\tC.ID_OPERACAO AS IdOperacao,\r\n\t\t\tC.CodModalidade,\r\n\t\t\tIPOC_3040 AS Ipoc,\r\n\t\t\t('60889128' + C.CODMODALIDADE + C.TIPO_PESSOA +  (CASE WHEN CAST(C.TIPO_PESSOA AS INT) IN (2,4,6) THEN SUBSTRING(C.CPF_CNPJ, 1, 8) ELSE C.CPF_CNPJ END) + C.CONTRATO_ORIGEM) AS IpocCalculado,\r\n\t\t\tCASE WHEN CAST(C.TIPO_PESSOA AS INT) IN (2,4,6) THEN SUBSTRING(C.CPF_CNPJ, 1, 8) ELSE C.CPF_CNPJ END AS CodigoCliente,\r\n\t\t\tC.CPF_CNPJ AS CpfCnpj,\r\n\t\t\tC.TIPO_PESSOA AS TpCliente,\r\n\t\t\tC.CONTRATO_ORIGEM AS NumeroContrato,\r\n\t\t\tC.DiasAtraso,\r\n\t\t\tINDSAIDA AS Saida,\r\n\t\t\t'N' AS Prejuizo,\r\n\t\t\tINDVENCIDO AS Vencido,\r\n\t\t\tC.CODCARTEIRA AS CarteiraLegado,\r\n\t\t\tC.CODESTAGIO AS EstagioLegado,\r\n\t\t\tC.VALOR_PRINCIPAL_EAD AS ValorPrincipal,\r\n\t\t\tC.VALOR_PRINCIPAL_EAD AS SaldoContabil,\r\n\t\t\tIE.CODCARTEIRA AS CarteiraMotorInterno,\r\n\t\t\tIE.CODESTAGIO AS EstagioMotorInterno,\r\n\t\t\tIE.CODPROBLEMATICO AS AtivoProblematicoMotorInterno,\r\n\t\t\tIE.VALOR_EL AS ValorProvisaoMotorInterno\t\t\t\t\t\t\r\n--INTO\t\t#Contratos\r\nFROM\t\tAB_GESTAORC.dbo.OPERACAO C WITH(NOLOCK) INNER JOIN AB_GESTAORC.dbo.INDICIOS_102025 I\r\n\t\t\t\tON\tC.IPOC_3040 = I.IPOC\r\n\t\t\tINNER JOIN AB_GESTAORC.dbo.IMPORTACAO_ESTATISTICO IE\r\n\t\t\t\tON\tIE.DATA_BASE = C.DATA_BASE\r\n\t\t\t\tAND IE.CODCOLIGADA = C.CODCOLIGADA\r\n\t\t\t\tAND IE.CODSISTEMA = C.CODSISTEMA\r\n\t\t\t\tAND IE.CPF_CNPJ = C.CPF_CNPJ\r\n\t\t\t\tAND IE.CONTRATO_ORIGEM = C.CONTRATO_ORIGEM\r\n\t\t\t\tAND IE.CODMODALIDADE = C.CODMODALIDADE\r\n\t\t\t\tAND IE.TIPO_PESSOA = C.TIPO_PESSOA\r\nWHERE\t\tC.DATA_BASE = '20251031'\r\nAND\t\t\tC.CODCOLIGADA = 1\r\nORDER BY\tC.CODCOLIGADA, C.NRO_CONTROLE, C.ID_OPERACAO ";

            try
            {
                conexao.Open();
                lista = conexao.Query<InfoPDDB9xMotorInterno>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar informações: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }


            return lista;
        }

        public List<B9ResumoIntegracaoCarteiras> listarResumoIntegracaoCarteiras(DateTime dataVigencia)
        {
            List<B9ResumoIntegracaoCarteiras> lista = new List<B9ResumoIntegracaoCarteiras>();
            var conexao = new SqlConnection(this.conexaoBD);
            string comandoSQL = String.Empty;

            comandoSQL += " SELECT\t\t(O.CODSISTEMA + ' - ' + S.NOMSISTEMA) AS Sistema, COUNT(*) AS Quantidade\r\nFROM\t\tOPERACOES O WITH(NOLOCK) INNER JOIN SISTEMAS S\r\n\t\t\t\tON O.CODSISTEMA = S.CODSISTEMA ";
            comandoSQL += " WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
            comandoSQL += " GROUP BY (O.CODSISTEMA +' - ' + S.NOMSISTEMA) ";
            comandoSQL += " ORDER BY (O.CODSISTEMA +' - ' + S.NOMSISTEMA) ";

            try
            {
                conexao.Open();
                lista = conexao.Query<B9ResumoIntegracaoCarteiras>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar informações: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }


            return lista;
        }

        public List<TabelaVencimentosBacen> listarDominiosVencimentosBacen()
        {
            List<TabelaVencimentosBacen> lista = new List<TabelaVencimentosBacen>();
            var conexao = new SqlConnection(this.conexaoBD);
            string comandoSQL = String.Empty;

            comandoSQL = "SELECT\t\t'v' + CONVERT(VARCHAR(10), CODVENCTO) AS CodigoVencimento, DESVENCTO AS Vencimento,\r\n\t\t\tFAIXA1 AS De, FAIXA2 AS Ate, INDVENCTO AS TipoVencimento\r\nFROM\t\tINTER_VENCIMENTOS\r\nWHERE\t\tDATVIGENCIA >= (SELECT MAX(DATVIGENCIA) FROM INTER_VENCIMENTOS)\r\nORDER BY\tCODVENCTO ";

            try
            {
                conexao.Open();
                lista = conexao.Query<TabelaVencimentosBacen>(comandoSQL,
                    null,
                    null,
                    true,
                    null,
                    commandType: CommandType.Text
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar informações: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }


            return lista;
        }

        public List<GarantiaIN659> listarGarantiasIN659(DateTime dataVigencia)
        {
            List<GarantiaIN659> lista = new List<GarantiaIN659>();
            string comandoSQL = "";
            comandoSQL += " SELECT      O.DATBASE AS DataVigencia, ";
            comandoSQL += " O.CODCOLIGADA AS CodigoColigada, ";
            comandoSQL += " O.CODPESSOA AS TipoCliente, ";
            comandoSQL += " (O.CODSISTEMA + ' - ' + S.NOMSISTEMA) AS Sistema, ";
            comandoSQL += " (O.CNPJCPFCLI + O.DVCNPJCLI) AS CpfCnpj, ";
            comandoSQL += " O.Ipoc, ";
            comandoSQL += " O.CONTRATO AS NumeroContrato, ";
            comandoSQL += " O.CODMODALIDADE AS CodigoModalidade, ";
            comandoSQL += " G.CODGARANTIA AS CodigoGarantia, ";
            comandoSQL += " ISNULL(G.CNPJCPFGAR, '') AS CpfCnpjGarantidor, ";
            comandoSQL += " ISNULL(G.CODPESSOA, '') AS TipoPessoaGarantidor, ";
            comandoSQL += " ISNULL(G.PERCGARANTIDOR, '0.01') AS PercentualGarantidor, ";
            comandoSQL += " ISNULL(G.VLRGARANTIAORI, 0.01) AS ValorOriginalGarantia, ";
            comandoSQL += " ISNULL(G.VLRGARANTIA, 0.01) AS ValorAtualGarantia, ";
            comandoSQL += " ISNULL(G.DATULTAVALIACAO, '1900-01-01') AS DataUltimaAvaliacao, ";
            comandoSQL += "  ISNULL(G.SITUACAO, '') AS SituacaoGarantia, ";
            comandoSQL += "  ISNULL(G.IDENT, '') AS IdentificacaoGarantia, ";
            comandoSQL += "  ISNULL(G.TIPO, '') AS TipoValorGarantia, ";
            comandoSQL += "  ISNULL(G.PERCDATA, '0.01') AS PercentualReavaliado, ";
            comandoSQL += "  ISNULL(G.COMPARTILHAMENTO, '') AS Compartilhamento, ";
            comandoSQL += " ('60889128' + O.CODMODALIDADE + O.CODPESSOA + ";
            comandoSQL += " (CASE WHEN CAST(O.CODPESSOA AS INT) IN(2, 4, 6) ";
            comandoSQL += " THEN SUBSTRING((O.CNPJCPFCLI +O.DVCNPJCLI), 1, 8) ELSE O.CNPJCPFCLI END) +O.CONTRATO) AS IpocCalculado ";
            comandoSQL += "  FROM GARANTIASOPE G WITH(NOLOCK) INNER JOIN OPERACOES O WITH(NOLOCK) ";
            comandoSQL += " ON G.DATBASE = O.DATBASE ";
            comandoSQL += " AND G.CODCOLIGADA = O.CODCOLIGADA ";
            comandoSQL += " AND G.NROCONTROLE = O.NROCONTROLE ";
            comandoSQL += " AND G.IDOPERACAO = O.IDOPERACAO ";
            comandoSQL += "  INNER JOIN SISTEMAS S ";
            comandoSQL += " ON O.CODSISTEMA = S.CODSISTEMA ";
            comandoSQL += " WHERE G.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
            comandoSQL += " AND G.NROCONTROLEIMP = 39 ";
            comandoSQL += " ORDER BY    O.CODCOLIGADA, (O.CODSISTEMA + ' - ' + S.NOMSISTEMA), O.CODMODALIDADE, ";
            comandoSQL += " O.CONTRATO, G.CODGARANTIA ";

            var conexao = new SqlConnection(this.conexaoBD);
                        
            try
            {
                conexao.Open();
                lista = conexao.Query<GarantiaIN659>(comandoSQL,
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
        public bool ajustarCodPessoa(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL = " UPDATE CLIENTES ";
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

        public bool tratarInfoAdicionalLCILCAV2(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;

                comandoSQL += " IF OBJECT_ID('tempdb..#InfAdicionalDataBaseAnt') IS NOT NULL DROP TABLE #InfAdicionalDataBaseAnt " +
                " IF OBJECT_ID('tempdb..#InfAdicionalDataBaseVigencia') IS NOT NULL DROP TABLE #InfAdicionalDataBaseVigencia " +
                " DECLARE @DATABASEANT DATETIME " +
                " SELECT @DATABASEANT = MAX(DATBASE) " +
                " FROM COLIGADAS " +
                " WHERE CODCOLIGADA = 1 " +
                " AND DATBASE< '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +

    " DELETE I " +
    " FROM INFADICIONAISOPE I " +
    " WHERE       DATBASE = '" +  dataVigencia.ToString("yyyy-MM-dd") + "' " +
    " AND CODCOLIGADA = 1 " +
    " AND(CODINFADD LIKE '%LCI%' OR CODINFADD LIKE '%LCA%') " +


    " SELECT O.DATBASE, O.CODCOLIGADA, O.CODSISTEMA, O.NROCONTROLE, O.IDOPERACAO, " +
                " O.CODMODALIDADE, O.CNPJCPFCLI, O.DVCNPJCLI, O.CONTRATO, " +
                " I.TPINFADD AS CODTIPCESSAO, I.CODINFADD AS NROCESSAO, I.IDENTIFICACAO AS IDENTCESSAO, " +
                " I.VALOR AS VLRCESSAO, I.PERCENTUAL AS PERCCESSAO, I.QTDREGISTROS AS QTDCESSAO, " +
                " I.NROCONTROLEIMP, I.SEQUENCIA " +
    " into		#InfAdicionalDataBaseAnt " +
    " FROM OPERACOES O WITH(NOLOCK) INNER JOIN INFADICIONAISOPE I WITH(NOLOCK) " +
                    " ON O.DATBASE = I.DATBASE " +
                    " AND O.CODCOLIGADA = I.CODCOLIGADA " +
                   " AND O.NROCONTROLE = I.NROCONTROLE " +
                    " AND O.IDOPERACAO = I.IDOPERACAO " +
    " WHERE O.DATBASE = @DATABASEANT " +
    " AND O.CODCOLIGADA = 1 " +
    " AND(CODINFADD LIKE '%LCI%' OR CODINFADD LIKE '%LCA%') " +

    " SELECT		O.IDOPERACAO AS IDOPERACAODATABASEVIGENCIA, " +
                    " I.* " +
        " into		#InfAdicionalDataBaseVigencia " +
        " FROM		#InfAdicionalDataBaseAnt I INNER JOIN OPERACOES O WITH(NOLOCK) " +
                        " ON	I.CODCOLIGADA = O.CODCOLIGADA " +
                        " AND	I.CODSISTEMA = O.CODSISTEMA " +
                        " AND	I.NROCONTROLE = O.NROCONTROLE " +
                        " AND	I.CODMODALIDADE = O.CODMODALIDADE " +
                        " AND I.CNPJCPFCLI = O.CNPJCPFCLI " +
                        " AND I.DVCNPJCLI = O.DVCNPJCLI " +
                        " AND I.CONTRATO = O.CONTRATO " +
        " WHERE		O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +

        " INSERT	INTO INFADICIONAISOPE " +
        " SELECT	'" + dataVigencia.ToString("yyyy-MM-dd") + "', " + " CODCOLIGADA, NROCONTROLE, IDOPERACAODATABASEVIGENCIA, SEQUENCIA, CODTIPCESSAO, " +
        " 		NROCESSAO, IDENTCESSAO, VLRCESSAO, PERCCESSAO, QTDCESSAO, NROCONTROLEIMP " +
        " FROM	#InfAdicionalDataBaseVigencia ";
    


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

        public bool tratarOrigemRecurso(DateTime dataVigencia)
        {
            return (tratarOrigemRecursoBNDES(dataVigencia) && tratarOrigemRecursoOutrosSistemas(dataVigencia) && excluirOcorrencia109(dataVigencia));
        }

        private bool excluirOcorrencia109(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL += " DELETE FROM OCORRENCIAS " +
                " WHERE   DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND CODERRO = 109 ";

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

        private bool tratarOrigemRecursoBNDES(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL += " UPDATE O " +
                " SET CODRECURSO = '0299' " +
                " FROM OPERACOES O " +
                " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND(CODRECURSO IS NULL OR CODRECURSO = '0000' OR LEN(LTRIM(RTRIM(CODRECURSO))) = 0) " +
                " AND CODSISTEMA = '120' ";

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

        private bool tratarOrigemRecursoOutrosSistemas(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL += " UPDATE O " +
                " SET CODRECURSO = '0199' " +
                " FROM OPERACOES O " +
                " WHERE O.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND(CODRECURSO IS NULL OR CODRECURSO = '0000' OR LEN(LTRIM(RTRIM(CODRECURSO))) = 0) " +
                " AND CODSISTEMA != '120' ";

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

        private bool excluirOcorrencia183(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL += " DELETE FROM OCORRENCIAS " +
                " WHERE   DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND CODERRO = 183 ";

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

        public bool excluirOcorrencia164(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL += " DELETE FROM OCORRENCIAS WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " + " AND CODERRO IN (164, 137, 109) ";

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

        public bool tratarOcorrencias109(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL += " DELETE FROM OCORRENCIAS WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " + " AND CODERRO = 109 ";

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


        private bool excluirRegistrosInfoAdicionalOcorrencia183(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL = " IF OBJECT_ID('tempdb..#ContratosRenegociados') IS NOT NULL DROP TABLE #ContratosRenegociados " +
                " SELECT      DATBASE, CODCOLIGADA, NROCONTROLE, IDOPERACAO, SEQUENCIA, CODTIPCESSAO AS TPINFADD, NROCESSAO AS IPOCNOVO " +
                " INTO		#ContratosRenegociados " +
                " FROM INFADICIONAISOPE I " +
                " WHERE   I.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                " AND         CODTIPCESSAO = '0316' " +

                " DELETE  I " +
                " FROM		#ContratosRenegociados I INNER JOIN OPERACOES O WITH(NOLOCK) " +
                " ON O.DATBASE = I.DATBASE " +
                " AND O.CODCOLIGADA = I.CODCOLIGADA " +
                " AND O.NROCONTROLE = I.NROCONTROLE " +
                " AND O.IPOC = I.IPOCNOVO " +

                " DELETE I " +
                " FROM INFADICIONAISOPE I INNER JOIN #ContratosRenegociados CR " +
                                " ON  I.DATBASE = CR.DATBASE " +
                                " AND I.CODCOLIGADA = CR.CODCOLIGADA " +
                                " AND I.NROCONTROLE = CR.NROCONTROLE " +
                                " AND I.IDOPERACAO = CR.IDOPERACAO " +
                                    " WHERE   I.DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' " +
                                    " AND         CODTIPCESSAO = '0316' ";


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

        public bool tratarOcorrencia183(DateTime dataVigencia)
        {
            return (excluirOcorrencia183(dataVigencia) && (excluirRegistrosInfoAdicionalOcorrencia183(dataVigencia)));
        }

        public bool tratarOcorrencia116(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;
                comandoSQL += " DELETE FROM OCORRENCIAS WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' AND CODERRO = 116 ";
                comandoSQL += " UPDATE OPERACOES SET DATCONTRATO = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " WHERE DATBASE = '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";
                comandoSQL += " AND DATCONTRATO > '" + dataVigencia.ToString("yyyy-MM-dd") + "' ";

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

        public bool tratarOcorrencias109ContasCorrentes(DateTime dataVigencia)
        {
            bool sucesso = false;
            var conexao = new SqlConnection(this.conexaoBD);

            try
            {
                string comandoSQL = String.Empty;


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

    }
}
