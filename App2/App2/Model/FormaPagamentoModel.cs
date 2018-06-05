using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
   public class FormaPagamentoModel
    {
        [JsonProperty("IdFormaPagamento")]
        public int IdFormaPagamento { get; set; }

        [JsonProperty("DescricaoFormaPagamento")]
        public string DescricaoFormaPagamento { get; set; }
    }
}
