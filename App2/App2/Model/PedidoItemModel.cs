using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public class PedidoItemModel
    {
        [JsonProperty("IdItem")]
        public int IdItem { get; set; }

        [JsonProperty("IdPedido")]
        public int IdPedido { get; set; }

        [JsonProperty("IdProduto")]
        public int IdProduto { get; set; }

        [JsonProperty("CodigoProduto")]
        public string CodigoProduto { get; set; }

        [JsonProperty("Quantidade")]
        public int Quantidade { get; set; }

        [JsonProperty("NomeProduto")]
        public string NomeProduto { get; set; }

        [JsonProperty("ValorProduto")]
        public decimal ValorProduto { get; set; }

        [JsonProperty("PercentualDesconto")]
        public decimal PercentualDesconto { get; set; }

        [JsonProperty("ValorDesconto")]
        public decimal ValorDesconto { get; set; }

        [JsonProperty("PercentualDescontoDist")]
        public decimal PercentualDescontoDist { get; set; }

        [JsonProperty("ValorDescontoDist")]
        public decimal ValorDescontoDist { get; set; }

        [JsonProperty("Obs")]
        public string Obs { get; set; }

        [JsonProperty("IdFornecedor")]
        public int IdFornecedor { get; set; }

        [JsonProperty("IdCampanha")]
        public int IdCampanha { get; set; }

        [JsonProperty("ValorTotal")]
        public decimal ValorTotal { get; set; }
    }

    class ItemList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<PedidoItemModel> _items;
        public ObservableCollection<PedidoItemModel> Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("PedidoItemModel"); }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ItemList(List<PedidoItemModel> itemList)
        {
            Items = new ObservableCollection<PedidoItemModel>();
            foreach (PedidoItemModel itm in itemList)
            {
                Items.Add(itm);
            }
        }
    }
}
