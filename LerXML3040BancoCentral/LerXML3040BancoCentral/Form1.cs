using Microsoft.VisualBasic.ApplicationServices;
using Read3040Bacen;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace LerXML3040BancoCentral
{
    public partial class Form1 : Form
    {
        string infoLog = String.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            processarArquivo();
            //incluirSistema110();
            this.Cursor = Cursors.Default;
        }


        private void incluirSistema110()
        {
            Doc3040 doc = new Doc3040();
            Doc3040 doc110 = new Doc3040();
            int contador = 1;

            XmlSerializer serializer = new XmlSerializer(typeof(Doc3040));
            Stream reader110 = new FileStream(@"C:\temp\3040\110.xml", FileMode.Open);
            doc110 = (Doc3040)serializer.Deserialize(reader110);


            using (Stream reader = new FileStream(@"C:\temp\3040\Oficial.xml", FileMode.Open))
            {
                doc = (Doc3040)serializer.Deserialize(reader);

                foreach (var cliente in doc.clientes)
                {
                    //var result = operacoesAtivoParaWO.Where(o => o.NumeroContrato == op.Contrt).FirstOrDefault();
                    var result = doc.clientes.Where(c => c.Cd == cliente.Cd).ToList();

                    if (result.Count == 0 || result == null)
                    {
                        doc.clientes.Add(cliente);
                    }
                    else
                    {
                        int index = doc.clientes.FindIndex(c => c.Cd == cliente.Cd);
                        doc.clientes[index].operacoes.AddRange(cliente.operacoes);
                    }

                    contador += 1;

                }

                gerarArquivoNovo(@"C:\temp\3040\Oficial.xml", doc);


            }
        }

        private void processarArquivo()
        {
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName.Replace("openFileDialog1", "");

            if (path.Length != 0)
            {
                infoLog = String.Concat("Login - ", Environment.UserName, Environment.NewLine,
                                            "Rotina - B9 Arquivo -> Tratamento xml - ", path, Environment.NewLine,
                                            "Data/Hora - ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                                        );

                Doc3040 doc = new Doc3040();
                XmlSerializer serializer = new XmlSerializer(typeof(Doc3040));

                using (Stream reader = new FileStream(@path, FileMode.Open))
                {
                    doc = (Doc3040)serializer.Deserialize(reader);
                    tratarClientes(doc.clientes, doc.CNPJ);
                    tratarOperacoesV2(doc.clientes, doc.DtBase, doc.CNPJ);

                }

                gerarArquivoNovo(path, doc);
                //Util.LogGenerator.WriteLog(infoLog);
                MessageBox.Show("Processo finalizado com sucesso");
            }
        }

        private void tratarClientes(List<Cliente> clientes, string cnpjEmpresa)
        {
            clientes = Tratamento.tratarCdClienteV2(clientes);
            clientes = Tratamento.tratarTipoClienteNuloVazio(clientes);
            clientes = Tratamento.tratarTipoCliente(clientes);
            clientes = Tratamento.tratarAutorizacaoConsultaBacen(clientes);
            clientes = Tratamento.tratarTipoControlePessoaJuridica(clientes);
            clientes = Tratamento.tratarInicioRelacionamento(clientes);
            clientes = Tratamento.tratarFaturamento(clientes);
            clientes = Tratamento.tratarPorteCliente(clientes);
            clientes = Tratamento.tratarIPOC(clientes, cnpjEmpresa);
        }

        private void tratarOperacoesV2(List<Cliente> lista, string vigencia, string cnpjEmpresa)
        {
            Cliente cli = new Cliente();
            int contador = 1;
            int ano = Convert.ToInt32(vigencia.Substring(0, 3));
            int mes = Convert.ToInt32(vigencia.Substring(5, 2));
            DateTime dataVigencia = new DateTime(ano, mes, 1);
            dataVigencia.AddMonths(1).AddDays(-1);

            foreach (var cliente in lista.ToList())
            {
                cli = cliente;

                try
                {

                    if (Tratamento.tratarOperacoesCNPJBanco(cliente))
                    {
                        lista.Remove(cliente);
                    }
                    else
                    {
                        cliente.operacoes = Tratamento.tratarOperacoesIpocsDuplicados(cliente);
                        cliente.operacoes = Tratamento.tratarGarantiasDuplicadas(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarCamposGarantias(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarInformacoesAdicionaisDuplicadas(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarOperacoesSemVencimentosComoSaida(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarEstagioV3(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarCarteiraV3(cliente.operacoes);

                        cliente.operacoes = Tratamento.tratarClassificacaoAtivoFinanceiro(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarPDEST(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarRiscoCredito(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarTJE4966(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarLocalidade(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarIndexador(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarCodigoClienteOperacao(cliente.Cd, cliente.operacoes, cliente.Tp);
                        cliente.operacoes = Tratamento.tratarDataVencimentoParcela(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarDataInicioContrato(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarModalidade08(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarCamposOperacaoSaida(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarNatureza02(cliente.operacoes, cnpjEmpresa);
                        cliente.operacoes = Tratamento.tratarOperacoesInfoAdicional1001(cliente.operacoes, cnpjEmpresa);
                        cliente.operacoes = Tratamento.sumprimirSicorOperacoesNaoRural(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarOperacoesModalidade09(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarOperacoesModalidades0401E1206(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarQtdInstrumentoModalidade18(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarOperacoesRequeremInfoAdicional1201(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarInfoAdicionalGrupoModalidade04(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarFluxoVencimentoOperacoesBaixadas(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarCasosPEAC(cliente.operacoes, cliente.Tp, cliente.Cd);
                        cliente.operacoes = Tratamento.tratarFluxoVencimentoModalidade15(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarAtivoProblematicoEstagio12(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarCaracEspecialAtivoProblematicoEstagio3(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarOperacaoFidc122025(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarFluxoVencimentos(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarOperacoesVencimento310E320(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarOperacoesVencimento330(cliente.operacoes);
                        cliente.operacoes = Tratamento.tratarOperacoesInfoAdicional1701V2(cliente.operacoes);

                        cliente.operacoes = Tratamento.tratarModalidade1899(cliente.operacoes);

                        cliente.operacoes = Tratamento.tratarCaracteristicaEspecialForaDominioBacen(cliente.operacoes);

                        //Incluindo o tratamento para motivo de alocação do estágio                                                
                        if (checkBox1.Checked)
                            cliente.operacoes = Tratamento.suprimirMotivoAlocacaoEstagio(cliente.operacoes);
                        else
                            cliente.operacoes = Tratamento.tratarMotivoAlocacaoEstagio(cliente.operacoes);

                        //Tratar valor de provisão para operações em prejuízo, suprimir a informação
                        cliente.operacoes = Tratamento.tratarValorProvisaoOperacoesPrejuizo(cliente.operacoes);

                        //Tratar reconhecimento de perda, campo valor
                        cliente.operacoes = Tratamento.tratarPerdasInstrumentoContabilizacao4966(cliente.operacoes);

                        //Tratar operações 0499
                        cliente.operacoes = Tratamento.excluirOperacoes0499(cliente.operacoes);

                        cliente.operacoes = Tratamento.tratarValores4966(cliente.operacoes);

                        //Tratar motivo de alocação de estágio duplicado
                        cliente.operacoes = Tratamento.tratarMotivoAlocacaoEstagioDuplicados(cliente.operacoes);
                    }

                    //Verificando se o cliente tem operações, se não tiver, remove ele
                    if (cliente.operacoes == null || cliente.operacoes.Count == 0)
                    {
                        lista.Remove(cliente);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro cliente:" + cliente.Cd + ex.Message.ToString());
                }

                contador += 1;
            }
        }

        private void tratarOperacoes(List<Cliente> lista, string vigencia, string cnpjEmpresa)
        {
            Cliente cli = new Cliente();
            int contador = 1;
            int ano = Convert.ToInt32(vigencia.Substring(0, 3));
            int mes = Convert.ToInt32(vigencia.Substring(5, 2));
            DateTime dataVigencia = new DateTime(ano, mes, 1);
            dataVigencia.AddMonths(1).AddDays(-1);

            foreach (var cliente in lista.ToList())
            {
                cli = cliente;

                try
                {

                    if (cliente.operacoes.Count > 0)
                    {
                        if (Tratamento.tratarOperacoesCNPJBanco(cliente))
                        {
                            lista.Remove(cliente);
                        }
                        else
                        {
                            cliente.operacoes = Tratamento.tratarOperacoesIpocsDuplicados(cliente);
                            cliente.operacoes = Tratamento.tratarGarantiasDuplicadas(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarCamposGarantias(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarInformacoesAdicionaisDuplicadas(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarEstagioV2(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarClassificacaoAtivoFinanceiro(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarPDEST(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarRiscoCredito(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarValores4966(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarTJE4966(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarLocalidade(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarIndexador(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarCodigoClienteOperacao(cliente.Cd, cliente.operacoes, cliente.Tp);
                            cliente.operacoes = Tratamento.tratarDataVencimentoParcela(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarDataInicioContrato(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarProvisaoConstituida(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarModalidade08(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarCamposOperacaoSaida(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarNatureza02(cliente.operacoes, cnpjEmpresa);
                            cliente.operacoes = Tratamento.tratarOperacoesInfoAdicional1001(cliente.operacoes, cnpjEmpresa);
                            cliente.operacoes = Tratamento.sumprimirSicorOperacoesNaoRural(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarOperacoesModalidade09(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarOperacoesModalidades0401E1206(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarQtdInstrumentoModalidade18(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarOperacoesRequeremInfoAdicional1201(cliente.operacoes);

                            //Verificar código
                            cliente.operacoes = Tratamento.tratarInfoAdicionalGrupoModalidade04(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarFluxoVencimentoOperacoesBaixadas(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarCasosPEAC(cliente.operacoes, cliente.Tp, cliente.Cd);
                            cliente.operacoes = Tratamento.tratarFluxoVencimentoModalidade15(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarAtivoProblematicoEstagio12(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarCaracEspecialAtivoProblematicoEstagio3(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarOperacoesVencimento310E320(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarOperacoesVencimento330(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarOperacoesInfoAdicional1701V2(cliente.operacoes);
                            cliente.operacoes = Tratamento.tratarCaracteristicaEspecialForaDominioBacen(cliente.operacoes);

                            //cliente.operacoes = Tratamento.tratarIndiciosBacen(cliente.operacoes);
                        }
                    }
                    else
                    {
                        lista.Remove(cliente);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro cliente:" + cliente.Cd);
                }

                contador += 1;
            }
        }

        private void gerarArquivoNovo(string xml, Doc3040 docOficial)
        {
            string path = xml.Replace(".xml", "").Replace(".XML", "");
            path = String.Concat(path, "_", DateTime.Now.ToString("ddMMyyyyHHmmss"), ".xml");

            using (var writer = new FileStream(@path, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Doc3040));
                serializer.Serialize(writer, docOficial);
            }

            //MessageBox.Show("Processo finalizado com sucesso.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
