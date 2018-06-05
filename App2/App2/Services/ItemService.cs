using App2.Model;
using App2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Services
{
    public class ItemService
    {
        private static ObservableCollection<MasterPageItem> menuLista { get; set; }

        public static ObservableCollection<MasterPageItem> GetMenuItens()
        {
            menuLista = new ObservableCollection<MasterPageItem>();
            // Criando as paginas para navegação
            // Definimos o titulo para o item
            // o icone do lado esquerdo e a pagina que vamos abrir
            var pagina1 = new MasterPageItem() { Title = "Home", Icon = "Home32.png", TargetType = typeof(HomePage) };
            var pagina2 = new MasterPageItem() { Title = "Novo Pedido", Icon = "Pedido32.png", TargetType = typeof(PedidoNovo) };
            var pagina3 = new MasterPageItem() { Title = "Produtos", Icon = "Produto32.png", TargetType = typeof(Produtos) };
            var pagina4 = new MasterPageItem() { Title = "Clientes", Icon = "Cliente.png", TargetType = typeof(Clientes) };
            //var pagina3 = new MasterPageItem() { Title = "Lançamentos", Icon = "CheckBook32.png", TargetType = typeof(LancamentosPage) };
            //var pagina4 = new MasterPageItem() { Title = "Relatórios", Icon = "BarChart32.png", TargetType = typeof(RelatoriosPage) };
            // Adicionando items no menuLista
            menuLista.Add(pagina1);
            menuLista.Add(pagina2);
            menuLista.Add(pagina3);
            menuLista.Add(pagina4);
            //retorna a lista de itens 
            return menuLista;
        }
    }
}
