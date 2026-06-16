using Read3040Bacen.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;



namespace Read3040Bacen
{
    public class Tratamento
    {
        private const string cnpjEmpresa023 = "60889129";
        private const decimal salarioMinimo = 1500;

        #region ############ TRATAMENTO PARA AS OPERAÇÕES ############

        // 0 - Tratamento para operações vigentes sem fluxo de vencimentos
        public static List<Operacao> tratarFluxoVencimentos(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            Vencimento vencimento = new Vencimento();
            int diasAtraso = 0;
            decimal totalVencimentos = 0;
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                totalVencimentos = retornaSomaVencimentos(op);
                diasAtraso = Convert.ToInt32(op.DiaAtraso == null ? "0" : op.DiaAtraso);

                if (!retornaOperacaoSaida(op) && retornaQuantidadeVencimentos(op) <= 1)
                {
                    if (op.vencimentos.v20 != null || op.vencimentos.v40 != null)
                    {
                        vencimento = retornaFaixaVencimentoOperacoesLimiteCredito(diasAtraso, totalVencimentos);
                    }
                    else if (op.vencimentos.v60 != null || op.vencimentos.v80 != null)
                    {
                        vencimento = retornaFaixaVencimentoOperacoesCreditosLiberar(diasAtraso, totalVencimentos);
                    }
                    else if (op.vencimentos.v110 != null || op.vencimentos.v120 != null || op.vencimentos.v130 != null || op.vencimentos.v140 != null ||
                            op.vencimentos.v150 != null || op.vencimentos.v160 != null || op.vencimentos.v165 != null || op.vencimentos.v170 != null ||
                            op.vencimentos.v175 != null || op.vencimentos.v180 != null || op.vencimentos.v190 != null || op.vencimentos.v199 != null
                            )
                    {
                        vencimento = retornaFaixaVencimentoOperacoesCreditosVencer(diasAtraso, totalVencimentos);
                    }
                    else if (op.vencimentos.v205 != null ||
                            op.vencimentos.v210 != null || op.vencimentos.v220 != null || op.vencimentos.v230 != null || op.vencimentos.v240 != null || op.vencimentos.v245 != null ||
                            op.vencimentos.v250 != null || op.vencimentos.v260 != null || op.vencimentos.v255 != null || op.vencimentos.v270 != null ||
                            op.vencimentos.v280 != null || op.vencimentos.v290 != null
                            )
                    {
                        vencimento = retornaFaixaVencimentoOperacoesVencidas(diasAtraso, totalVencimentos);
                    }
                    else
                    {
                        vencimento = retornaFaixaVencimentoOperacoesPrejuizo(diasAtraso, totalVencimentos);
                    }

                    op.vencimentos = vencimento;
                }
            }

            return operacoesTratadas;
        }

        // 1 - Tratamento para os dias em atraso dos contratos         
        public static List<Operacao> tratarDiasEmAtraso(List<Operacao> operacoes, DateTime dataVigencia)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {

                op.DiaAtraso = (
                    retornaDiasAtraso(Convert.ToDateTime(op.DtContr),
                       dataVigencia,
                       op.DiaAtraso)).ToString();

            }

            return operacoesTratadas;
        }

        // 2 - Tratamento para o estágio da operação         
        public static List<Operacao> tratarEstagio(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (existeInformacaoAdicional1701(op) && !retornaOperacaoSaida(op))

                    if (op.vencimentos.v330 != null || op.vencimentos.v320 != null || op.vencimentos.v310 != null)
                    {
                        op.contabilizacao4966.EstInstFin = "3";
                        op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                    }
                    else if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 91)
                    {
                        op.contabilizacao4966.EstInstFin = "3";
                    }
                    else if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 61 && Convert.ToInt32(op.DiaAtraso) <= 90)
                    {
                        op.contabilizacao4966.EstInstFin = "2";
                        op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                    }
                    else
                    {
                        op.contabilizacao4966.EstInstFin = "1";
                        op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                    }
            }

            return operacoesTratadas;
        }

        // 2a - Tratamento para quando o estágio for nulo ou inexistente
        public static List<Operacao> tratarEstagioV2(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (!existeInformacaoAdicionalSaida(op))
                {
                    if (op.vencimentos.v330 != null || op.vencimentos.v320 != null || op.vencimentos.v310 != null)
                    {
                        op.contabilizacao4966.EstInstFin = "3";
                        op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                    }
                    else if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 91)
                    {
                        op.contabilizacao4966.EstInstFin = "3";
                    }
                    else if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 61 && Convert.ToInt32(op.DiaAtraso) <= 90)
                    {
                        op.contabilizacao4966.EstInstFin = "2";
                        op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                    }
                    else
                    {
                        op.contabilizacao4966.EstInstFin = "1";
                        op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                    }
                }
                else
                {
                    if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 91)
                    {
                        op.contabilizacao4966.EstInstFin = "3";
                        op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                    }
                    else if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 61 && Convert.ToInt32(op.DiaAtraso) <= 90)
                    {
                        op.contabilizacao4966.EstInstFin = "2";
                        op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                    }
                    else
                    {
                        op.contabilizacao4966.EstInstFin = "1";
                        op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                    }
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarEstagioV3(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.contabilizacao4966.EstInstFin == null || op.contabilizacao4966.EstInstFin.TrimEnd().TrimStart().Length == 0)
                {
                    if (operacaoEstaEmPrejuizo(op) || Convert.ToInt32(op.DiaAtraso) >= 91)
                    {
                        op.contabilizacao4966.EstInstFin = "3";
                        op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                    }
                    else if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 61 && Convert.ToInt32(op.DiaAtraso) <= 90)
                    {
                        op.contabilizacao4966.EstInstFin = "2";
                        op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                    }
                    else
                    {
                        op.contabilizacao4966.EstInstFin = "1";
                        op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                    }
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarEstagioOperacoesPrejuizo(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (operacaoEstaEmPrejuizo(op) && op.contabilizacao4966.EstInstFin == null)
                {
                    op.contabilizacao4966.EstInstFin = "3";
                    op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarEstagioPF(List<Operacao> operacoes, string tipoCliente)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                //Incluindo uma condição para tratar somente PF
                if (tipoCliente == "1" || tipoCliente == "3" || tipoCliente == "5")
                {

                    if (!existeInformacaoAdicionalSaida(op))
                    {
                        if (op.vencimentos.v330 != null || op.vencimentos.v320 != null || op.vencimentos.v310 != null)
                        {
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                        }
                        else if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 91)
                        {
                            op.contabilizacao4966.EstInstFin = "3";
                        }
                        else if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 61 && Convert.ToInt32(op.DiaAtraso) <= 90)
                        {
                            op.contabilizacao4966.EstInstFin = "2";
                            op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                        }
                        else
                        {
                            op.contabilizacao4966.EstInstFin = "1";
                            op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                        }
                    }
                    else
                    {
                        if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 91)
                        {
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                        }
                        else if (op.DiaAtraso != null && Convert.ToInt32(op.DiaAtraso) >= 61 && Convert.ToInt32(op.DiaAtraso) <= 90)
                        {
                            op.contabilizacao4966.EstInstFin = "2";
                            op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                        }
                        else
                        {
                            op.contabilizacao4966.EstInstFin = "1";
                            op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                        }
                    }
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> setarAtivoProblematicoEstagio3(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (operacaoEstaEmPrejuizo(op))
                {
                    op.contabilizacao4966.EstInstFin = "3";
                    op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
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
                if (op.vencimentos != null)
                {
                    if (existeInformacaoAdicional1701(op) || op.vencimentos.v330 != null)
                    {
                        op.contabilizacao4966.CartProvMin = "C5";
                    }
                    else if (op.contabilizacao4966.EstInstFin == "3")
                    {
                        op.contabilizacao4966.CartProvMin = "C4";
                    }
                    else if (op.contabilizacao4966.EstInstFin == "2")
                    {
                        op.contabilizacao4966.CartProvMin = "C2";
                    }
                    else
                    {
                        op.contabilizacao4966.CartProvMin = "C1";
                    }
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarCarteiraV2(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.contabilizacao4966.CartProvMin == null || op.contabilizacao4966.CartProvMin.TrimStart().TrimEnd().Length == 0)
                {
                    if (op.vencimentos != null)
                    {
                        if (existeInformacaoAdicional1701(op) || op.vencimentos.v330 != null)
                        {
                            op.contabilizacao4966.CartProvMin = "C5";
                        }
                        else if (op.contabilizacao4966.EstInstFin == "3")
                        {
                            op.contabilizacao4966.CartProvMin = "C4";
                        }
                        else if (op.contabilizacao4966.EstInstFin == "2")
                        {
                            op.contabilizacao4966.CartProvMin = "C2";
                        }
                        else
                        {
                            op.contabilizacao4966.CartProvMin = "C1";
                        }
                    }
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarCarteiraV3(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;
            decimal totalVencimentos = 0;

            foreach (var op in operacoesTratadas)
            {
                totalVencimentos = retornaSomaVencimentos(op);

                if ((op.contabilizacao4966.CartProvMin == null || op.contabilizacao4966.CartProvMin.TrimEnd().TrimStart().Length == 0)
                  )
                {
                    if (existeInformacaoAdicional1701(op))
                    {
                        op.contabilizacao4966.CartProvMin = "C5";
                    }
                    else if (op.contabilizacao4966.EstInstFin == "3")
                    {
                        op.contabilizacao4966.CartProvMin = "C4";
                    }
                    else if (op.contabilizacao4966.EstInstFin == "2")
                    {
                        op.contabilizacao4966.CartProvMin = "C2";
                    }
                    else
                    {
                        op.contabilizacao4966.CartProvMin = "C1";
                    }

                }
            }

            return operacoesTratadas;
        }

        //4 - Tratamento para classificação do ativo Financeiro
        public static List<Operacao> tratarClassificacaoAtivoFinanceiro(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;
            decimal totalVencimentos = 0;

            foreach (var op in operacoesTratadas)
            {
                totalVencimentos = retornaSomaVencimentos(op);

                if (op.contabilizacao4966.ClasAtFin == null)
                {
                    op.contabilizacao4966.ClasAtFin = "1";
                }
                else
                {
                    op.contabilizacao4966.ClasAtFin = op.contabilizacao4966.ClasAtFin.TrimEnd().TrimStart();
                }

                //op.contabilizacao4966.VlrContBr = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                //op.contabilizacao4966.VlrJusto = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", "."); ;


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
            decimal totalVencimentos = 0;

            foreach (var op in operacoesTratadas)
            {
                totalVencimentos = retornaSomaVencimentos(op);

                if (op.contabilizacao4966.VlrContBr == null || op.contabilizacao4966.VlrContBr.TrimEnd().TrimStart().Length == 0)
                    op.contabilizacao4966.VlrContBr = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");

                if (op.contabilizacao4966.VlrJusto == null || op.contabilizacao4966.VlrJusto.TrimEnd().TrimStart().Length == 0)
                    op.contabilizacao4966.VlrJusto = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", "."); ;
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
                if (op.contabilizacao4966.TJE == null || op.contabilizacao4966.TJE.TrimEnd().TrimStart().Length == 0)
                {
                    op.contabilizacao4966.TJE = "0.000000";
                }

            }

            return operacoesTratadas;
        }

        //9 - Tratamento para ativo problemático das operações com estágio 3
        public static List<Operacao> tratarCaracEspecialAtivoProblematicoEstagio3(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.contabilizacao4966.EstInstFin != null && op.contabilizacao4966.EstInstFin == "3")
                {
                    op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                }
                else
                {
                    op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                }
            }

            return operacoesTratadas;
        }

        /*10 - Tratamento para ativo problemático das operações em estágio 1 e 2
         * Não pode haver marcação de ativo problemático para operações com estágio 1 e 2         
         */

        public static List<Operacao> tratarAtivoProblematicoEstagio12(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.contabilizacao4966.EstInstFin != null && (op.contabilizacao4966.EstInstFin == "1" || op.contabilizacao4966.EstInstFin == "2"))
                {
                    if (op.CaracEspecial != null)
                    {
                        op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                    }
                }
            }

            return operacoesTratadas;
        }


        /*11 - Tratar marcação Característica especial 11 para vencimentos 330
        */
        public static List<Operacao> tratarCaracEspecialVencimento330(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.vencimentos != null)
                {
                    if (op.vencimentos.v330 != null)
                    {
                        if (op.CaracEspecial == null || op.CaracEspecial.TrimEnd().TrimStart().Length == 0)
                        {
                            op.CaracEspecial = "11;19";
                        }
                        else if (!op.CaracEspecial.Contains("11"))
                        {
                            op.CaracEspecial = String.Concat(op.CaracEspecial, ";11;19");
                        }
                    }
                }
            }

            return operacoesTratadas;
        }

        // 12 - Tratamento para localidade nula das operações
        public static List<Operacao> tratarLocalidade(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.CEP == null || op.CEP.TrimEnd().TrimStart().Length < 8)
                {
                    op.CEP = "01418100";
                }
            }

            return operacoesTratadas;
        }

        //13 - Tratamento para indexador da operação
        public static List<Operacao> tratarIndexador(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.Indx == null && (op.PercIndx == null || Convert.ToDouble(op.PercIndx) == 0.00))
                {
                    op.Indx = "11";
                }
                else if (op.Indx != null &&
                        (Convert.ToDouble(op.PercIndx) == 0.00 || op.PercIndx == null ||
                        op.Indx == "00")
                    )
                {
                    op.Indx = "31";
                    op.PercIndx = "100.00";
                }
            }

            return operacoesTratadas;
        }

        /* 14 - Tratamento Carteira para estágio 3
         * Classificação da carteira não pode ser C1 e C2 para estágio 3
        */

        // 15 - Tratamento do código do cliente na operação
        public static List<Operacao> tratarCodigoClienteOperacao(string cpfCnpj, List<Operacao> operacoes, string tipoCliente)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {

                if (tipoCliente == null || tipoCliente.Length == 0)
                {
                    tipoCliente = (cpfCnpj.Length <= 8 || cpfCnpj.Length > 11) ? "2" : "1";
                }

                //SE PJ
                if (Convert.ToInt32(tipoCliente) == 2 || Convert.ToInt32(tipoCliente) == 4 || Convert.ToInt32(tipoCliente) == 6)
                {
                    if (op.DetCli == null || op.DetCli.TrimStart().TrimEnd().Length == 0)
                    {
                        op.DetCli = cpfCnpj;
                    }
                }
            }

            return operacoesTratadas;
        }

        /* 16 - Tratamento para as operações com o mesmo CNPJ do banco
         * Não pode haver operações com o CNPJ do Banco
        */

        public static bool tratarOperacoesCNPJBanco(Cliente cliente)
        {
            return (cliente.Cd.StartsWith("60889128"));
        }

        // 17 - Operações no estágio 3 não podem ser C1 e C2        
        public static List<Operacao> tratarCarteiraEstagio3(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.contabilizacao4966.EstInstFin == "3" && (op.contabilizacao4966.CartProvMin == "C1" || op.contabilizacao4966.CartProvMin == "C2"))
                {
                    op.contabilizacao4966.CartProvMin = "C4";
                }
            }

            return operacoesTratadas;
        }

        // 18 - Tratar data de vencimento da parcela
        public static List<Operacao> tratarDataVencimentoParcela(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.DtVencOp == null)
                {
                    op.DtVencOp = "2099-12-31";
                }
                else if (Convert.ToDateTime(op.DtVencOp) <= Convert.ToDateTime(op.DtContr))
                {
                    op.DtVencOp = "2099-12-31";
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarDataInicioContrato(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.DtContr == null)
                {
                    op.DtContr = "2000-01-01";
                }
                else if (Convert.ToDateTime(op.DtContr) > Convert.ToDateTime(op.DtVencOp))
                {
                    op.DtContr = (Convert.ToDateTime(op.DtContr)).AddDays(-30).ToString("yyyy-MM-dd");
                }
            }

            return operacoesTratadas;
        }

        //19 - Tratar provisão constituida
        public static List<Operacao> tratarProvisaoConstituida(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.ProvConsttd == null)
                {
                    op.ProvConsttd = "0.00";
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarValorProvisaoOperacoesPrejuizo(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (operacaoEstaEmPrejuizo(op))
                {
                    op.ProvConsttd = "0.00";
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarModalidade1899(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.Mod == "1899")
                {
                    op.CaracEspecial = setarCaracteristicaEspecial39(op);
                }
            }

            return operacoesTratadas;
        }

        // 20 - Tratamento para modalidade 08 - operações de rural
        public static List<Operacao> tratarModalidade08(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.Mod.StartsWith("08"))
                {
                    op.CaracEspecial = setarCaracteristicaEspecial38(op);
                }
            }

            return operacoesTratadas;
        }

        // 21 - Tratamento IPOC
        public static List<Cliente> tratarIPOC(List<Cliente> clientes, string cnpjEmpresa)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;


            //foreach(var cliente in clientesTratados)
            //{
            //    if (cnpjEmpresa == cnpjEmpresa023)
            //    {
            //        foreach(var op in cliente.operacoes)
            //        {
            //            if (op.IPOC  == null)
            //            {
            //                op.IPOC = String.Concat(cnpjEmpresa023, op.Mod, cliente.Tp, cliente.Cd, op.Contrt);
            //            }
            //        }
            //    }
            //}

            foreach (var cliente in clientesTratados)
            {
                foreach (var op in cliente.operacoes)
                {
                    op.IPOC = String.Concat(cnpjEmpresa, op.Mod, cliente.Tp, cliente.Cd, op.Contrt);
                }
            }

            return clientesTratados;
        }

        // 22 - Tratamento para Informação adicional duplicada
        public static List<Operacao> tratarInformacoesAdicionaisDuplicadas(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.informacoesAdiacionais.Count > 0)
                {
                    op.informacoesAdiacionais = retornarInfoAdicionaisSemDuplicidade(op);
                }
            }

            return operacoesTratadas;
        }

        // 23 - Tratamento para campos se tiver saída de operação
        public static List<Operacao> tratarCamposOperacaoSaida(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas.ToList())
            {
                if (retornaOperacaoSaida(op))
                {
                    op.ProvConsttd = null;

                    foreach (var gar in op.garantias.ToList())
                    {
                        op.garantias.Remove(gar);
                    }
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> excluirOperacoes0499(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas.ToList())
            {
                if (op.Mod == "0499")
                {
                    operacoesTratadas.Remove(op);
                }
            }

            return operacoesTratadas;
        }

        // 24 - Tratamento para operações com Informação Adicional 1701
        public static List<Operacao> tratarOperacoesInfoAdicional1701(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (retornaTemInformacao1701(op))
                {
                    //op.contabilizacao4966.EstInstFin = "3";
                    //op.contabilizacao4966.CartProvMin = "C4";

                    if (op.CaracEspecial == null || op.CaracEspecial.TrimEnd().TrimStart().Length == 0)
                    {
                        op.CaracEspecial = "11;19";
                    }
                    else
                    {
                        if (!op.CaracEspecial.Contains("11"))
                        {
                            op.CaracEspecial = String.Concat(op.CaracEspecial, ";11");
                        }

                        if (!op.CaracEspecial.Contains("19"))
                        {
                            op.CaracEspecial = String.Concat(op.CaracEspecial, ";19");
                        }

                    }
                }
            }

            return operacoesTratadas;
        }

        // 25 - Tratamento para regra PJ para operações em prejuízo - 541 dias em atraso

        public static List<Operacao> tratarOperacoesEmPJ541DiasAtraso(Cliente cliente)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = cliente.operacoes;

            decimal totalVencimentos = 0;

            if (cliente.Tp == "2" || cliente.Tp == "4" || cliente.Tp == "6")
            {
                if (cliente.operacoes.Count > 0)
                {
                    foreach (var op in operacoesTratadas)
                    {
                        if (!retornaOperacaoSaida(op) && Convert.ToInt32(op.DiaAtraso) >= 541)
                        {
                            totalVencimentos = retornaSomaVencimentos(op);
                            op.vencimentos = new Vencimento();
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            //op.contabilizacao4966.VlrContBr = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            //op.contabilizacao4966.VlrJusto = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            //op.CaracEspecial = retornaCaractaristicaEspecialRegraPJ541DiasAtraso(op);
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                            op.CaracEspecial = setarCaracteristicaEspecial11(op);
                            op.contabilizacao4966.CartProvMin = "C5";
                            op.contabilizacao4966.EstInstFin = "3";
                        }
                    }
                }
            }

            return operacoesTratadas;
        }

        // 26 - Tratamento para fluxo de vencimento das operações baixadas
        public static List<Operacao> tratarFluxoVencimentoOperacoesBaixadas(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (retornaOperacaoSaida(op))
                {
                    op.vencimentos = new Vencimento();
                    op.vencimentos.v20 = null;
                    op.vencimentos.v40 = null;
                    op.vencimentos.v60 = null;
                    op.vencimentos.v80 = null;
                    op.vencimentos.v110 = null;
                    op.vencimentos.v120 = null;
                    op.vencimentos.v130 = null;
                    op.vencimentos.v140 = null;
                    op.vencimentos.v150 = null;
                    op.vencimentos.v160 = null;
                    op.vencimentos.v165 = null;
                    op.vencimentos.v170 = null;
                    op.vencimentos.v175 = null;
                    op.vencimentos.v180 = null;
                    op.vencimentos.v190 = null;
                    op.vencimentos.v199 = null;
                    op.vencimentos.v205 = null;
                    op.vencimentos.v210 = null;
                    op.vencimentos.v220 = null;
                    op.vencimentos.v230 = null;
                    op.vencimentos.v240 = null;
                    op.vencimentos.v245 = null;
                    op.vencimentos.v250 = null;
                    op.vencimentos.v255 = null;
                    op.vencimentos.v260 = null;
                    op.vencimentos.v270 = null;
                    op.vencimentos.v280 = null;
                    op.vencimentos.v290 = null;
                    op.vencimentos.v310 = null;
                    op.vencimentos.v320 = null;
                    op.vencimentos.v330 = null;
                    //op.contabilizacao4966.VlrContBr = null;
                    //op.contabilizacao4966.VlrJusto = null;
                }
            }

            return operacoesTratadas;
        }

        // 27 - Tratamento para caso Home Equity de operação PF em prejuizo indevidamente
        public static List<Operacao> tratarCasoHomeEquityPF(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;
            decimal totalVencimentos = 0;

            foreach (var op in operacoesTratadas)
            {
                if (op.Contrt == "2023HE28674")
                {
                    totalVencimentos = retornaSomaVencimentos(op);
                    op.vencimentos.v20 = null;
                    op.vencimentos.v40 = null;
                    op.vencimentos.v60 = null;
                    op.vencimentos.v80 = null;
                    op.vencimentos.v110 = null;
                    op.vencimentos.v120 = null;
                    op.vencimentos.v130 = null;
                    op.vencimentos.v140 = null;
                    op.vencimentos.v150 = null;
                    op.vencimentos.v160 = null;
                    op.vencimentos.v165 = null;
                    op.vencimentos.v170 = null;
                    op.vencimentos.v175 = null;
                    op.vencimentos.v180 = null;
                    op.vencimentos.v190 = null;
                    op.vencimentos.v199 = null;
                    op.vencimentos.v205 = null;
                    op.vencimentos.v210 = null;
                    op.vencimentos.v220 = null;
                    op.vencimentos.v230 = null;
                    op.vencimentos.v240 = null;
                    op.vencimentos.v245 = null;
                    op.vencimentos.v250 = null;
                    op.vencimentos.v255 = null;
                    op.vencimentos.v260 = null;
                    op.vencimentos.v270 = null;
                    op.vencimentos.v280 = null;
                    op.vencimentos.v290 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                    op.vencimentos.v310 = null;
                    op.vencimentos.v320 = null;
                    op.vencimentos.v330 = null;
                    //op.contabilizacao4966.VlrContBr = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                    //op.contabilizacao4966.VlrJusto = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");                    
                }
            }

            return operacoesTratadas;
        }

        // 28 - Tratar casos irrecuperaveis
        public static List<Operacao> tratarCasosIrrecuperaveis(List<Operacao> operacoes, string tpCliente, string cpfCnpj)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            List<ContratoIrrecuperavel> operacoesIrrecuperaveis = new List<ContratoIrrecuperavel>();
            operacoesTratadas = operacoes;
            operacoesIrrecuperaveis = popularContratosIrrecuperaveis();

            foreach (var op in operacoesTratadas.ToList())
            {
                if (tpCliente == null || tpCliente.Length == 0)
                {
                    tpCliente = (cpfCnpj.Length <= 8 || cpfCnpj.Length > 11) ? "2" : "1";
                }

                if (Convert.ToInt32(tpCliente) == 2 || Convert.ToInt32(tpCliente) == 4 || Convert.ToInt32(tpCliente) == 6)
                {
                    var result = operacoesIrrecuperaveis.Where(o => o.NumeroContrato == op.Contrt).FirstOrDefault();

                    if (result != null)
                    {
                        operacoesTratadas.Remove(op);
                    }
                }
            }

            return operacoesTratadas;
        }

        // 29 - Tratar casos de ativo para WO 
        public static List<Operacao> tratarCasosAtivoParaWO(List<Operacao> operacoes, string tpCliente)
        {
            List<Operacao> operacoesTratatas = new List<Operacao>();
            List<ContratoAtivoParaWO> operacoesAtivoParaWO = new List<ContratoAtivoParaWO>();
            decimal totalVencimentos = 0;
            int diasAtraso = 0;

            operacoesTratatas = operacoes;
            //operacoesAtivoParaWO = popularContratosAtivosParaWO();

            foreach (var op in operacoesTratatas)
            {

                diasAtraso = Convert.ToInt32(op.DiaAtraso);

                if (Convert.ToInt32(tpCliente) == 2 || Convert.ToInt32(tpCliente) == 4 || Convert.ToInt32(tpCliente) == 6)
                {
                    var result = operacoesAtivoParaWO.Where(o => o.NumeroContrato == op.Contrt).FirstOrDefault();

                    if (result != null && !retornaOperacaoSaida(op))
                    {
                        totalVencimentos = retornaSomaVencimentos(op);

                        op.vencimentos = retornaFaixaVencimentoOperacoesPrejuizo(diasAtraso, totalVencimentos);
                        //op.contabilizacao4966.VlrContBr = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        //op.contabilizacao4966.VlrJusto = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);

                        if (op.vencimentos.v310 != null || op.vencimentos.v320 != null)
                        {
                            op.contabilizacao4966.CartProvMin = "C4";
                            op.contabilizacao4966.EstInstFin = "3";
                        }
                        else
                        {
                            op.contabilizacao4966.CartProvMin = "C5";
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaEspecial11(op);
                        }
                    }
                }
            }


            return operacoesTratatas;
        }

        // 30 - Tratar casos WO para ativo com menos de 541 dias de atraso
        public static List<Operacao> tratarCasosWOParaAtivoMenor541Dias(List<Operacao> operacoes, string tpCliente)
        {
            List<Operacao> operacoesTratatas = new List<Operacao>();
            List<ContratoWOParaAtivoMenor541Dias> operacoesWOParaAtivo = new List<ContratoWOParaAtivoMenor541Dias>();
            decimal totalVencimentos = 0;
            int diasAtraso = 0;

            operacoesTratatas = operacoes;
            operacoesWOParaAtivo = popularContratoWOParaAtivoMenor541Dias();

            foreach (var op in operacoesTratatas)
            {

                if (Convert.ToInt32(tpCliente) == 2 || Convert.ToInt32(tpCliente) == 4 || Convert.ToInt32(tpCliente) == 6)
                {
                    var result = operacoesWOParaAtivo.Where(o => o.NumeroContrato == op.Contrt).FirstOrDefault();

                    if (result != null && !retornaOperacaoSaida(op))
                    {
                        totalVencimentos = retornaSomaVencimentos(op);
                        diasAtraso = Convert.ToInt32(op.DiaAtraso);
                        op.vencimentos = new Vencimento();

                        if (Convert.ToInt32(op.DiaAtraso) <= 14)
                        {
                            op.vencimentos.v205 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C1";
                            op.contabilizacao4966.EstInstFin = "1";
                            op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                        }
                        else if (Convert.ToInt32(op.DiaAtraso) <= 30)
                        {
                            op.vencimentos.v210 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C1";
                            op.contabilizacao4966.EstInstFin = "1";
                            op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                        }
                        else if (diasAtraso <= 60)
                        {
                            op.vencimentos.v220 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C1";
                            op.contabilizacao4966.EstInstFin = "1";
                            op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                        }
                        else if (diasAtraso <= 90)
                        {
                            op.vencimentos.v230 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C2";
                            op.contabilizacao4966.EstInstFin = "2";
                            op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                        }
                        else if (diasAtraso <= 120)
                        {
                            op.vencimentos.v240 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C3";
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                        }
                        else if (diasAtraso <= 150)
                        {
                            op.vencimentos.v245 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C3";
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                        }
                        else if (diasAtraso <= 180)
                        {
                            op.vencimentos.v250 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C3";
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                        }
                        else if (diasAtraso <= 240)
                        {
                            op.vencimentos.v255 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C3";
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                        }
                        else if (diasAtraso <= 300)
                        {
                            op.vencimentos.v260 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C3";
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                        }
                        else if (diasAtraso <= 360)
                        {
                            op.vencimentos.v270 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C3";
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                        }
                        else if (diasAtraso <= 540)
                        {
                            op.vencimentos.v280 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v290 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C3";
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                        }
                        else if (diasAtraso > 541)
                        {
                            op.vencimentos.v290 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                            op.vencimentos.v20 = null;
                            op.vencimentos.v40 = null;
                            op.vencimentos.v60 = null;
                            op.vencimentos.v80 = null;
                            op.vencimentos.v110 = null;
                            op.vencimentos.v120 = null;
                            op.vencimentos.v130 = null;
                            op.vencimentos.v140 = null;
                            op.vencimentos.v150 = null;
                            op.vencimentos.v160 = null;
                            op.vencimentos.v165 = null;
                            op.vencimentos.v170 = null;
                            op.vencimentos.v175 = null;
                            op.vencimentos.v180 = null;
                            op.vencimentos.v190 = null;
                            op.vencimentos.v199 = null;
                            op.vencimentos.v205 = null;
                            op.vencimentos.v210 = null;
                            op.vencimentos.v220 = null;
                            op.vencimentos.v230 = null;
                            op.vencimentos.v240 = null;
                            op.vencimentos.v245 = null;
                            op.vencimentos.v250 = null;
                            op.vencimentos.v255 = null;
                            op.vencimentos.v260 = null;
                            op.vencimentos.v270 = null;
                            op.vencimentos.v280 = null;
                            op.vencimentos.v310 = null;
                            op.vencimentos.v320 = null;
                            op.vencimentos.v330 = null;
                            op.contabilizacao4966.CartProvMin = "C3";
                            op.contabilizacao4966.EstInstFin = "3";
                            op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                        }

                        //op.contabilizacao4966.VlrContBr = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        //op.contabilizacao4966.VlrJusto = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");

                    }
                }
            }


            return operacoesTratatas;
        }


        //31 - Tratar cassos PEAC
        public static List<Operacao> tratarCasosPEAC(List<Operacao> operacoes, string tpCliente, string cpfCnpj)
        {
            decimal totalVencimentos = 0;
            List<Operacao> operacoesTratadas = new List<Operacao>();
            List<ContratoInfoAdd0408Gar0674> operacoesPeac = new List<ContratoInfoAdd0408Gar0674>();
            operacoesPeac = popularContratosInfoAdicional0408Garantia0674();

            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {

                if (tpCliente == null || tpCliente.Length == 0)
                {
                    tpCliente = (cpfCnpj.Length <= 8 || cpfCnpj.Length > 11) ? "2" : "1";
                }


                if (Convert.ToInt32(tpCliente) == 2 || Convert.ToInt32(tpCliente) == 4 || Convert.ToInt32(tpCliente) == 6)
                {
                    var result = operacoesPeac.Where(o => o.NumeroContrato == op.Contrt).FirstOrDefault();

                    if (result != null && !retornaOperacaoSaida(op))
                    {

                        //Incluindo a garantia do governo BR, caso não exista
                        totalVencimentos = retornaSomaVencimentos(op);

                        if (op.garantias.Count == 0 || !existeGarantia0674(op))
                        {
                            op.garantias.Add(new Garantia
                            {
                                Tp = "0674",
                                VlrOrig = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".")
                            });
                        }

                        //Incluindo a informação adicional 0408, caso não exista
                        if (op.informacoesAdiacionais.Count == 0 || !existeInformacaoAdicional0408(op))
                        {
                            op.informacoesAdiacionais.Add(new InformacaoAdicional
                            {
                                Tp = "0408",
                                Ident = op.DtContr,
                                Cd = "02"
                            });
                        }
                    }
                }
            }

            return operacoesTratadas;
        }

        // 32 - Tratar casos de InfoAdicional 1001
        public static List<Operacao> tratarOperacoesInfoAdicional1001(List<Operacao> operacoes, string cnpjEmpresa)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;
            decimal totalVencimentos = 0;

            foreach (var op in operacoesTratadas)
            {

                totalVencimentos = retornaSomaVencimentos(op);

                if (existeInfoAdicional1001(op))
                {
                    foreach (var infoAdicional in op.informacoesAdiacionais)
                    {
                        if (infoAdicional.Tp == "1001")
                        {
                            infoAdicional.Ident = cnpjEmpresa;
                            infoAdicional.Cd = op.DtContr;
                            infoAdicional.Valor = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        }
                    }
                }
                //else
                //{
                //    //Incluir uma informação adicional do tipo 1001
                //    op.informacoesAdiacionais.Add(new InformacaoAdicional
                //    {
                //        Ident = cnpjEmpresa,
                //        Cd = op.DtContr,
                //        Valor = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".")
                //    });
                //}
            }


            return operacoesTratadas;
        }

        // 33 - Suprimir operações modalidade diferente de rural
        public static List<Operacao> sumprimirSicorOperacoesNaoRural(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratatadas = new List<Operacao>();
            operacoesTratatadas = operacoes;

            foreach (var op in operacoesTratatadas)
            {
                if (!op.Mod.StartsWith("08"))
                {
                    op.sicor = null;
                }
            }

            return operacoesTratatadas;
        }

        // 35 - Tratar garantias duplicadas
        public static List<Operacao> tratarGarantiasDuplicadas(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas.ToList())
            {
                if (op.garantias.Count > 0)
                {
                    op.garantias = retornarGarantiasSemDuplicidade(op);
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarMotivoAlocacaoEstagioDuplicados(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas.ToList())
            {
                if (op.contabilizacao4966.estagios != null && op.contabilizacao4966.estagios.Count > 0)
                {
                    op.contabilizacao4966.estagios = retornarMotivoAlocacaoSemDuplicidade(op);
                }
            }

            return operacoesTratadas;
        }

        // 34 - Tratar IPOC's duplicados
        public static List<Operacao> tratarOperacoesIpocsDuplicados(Cliente cliente)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = retornarIPOCSemDuplicidade(cliente);
            return operacoesTratadas;
        }

        public static Vencimento suprimirVencimento()
        {
            Vencimento vencimento = new Vencimento();

            vencimento = new Vencimento
            {
                v110 = null,
                v120 = null,
                v130 = null,
                v140 = null,
                v150 = null,
                v160 = null,
                v165 = null,
                v170 = null,
                v175 = null,
                v180 = null,
                v190 = null,
                v199 = null,
                v20 = null,
                v205 = null,
                v210 = null,
                v220 = null,
                v230 = null,
                v240 = null,
                v245 = null,
                v250 = null,
                v255 = null,
                v260 = null,
                v270 = null,
                v280 = null,
                v290 = null,
                v310 = null,
                v320 = null,
                v330 = null,
                v40 = null,
                v60 = null,
                v80 = null
            };

            return vencimento;
        }

        // 36 - Tratar operações de modalidade 09 - Incluir info adicional 2399
        public static List<Operacao> tratarOperacoesModalidade09(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (!existeInformacaoAdicional23(op) && op.Mod.StartsWith("09"))
                {
                    op.informacoesAdiacionais.Add(new InformacaoAdicional
                    {
                        Tp = "2399"
                    });
                }

            }


            return operacoesTratadas;
        }

        // 37 - Tratar operações com modalidade 15 - Não pode ter fluxo de vencimentos 310 / 320 e 330
        public static List<Operacao> tratarFluxoVencimentoOperacoesVencidas(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;
            decimal totalVencimentos = 0;
            int diasAtraso = 0;

            foreach (var op in operacoesTratadas)
            {
                totalVencimentos = retornaSomaVencimentos(op);
                diasAtraso = op.DiaAtraso == null ? 0 : Convert.ToInt32(op.DiaAtraso);

                if (!op.Mod.StartsWith("15") && operacaoEstaVencida(op))
                {
                    op.vencimentos = retornaFaixaVencimentoOperacoesVencidasV2(diasAtraso, totalVencimentos);
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarFluxoVencimentoModalidade15(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;
            decimal totalVencimentos = 0;
            int diasAtraso = 0;

            foreach (var op in operacoesTratadas)
            {
                totalVencimentos = retornaSomaVencimentos(op);
                diasAtraso = op.DiaAtraso == null ? 0 : Convert.ToInt32(op.DiaAtraso);

                if (op.Mod.StartsWith("15") && operacaoEstaEmPrejuizo(op))
                {
                    op.vencimentos = retornaFaixaVencimentoOperacoesVencidasV2(diasAtraso, totalVencimentos);
                }
            }

            return operacoesTratadas;
        }

        // 38 - Tratar garantias com informações faltantes
        public static List<Operacao> tratarCamposGarantias(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            List<Garantia> garantias = new List<Garantia>();
            operacoesTratadas = operacoes;


            foreach (var op in operacoesTratadas)
            {
                foreach (var gar in op.garantias)
                {
                    if (gar.VlrData != null)
                    {
                        garantias.Add(new Garantia
                        {
                            Tp = gar.Tp,
                            Ident = gar.Ident,
                            VlrOrig = gar.VlrData,
                            VlrData = null,
                            PercGar = null,
                            DtReav = null
                        });
                    }
                    else if (gar.VlrOrig != null)
                    {
                        garantias.Add(new Garantia
                        {
                            Tp = gar.Tp,
                            Ident = gar.Ident,
                            VlrOrig = gar.VlrOrig,
                            VlrData = null,
                            PercGar = null,
                            DtReav = null
                        });
                    }
                    else
                    {
                        garantias.Add(new Garantia
                        {
                            Tp = gar.Tp,
                            Ident = gar.Ident,
                            DtReav = gar.DtReav,
                            PercGar = gar.PercGar,
                            VlrData = gar.VlrData,
                            VlrOrig = gar.VlrOrig
                        });
                    }
                }
            }


            return operacoesTratadas;
        }


        // 39 - Tratar Operações com modalidade 0401, necessário incluir informação adicional 0401
        public static List<Operacao> tratarOperacoesModalidades0401E1206(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (
                    (op.Mod == "0401" || op.Mod == "1206") &&
                    (!existeInformacaoAdicional0401(op))
                    )
                {
                    op.informacoesAdiacionais.Add(new InformacaoAdicional
                    {
                        Tp = "0401",
                        Ident = "1"
                        //Ident = op.Contrt
                    });
                }
            }

            return operacoesTratadas;
        }

        // 40 - Tratar quantidade do instrumento modalidade 18
        public static List<Operacao> tratarQtdInstrumentoModalidade18(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.Mod.StartsWith("18") && (op.contabilizacao4966.QtdInst == null))
                {
                    op.contabilizacao4966.QtdInst = "1000";
                }
            }

            return operacoesTratadas;
        }

        // 41 - Tratar identificação da info adicional 0401
        public static List<Operacao> tratarIdentificacaoInfoAdicional04(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                foreach (var info in op.informacoesAdiacionais)
                {
                    if (info.Tp.StartsWith("04"))
                    {
                        info.Ident = op.DtContr;
                    }
                }
            }

            return operacoesTratadas;
        }

        // 42 - Tratar operações com modalidades que requerem info adicional 1201
        public static List<Operacao> tratarOperacoesRequeremInfoAdicional1201(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.Mod == "1511" || op.Mod == "1512" || op.Mod == "2001" || op.Mod == "2002")
                {
                    if (!existeInformacaoAdicional1201(op))
                    {
                        op.informacoesAdiacionais.Add(new InformacaoAdicional
                        {
                            Tp = "1201",
                            Ident = op.Mod,
                            Cd = op.DtContr
                        });
                    }
                }
            }

            return operacoesTratadas;
        }

        // 43 - Tratar vencimentos 330
        public static List<Operacao> tratarOperacoesVencimento330(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (existeVencimentoFaixa330(op))
                {
                    //Setar estágio da carteira para 3
                    op.contabilizacao4966.EstInstFin = "3";
                    //Setar classificação para C3 a C5

                    //op.contabilizacao4966.CartProvMin = "C5";
                    op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                    op.CaracEspecial = setarCaracteristicaEspecial11(op);
                }
            }

            return operacoesTratadas;
        }

        // 44 - Tratar vencimentos 310 e 320
        public static List<Operacao> tratarOperacoesVencimento310E320(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (existeVencimentoFaixa310Ou320(op))
                {
                    //Setar estágio da carteira para 3
                    op.contabilizacao4966.EstInstFin = "3";
                    //Setar classificação para C3 a C5

                    //op.contabilizacao4966.CartProvMin = "C4";
                    op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                }
            }

            return operacoesTratadas;
        }

        // 45 - Tratar informação adicional 1701 V2
        public static List<Operacao> tratarOperacoesInfoAdicional1701V2(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (existeInformacaoAdicional1701(op))
                {
                    //Setar estágio da carteira para 3
                    op.contabilizacao4966.EstInstFin = "3";
                    //Setar classificação para C3 a C5

                    //op.contabilizacao4966.CartProvMin = "C5";
                    op.CaracEspecial = setarCaracteristicaEspecial11(op);
                    op.CaracEspecial = setarCaracteristicaAtivoProblematico19(op);
                }
            }

            return operacoesTratadas;
        }

        // 46 - Tratamento esporádico para casos Orbital
        public static List<Operacao> tratarEsporadicoOrbital(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratatas = new List<Operacao>();
            List<ContratoOrbitallWOAtivo> operacoesAtivoParaWO = new List<ContratoOrbitallWOAtivo>();
            decimal totalVencimentos = 0;
            int diasAtraso = 0;

            operacoesTratatas = operacoes;
            operacoesAtivoParaWO = preencherContratosOrbitalAjuste();

            foreach (var op in operacoesTratatas)
            {

                diasAtraso = Convert.ToInt32(op.DiaAtraso);
                var result = operacoesAtivoParaWO.Where(o => o.NumeroContrato == op.Contrt).FirstOrDefault();

                if (result != null && !retornaOperacaoSaida(op))
                {
                    totalVencimentos = retornaSomaVencimentos(op);
                    op.vencimentos = retornaFaixaVencimentoOperacoesVencidas(diasAtraso, totalVencimentos);
                    //op.contabilizacao4966.VlrContBr = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                    //op.contabilizacao4966.VlrJusto = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                    op.contabilizacao4966.CartProvMin = "C3";
                    op.contabilizacao4966.EstInstFin = "3";
                    op.contabilizacao4966.TratRisc = "S";
                    op.contabilizacao4966.PdEst1 = "N";
                }

            }

            return operacoesTratatas;
        }

        // 47 - Tratamento para informações adicionais das operações grupo de modalidade 04
        public static List<Operacao> tratarInfoAdicionalGrupoModalidade04(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;
            decimal totalVencimentos = 0;


            foreach (var op in operacoesTratadas)
            {
                totalVencimentos = retornaSomaVencimentos(op);

                //if (op.Mod == "0401" || op.Mod == "0402" || op.Mod == "0403" || op.Mod == "0405" ||
                //    op.Mod == "0407" || op.Mod == "0408" || op.Mod == "0409" || op.Mod == "0410" ||
                //    op.Mod == "0411")
                //{
                //    foreach (var info in op.informacoesAdiacionais)
                //    {
                //        if (info.Tp.StartsWith("04"))
                //        {
                //            info.Ident = "1";
                //            info.Cd = null;
                //            info.Tp = op.Mod;
                //            info.Perc = null;
                //            info.Qtd = null;
                //            info.Valor = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                //        }
                //    }
                //}

                foreach (var info in op.informacoesAdiacionais)
                {
                    if (info.Tp != null && info.Tp.StartsWith("04"))
                    {
                        if (op.Mod == "0402" || op.Mod == "0403" || op.Mod == "0405" || op.Mod == "0407" || op.Mod == "0408" || op.Mod == "0409" || op.Mod == "0410" || op.Mod == "0411")
                        {
                            info.Ident = op.DtContr;
                            info.Cd = "1";
                            info.Tp = op.Mod;
                            info.Perc = null;
                            info.Qtd = null;
                            info.Valor = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        }
                        //else
                        //{                            
                        //    info.Cd = null;
                        //    info.Tp = op.Mod;
                        //    info.Perc = null;
                        //    info.Qtd = null;
                        //    info.Valor = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        //}
                    }
                }

            }

            return operacoesTratadas;
        }

        // 48 - Tratamento para caracteristica especial para estágios 1 e 2
        public static List<Operacao> tratarAtivoProblematicoEstagio12V2(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (Convert.ToInt32(op.contabilizacao4966.EstInstFin) != 3)
                {
                    op.CaracEspecial = retirarMarcacaoAtivoProblematico(op);
                }
            }

            return operacoesTratadas;
        }

        // 49 - Tratamento para natureza 02 - incluir info adicional 1001
        public static List<Operacao> tratarNatureza02(List<Operacao> operacoes, string cnpjEmpresa)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.NatuOp != null && op.NatuOp == "02")
                {
                    if (!existeInfoAdicional1001(op))
                    {
                        op.informacoesAdiacionais.Add(new InformacaoAdicional
                        {
                            Tp = "1001",
                            Ident = cnpjEmpresa
                        });
                    }
                    else
                    {
                        foreach (var infoAdicional in op.informacoesAdiacionais)
                        {
                            if (infoAdicional.Tp == "1001")
                            {
                                infoAdicional.Ident = cnpjEmpresa;
                            }
                        }
                    }
                }

            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarIndiciosBacen(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            List<IndicioBacen> indicios = new List<IndicioBacen>();
            indicios = popularIpocsIndiciosBacen();
            operacoesTratadas = operacoes;
            decimal totalVencimentos = 0;

            foreach (var op in operacoesTratadas)
            {
                totalVencimentos = retornaSomaVencimentos(op);
                var result = indicios.Where(o => o.Ipoc == op.IPOC).FirstOrDefault();

                if (result != null)
                {
                    //Incluir uma garantia
                    op.garantias.Add(new Garantia
                    {
                        Tp = "0428",
                        VlrOrig = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", "."),
                    });
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarCaracteristicaEspecialForaDominioBacen(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.CaracEspecial != null)
                {
                    if (op.CaracEspecial.TrimEnd().TrimStart().Length == 0)
                    {
                        op.CaracEspecial = null;
                    }
                    else if (op.CaracEspecial.Contains(";;"))
                    {
                        op.CaracEspecial = op.CaracEspecial.Replace(";;", ";");
                    }
                    else if (op.CaracEspecial[op.CaracEspecial.Length - 1] == ';')
                    {
                        op.CaracEspecial = op.CaracEspecial.Remove(op.CaracEspecial.Length - 1);
                    }
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarOperacoesSemVencimentosComoSaida(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.vencimentos == null && !existeInformacaoAdicionalSaida(op))
                {
                    op.informacoesAdiacionais.Add(new InformacaoAdicional
                    {
                        Tp = "0399"
                    });
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarOperacaoFidc122025(List<Operacao> operacoes)
        {
            List<Fidc122025> contratosFidc = new List<Fidc122025>();
            List<Operacao> operacoesTratadas = operacoes;
            contratosFidc = popularContratosFidc122025();

            foreach (var op in operacoesTratadas)
            {
                var result = contratosFidc.Where(f => f.NumeroContrato == op.Contrt).FirstOrDefault();

                if (result != null)
                {
                    foreach (var infoAdicional in op.informacoesAdiacionais)
                    {
                        if (infoAdicional.Tp.StartsWith("03"))
                        {
                            infoAdicional.Tp = "0304";
                            infoAdicional.Cd = "2025-12-30";
                            infoAdicional.Ident = result.Cessionario;
                            infoAdicional.Valor = result.Valor;
                        }
                    }
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> ajustarVerticeOperacoesAtrasadas(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            return operacoes;
        }

        public static List<Operacao> tratarMotivoAlocacaoEstagio(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;
            string motivoAlocao = String.Empty;
            string dataAlocacao = String.Empty;
            DateTime dataContrato = new DateTime();
            DateTime data4966 = new DateTime(2025, 1, 1);

            foreach (var op in operacoesTratadas)
            {
                dataContrato = new DateTime(Convert.ToInt32(op.DtContr.Substring(0, 4)),
                    Convert.ToInt32(op.DtContr.Substring(5, 2)),
                    Convert.ToInt32(op.DtContr.Substring(8, 2))
                    );

                if (dataContrato < data4966)
                {
                    dataAlocacao = data4966.ToString("yyyy-MM");
                }
                else
                {
                    dataAlocacao = op.DtContr.Substring(0, 7);
                }

                if (op.contabilizacao4966.EstInstFin == "1" || op.contabilizacao4966.EstInstFin == "2")
                {
                    motivoAlocao = op.contabilizacao4966.EstInstFin + "01";
                }
                else
                {
                    if (Convert.ToInt32(op.DiaAtraso) <= 90)
                    {
                        motivoAlocao = "301";
                    }
                    else
                    {
                        motivoAlocao = "302";
                    }
                }


                //Se não existe a informação de alocação do estágio no arquivo, incluir
                if (op.contabilizacao4966.estagios == null || op.contabilizacao4966.estagios.Count == 0)
                {

                    //Incluir o motivo de alocação do estágio e a data de alocação
                    op.contabilizacao4966.estagios.Add(new Estagio
                    {
                        DtAlocacao = dataAlocacao,
                        Motivo = motivoAlocao
                    });
                }
                else
                {
                    foreach (var estagio in op.contabilizacao4966.estagios)
                    {
                        if (estagio.DtAlocacao == null || estagio.DtAlocacao.Length > 7)
                            estagio.DtAlocacao = dataAlocacao;

                        //if (estagio.Motivo == null || estagio.Motivo.Length < 3)
                        estagio.Motivo = motivoAlocao;

                    }
                }
            }

            return operacoesTratadas;
        }

        public static List<Operacao> suprimirMotivoAlocacaoEstagio(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                op.contabilizacao4966.estagios = null;
            }

            return operacoesTratadas;
        }

        public static List<Operacao> tratarPerdasInstrumentoContabilizacao4966(List<Operacao> operacoes)
        {
            List<Operacao> operacoesTratadas = new List<Operacao>();
            operacoesTratadas = operacoes;

            foreach (var op in operacoesTratadas)
            {
                if (op.contabilizacao4966.perdas == null)
                {
                    continue;
                }
                else
                {
                    if (op.contabilizacao4966.perdas.Count > 0)
                    {
                        foreach (var perda in op.contabilizacao4966.perdas)
                        {
                            if (perda.VlrPerda == null)
                            {
                                perda.VlrPerda = "0.00";
                            }
                        }
                    }
                }
            }

            return operacoesTratadas;
        }

        #endregion

        #region ############ TRATAMENTO PARA CLIENTE ############

        public static List<Cliente> tratarCdCliente(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;

            foreach (var cliente in clientesTratados)
            {
                if (cliente.Cd.TrimStart().TrimEnd().Length > 11)
                {
                    string cdCliente = cliente.Cd.Substring(0, 8);
                    cliente.Cd = cdCliente;
                }
            }


            return clientesTratados;
        }

        public static List<Cliente> tratarCdClienteV2(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;
            string tipoCliente = String.Empty;

            foreach (var cli in clientesTratados)
            {
                if (cli.Tp == null || cli.Tp.TrimStart().TrimEnd().Length == 0)
                {
                    tipoCliente = (cli.Cd.Length <= 8 || cli.Cd.Length > 11) ? "2" : "1";
                }
                else
                {
                    tipoCliente = cli.Tp;
                }

                if (tipoCliente == "2" || tipoCliente == "4" || tipoCliente == "6")
                {
                    string cdCliente = cli.Cd.Substring(0, 8);
                    cli.Cd = cdCliente;
                    cli.Tp = tipoCliente;
                }
            }

            return clientesTratados;
        }

        // 1 - Tratamento para o tipo de cliente se vier nulo
        public static List<Cliente> tratarTipoCliente(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;

            foreach (var cliente in clientesTratados)
            {
                if (cliente.Tp == null || cliente.Tp.TrimStart().TrimEnd().Length == 0)
                {
                    cliente.Tp = cliente.Cd.Length == 11 ? "1" : "2";
                }
            }

            return clientesTratados;
        }

        // 2 - Tratamento para o campo Autorização consulta ao Bacen se vier nulo
        public static List<Cliente> tratarAutorizacaoConsultaBacen(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;

            foreach (var cliente in clientesTratados)
            {
                if (cliente.Autorzc == null)
                {
                    cliente.Autorzc = "N";
                }
            }

            return clientesTratados;
        }

        // 3 - Tratamento para o porte do cliente se vier nulo
        public static List<Cliente> tratarPorteCliente(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;
            decimal faturamento = 0;


            foreach (var cliente in clientesTratados)
            {

                //if (cliente.PorteCli == null)
                //{

                if (cliente.FatAnual != null)
                {
                    faturamento = decimal.Parse(cliente.FatAnual.Replace(".", ","));

                    //Tratamento para pessoa física
                    if (Convert.ToInt32(cliente.Tp) == 1 || Convert.ToInt32(cliente.Tp) == 3 || Convert.ToInt32(cliente.Tp) == 5)
                    {
                        if (faturamento <= 1)
                        {
                            cliente.PorteCli = "0";
                        }
                        else if (faturamento <= salarioMinimo)
                        {
                            cliente.PorteCli = "2";
                        }
                        else if (faturamento > salarioMinimo && faturamento <= (salarioMinimo * 2))
                        {
                            cliente.PorteCli = "3";
                        }
                        else if (faturamento > (salarioMinimo * 2) && faturamento <= (salarioMinimo * 3))
                        {
                            cliente.PorteCli = "4";
                        }
                        else if (faturamento > (salarioMinimo * 3) && faturamento <= (salarioMinimo * 5))
                        {
                            cliente.PorteCli = "5";
                        }
                        else if (faturamento > (salarioMinimo * 5) && faturamento <= (salarioMinimo * 10))
                        {
                            cliente.PorteCli = "6";
                        }
                        else if (faturamento > (salarioMinimo * 10) && faturamento <= (salarioMinimo * 20))
                        {
                            cliente.PorteCli = "7";
                        }
                        else
                        {
                            cliente.PorteCli = "8";
                        }
                    }
                    else //Tratamento para PJ e faturamento != 0
                    {
                        if (faturamento <= 360000)
                        {
                            cliente.PorteCli = "1";
                        }
                        else if (faturamento <= 4800000)
                        {
                            cliente.PorteCli = "2";
                        }
                        else if (faturamento <= 300000000)
                        {
                            cliente.PorteCli = "3";
                        }
                        else
                        {
                            cliente.PorteCli = "4";
                        }
                    }
                }
                else
                {
                    cliente.PorteCli = "0";
                }
                //}
            }

            return clientesTratados;
        }

        public static List<Cliente> tratarPorteClienteFaturamentoIgual1(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;
            decimal faturamento = 0;


            foreach (var cliente in clientesTratados)
            {

                if (cliente.FatAnual != null && cliente.FatAnual == "1.00")
                {
                    cliente.PorteCli = "0";
                }

            }

            return clientesTratados;
        }

        public static List<Cliente> tratarPorteClienteIndiciosBacen(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            List<PorteCliente> clientesTratar = new List<PorteCliente>();
            clientesTratar = popularClientesTratarPorte();
            clientesTratados = clientes;

            foreach (var cliente in clientesTratados)
            {
                var result = clientesTratar.Where(c => c.CodigoCliente == cliente.Cd).FirstOrDefault();
                decimal faturamento = 0;

                if (result != null)
                {
                    if (cliente.FatAnual != null)
                    {
                        faturamento = decimal.Parse(cliente.FatAnual.Replace(".", ","));

                        //Tratamento para pessoa física
                        if (Convert.ToInt32(cliente.Tp) == 1 || Convert.ToInt32(cliente.Tp) == 3 || Convert.ToInt32(cliente.Tp) == 5)
                        {
                            if (faturamento <= 1)
                            {
                                cliente.PorteCli = "1";
                            }
                            else if (faturamento <= salarioMinimo)
                            {
                                cliente.PorteCli = "2";
                            }
                            else if (faturamento > salarioMinimo && faturamento <= (salarioMinimo * 2))
                            {
                                cliente.PorteCli = "3";
                            }
                            else if (faturamento > (salarioMinimo * 2) && faturamento <= (salarioMinimo * 3))
                            {
                                cliente.PorteCli = "4";
                            }
                            else if (faturamento > (salarioMinimo * 3) && faturamento <= (salarioMinimo * 5))
                            {
                                cliente.PorteCli = "5";
                            }
                            else if (faturamento > (salarioMinimo * 5) && faturamento <= (salarioMinimo * 10))
                            {
                                cliente.PorteCli = "6";
                            }
                            else if (faturamento > (salarioMinimo * 10) && faturamento <= (salarioMinimo * 20))
                            {
                                cliente.PorteCli = "7";
                            }
                            else
                            {
                                cliente.PorteCli = "8";
                            }
                        }
                        else //Tratamento para PJ e faturamento != 0
                        {
                            if (faturamento <= 360000)
                            {
                                cliente.PorteCli = "1";
                            }
                            else if (faturamento <= 4800000)
                            {
                                cliente.PorteCli = "2";
                            }
                            else if (faturamento <= 300000000)
                            {
                                cliente.PorteCli = "3";
                            }
                            else
                            {
                                cliente.PorteCli = "4";
                            }
                        }
                    }
                    else
                    {
                        cliente.PorteCli = "0";
                    }
                }
            }

            return clientesTratados;
        }


        // 4 - Tratamento para o tipo de controle para PJ
        public static List<Cliente> tratarTipoControlePessoaJuridica(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;

            foreach (var cliente in clientesTratados)
            {

                if (Convert.ToInt32(cliente.Tp) == 2 || Convert.ToInt32(cliente.Tp) == 4 || Convert.ToInt32(cliente.Tp) == 6)
                {
                    if (cliente.TpCtrl == null)
                    {
                        cliente.TpCtrl = "01";
                    }
                }
                else
                {
                    cliente.TpCtrl = null;
                }

            }

            return clientesTratados;
        }

        // 5 - Tratamento para inicio de relacionamento do cliente, se vier nulo
        public static List<Cliente> tratarInicioRelacionamento(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;

            foreach (var cliente in clientesTratados)
            {
                if (cliente.IniRelactCli == null)
                {
                    cliente.IniRelactCli = cliente.operacoes.Min(o => o.DtContr);
                }
            }

            return clientesTratados;
        }

        // 6 - Tratamento para faturamento, se vier nulo
        public static List<Cliente> tratarFaturamento(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;
            decimal faturamento = 0;

            foreach (var cliente in clientesTratados)
            {
                if (cliente.FatAnual == null)
                {
                    cliente.FatAnual = "1500.00";
                }
            }

            return clientesTratados;
        }

        // 7 - Tratamento para tipo de controle para PJ
        public static List<Cliente> tratarTipoControlePJ(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();

            foreach (var cliente in clientesTratados.ToList())
            {
                if (Convert.ToInt32(cliente.Tp) == 2 || Convert.ToInt32(cliente.Tp) == 4 || Convert.ToInt32(cliente.Tp) == 6)
                {
                    if (cliente.TpCtrl == null)
                    {
                        cliente.TpCtrl = "0";
                    }
                }
                else
                {
                    cliente.TpCtrl = null;
                }
            }

            return clientesTratados;
        }


        // 8 - Tratamento para o tipo de cliente
        public static List<Cliente> tratarTipoClienteNuloVazio(List<Cliente> clientes)
        {
            List<Cliente> clientesTratados = new List<Cliente>();
            clientesTratados = clientes;

            foreach (var cliente in clientesTratados)
            {
                if (cliente.Tp == null || cliente.Tp.TrimStart().TrimStart().Length == 0)
                {
                    if (cliente.Cd.Length == 1)
                    {
                        cliente.Tp = "1";
                    }
                    else
                    {
                        cliente.Tp = "2";
                    }
                }
            }

            return clientesTratados;
        }

        #endregion

        #region ###### METODOS PRIVADOS - AUXILIARES ######

        private static string retornaCaracteristicaEspecialTratada(Operacao operacao)
        {
            string caracEspecial = String.Empty;

            if (operacao.CaracEspecial != null)
            {
                caracEspecial = caracEspecial.Replace(";;", "");
                char ultimoCaractere = caracEspecial[caracEspecial.Length - 1];

                if (ultimoCaractere == ';')
                {
                    caracEspecial = caracEspecial.Substring(0, caracEspecial.Length - 2);
                }

                if (caracEspecial.StartsWith(";"))
                {
                    caracEspecial = caracEspecial.Substring(1, caracEspecial.Length - 2);
                }

            }

            return caracEspecial;
        }

        private static bool existeVencimentoFaixa330(Operacao operacao)
        {
            bool retorno = false;

            if (operacao.vencimentos != null)
            {
                if (operacao.vencimentos.v330 != null)
                {
                    retorno = true;
                }
            }

            return retorno;
        }

        private static bool existeVencimentoFaixa310Ou320(Operacao operacao)
        {
            bool retorno = false;

            if (operacao.vencimentos != null)
            {
                if (operacao.vencimentos.v310 != null || operacao.vencimentos.v320 != null)
                {
                    retorno = true;
                }
            }

            return retorno;
        }

        private static bool existeInfoAdicional1001(Operacao op)
        {
            bool existe = false;

            foreach (var infoAdicional in op.informacoesAdiacionais)
            {
                if (infoAdicional.Tp == "1001")
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }

        private static bool existeInformacaoAdicional0408(Operacao op)
        {
            bool existe = false;

            foreach (var infoAdicional in op.informacoesAdiacionais)
            {
                if (infoAdicional.Tp == "0408")
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }

        private static bool existeInformacaoAdicional1701(Operacao op)
        {
            bool existe = false;

            foreach (var infoAdicional in op.informacoesAdiacionais)
            {
                if (infoAdicional.Tp == "1701")
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }

        private static bool existeInformacaoAdicional0401(Operacao op)
        {
            bool existe = false;

            foreach (var infoAdicional in op.informacoesAdiacionais)
            {
                if (infoAdicional.Tp == "0401")
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }

        private static bool existeInformacaoAdicional1201(Operacao op)
        {
            bool existe = false;

            foreach (var infoAdicional in op.informacoesAdiacionais)
            {
                if (infoAdicional.Tp == "1201")
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }

        private static bool existeInformacaoAdicional23(Operacao op)
        {
            bool existe = false;

            foreach (var infoAdicional in op.informacoesAdiacionais)
            {
                if (infoAdicional.Tp != null && infoAdicional.Tp.StartsWith("23"))
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }

        private static bool existeGarantia0674(Operacao op)
        {
            bool existe = false;

            foreach (var garantia in op.garantias)
            {
                if (garantia.Tp == "0674")
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }

        private static bool existeInformacaoAdicionalSaida(Operacao op)
        {
            bool existe = false;

            foreach (var infoAdicional in op.informacoesAdiacionais)
            {
                if (infoAdicional.Tp != null && infoAdicional.Tp.StartsWith("03"))
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }

        public static bool retornarOperacaoSaida(Operacao operacao)
        {
            bool retorno = false;

            foreach (var infAdicional in operacao.informacoesAdiacionais)
            {
                if (infAdicional.Tp != null && infAdicional.Tp.StartsWith("03"))
                {
                    retorno = true;
                    break;
                }
            }

            return retorno;
        }

        public static bool existeMotivoAlocacaoEstagio(Operacao operacao, string motivoAlocacaoEstagio)
        {
            bool existe = false;

            foreach (var motivoAlocacao in operacao.contabilizacao4966.estagios)
            {
                if (motivoAlocacao.Motivo == motivoAlocacaoEstagio)
                {
                    existe = true;
                }
            }

            return existe;
        }

        public static bool operacaoEmPrejuizo(Operacao op)
        {
            bool retorno = false;

            if (op.vencimentos != null)
            {
                if (op.vencimentos.v310 != null || op.vencimentos.v320 != null || op.vencimentos.v330 != null)
                {
                    retorno = true;
                }
            }

            return retorno;
        }

        private static List<ContratoInfoAdd0408Gar0674> popularContratosInfoAdicional0408Garantia0674()
        {
            List<ContratoInfoAdd0408Gar0674> operacoes = new List<ContratoInfoAdd0408Gar0674>();
            //Removendo duplicidades
            operacoes = operacoes.GroupBy(o => o.NumeroContrato).Select(o => o.FirstOrDefault()).ToList();

            return operacoes;
        }
        private static List<ContratoWOParaAtivoMenor541Dias> popularContratoWOParaAtivoMenor541Dias()
        {
            List<ContratoWOParaAtivoMenor541Dias> operacoes = new List<ContratoWOParaAtivoMenor541Dias>();
            return operacoes;
        }
        private static List<ContratoIrrecuperavel> popularContratosIrrecuperaveis()
        {
            List<ContratoIrrecuperavel> operacoesIrrecuperaveis = new List<ContratoIrrecuperavel>();
            return operacoesIrrecuperaveis;
        }

        private static string setarCaracteristicaAtivoProblematico19(Operacao operacao)
        {
            string caracEspecial = String.Empty;
            caracEspecial = operacao.CaracEspecial;

            if (caracEspecial == null || caracEspecial.TrimEnd().TrimStart().Length == 0)
            {
                caracEspecial = "19";
            }
            else
            {
                if (!operacao.CaracEspecial.Contains("19"))
                    caracEspecial = String.Concat(caracEspecial, ";19");

            }


            return caracEspecial;
        }

        public static string setarCaracteristicaAtivoProblematico(Operacao operacao)
        {
            string caracEspecial = String.Empty;
            caracEspecial = operacao.CaracEspecial;

            if (caracEspecial == null || caracEspecial.TrimEnd().TrimStart().Length == 0)
            {
                caracEspecial = "19";
            }
            else
            {
                if (!operacao.CaracEspecial.Contains("19"))
                    caracEspecial = String.Concat(caracEspecial, ";19");

            }


            return caracEspecial;
        }

        private static string setarCaracteristicaEspecial38(Operacao operacao)
        {
            string caracEspecial = String.Empty;
            caracEspecial = operacao.CaracEspecial;

            if (caracEspecial == null || caracEspecial.TrimEnd().TrimStart().Length == 0)
            {
                caracEspecial = "38";
            }
            else
            {
                if (!operacao.CaracEspecial.Contains("38"))
                    caracEspecial = String.Concat(caracEspecial, ";38");

            }


            return caracEspecial;
        }

        private static string setarCaracteristicaEspecial39(Operacao operacao)
        {
            string caracEspecial = String.Empty;
            caracEspecial = operacao.CaracEspecial;

            if (caracEspecial == null || caracEspecial.TrimEnd().TrimStart().Length == 0)
            {
                caracEspecial = "39";
            }
            else
            {
                if (!operacao.CaracEspecial.Contains("39"))
                    caracEspecial = String.Concat(caracEspecial, ";39");

            }


            return caracEspecial;
        }

        private static string setarCaracteristicaEspecial11(Operacao operacao)
        {
            string caracEspecial = String.Empty;
            caracEspecial = operacao.CaracEspecial;

            if (caracEspecial == null || caracEspecial.TrimEnd().TrimStart().Length == 0)
            {
                caracEspecial = "11";
            }
            else
            {
                if (!caracEspecial.Contains("11"))
                    caracEspecial = String.Concat(caracEspecial, ";11");

            }


            return caracEspecial;
        }
        private static string retornaCaractaristicaEspecialRegraPJ541DiasAtraso(Operacao operacao)
        {
            string caracEspecial = String.Empty;

            if (operacao.CaracEspecial == null || operacao.CaracEspecial.TrimEnd().TrimStart().Length == 0)
            {
                caracEspecial = "11;19";
            }
            else
            {
                if (!operacao.CaracEspecial.Contains("11"))
                    caracEspecial = String.Concat(operacao.CaracEspecial, ";11");

                if (!operacao.CaracEspecial.Contains("19"))
                    caracEspecial = String.Concat(operacao.CaracEspecial, ";19");

            }


            return caracEspecial;
        }

        private static int retornaDiasAtraso(DateTime dataInicial, DateTime dataFinal, string? diaAtraso)
        {
            int diasAtraso = 0;

            if (diasAtraso == null)
            {
                diasAtraso = (dataFinal - dataInicial).Days;
            }
            else
            {
                diasAtraso = Convert.ToInt32(diaAtraso);
            }

            return diasAtraso;
        }

        private static bool retornaOperacaoSaida(Operacao operacao)
        {
            bool retorno = false;

            foreach (var infAdicional in operacao.informacoesAdiacionais)
            {
                if (infAdicional.Tp != null && infAdicional.Tp.StartsWith("03"))
                {
                    retorno = true;
                    break;
                }
            }

            return retorno;
        }

        private static bool retornaTemInformacao1701(Operacao operacao)
        {
            bool retorno = false;

            foreach (var info in operacao.informacoesAdiacionais)
            {
                if (info.Tp == "1701")
                {
                    retorno = true;
                    break;
                }
            }

            return retorno;
        }

        private static int retornaQuantidadeVencimentos(Operacao operacao)
        {
            int quantidade = 0;

            if (operacao.vencimentos.v110 != null)
                quantidade += 1;

            if (operacao.vencimentos.v120 != null)
                quantidade += 1;

            if (operacao.vencimentos.v130 != null)
                quantidade += 1;

            if (operacao.vencimentos.v140 != null)
                quantidade += 1;

            if (operacao.vencimentos.v150 != null)
                quantidade += 1;

            if (operacao.vencimentos.v160 != null)
                quantidade += 1;

            if (operacao.vencimentos.v165 != null)
                quantidade += 1;

            if (operacao.vencimentos.v170 != null)
                quantidade += 1;

            if (operacao.vencimentos.v175 != null)
                quantidade += 1;

            if (operacao.vencimentos.v180 != null)
                quantidade += 1;

            if (operacao.vencimentos.v190 != null)
                quantidade += 1;

            if (operacao.vencimentos.v199 != null)
                quantidade += 1;

            if (operacao.vencimentos.v20 != null)
                quantidade += 1;

            if (operacao.vencimentos.v205 != null)
                quantidade += 1;

            if (operacao.vencimentos.v210 != null)
                quantidade += 1;

            if (operacao.vencimentos.v220 != null)
                quantidade += 1;

            if (operacao.vencimentos.v230 != null)
                quantidade += 1;

            if (operacao.vencimentos.v240 != null)
                quantidade += 1;

            if (operacao.vencimentos.v245 != null)
                quantidade += 1;

            if (operacao.vencimentos.v250 != null)
                quantidade += 1;

            if (operacao.vencimentos.v255 != null)
                quantidade += 1;

            if (operacao.vencimentos.v260 != null)
                quantidade += 1;

            if (operacao.vencimentos.v270 != null)
                quantidade += 1;

            if (operacao.vencimentos.v280 != null)
                quantidade += 1;

            if (operacao.vencimentos.v290 != null)
                quantidade += 1;

            if (operacao.vencimentos.v310 != null)
                quantidade += 1;

            if (operacao.vencimentos.v320 != null)
                quantidade += 1;

            if (operacao.vencimentos.v330 != null)
                quantidade += 1;

            if (operacao.vencimentos.v40 != null)
                quantidade += 1;

            if (operacao.vencimentos.v60 != null)
                quantidade += 1;

            if (operacao.vencimentos.v80 != null)
                quantidade += 1;

            return quantidade;
        }

        private static Decimal retornaSomaVencimentos(Operacao operacao)
        {
            Decimal totalVencimentos = 0M;

            if (retornaOperacaoSaida(operacao))
            {
                totalVencimentos = decimal.Parse("0.00".Replace(".", ","));
            }
            else
            {

                if (operacao.vencimentos.v110 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v110.Replace(".", ","));

                if (operacao.vencimentos.v120 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v120.Replace(".", ","));

                if (operacao.vencimentos.v130 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v130.Replace(".", ","));

                if (operacao.vencimentos.v140 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v140.Replace(".", ","));

                if (operacao.vencimentos.v150 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v150.Replace(".", ","));

                if (operacao.vencimentos.v160 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v160.Replace(".", ","));

                if (operacao.vencimentos.v165 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v165.Replace(".", ","));

                if (operacao.vencimentos.v170 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v170.Replace(".", ","));

                if (operacao.vencimentos.v175 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v175.Replace(".", ","));

                if (operacao.vencimentos.v180 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v180.Replace(".", ","));

                if (operacao.vencimentos.v190 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v190.Replace(".", ","));

                if (operacao.vencimentos.v199 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v199.Replace(".", ","));

                if (operacao.vencimentos.v20 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v20.Replace(".", ","));

                if (operacao.vencimentos.v205 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v205.Replace(".", ","));

                if (operacao.vencimentos.v210 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v210.Replace(".", ","));

                if (operacao.vencimentos.v220 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v220.Replace(".", ","));

                if (operacao.vencimentos.v230 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v230.Replace(".", ","));

                if (operacao.vencimentos.v240 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v240.Replace(".", ","));

                if (operacao.vencimentos.v245 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v245.Replace(".", ","));

                if (operacao.vencimentos.v250 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v250.Replace(".", ","));

                if (operacao.vencimentos.v255 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v255.Replace(".", ","));

                if (operacao.vencimentos.v260 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v260.Replace(".", ","));

                if (operacao.vencimentos.v270 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v270.Replace(".", ","));

                if (operacao.vencimentos.v280 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v280.Replace(".", ","));

                if (operacao.vencimentos.v290 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v290.Replace(".", ","));

                if (operacao.vencimentos.v310 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v310.Replace(".", ","));

                if (operacao.vencimentos.v320 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v320.Replace(".", ","));

                if (operacao.vencimentos.v330 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v330.Replace(".", ","));

                if (operacao.vencimentos.v40 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v40.Replace(".", ","));

                if (operacao.vencimentos.v60 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v60.Replace(".", ","));

                if (operacao.vencimentos.v80 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v80.Replace(".", ","));
            }


            return totalVencimentos;
        }

        public static Decimal retornarSomaVencimentos(Operacao operacao)
        {
            Decimal totalVencimentos = 0M;

            if (retornaOperacaoSaida(operacao))
            {
                totalVencimentos = decimal.Parse("0.00".Replace(".", ","));
            }
            else
            {

                if (operacao.vencimentos.v110 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v110.Replace(".", ","));

                if (operacao.vencimentos.v120 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v120.Replace(".", ","));

                if (operacao.vencimentos.v130 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v130.Replace(".", ","));

                if (operacao.vencimentos.v140 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v140.Replace(".", ","));

                if (operacao.vencimentos.v150 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v150.Replace(".", ","));

                if (operacao.vencimentos.v160 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v160.Replace(".", ","));

                if (operacao.vencimentos.v165 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v165.Replace(".", ","));

                if (operacao.vencimentos.v170 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v170.Replace(".", ","));

                if (operacao.vencimentos.v175 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v175.Replace(".", ","));

                if (operacao.vencimentos.v180 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v180.Replace(".", ","));

                if (operacao.vencimentos.v190 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v190.Replace(".", ","));

                if (operacao.vencimentos.v199 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v199.Replace(".", ","));

                if (operacao.vencimentos.v20 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v20.Replace(".", ","));

                if (operacao.vencimentos.v205 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v205.Replace(".", ","));

                if (operacao.vencimentos.v210 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v210.Replace(".", ","));

                if (operacao.vencimentos.v220 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v220.Replace(".", ","));

                if (operacao.vencimentos.v230 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v230.Replace(".", ","));

                if (operacao.vencimentos.v240 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v240.Replace(".", ","));

                if (operacao.vencimentos.v245 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v245.Replace(".", ","));

                if (operacao.vencimentos.v250 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v250.Replace(".", ","));

                if (operacao.vencimentos.v255 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v255.Replace(".", ","));

                if (operacao.vencimentos.v260 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v260.Replace(".", ","));

                if (operacao.vencimentos.v270 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v270.Replace(".", ","));

                if (operacao.vencimentos.v280 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v280.Replace(".", ","));

                if (operacao.vencimentos.v290 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v290.Replace(".", ","));

                if (operacao.vencimentos.v310 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v310.Replace(".", ","));

                if (operacao.vencimentos.v320 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v320.Replace(".", ","));

                if (operacao.vencimentos.v330 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v330.Replace(".", ","));

                if (operacao.vencimentos.v40 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v40.Replace(".", ","));

                if (operacao.vencimentos.v60 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v60.Replace(".", ","));

                if (operacao.vencimentos.v80 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v80.Replace(".", ","));
            }


            return totalVencimentos;
        }

        public static Decimal retornarSomaVencimentosDLOExposicaoCliente(Operacao operacao)
        {
            Decimal totalVencimentos = 0M;

            if (retornaOperacaoSaida(operacao))
            {
                totalVencimentos = decimal.Parse("0.00".Replace(".", ","));
            }
            else
            {
                if (operacao.vencimentos.v20 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v20.Replace(".", ","));

                if (operacao.vencimentos.v40 != null)
                    totalVencimentos += decimal.Parse(operacao.vencimentos.v40.Replace(".", ","));
            }


            return totalVencimentos;
        }

        private static string retirarMarcacaoAtivoProblematico(Operacao operacao)
        {
            string caracEspecial = String.Empty;
            caracEspecial = operacao.CaracEspecial;

            if (caracEspecial != null)
            {
                if (caracEspecial.Contains("11"))
                    caracEspecial = caracEspecial.Replace("11", "");

                if (operacao.CaracEspecial.Contains("19"))
                    caracEspecial = caracEspecial.Replace("19", "");

                caracEspecial = caracEspecial.Replace(";;", ";");
            }

            return caracEspecial;
        }

        public static string removerMarcacaoAtivoProblematico(Operacao operacao)
        {
            string caracEspecial = String.Empty;
            caracEspecial = operacao.CaracEspecial;

            if (caracEspecial != null)
            {
                if (caracEspecial.Contains("11"))
                    caracEspecial = caracEspecial.Replace("11", "");

                if (operacao.CaracEspecial.Contains("19"))
                    caracEspecial = caracEspecial.Replace("19", "");

                caracEspecial = caracEspecial.Replace(";;", ";");
            }

            return caracEspecial;
        }

        private static List<InformacaoAdicional> retornarInfoAdicionaisSemDuplicidade(Operacao op)
        {
            return op.informacoesAdiacionais.GroupBy(i => i.Tp).Select(i => i.First()).ToList();
        }


        public static List<Garantia> retornarGarantiasSemDuplicidades(Operacao op)
        {
            return op.garantias.GroupBy(g => g.Tp).Select(g => g.First()).ToList();
        }

        private static List<Garantia> retornarGarantiasSemDuplicidade(Operacao op)
        {
            return op.garantias.GroupBy(g => g.Tp).Select(g => g.First()).ToList();
        }

        private static List<Operacao> retornarIPOCSemDuplicidade(Cliente cliente)
        {
            return cliente.operacoes.GroupBy(o => o.IPOC).Select(o => o.FirstOrDefault()).ToList();
        }

        private static List<Estagio> retornarMotivoAlocacaoSemDuplicidade(Operacao operacao)
        {
            return operacao.contabilizacao4966.estagios.GroupBy(g => g.Motivo).Select(g => g.First()).ToList();
        }

        private static bool operacaoEstaEmPrejuizo(Operacao op)
        {
            bool retorno = false;

            if (op.vencimentos != null)
            {
                if (op.vencimentos.v310 != null || op.vencimentos.v320 != null || op.vencimentos.v330 != null)
                {
                    retorno = true;
                }
            }

            return retorno;
        }

        private static bool operacaoEstaVencida(Operacao op)
        {
            bool retorno = false;

            if (op.vencimentos != null)
            {
                if (op.vencimentos.v205 != null || op.vencimentos.v210 != null || op.vencimentos.v220 != null ||
                    op.vencimentos.v230 != null || op.vencimentos.v240 != null || op.vencimentos.v245 != null ||
                    op.vencimentos.v250 != null || op.vencimentos.v255 != null || op.vencimentos.v260 != null ||
                    op.vencimentos.v270 != null || op.vencimentos.v280 != null || op.vencimentos.v290 != null
                    )
                {
                    retorno = true;
                }
            }

            return retorno;
        }

        private static Vencimento retornaFaixaVencimentoOperacoesVencidas(int diasAtraso, decimal totalVencimentos)
        {
            Vencimento vencimento = new Vencimento();

            switch
                (
                    diasAtraso <= 14 ? "205" :
                    diasAtraso <= 30 ? "210" :
                    diasAtraso <= 60 ? "220" :
                    diasAtraso <= 90 ? "230" :
                    diasAtraso <= 120 ? "240" :
                    diasAtraso <= 150 ? "245" :
                    diasAtraso <= 180 ? "250" :
                    diasAtraso <= 240 ? "255" :
                    diasAtraso <= 300 ? "260" :
                    diasAtraso <= 360 ? "270" :
                    diasAtraso <= 540 ? "280" :
                    "290"
                )
            {
                case "205":
                    {
                        vencimento.v205 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "210":
                    {
                        vencimento.v210 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "220":
                    {
                        vencimento.v220 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "230":
                    {
                        vencimento.v230 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "240":
                    {
                        vencimento.v240 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "245":
                    {
                        vencimento.v245 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }


                case "250":
                    {
                        vencimento.v250 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "255":
                    {
                        vencimento.v255 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "260":
                    {
                        vencimento.v260 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "270":
                    {
                        vencimento.v270 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "280":
                    {
                        vencimento.v280 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "290":
                    {
                        vencimento.v290 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }
            }


            return vencimento;
        }

        private static Vencimento retornaFaixaVencimentoOperacoesVencidasV2(int diasAtraso, decimal totalVencimentos)
        {
            Vencimento vencimento = new Vencimento();

            switch
                (
                    diasAtraso <= 14 ? "205" :
                    diasAtraso <= 30 ? "210" :
                    diasAtraso <= 60 ? "220" :
                    diasAtraso <= 90 ? "230" :
                    diasAtraso <= 120 ? "240" :
                    diasAtraso <= 150 ? "245" :
                    diasAtraso <= 180 ? "250" :
                    diasAtraso <= 240 ? "255" :
                    diasAtraso <= 300 ? "260" :
                    diasAtraso <= 360 ? "270" :
                    diasAtraso <= 540 ? "280" :
                    "290"
                )
            {
                case "205":
                    {
                        vencimento.v205 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v210 = null;
                        vencimento.v220 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "210":
                    {
                        vencimento.v210 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v220 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "220":
                    {
                        vencimento.v220 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "230":
                    {
                        vencimento.v230 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v220 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "240":
                    {
                        vencimento.v240 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v220 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "245":
                    {
                        vencimento.v245 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v220 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }


                case "250":
                    {
                        vencimento.v250 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v220 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "255":
                    {
                        vencimento.v255 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v220 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "260":
                    {
                        vencimento.v260 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v220 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "270":
                    {
                        vencimento.v270 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v220 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "280":
                    {
                        vencimento.v280 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v220 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "290":
                    {
                        vencimento.v290 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v220 = null;
                        break;
                    }
            }


            return vencimento;
        }

        #region ##### NOVO TRATAMENTO METODOS PUBLICOS PARA PEGAR FAIXA DE VENCIMENTO
        public static Vencimento retornarFaixaVencimentoVencidos(int diasAtraso, string totalVencimentos)
        {
            Vencimento vencimento = new Vencimento();

            switch
                (
                    diasAtraso <= 14 ? "205" :
                    diasAtraso <= 30 ? "210" :
                    diasAtraso <= 60 ? "220" :
                    diasAtraso <= 90 ? "230" :
                    diasAtraso <= 120 ? "240" :
                    diasAtraso <= 150 ? "245" :
                    diasAtraso <= 180 ? "250" :
                    diasAtraso <= 240 ? "255" :
                    diasAtraso <= 300 ? "260" :
                    diasAtraso <= 360 ? "270" :
                    diasAtraso <= 540 ? "280" :
                    "290"
                )
            {
                case "205":
                    {
                        vencimento.v205 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v210 = null;
                        vencimento.v220 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "210":
                    {
                        vencimento.v210 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v220 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "220":
                    {
                        vencimento.v220 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "230":
                    {
                        vencimento.v230 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v220 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "240":
                    {
                        vencimento.v240 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v220 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "245":
                    {
                        vencimento.v245 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v220 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }


                case "250":
                    {
                        vencimento.v250 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v220 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "255":
                    {
                        vencimento.v255 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v220 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "260":
                    {
                        vencimento.v260 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v220 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "270":
                    {
                        vencimento.v270 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v220 = null;
                        vencimento.v280 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "280":
                    {
                        vencimento.v280 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v220 = null;
                        vencimento.v290 = null;
                        break;
                    }

                case "290":
                    {
                        vencimento.v290 = totalVencimentos.Replace(",", ".");
                        vencimento.v20 = null;
                        vencimento.v40 = null;
                        vencimento.v60 = null;
                        vencimento.v80 = null;
                        vencimento.v110 = null;
                        vencimento.v120 = null;
                        vencimento.v130 = null;
                        vencimento.v140 = null;
                        vencimento.v150 = null;
                        vencimento.v160 = null;
                        vencimento.v165 = null;
                        vencimento.v170 = null;
                        vencimento.v175 = null;
                        vencimento.v180 = null;
                        vencimento.v190 = null;
                        vencimento.v199 = null;
                        vencimento.v205 = null;
                        vencimento.v210 = null;
                        vencimento.v230 = null;
                        vencimento.v240 = null;
                        vencimento.v245 = null;
                        vencimento.v250 = null;
                        vencimento.v255 = null;
                        vencimento.v260 = null;
                        vencimento.v270 = null;
                        vencimento.v280 = null;
                        vencimento.v220 = null;
                        break;
                    }
            }

            return vencimento;
        }

        public static Vencimento retornarFaixaVencimentoPrejuizo(int diasAtraso, string totalVencimentos)
        {
            Vencimento vencimento = new Vencimento();

            switch
                (
                    diasAtraso <= 360 ? "310" :
                    diasAtraso <= 1440 ? "320" :
                    "330"
                )
            {
                case "310":
                    {
                        vencimento.v310 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "320":
                    {
                        vencimento.v320 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "330":
                    {
                        vencimento.v330 = totalVencimentos.Replace(",", ".");
                        break;
                    }

            }


            return vencimento;
        }

        public static Vencimento retornaFaixaVencimentoVencer(int diasAtraso, string totalVencimentos)
        {
            Vencimento vencimento = new Vencimento();

            switch
                (
                    diasAtraso <= 30 ? "110" :
                    diasAtraso <= 60 ? "120" :
                    diasAtraso <= 90 ? "130" :
                    diasAtraso <= 180 ? "140" :
                    diasAtraso <= 360 ? "150" :
                    diasAtraso <= 720 ? "160" :
                    diasAtraso <= 1080 ? "165" :
                    diasAtraso <= 1440 ? "170" :
                    diasAtraso <= 1800 ? "175" :
                    diasAtraso <= 5400 ? "180" :
                    diasAtraso <= 10800 ? "190" :
                    "199"
                )
            {
                case "110":
                    {
                        vencimento.v110 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "120":
                    {
                        vencimento.v120 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "130":
                    {
                        vencimento.v130 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "140":
                    {
                        vencimento.v140 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "150":
                    {
                        vencimento.v150 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "160":
                    {
                        vencimento.v160 = totalVencimentos.Replace(",", ".");
                        break;
                    }


                case "165":
                    {
                        vencimento.v165 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "170":
                    {
                        vencimento.v170 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "175":
                    {
                        vencimento.v175 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "180":
                    {
                        vencimento.v180 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "190":
                    {
                        vencimento.v190 = totalVencimentos.Replace(",", ".");
                        break;
                    }

                case "199":
                    {
                        vencimento.v199 = totalVencimentos.Replace(",", ".");
                        break;
                    }
            }


            return vencimento;
        }


        #endregion


        private static Vencimento retornaFaixaVencimentoOperacoesPrejuizo(int diasAtraso, decimal totalVencimentos)
        {
            Vencimento vencimento = new Vencimento();

            switch
                (
                    diasAtraso <= 360 ? "310" :
                    diasAtraso <= 1440 ? "320" :
                    "330"
                )
            {
                case "310":
                    {
                        vencimento.v310 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "320":
                    {
                        vencimento.v320 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "330":
                    {
                        vencimento.v330 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

            }


            return vencimento;
        }

        private static Vencimento retornaFaixaVencimentoOperacoesCreditosVencer(int diasAtraso, decimal totalVencimentos)
        {
            Vencimento vencimento = new Vencimento();

            switch
                (
                    diasAtraso <= 30 ? "110" :
                    diasAtraso <= 60 ? "120" :
                    diasAtraso <= 90 ? "130" :
                    diasAtraso <= 180 ? "140" :
                    diasAtraso <= 360 ? "150" :
                    diasAtraso <= 720 ? "160" :
                    diasAtraso <= 1080 ? "165" :
                    diasAtraso <= 1440 ? "170" :
                    diasAtraso <= 1800 ? "175" :
                    diasAtraso <= 5400 ? "180" :
                    diasAtraso <= 10800 ? "190" :
                    "199"
                )
            {
                case "110":
                    {
                        vencimento.v110 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "120":
                    {
                        vencimento.v120 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "130":
                    {
                        vencimento.v130 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "140":
                    {
                        vencimento.v140 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "150":
                    {
                        vencimento.v150 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "160":
                    {
                        vencimento.v160 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }


                case "165":
                    {
                        vencimento.v165 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "170":
                    {
                        vencimento.v170 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "175":
                    {
                        vencimento.v175 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "180":
                    {
                        vencimento.v180 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "190":
                    {
                        vencimento.v190 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "199":
                    {
                        vencimento.v199 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }
            }


            return vencimento;
        }

        private static Vencimento retornaFaixaVencimentoOperacoesCreditosLiberar(int diasAtraso, decimal totalVencimentos)
        {
            Vencimento vencimento = new Vencimento();

            switch
                (
                    diasAtraso <= 360 ? "60" :
                    "80"
                )
            {
                case "60":
                    {
                        vencimento.v60 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "80":
                    {
                        vencimento.v80 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

            }


            return vencimento;
        }

        private static Vencimento retornaFaixaVencimentoOperacoesLimiteCredito(int diasAtraso, decimal totalVencimentos)
        {
            Vencimento vencimento = new Vencimento();

            switch
                (
                    diasAtraso <= 360 ? "20" :
                    "40"
                )
            {
                case "20":
                    {
                        vencimento.v20 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

                case "40":
                    {
                        vencimento.v40 = String.Format("{0:0.00}", Convert.ToDecimal(totalVencimentos)).ToString().Replace(",", ".");
                        break;
                    }

            }


            return vencimento;
        }

        private static List<ContratoOrbitallWOAtivo> preencherContratosOrbitalAjuste()
        {
            List<ContratoOrbitallWOAtivo> contratos = new List<ContratoOrbitallWOAtivo>();
            return contratos;
        }

        private static List<IndicioBacen> popularIpocsIndiciosBacen()
        {
            List<IndicioBacen> indicios = new List<IndicioBacen>();
            return indicios;
        }

        private static List<Fidc122025> popularContratosFidc122025()
        {
            List<Fidc122025> contratos = new List<Fidc122025>();
            return contratos;
        }

        private static List<PorteCliente> popularClientesTratarPorte()
        {
            List<PorteCliente> clientesPorte = new List<PorteCliente>();
            return clientesPorte;
        }

        #endregion

    }
}
