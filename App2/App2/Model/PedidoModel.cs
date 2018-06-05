using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public class PedidoModel
    {
        [JsonProperty("IdPedido")]
        public int IdPedido { get; set; }

        [JsonProperty("IdLote")]
        public int IdLote { get; set; }

        [JsonProperty("IdFilial")]
        public int IdFilial { get; set; }

        [JsonProperty("IdCliente")]
        public int IdCliente { get; set; }

        [JsonProperty("IdFuncionario")]
        public int IdFuncionario { get; set; }

        [JsonProperty("IdPagamento")]
        public int IdPagamento { get; set; }

        [JsonProperty("IdStatus")]
        public int IdStatus { get; set; }

        [JsonProperty("Sessao")]
        public string Sessao { get; set; }

        [JsonProperty("Parcelas")]
        public int Parcelas { get; set; }

        [JsonProperty("Itens")]
        public int Itens { get; set; }

        [JsonProperty("Data")]
        public string Data { get; set; }

        [JsonProperty("DataEmissao")]
        public string DataEmissao { get; set; }

        [JsonProperty("ValorFrete")]
        public decimal ValorFrete { get; set; }

        [JsonProperty("ValorBruto")]
        public decimal ValorBruto { get; set; }

        [JsonProperty("ValorDesconto")]
        public decimal ValorDesconto { get; set; }

        [JsonProperty("ValorDescontoDist")]
        public decimal ValorDescontoDist { get; set; }

        [JsonProperty("ValorLiquido")]
        public decimal ValorLiquido { get; set; }

        [JsonProperty("ValorCredito")]
        public decimal ValorCredito { get; set; }

        [JsonProperty("ValorTotal")]
        public decimal ValorTotal { get; set; }

        [JsonProperty("Obs")]
        public string Obs { get; set; }

        [JsonProperty("IdTransportadora")]
        public int IdTransportadora { get; set; }

        [JsonProperty("DataEntrega")]
        public string DataEntrega { get; set; }

        [JsonProperty("DescontoGeral")]
        public decimal DescontoGeral { get; set; }

        [JsonProperty("IdpedidoCompra")]
        public int IdpedidoCompra { get; set; }

        [JsonProperty("ListaItens")]
        private List<PedidoItemModel> ListaItens { get; set; }

    }
}
