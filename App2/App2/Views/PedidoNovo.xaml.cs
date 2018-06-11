using App2.Model;
using App2.Services;
using App2.Utils;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PedidoNovo : Xamarin.Forms.TabbedPage
    {
        PedidoItemService Item = new PedidoItemService();
        LoginService clienteLogado = new LoginService();
        List<PedidoItemModel> ped_items = new List<PedidoItemModel>();
        ItemList pItem;
        public PedidoNovo()
        {
            InitializeComponent();
            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
            this.BindingContext = this;
            this.IsBusy = false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            PedidoModel pedido = new PedidoModel();
            GlobalVariables.GlobalPedido = pedido;

            ClienteModel cliente = new ClienteModel();
            GlobalVariables.GlobalClientePedido = cliente;

            //Invoke on Main thread, or this won't work
            Device.BeginInvokeOnMainThread(() =>
            {
                ECcodigo.Keyboard = Keyboard.Numeric;
                ECcodigo.Focus();
            });

            ECcodigo.Completed += (s, e) =>
            {
                ECqtde.Keyboard = Keyboard.Numeric;
                ECqtde.Focus();
                lblItemSelecionado.TextColor = Color.FromHex("#3B5998");
                BuscaNomeProduto();
            };

            ECqtde.Completed += (s, e) =>
            {
                ECcodigo.Keyboard = Keyboard.Numeric;
                InserirItem(s, e);
                ECcodigo.Text = "";
            };
        }
        private void ECcodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            string last = "";
            if (String.IsNullOrEmpty(e.NewTextValue) == false)
                if (e.NewTextValue.Length > 2)
                    last = e.NewTextValue.Substring(e.NewTextValue.Length - 1, 1);
            if (last == "\n")
            {
                ECqtde.Focus();
            }
        }
        private void ECqtde_TextChanged(object sender, TextChangedEventArgs e)
        {
            string last = "";
            if (String.IsNullOrEmpty(e.NewTextValue) == false)
                if (e.NewTextValue.Length > 2)
                    last = e.NewTextValue.Substring(e.NewTextValue.Length - 1, 1);
            if (last == "\n")
            {
                ECcodigo.Focus();
            }
        }
        private async void BuscaNomeProduto()
        {
            try
            {
                if (!string.IsNullOrEmpty(ECcodigo.Text))
                {
                    ProdutosService prodService = new ProdutosService();
                    List<ProdutosModel> prod = new List<ProdutosModel>();
                    prod = await prodService.BuscaProdutosPorCodigo(ECcodigo.Text, GlobalVariables.campanha);
                    if (prod != null && prod.Count > 0)
                    {
                        lblItemSelecionado.HorizontalTextAlignment = TextAlignment.Start;
                        lblItemDigitado.Text = prod[0].NomeProduto + " - " + "Valor R$ " + prod[0].Preco.ToString();
                    }
                    else
                    {
                        lblItemDigitado.TextColor = Color.Red;
                        lblItemSelecionado.HorizontalTextAlignment = TextAlignment.Center;
                        lblItemDigitado.Text = "Produto Inexistente";
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        async void NovoPedido(object sender, EventArgs e)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alerta!", "Sem conexão com à Internet.", "OK");
                    return;
                }
                if (GlobalVariables.GlobalClientePedido.IdCliente == 0)
                {
                    await DisplayAlert("Alerta!", "Id do cliente inválido.", "OK");
                    return;
                }
                if (GlobalVariables.GlobalFuncionarioLogado.IdFuncionario == 0)
                {
                    await DisplayAlert("Alerta!", "Id do vendedor inválido.", "OK");
                    return;
                }

                PedidoService srvPedido = new PedidoService();
                PedidoModel novoPedido = new PedidoModel();

                // Verifica se existe pedido em aberto
                novoPedido = await srvPedido.BuscaPedidoEmAbertoPorIdCliente(GlobalVariables.GlobalClientePedido.IdCliente);
                if (novoPedido != null && novoPedido.IdPedido > 0)
                {
                    // Carrega pedido
                    GlobalVariables.GlobalPedido = novoPedido;
                    GlobalVariables.GlobalPedido.IdPedido = novoPedido.IdPedido;
                    GlobalVariables.formaPagamento = novoPedido.IdPagamento;
                    List<PedidoItemModel> items = await Item.BuscaItemPorIdPedido(novoPedido.IdPedido);

                    if (items.Count > 0)
                    {
                        ped_items.Clear();
                        foreach (PedidoItemModel itm in items)
                        {
                            ped_items.Add(itm);
                        }
                        CarregaItensPedido(items);
                    }

                    CalculaQtdItemPedido();
                    CalculaVlrPedido();
                    CalculaDescPedido();
                    CalculaTotalPedido();

                    PedidoItemService srvPedidoItem = new PedidoItemService();
                    var descPedido = await srvPedidoItem.RetornaPercentualDescPedido(GlobalVariables.GlobalPedido.IdPedido);
                    ECpercDesc.Text = descPedido.ToString();

                    CarregaPagamento();

                    CurrentPage = Children[1];
                }
                else
                {
                    // Insere novo pedido se não existir
                    novoPedido.IdFilial = 1;
                    novoPedido.IdLote = 0;
                    novoPedido.IdCliente = GlobalVariables.GlobalClientePedido.IdCliente;
                    novoPedido.IdFuncionario = GlobalVariables.GlobalFuncionarioLogado.IdFuncionario;
                    novoPedido.IdPagamento = GlobalVariables.formaPagamento;
                    novoPedido.IdStatus = 1;
                    novoPedido.Sessao = "";
                    novoPedido.Parcelas = 0;
                    novoPedido.Data = DateTime.Now.ToString("yyyyMMdd hh:MM:ss");
                    novoPedido.DataEmissao = novoPedido.Data;
                    novoPedido.Itens = 0;
                    novoPedido.ValorBruto = 0;
                    novoPedido.ValorDesconto = 0;
                    novoPedido.ValorDescontoDist = 0;
                    novoPedido.ValorLiquido = 0;
                    novoPedido.Obs = "Pedido inserido via App";
                    novoPedido.IdTransportadora = 1;
                    novoPedido.DataEntrega = null;
                    novoPedido.DescontoGeral = 0;

                    var inseriu = await srvPedido.NovoPedido(novoPedido);

                    if (inseriu > 0)
                    {
                        novoPedido = new PedidoModel();
                        novoPedido = await srvPedido.BuscaPedidoPorIdPedido(inseriu);
                        // Carrega pedido
                        GlobalVariables.GlobalPedido = novoPedido;
                        GlobalVariables.GlobalPedido.IdPedido = novoPedido.IdPedido;
                        CalculaQtdItemPedido();
                        CalculaVlrPedido();
                        CalculaDescPedido();
                        CalculaTotalPedido();
                        CurrentPage = Children[1];
                    }
                    else
                    {
                        GlobalVariables.GlobalPedido = null;
                        GlobalVariables.GlobalPedido.IdPedido = 0;
                    }
                }

            }
            catch (Exception)
            {
            }
        }
        private async void SalvarPedido(object sender, EventArgs e)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alerta!", "Sem conexão com à Internet.", "OK");
                    return;
                }
                if (GlobalVariables.GlobalClientePedido.IdCliente == 0)
                {
                    await DisplayAlert("Alerta!", "Cliente não encontrado.", "OK");
                    return;
                }
                if (GlobalVariables.GlobalFuncionarioLogado.IdFuncionario == 0)
                {
                    await DisplayAlert("Alerta!", "Vendedor não encontrado.", "OK");
                    return;
                }
                if (GlobalVariables.formaPagamento == 0)
                {
                    await DisplayAlert("Alerta!", "Forma de pagamento inválida.", "OK");
                    return;
                }
                if (GlobalVariables.GlobalPedido.IdPedido == 0)
                {
                    await DisplayAlert("Alerta!", "Nenhum pedido para enviar.", "OK");
                    return;
                }

                PedidoService srvPedido = new PedidoService();
                PedidoModel salvarPedido = new PedidoModel();

                var salvou = await srvPedido.FinalizaPedido(GlobalVariables.GlobalPedido.IdPedido);

                if (salvou)
                {
                    await DisplayAlert("Info!", "Pedido Finalizado com sucesso.", "OK");
                }
                else
                {
                    await DisplayAlert("Alerta!", "Falha ao enviar pedido. Tente novamente.", "OK");
                }

            }
            catch (Exception)
            {
            }
        }
        private async void CancelarPedido(object sender, EventArgs e)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alerta!", "Sem conexão com à Internet.", "OK");
                    return;
                }
                if (GlobalVariables.GlobalPedido.IdPedido == 0)
                {
                    await DisplayAlert("Alerta!", "Nenhum pedido para cancelar.", "OK");
                    return;
                }

                PedidoService srvPedido = new PedidoService();
                PedidoModel salvarPedido = new PedidoModel();

                var cancelou = await srvPedido.CancelaPedido(GlobalVariables.GlobalPedido.IdPedido);

                if (cancelou)
                {
                    GlobalVariables.campanha = 0;
                    GlobalVariables.formaPagamento = 0;
                    GlobalVariables.percDesconto = 0;
                    GlobalVariables.GlobalFuncionarioLogado = null;
                    GlobalVariables.GlobalClientePedido = null;
                    GlobalVariables.GlobalPedido = null;
                    await DisplayAlert("Info!", "Pedido cancelado com sucesso.", "OK");
                }
                else
                {
                    await DisplayAlert("Alerta!", "Falha ao cancelar pedido. Tente novamente.", "OK");
                }
            }
            catch (Exception)
            {
            }
        }
        private async void InserirItem(object sender, EventArgs e)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alerta!", "Sem conexão com à Internet.", "OK");
                    return;
                }
                if (string.IsNullOrEmpty(ECqtde.Text))
                {
                    ECqtde.Text = "1";
                }
                ProdutosService prodService = new ProdutosService();
                List<ProdutosModel> prod = new List<ProdutosModel>();
                if (ECqtde.Text == "0")
                {
                    PedidoItemService novoItem = new PedidoItemService();
                    var answer = await DisplayAlert("Alerta!", "Deseja excluir o item" + ECcodigo.Text + "? ", "Sim", "Não");
                    if (answer)
                    {
                        var deletou = await novoItem.DeletaPedidoItem(GlobalVariables.GlobalPedido.IdPedido, ECcodigo.Text);
                        if (deletou > 0)
                        {
                            await DisplayAlert("Alerta!", "Item deletado com sucesso.", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Alerta!", "Falha ao excluir item. Tente novamente.", "OK");
                        }
                    }
                    ECcodigo.Text = "";
                    ECqtde.Text = "";
                    lblItemDigitado.Text = "";
                    ECcodigo.Focus();
                    return;
                }
                prod = await prodService.BuscaProdutosPorCodigo(ECcodigo.Text, GlobalVariables.campanha);

                if (prod != null && prod.Count > 0)
                {
                    PedidoItemService novoItem = new PedidoItemService();
                    PedidoItemModel item = new PedidoItemModel();

                    Int32 existente = 0;
                    Boolean novo = false;

                    item.IdPedido = GlobalVariables.GlobalPedido.IdPedido;
                    item.IdFornecedor = prod[0].IdFornecedor;
                    item.IdProduto = prod[0].IdProduto;
                    item.NomeProduto = prod[0].NomeProduto;
                    item.CodigoProduto = prod[0].Codigo;
                    item.Quantidade = Convert.ToInt16(ECqtde.Text);
                    item.ValorProduto = prod[0].Preco;
                    item.PercentualDesconto = (decimal)GlobalVariables.percDesconto;
                    item.PercentualDescontoDist = prod[0].DescDist;
                    item.ValorDesconto = item.ValorProduto * (item.PercentualDesconto / 100);
                    item.ValorDescontoDist = item.ValorProduto * (item.PercentualDescontoDist / 100);
                    item.Obs = "";
                    item.IdCampanha = GlobalVariables.campanha;

                    existente = await novoItem.ExisteItemNoPedido(item.IdPedido, item.IdProduto);

                    if (existente > 0)
                    {
                        this.IsBusy = true;
                        item.IdItem = existente;
                        await novoItem.AtualizaPedidoItem(item);
                        ped_items.Add(item);
                        this.IsBusy = false;
                    }
                    else
                    {
                        this.IsBusy = true;
                        novo = await novoItem.InserePedidoItem(item);
                        ped_items.Add(item);
                        this.IsBusy = false;
                    }
                    if (existente > 0 || novo == true)
                    {
                        CalculaQtdItemPedido();
                        CalculaVlrPedido();
                        CalculaDescPedido();
                        CalculaTotalPedido();

                        ECcodigo.Text = "";
                        ECqtde.Text = "";
                        lblItemDigitado.Text = "";
                        ECcodigo.Focus();

                        List<PedidoItemModel> items = await Item.BuscaItemPorIdPedido(GlobalVariables.GlobalPedido.IdPedido);
                        {
                            if (items.Count > 0)
                            {
                                CarregaItensPedido(items);
                                CalculaQtdItemPedido();
                                CalculaVlrPedido();
                                CalculaDescPedido();
                                CalculaTotalPedido();
                            }
                        }
                    }
                }
                else
                {
                    ECcodigo.Text = "";
                    ECqtde.Text = "";
                    lblItemDigitado.Text = "";
                    ECcodigo.Focus();
                }
            }
            catch (Exception)
            {
            }
        }
        public async void DeletarItem(object sender, EventArgs e)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alerta!", "Sem conexão com à Internet.", "OK");
                    return;
                }
                var answer = await DisplayAlert("Alerta!", "Deseja excluir o item selecionado?", "Sim", "Não");
                if (answer)
                {
                    var item = (Xamarin.Forms.Button)sender;
                    PedidoItemModel lst = (from itm in pItem.Items
                                           where itm.IdItem.ToString() == item.CommandParameter.ToString()
                                           select itm).FirstOrDefault<PedidoItemModel>();
                    //pItem.Items.Remove(lst);
                    Item = new PedidoItemService();
                    var deletou = await Item.DeletaPedidoItem(Convert.ToInt32(item.CommandParameter));
                    List<PedidoItemModel> items = new List<PedidoItemModel>();
                    Item = new PedidoItemService();
                    items = await Item.BuscaItemPorIdPedido(GlobalVariables.GlobalPedido.IdPedido);
                    if (items.Count > 0)
                    {
                        ped_items.Clear();
                        foreach (PedidoItemModel itm in items)
                        {
                            ped_items.Add(itm);
                        }
                        CarregaItensPedido(items);
                        CalculaQtdItemPedido();
                        CalculaVlrPedido();
                        CalculaDescPedido();
                        CalculaTotalPedido();
                    }
                    if (deletou > 0)
                    {
                        await DisplayAlert("Alerta!", "Item deletado com sucesso.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Alerta!", "Falha ao excluir item. Tente novamente.", "OK");
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        protected async void MudaTab(object sender, EventArgs e)
        {
            try
            {
                var i = this.Children.IndexOf(this.CurrentPage);
                if (i == 3)
                {
                    if (GlobalVariables.GlobalClientePedido.IdCliente == 0)
                    {
                        await DisplayAlert("Alerta!", "Selecione o cliente para continuar", "OK");
                        CurrentPage = Children[0];
                        return;
                    }
                    if (GlobalVariables.formaPagamento == 0)
                    {
                        await DisplayAlert("Alerta!", "Selecione a forma de pagamento para continuar", "OK");
                        CurrentPage = Children[1];
                        return;
                    }
                    List<PedidoItemModel> items = await Item.BuscaItemPorIdPedido(GlobalVariables.GlobalPedido.IdPedido);
                    if (items.Count > 0)
                    {
                        CalculaQtdItemPedido();
                        CalculaVlrPedido();
                        CalculaDescPedido();
                        CalculaTotalPedido();
                    }
                }
                if (i == 2)
                {
                    if (GlobalVariables.GlobalClientePedido.IdCliente == 0)
                    {
                        await DisplayAlert("Alerta!", "Selecione o cliente para continuar", "OK");
                        CurrentPage = Children[0];
                        return;
                    }
                    if (GlobalVariables.formaPagamento == 0)
                    {
                        await DisplayAlert("Alerta!", "Selecione a forma de pagamento para continuar", "OK");
                        CurrentPage = Children[1];
                        return;
                    }
                    List<PedidoItemModel> items = await Item.BuscaItemPorIdPedido(GlobalVariables.GlobalPedido.IdPedido);
                    if (items.Count > 0)
                    {
                        CarregaItensPedido(items);
                    }
                }
                if (i == 1)
                {
                    if (GlobalVariables.GlobalClientePedido.IdCliente == 0)
                    {
                        await DisplayAlert("Alerta!", "Selecione o cliente para continuar", "OK");
                        CurrentPage = Children[0];
                        return;
                    }
                    if (GlobalVariables.GlobalPedido.IdPedido > 0)
                    {
                        PedidoItemService srvPedidoItem = new PedidoItemService();
                        var descPedido = await srvPedidoItem.RetornaPercentualDescPedido(GlobalVariables.GlobalPedido.IdPedido);
                        ECpercDesc.Text = descPedido.ToString();
                    }
                    CarregaPagamento();
                }
            }
            catch (Exception)
            {
            }
        }
        private void CarregaItensPedido(List<PedidoItemModel> lst)
        {
            try
            {
                listViewItem.IsVisible = true;
                //lblmsg.IsVisible = false;
                listViewItem.ItemsSource = lst;
                listViewItem.BindingContext = lst;
            }
            catch (Exception)
            {
            }
        }
        private void pckPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FormaPagamentoModel itemSelecionado = (FormaPagamentoModel)pckPagamento.SelectedItem;
                if (itemSelecionado != null)
                {
                    GlobalVariables.formaPagamento = Convert.ToInt32(itemSelecionado.IdFormaPagamento);
                    if (GlobalVariables.formaPagamento == 145)
                    {
                        pckPagamento.SelectedIndex = 36;
                        ECpercDesc.IsEnabled = true;
                    }
                    else
                    {
                        ECpercDesc.IsEnabled = false;
                    }
                    btnProximoPagamento.IsVisible = true;
                }
                else
                {
                    btnProximoPagamento.IsVisible = false;
                }

            }
            catch (Exception)
            {
            }
        }
        protected async void CarregaPagamento()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alerta!", "Sem conexão com à Internet.", "OK");
                    return;
                }
                List<FormaPagamentoModel> pag = new List<FormaPagamentoModel>();
                FormaPagamentoService pagService = new FormaPagamentoService();
                if (GlobalVariables.GlobalPedido.IdPedido > 0 && GlobalVariables.GlobalPedido.IdPagamento > 0)
                {
                    pag = await pagService.RetornaFormaPagamentoPorIdPedido(GlobalVariables.GlobalPedido.IdPedido);
                }
                else
                {
                    pag = await pagService.RetornaFormaPagamento();
                }
                pckPagamento.ItemsSource = null;
                if (pag[0].IdFormaPagamento > 0 && pag.Count > 0)
                {
                    pckPagamento.ItemsSource = pag;
                }
                if (GlobalVariables.formaPagamento == 145)
                {
                    pckPagamento.ItemsSource = pag;
                    pckPagamento.SelectedIndex = 36;
                    pckPagamento.IsEnabled = false;
                    ECpercDesc.IsEnabled = false;
                    GlobalVariables.percDesconto = Convert.ToDouble(ECpercDesc.Text);
                }
                else
                {
                    pckPagamento.ItemsSource = pag;
                    pckPagamento.SelectedIndex = 0;
                    pckPagamento.SelectedItem = GlobalVariables.formaPagamento;
                    pckPagamento.IsEnabled = true;
                    ECpercDesc.IsEnabled = true;
                    GlobalVariables.percDesconto = Convert.ToDouble(ECpercDesc.Text);
                }
            }
            catch (Exception)
            {
            }
        }
        private void CalculaQtdItemPedido()
        {
            try
            {
                pItem = new ItemList(ped_items);
                var i = new PedidoItemModel
                {
                    Quantidade = pItem.Items.Sum(it => it.Quantidade),
                };
                lblQuantidade.Text = i.Quantidade.ToString();
            }
            catch (Exception)
            {
            }
        }
        private void CalculaVlrPedido()
        {
            try
            {
                pItem = new ItemList(ped_items);
                var i = new PedidoItemModel
                {
                    ValorProduto = pItem.Items.Sum(it => it.ValorProduto * it.Quantidade),
                };
                lblValor.Text = i.ValorProduto.ToString();
            }
            catch (Exception)
            {
            }
        }
        private void CalculaDescPedido()
        {
            try
            {
                pItem = new ItemList(ped_items);
                var i = new PedidoItemModel
                {
                    ValorDesconto = pItem.Items.Sum(it => it.ValorDesconto),
                };
                lblDesconto.Text = i.ValorDesconto.ToString();
            }
            catch (Exception)
            {
            }
        }
        private void CalculaTotalPedido()
        {
            try
            {
                pItem = new ItemList(ped_items);
                var i = new PedidoItemModel
                {
                    ValorTotal = pItem.Items.Sum(it => (it.ValorProduto - it.ValorDesconto) * it.Quantidade),
                };
                lblTotal.Text = i.ValorTotal.ToString();

            }
            catch (Exception)
            {
            }
        }
        async Task ConsultaCNPJ(object sender, EventArgs e)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alerta!", "Sem conexão com à Internet.", "OK");
                    return;
                }
                if (string.IsNullOrEmpty(userCodeEntry.Text))
                {
                    await DisplayAlert("Alerta!", "Preencha o CPF ou CNPJ.", "OK");
                    btnProximoPedido.IsVisible = false;
                    return;
                }
                string buscaCliente = Util.RemoveSpecialCharacters(userCodeEntry.Text.Trim());
                if (buscaCliente.Length != 11 && buscaCliente.Length != 14)
                {
                    await DisplayAlert("Alerta!", "O CPF ou CNPJ não é válido.", "OK");
                    btnProximoPedido.IsVisible = false;
                    return;
                }
                if (buscaCliente.Length == 11)
                {
                    if (!Util.IsCpf(buscaCliente))
                    {
                        await DisplayAlert("Alerta!", "O CPF não é válido.", "OK");
                        btnProximoPedido.IsVisible = false;
                        return;
                    }
                }
                if (buscaCliente.Length == 14)
                {
                    if (!Util.IsCnpj(buscaCliente))
                    {
                        await DisplayAlert("Alerta!", "O CNPJ não é válido.", "OK");
                        btnProximoPedido.IsVisible = false;
                        return;
                    }
                }

                //Verifica se já existe CNPJ cadastrado
                this.IsBusy = true;
                ClienteService clienteLogado = new ClienteService();
                ClienteModel cliente = new ClienteModel();
                cliente = await clienteLogado.BuscaClientePorCnpj(buscaCliente);
                this.IsBusy = false;

                if (cliente != null && cliente.IdCliente > 0)
                {
                    GlobalVariables.GlobalClientePedido = cliente;
                    lblRazaoSocial.IsVisible = true;
                    lblCnpjCpf.IsVisible = true;
                    lblCep.IsVisible = true;
                    lblEndereco.IsVisible = true;
                    lblRazaoSocial.Text = cliente.RazaoSocial;
                    lblCnpjCpf.Text = cliente.CnpjCpf;
                    lblCep.Text = cliente.Cep;
                    lblEndereco.Text = cliente.Endereco;
                    lblNaoEncontrado.Text = "";
                    btnProximoPedido.IsVisible = true;
                    NovoPedido(sender, e);
                    return;
                }
                else
                {
                    await DisplayAlert("Alerta", "Cliente não cadastrado!", "OK");
                    lblRazaoSocial.IsVisible = false;
                    lblCnpjCpf.IsVisible = false;
                    lblCep.IsVisible = false;
                    lblEndereco.IsVisible = false;
                    lblRazaoSocial.Text = "";
                    lblCnpjCpf.Text = "";
                    lblCep.Text = "";
                    lblEndereco.Text = "";
                    lblNaoEncontrado.Text = "Cliente não encontrado";
                    btnProximoPedido.IsVisible = false;
                    return;
                }
            }
            catch (Exception)
            {
            }
        }
        void ProximoPedido()
        {
            try
            {
                CarregaPagamento();
                var i = this.Children.IndexOf(this.CurrentPage);
                CurrentPage = Children[1];
            }
            catch (Exception)
            {
            }
        }
        async void ProximoPagamento()
        {
            try
            {
                if (string.IsNullOrEmpty(ECpercDesc.Text)) ECpercDesc.Text = "0";
                if (Convert.ToDouble(ECpercDesc.Text) > 10)
                {
                    await DisplayAlert("Alerta!", "Desconto inválido", "OK");
                    return;
                }
                if (GlobalVariables.formaPagamento > 0)
                {
                    if (GlobalVariables.GlobalPedido.IdPedido > 0)
                    {
                        PedidoItemService srvPedidoItem = new PedidoItemService();
                        var descPedido = srvPedidoItem.RetornaPercentualDescPedido(GlobalVariables.GlobalPedido.IdPedido);
                        ECpercDesc.Text = descPedido.ToString();
                        if (GlobalVariables.formaPagamento == 0)
                        {
                            PedidoService srvPedido = new PedidoService();
                            var atu = await srvPedido.AtualizaFormaPagamentoPedido(GlobalVariables.GlobalPedido.IdPedido, GlobalVariables.formaPagamento);
                        }
                    }
                }
                var i = this.Children.IndexOf(this.CurrentPage);
                CurrentPage = Children[2];
            }
            catch (Exception)
            {
            }
        }
        void AnterirorPagamento()
        {
            try
            {
                var i = this.Children.IndexOf(this.CurrentPage);
                userCodeEntry.IsEnabled = false;
                CurrentPage = Children[0];
            }
            catch (Exception)
            {
            }
        }
        void ProximoItem()
        {
            try
            {
                var i = this.Children.IndexOf(this.CurrentPage);
                CurrentPage = Children[3];
            }
            catch (Exception)
            {
            }
        }
        void AnterirorItem()
        {
            try
            {
                pckPagamento.IsEnabled = false;
                ECpercDesc.IsEnabled = false;
                var i = this.Children.IndexOf(this.CurrentPage);
                CurrentPage = Children[1];
            }
            catch (Exception)
            {
            }
        }
        private async void ItemsPedido_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                List<PedidoItemModel> pedItem = new List<PedidoItemModel>();
                // verifica a quantidade de caracteres digitados
                if (e.NewTextValue.Length >= 5)
                {
                    pedItem = await Item.BuscaItemPorCodigoOuDescricao(GlobalVariables.GlobalPedido.IdPedido, e.NewTextValue);
                    if (pedItem == null || pedItem.Count == 0)
                    {
                        // esconde o listview e exibe o label
                        // exibe a mensagem no label
                        listViewItem.IsVisible = false;
                        //lblmsg.IsVisible = true;
                        //lblmsg.Text = "Produto não encontrado.";
                        //lblmsg.TextColor = Color.Red;
                    }
                    else
                    {
                        // exibe o listview e esconde o label 
                        // exibe a lista de produtos
                        listViewItem.IsVisible = true;
                        //lblmsg.IsVisible = false;
                        listViewItem.ItemsSource = pedItem;
                        listViewItem.BindingContext = pedItem;
                    }
                }
                if (e.NewTextValue.Length == 0)
                {
                    pedItem = new List<PedidoItemModel>();
                    pedItem = await Item.BuscaItemPorIdPedido(GlobalVariables.GlobalPedido.IdPedido);
                    if (pedItem.Count > 0)
                    {
                        // exibe o listview e esconde o label 
                        // exibe a lista de produtos
                        listViewItem.IsVisible = true;
                        //lblmsg.IsVisible = false;
                        listViewItem.ItemsSource = pedItem;
                        listViewItem.BindingContext = pedItem;
                    }
                    else
                    {
                        // esconde o listview e exibe o label coma mensagem
                        listViewItem.IsVisible = false;
                        //lblmsg.IsVisible = true;
                        //lblmsg.Text = "Digite a descrição ou código do produto.";
                    }
                }

            }
            catch (Exception)
            {

            }
        }
        private async void bntProcurarItem_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Produtos());
        }
    }
}