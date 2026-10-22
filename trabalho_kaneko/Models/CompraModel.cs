using System;

namespace trabalho_kaneko.Models
{
    public class CompraModel
    {
        // Chave Primária Composta
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public int NumNfe { get; set; }
        public int IdFornecedor { get; set; }

        public int? Pagina { get; set; }
        public string NatOper { get; set; }
        public string ProtAcesso { get; set; }
        public DateTime? DataProtAcesso { get; set; }
        public TimeSpan? HoraProtAcesso { get; set; }
        public string ChaveAcessoNfe { get; set; }
        public DateTime? DataEmitNfe { get; set; }
        public DateTime? DataEntNfe { get; set; }
        public TimeSpan? HoraEntNfe { get; set; }

        public decimal? BaseCalcIcms { get; set; }
        public decimal? ValorIcms { get; set; }
        public decimal? BaseCalcIcmsSub { get; set; }
        public decimal? ValorIcmsSub { get; set; }
        public decimal? ValorFreteNfe { get; set; }
        public decimal? ValorSeguroNfe { get; set; }
        public decimal? DescontoNfe { get; set; }
        public decimal? OutrasDespNfe { get; set; }
        public decimal? ValorIpi { get; set; }

        public int? IdTransportadora { get; set; }
        public int? FretePorContaNfe { get; set; }
        public int? IdVeiculo { get; set; }
        public int? QtdeVol { get; set; }
        public string EspecieVol { get; set; }
        public string MarcaVol { get; set; }
        public string NumVol { get; set; }
        public decimal? PesoBruto { get; set; }
        public decimal? PesoLiq { get; set; }
        public string InfComp { get; set; }

        public int IdCondicao { get; set; }

        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}