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
    public class LoginFuncService
    {
        private HttpClient _client = new HttpClient();
        private LoginFuncModel _login;
        public async Task<LoginFuncModel> BuscaLoginFuncionario(string email, string senha)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
                {
                    return null;
                }
                else
                {
                    string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/LoginFuncionario/RetornaLoginFuncionario?email={0}&senha={1}", email, senha));

                    _client = new HttpClient();
                    var response = await _client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _login = new LoginFuncModel();
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var login = JsonConvert.DeserializeObject<LoginFuncModel>(content);
                        _login = new LoginFuncModel();
                        _login = (LoginFuncModel)login;
                    }
                    return _login;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
