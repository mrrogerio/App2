using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Model
{
    public class LoginFuncModel
    {
        [JsonProperty("IdFuncionario")]
        public int IdFuncionario { get; set; }

        [JsonProperty("CargoFuncionario")]
        public string CargoFuncionario { get; set; }

        [JsonProperty("NomeFuncionario")]
        public string NomeFuncionario { get; set; }

        [JsonProperty("CpfFuncionario")]
        public string CpfFuncionario { get; set; }

        [JsonProperty("EmailFuncionario")]
        public string EmailFuncionario { get; set; }

        [JsonProperty("SenhaFuncionario")]
        public string SenhaFuncionario { get; set; }
    }
}
