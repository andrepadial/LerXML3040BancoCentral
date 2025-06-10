using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Read3040Bacen
{
    public class Tratamento
    {
        /* 1 - Tratamento para os dias em atraso dos contratos
         */
        public static List<Operacao> tratarDiasEmAtraso(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {

                op.DiaAtraso = (
                    retornaDiasAtraso(Convert.ToDateTime(op.DtContr),
                       new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), 
                       op.DiaAtraso)).ToString();

            }

            return operacoesTratadas;
        }

        // 2 - Tratamento para o estágio da operação         
        public static List<Operacao> tratarEstagio(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;
            
            foreach(var op in operacoesTratadas)
            {
                if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 91)
                {
                    op.contabilizacao4966.EstInstFin = "3";                    
                }
                else if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 61 && Convert.ToInt32(op.DiaAtraso) <= 90)
                {
                    op.contabilizacao4966.EstInstFin = "2";
                }
                else
                {
                    op.contabilizacao4966.EstInstFin = "1";
                }
            }

            return operacoesTratadas;
        }

        //3 - Tratamento para a carteira
        public static List<Operacao> tratarCarteira(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.contabilizacao4966.EstInstFin == "3")
                {
                    op.contabilizacao4966.CartProvMin = "C5";
                }
                else if (op.contabilizacao4966.EstInstFin == "2")
                {
                    op.contabilizacao4966.EstInstFin = "C2";
                }
                else
                {
                    op.contabilizacao4966.EstInstFin = "C1";
                }
            }

            return operacoesTratadas;
        }

        //4 - Tratamento para classificação do ativo Financeiro
        public static List<Operacao> tratarClassificacaoAtivoFinanceiro(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.contabilizacao4966.ClasAtFin == null)
                {
                    op.contabilizacao4966.ClasAtFin = "1";
                }
                else
                {
                    op.contabilizacao4966.ClasAtFin = op.contabilizacao4966.ClasAtFin.TrimEnd().TrimStart();
                }
            }

            return operacoesTratadas;
        }

        //5 - Tratamento para PDEST
        public static List<Operacao> tratarPDEST(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                op.contabilizacao4966.PdEst1 = "S";
            }

            return operacoesTratadas;
        }

        //6 - Tratamento para PDEST
        public static List<Operacao> tratarRiscoCredito(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                op.contabilizacao4966.TratRisc = "N";
            }

            return operacoesTratadas;
        }

        //7 - Tratamento para Valor Contabil e Valor Bruto
        public static List<Operacao> tratarValores4966(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                op.contabilizacao4966.VlrContBr = op.VlrContr;
                op.contabilizacao4966.VlrJusto = op.VlrContr;
            }

            return operacoesTratadas;
        }

        //8 - Tratamento para TJE da 4966
        public static List<Operacao> tratarTJE4966(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.contabilizacao4966.TJE ==  null)
                {
                    op.contabilizacao4966.TJE = "0.000000";
                }
                
            }

            return operacoesTratadas;
        }

        private static int retornaDiasAtraso(DateTime dataInicial, DateTime dataFinal, string? diaAtraso)
        {
            int diasAtraso = 0;

            if (diaAtraso!= null && Convert.ToInt32(diaAtraso) > 0)
            {
                diasAtraso = (dataFinal - dataInicial).Days;
            }

            return diasAtraso;
        }
    }
}
