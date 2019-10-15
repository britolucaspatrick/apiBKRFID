using apiBKRFID.DAO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace apiBKRFID.Business
{
    public class SpedFiscal
    {
        private static decimal Qtd_linhaK;
        private static TList_RegArquivo RegArq;

        private static void GerarBlocoK(StringBuilder SpedFiscal,
                                        DateTime? Dt_ini,
                                        DateTime? Dt_fin,
                                        List<TRegistro_SaldoEstoque> _SaldoEstoques)
        {
            GerarRegistroK001(_SaldoEstoques.Count(p => p.Quantidade > 0) > 0, SpedFiscal);
            if (_SaldoEstoques.Count(p => p.Quantidade > 0) > 0)
            {
                GerarRegistroK100(SpedFiscal, Dt_ini, Dt_fin);
                GerarRegistroK200(SpedFiscal, _SaldoEstoques, Dt_fin);
            }
            GerarRegistroK990(SpedFiscal);
        }

        private static void GerarRegistroK001(bool St_movimento,
                                              StringBuilder SpedFiscal)
        {
            Qtd_linhaK = decimal.Zero;
            string regK001 = "|K001|";
            regK001 += St_movimento ? "0|" : "1|";

            SpedFiscal.AppendLine(regK001);
            Qtd_linhaK++;
            RegArq.Adiciona(new TRegistro_RegArquivo() { Registro = "K001", Qtd_linha = 1 });
        }

        private static void GerarRegistroK100(StringBuilder SpedFiscal,
                                              DateTime? Dt_ini,
                                              DateTime? Dt_fin)
        {
            string regK100 = "|K100|";
            //Data Inicial
            regK100 += Dt_ini.Value.ToString("ddMMyyyy") + "|";
            //Data Final
            regK100 += Dt_fin.Value.ToString("ddMMyyyy") + "|";
            SpedFiscal.AppendLine(regK100);
            Qtd_linhaK++;
            RegArq.Adiciona(new TRegistro_RegArquivo { Registro = "K100", Qtd_linha = 1 });
        }

        private static void GerarRegistroK200(StringBuilder SpedFiscal, List<TRegistro_SaldoEstoque> lEstoques, DateTime? Dt_fin)
        {
            decimal cont = decimal.Zero;
            lEstoques.Where(p => p.Quantidade > decimal.Zero)
                .ToList()
                .ForEach(p =>
                {
                    string regK200 = "|K200|";
                    //Data do Estoque Final
                    regK200 += Dt_fin.Value.ToString("ddMMyyyy") + "|";
                    //Codigo Produto
                    regK200 += p.Cd_produto.Trim() + "|";
                    //Quantidade Estoque
                    regK200 += p.Quantidade.ToString("N3").Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator, string.Empty).Replace('.', ',') + "|";
                    //Tipo Estoque
                    regK200 += "0|";
                    //Codigo participante, quando estoque for de terceiro
                    regK200 += "|";
                    SpedFiscal.AppendLine(regK200);
                    Qtd_linhaK++;
                    cont++;
                });
            if (cont > decimal.Zero)
                RegArq.Adiciona(new TRegistro_RegArquivo() { Registro = "K200", Qtd_linha = cont });
        }

        private static void GerarRegistroK990(StringBuilder SpedFiscal)
        {
            string regK990 = "|K990|";
            Qtd_linhaK++;
            regK990 += Qtd_linhaK.ToString() + "|";

            SpedFiscal.AppendLine(regK990);
            RegArq.Adiciona(new TRegistro_RegArquivo() { Registro = "K990", Qtd_linha = 1 });
        }

        public static string ProcessarSpedFiscal(DateTime? Dt_ini,
                                                 DateTime? Dt_fin,
                                                 List<TRegistro_SaldoEstoque> _SaldoEstoques)
        {
            StringBuilder SpedFiscal = new StringBuilder();
            RegArq = new TList_RegArquivo();

            GerarBlocoK(SpedFiscal, Dt_ini, Dt_fin, _SaldoEstoques);

            return SpedFiscal.ToString().Trim();

        }
    }
}