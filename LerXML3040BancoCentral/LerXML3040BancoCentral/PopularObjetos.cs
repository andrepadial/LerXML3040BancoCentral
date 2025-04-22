using LerXML3040BancoCentral.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral
{
    public class PopularObjetos
    {
        List<Estagio> estagios = new List<Estagio>();
        List<MotivoAlocacaoEstagio> motivoAlocacaoEstagio = new List<MotivoAlocacaoEstagio>();
        public PopularObjetos()
        {

        }

        public static List<Vencto> popularVencimentos()
        {
            List<Vencto> vencimentos = new List<Vencto>();

            vencimentos.Add(new Vencto { Codigo = "20", Descricao = "Limite de crédito com vencimento até 360 dias " });
            vencimentos.Add(new Vencto { Codigo = "40", Descricao = "Limite de crédito com vencimento acima de 360 dias " });
            vencimentos.Add(new Vencto { Codigo = "60", Descricao = "Créditos a liberar até 360 dias " });
            vencimentos.Add(new Vencto { Codigo = "80", Descricao = "Créditos a liberar acima de 360 dias " });
            vencimentos.Add(new Vencto { Codigo = "110", Descricao = "Créditos a vencer até 30 dias" });
            vencimentos.Add(new Vencto { Codigo = "120", Descricao = "Créditos a vencer de 31 a 60 dias" });
            vencimentos.Add(new Vencto { Codigo = "130", Descricao = "Créditos a vencer de 61 a 90 dias" });
            vencimentos.Add(new Vencto { Codigo = "140", Descricao = "Créditos a vencer de 91 a 180 dias" });
            vencimentos.Add(new Vencto { Codigo = "150", Descricao = "Créditos a vencer de 181 a 360 dias " });
            vencimentos.Add(new Vencto { Codigo = "160", Descricao = "Créditos a vencer de 361 a 720 dias" });
            vencimentos.Add(new Vencto { Codigo = "165", Descricao = "Créditos a vencer de 721 a 1080 dias" });
            vencimentos.Add(new Vencto { Codigo = "170", Descricao = "Créditos a vencer de 1081 a 1440 dias" });
            vencimentos.Add(new Vencto { Codigo = "175", Descricao = "Créditos a vencer de 1441 a 1800 dias" });
            vencimentos.Add(new Vencto { Codigo = "180", Descricao = "Créditos a vencer de 1801 a 5400 dias" });
            vencimentos.Add(new Vencto { Codigo = "190", Descricao = "Créditos a vencer acima de 5400 dias" });
            vencimentos.Add(new Vencto { Codigo = "199", Descricao = "Créditos a vencer com prazo indeterminado" });
            vencimentos.Add(new Vencto { Codigo = "205", Descricao = "Créditos vencidos de 1 a 14 dias" });
            vencimentos.Add(new Vencto { Codigo = "210", Descricao = "Créditos vencidos de 15 a 30 dias" });
            vencimentos.Add(new Vencto { Codigo = "220", Descricao = "Créditos vencidos de 31 a 60 dias" });
            vencimentos.Add(new Vencto { Codigo = "230", Descricao = "Créditos vencidos de 61 a 90 dias" });
            vencimentos.Add(new Vencto { Codigo = "240", Descricao = "Créditos vencidos de 91 a 120 dias" });
            vencimentos.Add(new Vencto { Codigo = "245", Descricao = "Créditos vencidos de 121 a 150 dias" });
            vencimentos.Add(new Vencto { Codigo = "250", Descricao = "Créditos vencidos de 151 a 180 dias" });
            vencimentos.Add(new Vencto { Codigo = "255", Descricao = "Créditos vencidos de 181 a 240 dias" });
            vencimentos.Add(new Vencto { Codigo = "260", Descricao = "Créditos vencidos de 241 a 300 dias" });
            vencimentos.Add(new Vencto { Codigo = "270", Descricao = "Créditos vencidos de 301 a 360 dias" });
            vencimentos.Add(new Vencto { Codigo = "280", Descricao = "Créditos vencidos de 361 a 540 dias" });
            vencimentos.Add(new Vencto { Codigo = "290", Descricao = "Créditos vencidos acima de 540 dias" });
            vencimentos.Add(new Vencto { Codigo = "310", Descricao = "Créditos baixados como prejuízo até 12 meses" });
            vencimentos.Add(new Vencto { Codigo = "320", Descricao = "Créditos baixados como prejuízo há mais de 12m e até 48 meses" });
            vencimentos.Add(new Vencto { Codigo = "330", Descricao = "Créditos baixados como prejuízo há mais de 48 meses" });


            return vencimentos;
        }

        public static List<Natureza> popularNaturezas()
        {
            List<Natureza> naturezas = new List<Natureza>();

            naturezas.Add(new Natureza { Codigo = "01", Descricao = "Operações concedidas pela própria instituição" });
            naturezas.Add(new Natureza { Codigo = "02", Descricao = "Operações adquiridas em negociação com pessoa integrante do SFN sem retenção substancial  de  risco e de benefícios ou de controle pelo  interveniente ou cedente" });
            naturezas.Add(new Natureza { Codigo = "03", Descricao = "Operações adquiridas em negociação com pessoa não integrante do SFN sem retenção substancial de risco e de benefícios ou de controle pelo interveniente ou cedente" });
            naturezas.Add(new Natureza { Codigo = "04", Descricao = "Operações adquiridas em negociação com pessoa integrante do SFN com retenção substancial de risco e de benefícios ou de controle pelo interveniente ou cedente" });
            naturezas.Add(new Natureza { Codigo = "05", Descricao = "Operações adquiridas em negociação com pessoa não integrante do SFN com retenção substancial de risco e de benefícios ou de controle pelo interveniente ou cedente (utilização exclusiva por FIDCs)" });
            naturezas.Add(new Natureza { Codigo = "11", Descricao = "Operações transferidas a pessoa integrante do SFN em negociação com retenção substancial  de risco e de benefícios ou de controle pelo cedente" });
            naturezas.Add(new Natureza { Codigo = "12", Descricao = "Operações transferidas a pessoa não integrante do SFN e controlada, em negociação sem retenção substancial  de risco e de benefícios ou de controle pelo cedente" });
            naturezas.Add(new Natureza { Codigo = "13", Descricao = "Operações transferidas a pessoa não integrante do SFN e controlada, em negociação com retenção substancial  de risco e de benefícios ou de controle pelo cedente" });
            naturezas.Add(new Natureza { Codigo = "14", Descricao = "Operações transferidas a pessoa não integrante do SFN e não controlada, em negociação com retenção substancial  de risco e de benefícios ou de controle pelo cedente" });
            naturezas.Add(new Natureza { Codigo = "15", Descricao = "Operações transferidas a fundo de investimento com retenção substancial de riscos e benefícios" });
            naturezas.Add(new Natureza { Codigo = "16", Descricao = "Operações transferidas a fundo de investimento administrado pela instituição financeira, sem retenção substancial de riscos e benefícios ou de controle" });
            naturezas.Add(new Natureza { Codigo = "32", Descricao = "Operações realizadas por dependências e empresas localizadas no exterior que  tenham  suas demonstrações consolidadas (NR) " });
            naturezas.Add(new Natureza { Codigo = "33", Descricao = "Operações realizadas por empresas no Brasil pertencentes ao mesmo consolidado prudencial (NR)" });
            naturezas.Add(new Natureza { Codigo = "34", Descricao = "Operações de crédito de programas ou fundos públicos" });


            return naturezas;
        }

        public static List<ModalidadeSCR> popularModalidades()
        {
            List<ModalidadeSCR> modalidades = new List<ModalidadeSCR>();

            modalidades.Add(new ModalidadeSCR { Codigo = "0101", Descricao = "Adiantamentos a depositantes" });

            modalidades.Add(new ModalidadeSCR { Codigo = "0202", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0203", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0204", Descricao = "Empréstimos" });            
            modalidades.Add(new ModalidadeSCR { Codigo = "0207", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0208", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0209", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0210", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0211", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0212", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0213", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0214", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0215", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0216", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0217", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0218", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0250", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0290", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0299", Descricao = "Empréstimos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0301", Descricao = "Direitos creditórios descontados" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0302", Descricao = "Direitos creditórios descontados" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0303", Descricao = "Direitos creditórios descontados" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0398", Descricao = "Direitos creditórios descontados" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0399", Descricao = "Direitos creditórios descontados" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0401", Descricao = "Financiamentos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0402", Descricao = "Financiamentos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0403", Descricao = "Financiamentos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0404", Descricao = "Financiamentos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0405", Descricao = "Financiamentos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0406", Descricao = "Financiamentos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0440", Descricao = "Financiamentos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0450", Descricao = "Financiamentos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0490", Descricao = "Financiamentos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0499", Descricao = "Financiamentos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0501", Descricao = "Financiamentos à exportação" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0502", Descricao = "Financiamentos à exportação" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0503", Descricao = "Financiamentos à exportação" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0504", Descricao = "Financiamentos à exportação" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0590", Descricao = "Financiamentos à exportação" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0599", Descricao = "Financiamentos à exportação" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0601", Descricao = "Financiamentos à importação" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0690", Descricao = "Financiamentos à importação" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0701", Descricao = "Financiamentos com interveniência" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0702", Descricao = "Financiamentos com interveniência" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0790", Descricao = "Financiamentos com interveniência" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0799", Descricao = "Financiamentos com interveniência" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0801", Descricao = "Financiamentos rurais" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0802", Descricao = "Financiamentos rurais" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0803", Descricao = "Financiamentos rurais" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0804", Descricao = "Financiamentos rurais" });

            modalidades.Add(new ModalidadeSCR { Codigo = "0901", Descricao = "Financiamentos imobiliários" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0902", Descricao = "Financiamentos imobiliários" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0903", Descricao = "Financiamentos imobiliários" });
            modalidades.Add(new ModalidadeSCR { Codigo = "0990", Descricao = "Financiamentos imobiliários" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1001", Descricao = "Financiamentos de títulos e valores mobiliários" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1101", Descricao = "Financiamentos de infraestrutura e desenvolvimento" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1190", Descricao = "Financiamentos de infraestrutura e desenvolvimento" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1201", Descricao = "Operações de arrendamento" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1202", Descricao = "Operações de arrendamento" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1203", Descricao = "Operações de arrendamento" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1205", Descricao = "Operações de arrendamento" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1206", Descricao = "Operações de arrendamento" });

            modalidades.Add(new ModalidadeSCR { Codigo = "1290", Descricao = "Operações de arrendamento" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1301", Descricao = "Outros créditos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1302", Descricao = "Outros créditos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1303", Descricao = "Outros créditos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1304", Descricao = "Outros créditos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1350", Descricao = "Outros créditos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1390", Descricao = "Outros créditos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1399", Descricao = "Outros créditos" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1401", Descricao = "Relações Interfinanceiras" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1402", Descricao = "Relações Interfinanceiras" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1403", Descricao = "Relações Interfinanceiras" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1501", Descricao = "Coobrigações" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1502", Descricao = "Coobrigações" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1503", Descricao = "Coobrigações" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1504", Descricao = "Coobrigações" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1505", Descricao = "Coobrigações" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1511", Descricao = "Coobrigações" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1512", Descricao = "Coobrigações" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1513", Descricao = "Coobrigações" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1590", Descricao = "Coobrigações" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1599", Descricao = "Coobrigações" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1801", Descricao = "Títulos de crédito (fora da carteira classificada)" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1802", Descricao = "Títulos de crédito (fora da carteira classificada)" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1803", Descricao = "Títulos de crédito (fora da carteira classificada)" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1804", Descricao = "Títulos de crédito (fora da carteira classificada)" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1899", Descricao = "Títulos de crédito (fora da carteira classificada)" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1901", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1902", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1903", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1904", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1905", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1906", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1907", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1908", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1909", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1910", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "1999", Descricao = "Limite" });
            modalidades.Add(new ModalidadeSCR { Codigo = "2001", Descricao = "Retenção de risco  " });
            modalidades.Add(new ModalidadeSCR { Codigo = "2002", Descricao = "Retenção de risco  " });

            return modalidades;
        }

        public static List<OrigemRecurso> popularOrigemRecurso()
        {
            List<OrigemRecurso> origemRecursos = new List<OrigemRecurso>();

            origemRecursos.Add(new OrigemRecurso { Codigo = "0101", Descricao = "Recursos livres" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0102", Descricao = "Recursos livres" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0199", Descricao = "Recursos livres" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0201", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0202", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0203", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0204", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0205", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0206", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0207", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0208", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0209", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0210", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0211", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0212", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0213", Descricao = "Recursos direcionados" });
            origemRecursos.Add(new OrigemRecurso { Codigo = "0299", Descricao = "Recursos direcionados" });




            return origemRecursos;
        }

        public static List<Indexador> popularIndexadores()
        {
            List<Indexador> indexadores = new List<Indexador>();

            indexadores.Add(new Indexador { Codigo = "11", Descricao = "Prefixado" });
            indexadores.Add(new Indexador { Codigo = "21", Descricao = "Pós-fixado" });
            indexadores.Add(new Indexador { Codigo = "22", Descricao = "Pós-fixado" });
            indexadores.Add(new Indexador { Codigo = "23", Descricao = "Pós-fixado" });
            indexadores.Add(new Indexador { Codigo = "24", Descricao = "Pós-fixado" });
            indexadores.Add(new Indexador { Codigo = "25", Descricao = "Pós-fixado" });
            indexadores.Add(new Indexador { Codigo = "29", Descricao = "Pós-fixado" });
            indexadores.Add(new Indexador { Codigo = "31", Descricao = "Flutuantes" });
            indexadores.Add(new Indexador { Codigo = "32", Descricao = "Flutuantes" });
            indexadores.Add(new Indexador { Codigo = "39", Descricao = "Flutuantes" });
            indexadores.Add(new Indexador { Codigo = "41", Descricao = "Índices de preços" });
            indexadores.Add(new Indexador { Codigo = "42", Descricao = "Índices de preços" });
            indexadores.Add(new Indexador { Codigo = "43", Descricao = "Índices de preços" });
            indexadores.Add(new Indexador { Codigo = "49", Descricao = "Índices de preços" });
            indexadores.Add(new Indexador { Codigo = "51", Descricao = "Crédito rural" });
            indexadores.Add(new Indexador { Codigo = "52", Descricao = "Crédito rural" });
            indexadores.Add(new Indexador { Codigo = "53", Descricao = "Crédito rural" });
            indexadores.Add(new Indexador { Codigo = "54", Descricao = "Crédito rural" });
            indexadores.Add(new Indexador { Codigo = "91", Descricao = "Outros indexadores" });
            indexadores.Add(new Indexador { Codigo = "99", Descricao = "Outros indexadores" });


            return indexadores;
        }

        public static List<Moeda> popularVariacoesCambiais()
        {
            List<Moeda> moedas = new List<Moeda>();

            moedas.Add(new Moeda { Codigo = "10012", Descricao = "AC - Acre" });
            moedas.Add(new Moeda { Codigo = "10036", Descricao = "AL - Alagoas" });
            moedas.Add(new Moeda { Codigo = "10013", Descricao = "AM - Amazonas" });
            moedas.Add(new Moeda { Codigo = "10014", Descricao = "AP - Amapá" });
            moedas.Add(new Moeda { Codigo = "10039", Descricao = "BA - Bahia" });
            moedas.Add(new Moeda { Codigo = "10032", Descricao = "CE - Ceará" });
            moedas.Add(new Moeda { Codigo = "10096", Descricao = "DF - Distrito Federal" });
            moedas.Add(new Moeda { Codigo = "10052", Descricao = "ES - Espírito Santo" });
            moedas.Add(new Moeda { Codigo = "10092", Descricao = "GO - Goiás" });


            return moedas;
        }

        public static List<Localidade> popularLocalidades()
        {
            List<Localidade> localidades = new List<Localidade>();

            localidades.Add(new Localidade { Codigo = "10012", Descricao = "AC - Acre" });
            localidades.Add(new Localidade { Codigo = "10036", Descricao = "AL - Alagoas" });
            localidades.Add(new Localidade { Codigo = "10013", Descricao = "AM - Amazonas" });
            localidades.Add(new Localidade { Codigo = "10014", Descricao = "AP - Amapá" });
            localidades.Add(new Localidade { Codigo = "10039", Descricao = "BA - Bahia" });
            localidades.Add(new Localidade { Codigo = "10032", Descricao = "CE - Ceará" });
            localidades.Add(new Localidade { Codigo = "10096", Descricao = "DF - Distrito Federal" });
            localidades.Add(new Localidade { Codigo = "10052", Descricao = "ES - Espírito Santo" });
            localidades.Add(new Localidade { Codigo = "10092", Descricao = "GO - Goiás" });
            localidades.Add(new Localidade { Codigo = "10030", Descricao = "MA - Maranhão" });
            localidades.Add(new Localidade { Codigo = "10050", Descricao = "MG - Minas Gerais" });
            localidades.Add(new Localidade { Codigo = "10091", Descricao = "MS - Mato Grosso do Sul" });
            localidades.Add(new Localidade { Codigo = "10090", Descricao = "MT - Mato Grosso" });
            localidades.Add(new Localidade { Codigo = "10017", Descricao = "PA - Pará" });
            localidades.Add(new Localidade { Codigo = "10034", Descricao = "PB - Paraíba" });
            localidades.Add(new Localidade { Codigo = "10035", Descricao = "PE - Pernambuco" });
            localidades.Add(new Localidade { Codigo = "10031", Descricao = "PI - Piauí" });
            localidades.Add(new Localidade { Codigo = "10073", Descricao = "PR - Paraná" });
            localidades.Add(new Localidade { Codigo = "10054", Descricao = "RJ - Rio de Janeiro" });
            localidades.Add(new Localidade { Codigo = "10033", Descricao = "RN - Rio Grande do Norte" });
            localidades.Add(new Localidade { Codigo = "10093", Descricao = "RO - Rondônia" });
            localidades.Add(new Localidade { Codigo = "10018", Descricao = "RR - Roraima" });
            localidades.Add(new Localidade { Codigo = "10077", Descricao = "RS - Rio Grande do Sul" });
            localidades.Add(new Localidade { Codigo = "10075", Descricao = "SC - Santa Catarina" });
            localidades.Add(new Localidade { Codigo = "10038", Descricao = "SE - Sergipe" });
            localidades.Add(new Localidade { Codigo = "10058", Descricao = "SP - São Paulo" });
            localidades.Add(new Localidade { Codigo = "10094", Descricao = "TO - Tocantins" });

            return localidades;
        }

        public static List<CaracteristicaEspecial> popularCaracteristicasEspeciais()
        {
            List<CaracteristicaEspecial> caracteristicaEspeciais = new List<CaracteristicaEspecial>();

            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "01", Descricao = "renegociação" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "02", Descricao = "recuperação do prejuízo" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "03", Descricao = "renegociação nos termos da Res. 2471 (Pesa)" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "04", Descricao = "renegociação nos termos da Recoop" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "05", Descricao = "dívida considerada não vencível por força de norma" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "06", Descricao = "dívida com data de vencimento postergada por força de norma" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "07", Descricao = "pagamento de operação deferido por órgão ou programa oficial aguardando liberação dos recursos" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "09", Descricao = "cobrança judicial" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "10", Descricao = "operação vinculada" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "11", Descricao = "operações em inadimplemento por prazo igual ou superior a 60 meses, na data-base ou operações com vencimentos baixados como prejuízo há mais de 48 meses " });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "12", Descricao = "operação portada (advinda segundo Resolução nº 3.401) " });

            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "14", Descricao = "operações concedidas com destaque de capital segundo a Resolução nº 4.589" });

            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "16", Descricao = "operação alienada ao FGC conforme Resolução 4.115 de 26 de julho de 2012" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "17", Descricao = "taxa regulada" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "18", Descricao = "financiamento do saldo remanescente do crédito rotativo, conforme art. 2º da Resolução 4.549." });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "19", Descricao = "ativo problemático" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "20", Descricao = "operação com parte relacionada" });

            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "22", Descricao = "Operação contratada e negociada com transferência substancial de riscos e benefícios na mesma data-base" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "23", Descricao = "Renegociação Covid-19  (Resolução 4.803/2020)" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "24", Descricao = "Operações amparadas pelo PESE - Programa Emergencial de Suporte a Empregos (Res. 4.846/2020)" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "25", Descricao = "Limite não cancelável unilateralmente" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "35", Descricao = "operações cedidas nos termos da Resolução 3.533/08." });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "36", Descricao = "Operações com garantias em alienação fiduciária compartilhada (Resolução 4.837/2020)" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "37", Descricao = "Operações de Adiantamento a Depositantes com origem em Conta Garantida" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "38", Descricao = "Operação de crédito rural isenta da tag Sicor " });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "39", Descricao = "Títulos com característica de concessão de crédito (NR)" });
            caracteristicaEspeciais.Add(new CaracteristicaEspecial { Codigo = "99", Descricao = "outras características especiais (domínios 03 a 10, 12 e 14) " });


            return caracteristicaEspeciais;
        }

        public static List<TipoControle> popularTiposControles()
        {
            List<TipoControle> tiposControles = new List<TipoControle>();

            tiposControles.Add(new TipoControle { Codigo = "01", Descricao = "privado" });
            tiposControles.Add(new TipoControle { Codigo = "02", Descricao = "público federal" });
            tiposControles.Add(new TipoControle { Codigo = "03", Descricao = "público estadual ou distrital" });
            tiposControles.Add(new TipoControle { Codigo = "04", Descricao = "público municipal" });


            return tiposControles;
        }

        public static List<TipoPessoa> popularTiposPessoas()
        {
            List<TipoPessoa> tiposPessoas = new List<TipoPessoa>();

            tiposPessoas.Add(new TipoPessoa { Codigo = "1", Descricao = "pessoa física - CPF" });
            tiposPessoas.Add(new TipoPessoa { Codigo = "2", Descricao = "pessoa jurídica - CNPJ" });
            tiposPessoas.Add(new TipoPessoa { Codigo = "3", Descricao = "pessoa física no exterior" });
            tiposPessoas.Add(new TipoPessoa { Codigo = "4", Descricao = "pessoa jurídica no exterior" });
            tiposPessoas.Add(new TipoPessoa { Codigo = "5", Descricao = "pessoa física sem CPF" });
            tiposPessoas.Add(new TipoPessoa { Codigo = "6", Descricao = "pessoa jurídica sem CNPJ" });


            return tiposPessoas;
        }

        public static List<Garantia> popularGarantias()
        {
            List<Garantia> garantias = new List<Garantia>();

            garantias.Add(new Garantia { Codigo = "11", Descricao = "Cessão de direitos creditórios" });
            garantias.Add(new Garantia { Codigo = "12", Descricao = "Cessão de direitos creditórios" });
            garantias.Add(new Garantia { Codigo = "13", Descricao = "Cessão de direitos creditórios" });
            garantias.Add(new Garantia { Codigo = "14", Descricao = "Cessão de direitos creditórios" });
            garantias.Add(new Garantia { Codigo = "15", Descricao = "Cessão de direitos creditórios" });
            garantias.Add(new Garantia { Codigo = "16", Descricao = "Cessão de direitos creditórios" });
            garantias.Add(new Garantia { Codigo = "17", Descricao = "Cessão de direitos creditórios" });
            garantias.Add(new Garantia { Codigo = "18", Descricao = "Cessão de direitos creditórios" });
            garantias.Add(new Garantia { Codigo = "199", Descricao = "Cessão de direitos creditórios" });
            garantias.Add(new Garantia { Codigo = "21", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "22", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "23", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "24", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "25", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "26", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "27", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "28", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "29", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "210", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "299", Descricao = "Caução" });
            garantias.Add(new Garantia { Codigo = "321", Descricao = "Penhor" });
            garantias.Add(new Garantia { Codigo = "322", Descricao = "Penhor" });
            garantias.Add(new Garantia { Codigo = "323", Descricao = "Penhor" });
            garantias.Add(new Garantia { Codigo = "324", Descricao = "Penhor" });
            garantias.Add(new Garantia { Codigo = "325", Descricao = "Penhor" });
            garantias.Add(new Garantia { Codigo = "350", Descricao = "Penhor" });
            garantias.Add(new Garantia { Codigo = "399", Descricao = "Penhor" });
            garantias.Add(new Garantia { Codigo = "423", Descricao = "Alienação Fiduciária" });
            garantias.Add(new Garantia { Codigo = "424", Descricao = "Alienação Fiduciária" });
            garantias.Add(new Garantia { Codigo = "426", Descricao = "Alienação Fiduciária" });
            garantias.Add(new Garantia { Codigo = "427", Descricao = "Alienação Fiduciária" });
            garantias.Add(new Garantia { Codigo = "428", Descricao = "Alienação Fiduciária" });
            garantias.Add(new Garantia { Codigo = "499", Descricao = "Alienação Fiduciária" });
            garantias.Add(new Garantia { Codigo = "562", Descricao = "Hipoteca" });
            garantias.Add(new Garantia { Codigo = "563", Descricao = "Hipoteca" });
            garantias.Add(new Garantia { Codigo = "564", Descricao = "Hipoteca" });
            garantias.Add(new Garantia { Codigo = "565", Descricao = "Hipoteca" });
            garantias.Add(new Garantia { Codigo = "671", Descricao = "Operações garantidas pelo governo" });
            garantias.Add(new Garantia { Codigo = "672", Descricao = "Operações garantidas pelo governo" });
            garantias.Add(new Garantia { Codigo = "673", Descricao = "Operações garantidas pelo governo" });
            garantias.Add(new Garantia { Codigo = "674", Descricao = "Operações garantidas pelo governo" });
            garantias.Add(new Garantia { Codigo = "799", Descricao = "Outras garantias não fidejussórias" });
            garantias.Add(new Garantia { Codigo = "881", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "882", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "883", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "884", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "885", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "886", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "887", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "888", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "889", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "890", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "899", Descricao = "Seguros e assemelhados" });
            garantias.Add(new Garantia { Codigo = "91", Descricao = "Garantia fidejussória" });
            garantias.Add(new Garantia { Codigo = "92", Descricao = "Garantia fidejussória" });
            garantias.Add(new Garantia { Codigo = "93", Descricao = "Garantia fidejussória" });
            garantias.Add(new Garantia { Codigo = "94", Descricao = "Garantia fidejussória" });
            garantias.Add(new Garantia { Codigo = "101", Descricao = "Bens arrendados" });
            garantias.Add(new Garantia { Codigo = "102", Descricao = "Bens arrendados" });
            garantias.Add(new Garantia { Codigo = "111", Descricao = "Garantias internacionais" });
            garantias.Add(new Garantia { Codigo = "112", Descricao = "Garantias internacionais" });
            garantias.Add(new Garantia { Codigo = "121", Descricao = "Operações garantidas por outras entidades" });
            garantias.Add(new Garantia { Codigo = "122", Descricao = "Operações garantidas por outras entidades" });
            garantias.Add(new Garantia { Codigo = "123", Descricao = "Operações garantidas por outras entidades" });
            garantias.Add(new Garantia { Codigo = "124", Descricao = "Operações garantidas por outras entidades" });
            garantias.Add(new Garantia { Codigo = "131", Descricao = "Acordos de Compensação" });



            return garantias;
        }

        public static List<Estagio> popularEstagios()
        {
            List<Estagio> estagios = new List<Estagio>();
            List<MotivoAlocacaoEstagio> motivosEst1 = new List<MotivoAlocacaoEstagio>();
            List<MotivoAlocacaoEstagio> motivosEst2 = new List<MotivoAlocacaoEstagio>();
            List<MotivoAlocacaoEstagio> motivosEst3 = new List<MotivoAlocacaoEstagio>();

            motivosEst1.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "01",
                Descricao = "Classificação inicial"
            });

            motivosEst1.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "02",
                Descricao = "Redução do risco de crédito"
            });

            motivosEst1.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "03",
                Descricao = "Ativo que deixou de ser caracterizado como ativo com problema de recuperação de crédito"
            });

            estagios.Add(new Estagio()
            {
                Codigo = "1",
                Descricao = "Estágio 1",
                motivosAlocaoEstagio = motivosEst1,
            });

            motivosEst2.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "01",
                Descricao = "Aumento significativo de risco"
            });

            motivosEst2.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "02",
                Descricao = "Ativo que deixou de ser caracterizado como ativo com problema de recuperação de crédito"
            });

            estagios.Add(new Estagio()
            {
                Codigo = "2",
                Descricao = "Estágio 2",
                motivosAlocaoEstagio = motivosEst2,
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "01",
                Descricao = "Classificação inicial"
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "02",
                Descricao = "Atraso superior a 90 dias"
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "03",
                Descricao = "Reestruturação - Recuperação judicial ou extrajudicial "
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "04",
                Descricao = "Reestruturação – Outros"
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "05",
                Descricao = "Falência decretada"
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "06",
                Descricao = "Outra medida judicial que limite, atrase ou impeça o cumprimento das obrigações nas condições pactuadas"
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "07",
                Descricao = "Descumprimento de cláusula contratual relevante"
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "08",
                Descricao = " Avaliação direta ou indireta de incapacidade financeira para honra da obrigação nas condições pactuadas"
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "09",
                Descricao = "Instrumento negociado com desconto significativo"
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "10",
                Descricao = "Arrasto"
            });

            motivosEst3.Add(new MotivoAlocacaoEstagio
            {
                Codigo = "11",
                Descricao = "Em processo de cura"
            });

            estagios.Add(new Estagio()
            {
                Codigo = "3",
                Descricao = "Estágio 3",
                motivosAlocaoEstagio = motivosEst3,
            });

            return estagios;
        }

        public static List<ClassificacaoInstrumentoFinanceiro> popularClassificacaoInstrumentoFinanceiro()
        {
            List<ClassificacaoInstrumentoFinanceiro> classificacoes = new List<ClassificacaoInstrumentoFinanceiro>();

            classificacoes.Add(new ClassificacaoInstrumentoFinanceiro
            {
                Codigo = "1",
                Descricao = "Custo amortizado"
            });

            classificacoes.Add(new ClassificacaoInstrumentoFinanceiro
            {
                Codigo = "2",
                Descricao = "Valor justo em outros resultados abrangentes"
            });

            classificacoes.Add(new ClassificacaoInstrumentoFinanceiro
            {
                Codigo = "3",
                Descricao = "Valor justo no resultado"
            });

            return classificacoes;
        }

        public static List<CarteiraProvisao> popularCarteirasProvisao()
        {
            List<CarteiraProvisao> carteiraProvisao = new List<CarteiraProvisao>();

            carteiraProvisao.Add(new CarteiraProvisao
            {
                Codigo= "1",
                Descricao = "Carteira 1"
            });

            carteiraProvisao.Add(new CarteiraProvisao
            {
                Codigo = "1",
                Descricao = "Carteira 1"
            });

            carteiraProvisao.Add(new CarteiraProvisao
            {
                Codigo = "2",
                Descricao = "Carteira 2"
            });

            carteiraProvisao.Add(new CarteiraProvisao
            {
                Codigo = "3",
                Descricao = "Carteira 3"
            });

            carteiraProvisao.Add(new CarteiraProvisao
            {
                Codigo = "4",
                Descricao = "Carteira 4"
            });

            carteiraProvisao.Add(new CarteiraProvisao
            {
                Codigo = "5",
                Descricao = "Carteira 5"
            });

            return carteiraProvisao;
        }

        public static List<MotivoPerda> popularMotivosPerda()
        {
            List<MotivoPerda> motivosPerdas = new List<MotivoPerda>();

            motivosPerdas.Add(new MotivoPerda
            {
                Codigo = "1",
                Descricao = "Perdas por reestruturação geradas por abatimentos concedidos, incluindo deságio em recuperação judicial ou extrajudicial."
            });

            motivosPerdas.Add(new MotivoPerda
            {
                Codigo = "2",
                Descricao = "Perdas por reestruturação geradas pela remensuração do instrumento pela taxa de juros efetiva original."
            });

            motivosPerdas.Add(new MotivoPerda
            {
                Codigo = "3",
                Descricao = "Perdas por abatimentos concedidos na liquidação total ou cessão do instrumento financeiro."
            });

            return motivosPerdas;
        }    

        

        

        

        
    }
}
