using App2.Model;
using App2.Services;
using App2.Utils;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Produtos : ContentPage

    {
        ProdutosService produto = new ProdutosService();
        public Produtos()
        {
            InitializeComponent();
        }

        private async void Produtos_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Alerta!", "Sem conexão com à Internet.", "OK");
                return;
            }
            // verifica a quantidade de caracteres digitados
            if (e.NewTextValue.Length >= 3)
            {
                List<ProdutosModel> produtos = await produto.BuscaProdutosPorWhere("(p.nome_produto LIKE '%" + e.NewTextValue + "%'" +
                                                                                   " OR p.codigo LIKE '%" + e.NewTextValue + "%')" +
                                                                                   " AND c.id_campanha=" + GlobalVariables.campanha);
                if (produtos == null || produtos.Count == 0)
                {
                    lvwProdutos.IsVisible = false;
                    lblmsg.IsVisible = true;
                    lblmsg.Text = "Produto não encontrado.";
                    lblmsg.TextColor = Color.Red;
                }
                else
                {
                    lvwProdutos.IsVisible = true;
                    lblmsg.IsVisible = false;
                    lvwProdutos.ItemsSource = produtos;
                }
            }
            else
            {
                lvwProdutos.IsVisible = false;
                lblmsg.IsVisible = true;
                lblmsg.Text = "Digite a descrição ou código do produto.";
            }
        }

        private async void lvwProdutos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            // obtem o item selecionado
            var produto = e.SelectedItem as ProdutosModel;
            //deseleciona o item do listview
            lvwProdutos.SelectedItem = null;
            // chama a pagina ProdutosDetailhes
            await Navigation.PushAsync(new ProdutosDetalhes(produto));
        }
    }
}