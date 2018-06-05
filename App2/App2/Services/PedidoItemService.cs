using App2.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App2.Services
{
    public class PedidoItemService
    {
        private HttpClient _client = new HttpClient();
        private PedidoItemModel _pedidoItem;
        private List<PedidoItemModel> _lstPedidoItem;

        public async Task<PedidoItemModel> BuscaItemPorIdItem(int id_item)
        {
            if (id_item == 0)
            {
                return null;
            }
            else
            {
                string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/PedidoItem/RetornaItemPorIdItem?id_item={0}", id_item));

                _client = new HttpClient();
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _pedidoItem = new PedidoItemModel();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pedidoitem = JsonConvert.DeserializeObject<PedidoItemModel>(content);
                    _pedidoItem = new PedidoItemModel();
                    _pedidoItem = (PedidoItemModel)pedidoitem;
                }
                return _pedidoItem;
            }
        }

        public async Task<Int32> ExisteItemNoPedido(int id_pedido, int id_produto)
        {
            if (id_pedido == 0)
            {
                return 0;
            }
            else
            {
                string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/PedidoItem/RetornaItemPorPedido?id_pedido={0}&id_produto={1}", id_pedido, id_produto));

                _client = new HttpClient();
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return 0;
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject output = JObject.Parse(content);
                    _pedidoItem = new PedidoItemModel();

                    _pedidoItem.IdItem = (int)output.SelectToken("IdItem");

                    if (_pedidoItem.IdItem > 0)
                    {
                        return _pedidoItem.IdItem;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public async Task<List<PedidoItemModel>> BuscaItemPorIdPedido(int id_pedido)
        {
            if (id_pedido == 0)
            {
                return null;
            }
            else
            {
                string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/PedidoItem/RetornaItemPorIdPedido?id_pedido={0}", id_pedido));

                _client = new HttpClient();
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _lstPedidoItem = new List<PedidoItemModel>();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<List<PedidoItemModel>>(content);
                    _lstPedidoItem = new List<PedidoItemModel>(Items);
                }
                return _lstPedidoItem;
            }
        }

        public async Task<List<PedidoItemModel>> BuscaItemPorCodigoOuDescricao(int id_pedido, string strBusca)
        {
            if (string.IsNullOrEmpty(strBusca))
            {
                return null;
            }
            else
            {
                string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/PedidoItem/RetornaItemPorCodigoDescricao?id_pedido={0}&busca={1}", id_pedido, strBusca));

                _client = new HttpClient();
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _lstPedidoItem = new List<PedidoItemModel>();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<List<PedidoItemModel>>(content);
                    _lstPedidoItem = new List<PedidoItemModel>(Items);
                }
                return _lstPedidoItem;
            }
        }

        public async Task<Double> RetornaPercentualDescPedido(Int32 id_pedido)
        {
            if (id_pedido == 0)
            {
                return 0;
            }
            else
            {
                string url = string.Concat("http://mrsistemas.net/grupo_mr_api/api/PedidoItem/RetornaPercDescPedido?IdPedido=" + id_pedido);
                _client = new HttpClient();

                var response = await _client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return 0d;
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (Convert.ToDouble(content) > 0)
                    {
                        return Convert.ToDouble(content);
                    }
                    else
                    {
                        return 0d;
                    }
                }
            }
        }

        public async Task<Boolean> InserePedidoItem(PedidoItemModel item)
        {
            if (item == null)
            {
                return false;
            }
            else
            {
                string url = string.Concat("http://mrsistemas.net/grupo_mr_api/api/PedidoItem/InserePedidoItem",
                                           "?IdPedido=" + item.IdPedido.ToString(),
                                           "&IdFornecedor=" + item.IdFornecedor.ToString(),
                                           "&IdProduto=" + item.IdProduto.ToString(),
                                           "&NomeProduto=" + item.NomeProduto.ToString(),
                                           "&CodigoProduto=" + item.CodigoProduto.ToString(),
                                           "&Quantidade=" + item.Quantidade.ToString(),
                                           "&ValorProduto=" + item.ValorProduto.ToString(),
                                           "&PercentualDesconto=" + item.PercentualDesconto.ToString(),
                                           "&ValorDesconto=" + item.ValorDesconto.ToString(),
                                           "&ValorDescontoDist=" + item.ValorDescontoDist.ToString(),
                                           "&PercentualDescontoDist=" + item.PercentualDescontoDist.ToString(),
                                           "&IdCampanha=" + item.IdCampanha.ToString(),
                                           "&Obs=" + item.Obs.ToString());
                _client = new HttpClient();
                var serializedProduto = JsonConvert.SerializeObject(item);
                var content = new StringContent(serializedProduto, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(url, content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<Boolean> AtualizaPedidoItem(PedidoItemModel item)
        {
            if (item == null)
            {
                return false;
            }
            else
            {
                string url = string.Concat("http://mrsistemas.net/grupo_mr_api/api/PedidoItem/AtualizaPedidoItem",
                                           "?IdItem=" + item.IdItem.ToString(),
                                           "&IdPedido=" + item.IdPedido.ToString(),
                                           "&IdFornecedor=" + item.IdFornecedor.ToString(),
                                           "&IdProduto=" + item.IdProduto.ToString(),
                                           "&NomeProduto=" + item.NomeProduto.ToString(),
                                           "&CodigoProduto=" + item.CodigoProduto.ToString(),
                                           "&Quantidade=" + item.Quantidade.ToString(),
                                           "&ValorProduto=" + item.ValorProduto.ToString(),
                                           "&PercentualDesconto=" + item.PercentualDesconto.ToString(),
                                           "&ValorDesconto=" + item.ValorDesconto.ToString(),
                                           "&PercentualDescontoDist=" + item.PercentualDescontoDist.ToString(),
                                           "&ValorDescontoDist=" + item.ValorDescontoDist.ToString(),
                                           "&IdCampanha=" + item.IdCampanha.ToString(),
                                           "&Obs=" + item.Obs);
                _client = new HttpClient();
                var serializedProduto = JsonConvert.SerializeObject(item);
                var content = new StringContent(serializedProduto, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(url, content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<Int16> DeletaPedidoItem(Int32 id_item)
        {
            if (id_item == 0)
            {
                return 0;
            }
            else
            {
                string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/PedidoItem/DeletaPedidoItem?id_item={0}", id_item));

                _client = new HttpClient();
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return 0;
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (Convert.ToInt16(content) > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
