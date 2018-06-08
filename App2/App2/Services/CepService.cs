using App2.Model;
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
    public class CepService
    {
        private HttpClient _client = new HttpClient();
        private CepModel _cep;
        public async Task<CepModel> BuscaCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
            {
                return null;
            }
            else
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var uri = new Uri(string.Format("https://viacep.com.br/ws/{0}/json/", cep));
                var response = await client.GetAsync(uri);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _cep = new CepModel();
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject output = JObject.Parse(content);
                    _cep = new CepModel();

                    _cep.Cep = output.SelectToken("cep").ToString()?.Trim();
                    _cep.Logradouro = output.SelectToken("logradouro").ToString()?.Trim();
                    _cep.Complemento = output.SelectToken("complemento").ToString()?.Trim();
                    _cep.Bairro = output.SelectToken("bairro").ToString()?.Trim();
                    _cep.Localidade = output.SelectToken("localidade").ToString()?.Trim();
                    _cep.UF = output.SelectToken("uf").ToString()?.Trim();
                    _cep.Ibge = output.SelectToken("ibge").ToString()?.Trim();
                }
                return _cep;
            }
        }
    }
    public static class ExtensionMethods
    {
        public static string TrimIfNotNull(this string value)
        {
            if (value != null)
            {
                return value.Trim();
            }
            return null;
        }
    }
}
