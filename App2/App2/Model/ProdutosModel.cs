using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public class ProdutosModel
    {
        [JsonProperty("id_produto")]
        public int IdProduto { get; set; }

        [JsonProperty("id_fornecedor")]
        public int IdFornecedor { get; set; }

        [JsonProperty("id_categoria")]
        public int IdCategoria { get; set; }

        [JsonProperty("id_subCategoria")]
        public int IdSubcategoriaCategoria { get; set; }

        [JsonProperty("origem")]
        public int Origem { get; set; }

        [JsonProperty("id_campanha")]
        public int IdCampanha { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("nome_produto")]
        public string NomeProduto { get; set; }

        [JsonProperty("unidade")]
        public string Unidade { get; set; }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }

        [JsonProperty("peso")]
        public string Peso { get; set; }

        [JsonProperty("preco")]
        public decimal Preco { get; set; }

        [JsonProperty("custo")]
        public decimal Custo { get; set; }

        [JsonProperty("preco_compra")]
        public decimal PrecoCompra { get; set; }

        [JsonProperty("ncm")]
        public string NCM { get; set; }

        [JsonProperty("cest")]
        public string CEST { get; set; }

        [JsonProperty("grupo")]
        public string Grupo { get; set; }

        [JsonProperty("desc_fixo")]
        public int DescFixo { get; set; }

        [JsonProperty("desc_rev")]
        public decimal DescRev { get; set; }

        [JsonProperty("desc_dist")]
        public decimal DescDist { get; set; }

        [JsonProperty("ativo")]
        public int Ativo { get; set; }

        [JsonProperty("foto")]
        public string Foto { get; set; }

        [JsonProperty("codigo_barra")]
        public string CodigoBarra { get; set; }

        [JsonProperty("referencia_interna")]
        public string ReferenciaInterna { get; set; }

        [JsonProperty("brinde")]
        public string Brinde { get; set; }

        [JsonProperty("qtd_b")]
        public int QtdBrinde { get; set; }

        [JsonProperty("excluir")]
        public int Excluir { get; set; }

        [JsonProperty("estoque")]
        public int Estoque { get; set; }

        [JsonProperty("aliquota_ipi")]
        public decimal AliquotaIPI { get; set; }

        [JsonProperty("credito_st")]
        public decimal CreditoST { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("fg_situacao")]
        public string FlagSituacao { get; set; }

        [JsonProperty("id_marca")]
        public string IdMarca { get; set; }

        [JsonProperty("situacao")]
        public string Situacao { get; set; }

        [JsonProperty("estacao")]
        public string Estacao { get; set; }

        [JsonProperty("gondola")]
        public string Gondola { get; set; }

        [JsonProperty("materia_prima")]
        public int MateriaPrima { get; set; }

        [JsonProperty("pagcatal")]
        public int Pagina { get; set; }
    }
}
