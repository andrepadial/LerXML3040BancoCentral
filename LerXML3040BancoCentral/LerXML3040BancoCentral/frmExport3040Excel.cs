using LerXML3040BancoCentral.Model;
using Microsoft.VisualBasic.ApplicationServices;
using Read3040Bacen;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using ClosedXML.Excel;

namespace LerXML3040BancoCentral
{
    public partial class frmExport3040Excel : Form
    {
        string infoLog = String.Empty;
        Doc3040 doc = new Doc3040();

        public frmExport3040Excel()
        {
            InitializeComponent();
        }

        private void processarArquivo()
        {

            opfArquivo3040.ShowDialog();
            string path = opfArquivo3040.FileName.Replace("opfArquivo3040", "");


            if (path.Length != 0)
            {

                infoLog = String.Concat("Login - ", Environment.UserName, Environment.NewLine,
                                            "Rotina - Exportar Arquivo -> 3040 p/ Excel - ", path, Environment.NewLine,
                                            "Data/Hora - ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                                        );


                XmlSerializer serializer = new XmlSerializer(typeof(Doc3040));

                using (Stream reader = new FileStream(@path, FileMode.Open))
                {
                    this.doc = (Doc3040)serializer.Deserialize(reader);
                }

                gerarArquivoNovo(path, doc);
                Util.LogGenerator.WriteLog(infoLog);
                MessageBox.Show("Processo finalizado com sucesso");
            }
        }



        private void gerarArquivoNovo(string xml, Doc3040 docOficial)
        {

            svfArquivo3040.ShowDialog();
            string path = String.Empty;
            List<ExportExcel> operacoes = new List<ExportExcel>();


            if (svfArquivo3040.FileName.Contains(".xls") || svfArquivo3040.FileName.Contains(".xlsx"))
            {

                path = svfArquivo3040.FileName;
                var clientes = docOficial.clientes;

                foreach (var cliente in clientes)
                {

                    string cpfCnpj = cliente.Cd;
                    string tipoCliente = cliente.Tp;
                    int contador = 1;
                    decimal totalVencimentos = 0;


                    foreach (var op in cliente.operacoes)
                    {
                        if (Convert.ToInt32(cliente.Tp) == 2 || Convert.ToInt32(cliente.Tp) == 4 || Convert.ToInt32(cliente.Tp) == 6)
                        {
                            cpfCnpj = op.DetCli;
                        }


                        string numeroContrato = op.Contrt;
                        string ipoc = op.IPOC;
                        string codModalidade = op.Mod;
                        string provisao = String.Empty;


                        if (op.ProvConsttd == null)
                        {
                            provisao = "0.00";
                        }
                        else if (op.ProvConsttd.StartsWith("."))
                        {
                            provisao = "0" + op.ProvConsttd;
                        }
                        else
                        {
                            provisao = op.ProvConsttd;
                        }

                        string diasEmAtraso = op.DiaAtraso == null ? "0" : op.DiaAtraso;
                        string operacaoSaida = Tratamento.retornarOperacaoSaida(op) ? "S" : "N";
                        string operacaoPrejuizo = Tratamento.operacaoEmPrejuizo(op) ? "S" : "N";
                        totalVencimentos = Tratamento.retornarSomaVencimentos(op);
                        string saldoContabil = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        string carteira = op.contabilizacao4966.CartProvMin;
                        string estagio = op.contabilizacao4966.EstInstFin;
                        string valorContBR = op.contabilizacao4966.VlrContBr;
                        string ativoProblematico = String.Empty;

                        if (op.CaracEspecial != null)
                        {
                            ativoProblematico = op.CaracEspecial.Contains("19") ? "S" : "N";
                        }

                        operacoes.Add(new ExportExcel
                        {
                            CpfCnpjRaiz = cpfCnpj,
                            TipoCliente = tipoCliente,
                            Ipoc = ipoc,
                            NumeroContrato = numeroContrato,
                            Modalidade = codModalidade,
                            ValorContabil = saldoContabil,
                            ValorContBR = saldoContabil,
                            Provisao = provisao,
                            DiasEmAtraso = diasEmAtraso,
                            Prejuizo = operacaoPrejuizo,
                            SaidaOperacao = operacaoSaida,
                            Carteira = carteira,
                            Estagio = estagio,
                            AtivoProblematico = ativoProblematico
                        });

                    }

                }

                gerarExcel(operacoes, path);

            }

        }



        private void gerarExcel(List<ExportExcel> operacoes, string path)
        {

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("3040");                    
                    worksheet.Cell("A1").Value = "CpfCnpjCliente";
                    worksheet.Cell("B1").Value = "TipoCliente";
                    worksheet.Cell("C1").Value = "Ipoc";
                    worksheet.Cell("D1").Value = "NumeroContrato";
                    worksheet.Cell("E1").Value = "Modalidade";
                    worksheet.Cell("F1").Value = "ValorContabil";
                    worksheet.Cell("G1").Value = "ValorContBR";
                    worksheet.Cell("H1").Value = "Provisao";
                    worksheet.Cell("I1").Value = "DiasEmAtraso";
                    worksheet.Cell("J1").Value = "Prejuizo";
                    worksheet.Cell("K1").Value = "SaidaOperacao";
                    worksheet.Cell("L1").Value = "Carteira";
                    worksheet.Cell("M1").Value = "Estagio";
                    worksheet.Cell("N1").Value = "AtivoProblematico";
                    worksheet.Cell(2, 1).InsertData(operacoes);
                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(path);
                }

                MessageBox.Show("Excel gerado com sucesso.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar Excel: " + ex.Message, "Erro na geração do Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            processarArquivo();
            this.Cursor = Cursors.Default;
        }
    }
}
