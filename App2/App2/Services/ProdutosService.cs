using App2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App2.Services
{
    public class ProdutosService
    {
        private HttpClient _client = new HttpClient();
        private List<ProdutosModel> _produtos;
        public async Task<List<ProdutosModel>> BuscaProdutosPorCampanha(int campanha)
        {
            if (campanha == 0)
            {
                return null;
            }
            else
            {
                string url = string.Format("http://mrsistemas.net/grupo_mr_api/api/Produtos/RetornaProdutosPorCampanha?campanha={0}", campanha);
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _produtos = new List<ProdutosModel>();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var produtos = JsonConvert.DeserializeObject<List<ProdutosModel>>(content);
                    _produtos = new List<ProdutosModel>(produtos);
                }
                return _produtos;
            }
        }

        public async Task<List<ProdutosModel>> BuscaProdutosPorCodigo(string codigo, int campanha)
        {
            if  (string.IsNullOrEmpty (codigo.Trim()))
            {
                return null;
            }
            else
            {
                string url = string.Format("http://mrsistemas.net/grupo_mr_api/api/Produtos/RetornaProdutosPorCodigo?codigo={0}&campanha={1}", codigo, campanha);
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _produtos = new List<ProdutosModel>();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var produtos = JsonConvert.DeserializeObject<List<ProdutosModel>>(content);
                    _produtos = new List<ProdutosModel>(produtos);
                }
                return _produtos;
            }
        }

        public async Task<List<ProdutosModel>> BuscaProdutosPorWhere(string where)
        {
            if (string.IsNullOrEmpty(where.Trim()))
            {
                return null;
            }
            else
            {
                string url = string.Format("http://mrsistemas.net/grupo_mr_api/api/Produtos/RetornaProdutosPorWhere?where={0}", where);
                var response = await _client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _produtos = new List<ProdutosModel>();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var produtos = JsonConvert.DeserializeObject<List<ProdutosModel>>(content);
                    _produtos = new List<ProdutosModel>(produtos);
                }
                return _produtos;
            }
        }
    }
}
