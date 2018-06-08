using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
   public class CepModel
    {
        [JsonProperty("Cep")]
        public string Cep { get; set; }

        [JsonProperty("Logradouro")]
        public string Logradouro { get; set; }

        [JsonProperty("Complemento")]
        public string Complemento { get; set; }

        [JsonProperty("Bairro")]
        public string Bairro { get; set; }

        [JsonProperty("Localidade")]
        public string Localidade { get; set; }

        [JsonProperty("UF")]
        public string UF { get; set; }

        [JsonProperty("Unidade")]
        public string Unidade { get; set; }

        [JsonProperty("Ibge")]
        public string Ibge { get; set; }
    }
}
