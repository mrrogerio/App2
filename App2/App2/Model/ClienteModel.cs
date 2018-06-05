using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public class ClienteModel
    {
        [JsonProperty("IdCliente")]
        public int IdCliente { get; set; }

        [JsonProperty("IdIndicante")]
        public int IdIndicante { get; set; }

        [JsonProperty("IdCategoria")]
        public int IdCategoria { get; set; }

        [JsonProperty("IdRegiao")]
        public int IdRegiao { get; set; }

        [JsonProperty("Codigo")]
        public string Codigo { get; set; }

        [JsonProperty("Senha")]
        public string Senha { get; set; }

        [JsonProperty("TipoPessoa")]
        public string TipoPessoa { get; set; }

        [JsonProperty("RazaoSocial")]
        public string RazaoSocial { get; set; }

        [JsonProperty("NomeFantasia")]
        public string NomeFantasia { get; set; }

        [JsonProperty("CnpjCpf")]
        public string CnpjCpf { get; set; }

        [JsonProperty("IE")]
        public string IE { get; set; }

        [JsonProperty("Fone")]
        public string Fone { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Cep")]
        public string Cep { get; set; }

        [JsonProperty("Endereco")]
        public string Endereco { get; set; }

        [JsonProperty("Numero")]
        public string Numero { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("Complemento")]
        public string Complemento { get; set; }

        [JsonProperty("Cidade")]
        public string Cidade { get; set; }

        [JsonProperty("Estado")]
        public string Estado { get; set; }

    }
}
