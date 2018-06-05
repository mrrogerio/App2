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
    public class PedidoService
    {
        private HttpClient _client = new HttpClient();
        private PedidoModel _pedido;
        public async Task<PedidoModel> BuscaPedidoPorIdPedido(int id_pedido)
        {
            if (id_pedido == 0)
            {
                return null;
            }
            else
            {
                string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/Pedido/RetornaPedidoPorIdPedido?id_pedido={0}", id_pedido));

                _client = new HttpClient();
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _pedido = new PedidoModel();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pedido = JsonConvert.DeserializeObject<PedidoModel>(content);
                    _pedido = new PedidoModel();
                    _pedido = (PedidoModel)pedido;
                }
                return _pedido;
            }
        }

        public async Task<PedidoModel> BuscaPedidoEmAbertoPorIdCliente(int id_cliente)
        {
            if (id_cliente == 0)
            {
                return null;
            }
            else
            {
                string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/Pedido/RetornaPedidoEmAbertoPorCliente?id_cliente={0}", id_cliente));

                _client = new HttpClient();
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _pedido = new PedidoModel();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pedido = JsonConvert.DeserializeObject<PedidoModel>(content);
                    _pedido = new PedidoModel();
                    _pedido = (PedidoModel)pedido;
                }
                return _pedido;
            }
        }

        public async Task<PedidoModel> BuscaPedidoPorIdLote(int id_lote)
        {
            if (id_lote == 0)
            {
                return null;
            }
            else
            {
                string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/Pedido/RetornaPedidoPorIdLote?id_lote={0}", id_lote));

                _client = new HttpClient();
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _pedido = new PedidoModel();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pedido = JsonConvert.DeserializeObject<PedidoModel>(content);
                    _pedido = new PedidoModel();
                    _pedido = (PedidoModel)pedido;
                }
                return _pedido;
            }
        }

        public async Task<Int32> NovoPedido(PedidoModel ped)
        {
            if (ped == null)
            {
                return 0;
            }
            else
            {
                string url = string.Concat("http://mrsistemas.net/grupo_mr_api/api/Pedido/InserePedido",
                                           "?IdFilial=" + ped.IdFilial,
                                           "&IdLote=" + ped.IdLote,
                                           "&IdCliente=" + ped.IdCliente,
                                           "&IdFuncionario=" + ped.IdFuncionario,
                                           "&IdPagamento=" + ped.IdPagamento,
                                           "&IdStatus=" + ped.IdStatus,
                                           "&Sessao=" + ped.Sessao,
                                           "&Parcelas=" + ped.Parcelas,
                                           "&Data=" + ped.Data,
                                           "&DataEmissao=" + ped.DataEmissao,
                                           "&Itens=" + ped.Itens,
                                           "&ValorBruto=" + ped.ValorBruto,
                                           "&ValorDesconto=" + ped.ValorDesconto,
                                           "&ValorDescontoDist=" + ped.ValorDescontoDist,
                                           "&ValorLiquido=" + ped.ValorLiquido,
                                           "&Obs=" + ped.Obs,
                                           "&IdTransportadora=" + ped.IdTransportadora,
                                           "&DataEntrega=" + ped.DataEntrega,
                                           "&DescontoGeral=" + ped.DescontoGeral);
                _client = new HttpClient();

                var response = await _client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return 0;
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (Convert.ToInt32(content) > 0)
                    {
                        return Convert.ToInt32(content);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public async Task<Boolean> FinalizaPedido(Int32 id_pedido)
        {
            if (id_pedido == 0)
            {
                return false;
            }
            else
            {
                string url = string.Concat("http://mrsistemas.net/grupo_mr_api/api/Pedido/FinalizaPedido?id_pedido=" + id_pedido);
                _client = new HttpClient();

                var response = await _client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (Convert.ToInt16(content) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public async Task<Boolean> CancelaPedido(Int32 id_pedido)
        {
            if (id_pedido == 0)
            {
                return false;
            }
            else
            {
                string url = string.Concat("http://mrsistemas.net/grupo_mr_api/api/Pedido/CancelaPedido?id_pedido=" + id_pedido);
                _client = new HttpClient();

                var response = await _client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (Convert.ToInt16(content) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public async Task<Int32> AtualizaFormaPagamentoPedido(Int32 id_pedido, Int32 id_forma_pagamento)
        {
            if (id_pedido == 0 || id_forma_pagamento == 0)
            {
                return 0;
            }
            else
            {
                string url = string.Concat("http://mrsistemas.net/grupo_mr_api/api/Pedido/AtualizaFormaPagamentoPedido",
                                           "?IdPagamento=" + id_forma_pagamento,
                                           "&IdPedido=" + id_pedido);
                _client = new HttpClient();

                var response = await _client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return 0;
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (Convert.ToInt32(content) > 0)
                    {
                        return Convert.ToInt32(content);
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
