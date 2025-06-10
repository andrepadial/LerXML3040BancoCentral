using Read3040Bacen;
using System.Configuration;
using System.Xml.Serialization;

namespace LerXML3040BancoCentral
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            processarArquivo();
        }

        private void processarArquivo()
        {
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName.Replace("openFileDialog1", "");

            if (path.Length != 0)
            {

                Doc3040 doc = new Doc3040();
                XmlSerializer serializer = new XmlSerializer(typeof(Doc3040));

                using (Stream reader = new FileStream(@path, FileMode.Open))
                {
                    doc = (Doc3040)serializer.Deserialize(reader);
                    tratarOperacoes(doc.clientes);
                }

                MessageBox.Show(doc.clientes.Count.ToString());
            }
        }

        private void tratarOperacoes(List<Cliente> lista)
        {
            foreach (var cliente in lista)
            {
                cliente.operacoes = Tratamento.tratarDiasEmAtraso(cliente.operacoes);
                cliente.operacoes = Tratamento.tratarEstagio(cliente.operacoes);
                cliente.operacoes = Tratamento.tratarCarteira(cliente.operacoes);
                cliente.operacoes = Tratamento.tratarClassificacaoAtivoFinanceiro(cliente.operacoes);
                cliente.operacoes = Tratamento.tratarPDEST(cliente.operacoes);
                cliente.operacoes = Tratamento.tratarRiscoCredito(cliente.operacoes);
                cliente.operacoes = Tratamento.tratarValores4966(cliente.operacoes);
                cliente.operacoes = Tratamento.tratarTJE4966(cliente.operacoes);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            processarArquivo();
        }
    }
}
