using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiBKRFID.DAO
{
    #region Registro Arquivo
    public class TList_RegArquivo : List<TRegistro_RegArquivo>
    {
        public void Adiciona(TRegistro_RegArquivo reg)
        {
            if (Exists(p => p.Registro.Trim().ToUpper().Equals(reg.Registro.Trim().ToUpper())))
                Find(p => p.Registro.Trim().ToUpper().Equals(reg.Registro.Trim().ToUpper())).Qtd_linha += reg.Qtd_linha;
            else
                Add(reg);
        }
    }

    public class TRegistro_RegArquivo
    {
        public string Registro
        { get; set; }
        public decimal Qtd_linha
        { get; set; }
    }

    public class TRegistro_SaldoEstoque
    {
        public string Cd_produto { get; set; } = string.Empty;
        public decimal Quantidade { get; set; } = decimal.Zero;
    }
    #endregion

}