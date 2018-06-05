using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public class LoginModel
    {
        [JsonProperty("IdCliente")]
        public int IdCliente { get; set; }

        [JsonProperty("idIndicante")]
        public int idIndicante { get; set; }

        [JsonProperty("Codigo")]
        public int Codigo { get; set; }

        [JsonProperty("IdCategoria")]
        public int IdCategoria { get; set; }

        [JsonProperty("IdRegiao")]
        public int IdRegiao { get; set; }

        [JsonProperty("TipoPessoa")]
        public string TipoPessoa { get; set; }

        [JsonProperty("RazaoSocial")]
        public string RazaoSocial { get; set; }

        [JsonProperty("NomeFantasia")]
        public string NomeFantasia { get; set; }

        [JsonProperty("CnpjCpf")]
        public string CnpjCpf { get; set; }

        [JsonProperty("Senha")]
        public string Senha { get; set; }

        [JsonProperty("BloqueiaCampanha")]
        public string BloqueiaCampanha { get; set; }
    }
}
