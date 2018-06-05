using App2.Model;
using App2.Utils;
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
    public class ClienteService
    {
        private HttpClient _client = new HttpClient();
        private ClienteModel _cliente;
        public async Task<ClienteModel> BuscaClientePorCnpj(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
            {
                return null;
            }
            else
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var uri = new Uri(string.Format("http://mrsistemas.net/grupo_mr_api/api/Cliente/RetornaClientePorCnpjCpf?cnpjCpf={0}", cnpj));
                var response = await client.GetAsync(uri);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _cliente = new ClienteModel();
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject output = JObject.Parse(content);
                    _cliente = new ClienteModel();

                    _cliente.IdCliente = (int)output.SelectToken("IdCliente");
                    _cliente.IdIndicante = (int)output.SelectToken("IdIndicante");
                    _cliente.Codigo = output.SelectToken("Codigo").ToString().Trim();
                    _cliente.IdCategoria = (int)output.SelectToken("IdCategoria");
                    _cliente.IdRegiao = (int)output.SelectToken("IdRegiao");
                    _cliente.TipoPessoa = output.SelectToken("TipoPessoa").ToString().Trim();
                    _cliente.RazaoSocial = output.SelectToken("RazaoSocial").ToString().Trim();
                    _cliente.NomeFantasia = output.SelectToken("NomeFantasia").ToString().Trim();
                    _cliente.CnpjCpf = output.SelectToken("CnpjCpf").ToString().Trim();
                    _cliente.Senha = output.SelectToken("Senha").ToString().Trim();
                    _cliente.Fone = output.SelectToken("Fone").ToString().Trim();
                    _cliente.Email = output.SelectToken("Email").ToString().Trim();
                    _cliente.Endereco = output.SelectToken("Endereco").ToString().Trim();
                    _cliente.Numero = output.SelectToken("Numero").ToString().Trim();
                    _cliente.Bairro = output.SelectToken("Bairro").ToString().Trim();
                    _cliente.Complemento = output.SelectToken("Complemento").ToString().Trim();
                    _cliente.Cidade = output.SelectToken("Cidade").ToString().Trim();
                    _cliente.Estado = output.SelectToken("Estado").ToString().Trim();
                    _cliente.Cep = output.SelectToken("Cep").ToString().Trim();
                    _cliente.IE = output.SelectToken("InscricaoEstadual").ToString().Trim();
                }
                return _cliente;
            }
        }

        public async Task<ClienteModel> BuscaClientePorIdCliente(int id_cliente)
        {
            if (id_cliente == 0)
            {
                return null;
            }
            else
            {
                string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/Cliente/RetornaClientePorCnpjCpf?cnpjCpf={0}", id_cliente));
                _client = new HttpClient();
                var response = await _client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _cliente = new ClienteModel();
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject output = JObject.Parse(content);
                    _cliente = new ClienteModel();

                    _cliente.IdCliente = (int)output.SelectToken("IdCliente");
                    _cliente.IdIndicante = (int)output.SelectToken("IdIndicante");
                    _cliente.Codigo = output.SelectToken("Codigo").ToString().Trim();
                    _cliente.IdCategoria = (int)output.SelectToken("IdCategoria");
                    _cliente.IdRegiao = (int)output.SelectToken("IdRegiao");
                    _cliente.TipoPessoa = output.SelectToken("TipoPessoa").ToString().Trim();
                    _cliente.TipoPessoa = output.SelectToken("TipoPessoa").ToString().Trim();
                    _cliente.RazaoSocial = output.SelectToken("RazaoSocial").ToString().Trim();
                    _cliente.NomeFantasia = output.SelectToken("NomeFantasia").ToString().Trim();
                    _cliente.CnpjCpf = output.SelectToken("CnpjCpf").ToString().Trim();
                    _cliente.Senha = output.SelectToken("Senha").ToString().Trim();
                    _cliente.Fone = output.SelectToken("Fone").ToString().Trim();
                    _cliente.Email = output.SelectToken("Email").ToString().Trim();
                    _cliente.Endereco = output.SelectToken("Endereco").ToString().Trim();
                    _cliente.Numero = output.SelectToken("Numero").ToString().Trim();
                    _cliente.Bairro = output.SelectToken("Bairro").ToString().Trim();
                    _cliente.Complemento = output.SelectToken("Complemento").ToString().Trim();
                    _cliente.Cidade = output.SelectToken("Cidade").ToString().Trim();
                    _cliente.Cep = output.SelectToken("Cep").ToString().Trim();
                }
                return _cliente;
            }
        }

        public async Task<ClienteModel> CadastraCliente(ClienteModel cliente)
        {

            if (cliente == null)
            {
                return null;
            }
            else
            {
                string url = string.Concat("http://mrsistemas.net/grupo_mr_api/api/", "Cliente/CadastraCliente",
                                           "?idCategoria=6",
                                           "&idIndicante=0",
                                           "&inscricaoEstadual=" + cliente.IE,
                                           "&cnpjCpf=" + cliente.CnpjCpf,
                                           "&foneCliente=" + Util.RemoveSpecialCharacters(cliente.Fone),
                                           "&emailCliente=" + cliente.Email,
                                           "&cepCliente=" + Util.RemoveSpecialCharacters(cliente.Cep),
                                           "&codigoCliente=",
                                           "&razaoSocial=" + cliente.RazaoSocial,
                                           "&nomeFantasia=" + cliente.NomeFantasia,
                                           "&endereco=" + cliente.Endereco,
                                           "&numero=" + cliente.Numero,
                                           "&bairro=" + cliente.Bairro,
                                           "&complemento=" + cliente.Complemento,
                                           "&cidade=" + cliente.Cidade,
                                           "&estado=" + cliente.Estado);
                _client = new HttpClient();

                var response = await _client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _cliente = new ClienteModel();
                }
                else
                    _cliente = new ClienteModel();
                _cliente = await response.Content.ReadAsAsync<ClienteModel>();
            }
            return _cliente;
        }
    }
}
