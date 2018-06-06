using App2.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App2.Services
{
    public class FormaPagamentoService
    {
        private HttpClient _client = new HttpClient();
        private List<FormaPagamentoModel> _lstPagamento;

        public async Task<List<FormaPagamentoModel>> RetornaFormaPagamento()
        {

            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var uri = new Uri("http://mrsistemas.net/grupo_mr_api/api/FormaPagamento/RetornaFormaPagamento");
            var response = await client.GetAsync(uri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _lstPagamento = new List<FormaPagamentoModel>();
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<FormaPagamentoModel>>(content);
                _lstPagamento = new List<FormaPagamentoModel>(Items);
            }
            return _lstPagamento;
        }

        public async Task<List<FormaPagamentoModel>> RetornaFormaPagamentoPorIdPedido(Int32 id_pedido)
        {

            if (id_pedido == 0)
            {
                return null;
            }
            else
            {
                string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/FormaPagamento/RetornaFormaPagamentoPorIdPedido?id_pedido={0}", id_pedido));

                _client = new HttpClient();
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _lstPagamento = new List<FormaPagamentoModel>();
                }
                else
                {
                    _lstPagamento = new List<FormaPagamentoModel>();
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<FormaPagamentoModel>(content);
                    _lstPagamento.Add(Items);
                }
                return _lstPagamento;
            }
        }
    }
}
