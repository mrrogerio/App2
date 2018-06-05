using App2.Model;
using App2.Utils;
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
    public class LoginService
    {
        private HttpClient _client = new HttpClient();
        private LoginModel _login;
        public async Task<LoginModel> BuscaLoginCliente(string user, string pass)
        {
            try
            {
                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
                {
                    return null;
                }
                else
                {
                    string url = Uri.EscapeUriString(string.Format("http://mrsistemas.net/grupo_mr_api/api/Login/RetornaLogin?user={0}&pass={1}", user, pass));

                    _client = new HttpClient();
                    var response = await _client.GetAsync(url);

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _login = new LoginModel();
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var login = JsonConvert.DeserializeObject<LoginModel>(content);
                        _login = new LoginModel();
                        _login = (LoginModel)login;
                    }
                    return _login;
                }
            }
            catch (Exception)
            {
                _login = new LoginModel();
                return _login;
            }
        }
    }
}
