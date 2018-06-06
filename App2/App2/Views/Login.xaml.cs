using App2.Model;
using App2.Services;
using App2.Utils;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            this.BindingContext = this;
            this.IsBusy = false;

            var getLogin = Util.getSettings();
            if (!string.IsNullOrEmpty(getLogin))
            {
                string[] partsFromString = getLogin.Split(new string[] { "|" }, StringSplitOptions.None);
                usernameEntry.Text = partsFromString[0];
                passwordEntry.Text = partsFromString[1];
            }
            var tog = Util.RetornaMostraSenha();
            swtcSenha.IsToggled = tog;

            Util.SalvaMostraSenha(swtcSenha.IsToggled);
        }

        protected override void OnAppearing()
        {
            usernameEntry.Completed += (s, e) =>
            {
                passwordEntry.Focus();
            };
        }

        async void OnExitClicked(object sender, EventArgs args)
        {
            var answer = await DisplayAlert("Alerta!", "Deseja sair", "Sim", "Não");
            if (answer)
            {
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
        }

        void OnLoginClicked(object sender, EventArgs args)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Alerta!", "Sem conexão com à Internet.", "OK");
                return;
            }
            LoginVendedor();
        }
        async void LoginCliente()
        {
            string usuario = usernameEntry.Text.Trim();
            string senha = passwordEntry.Text.Trim();

            if (string.IsNullOrEmpty(usuario))
            {
                await DisplayAlert("Alerta!", "Usuário inválido.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(senha))
            {
                await DisplayAlert("Alerta!", "Senha inválida.", "OK");
                return;
            }

            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var uri = new Uri(string.Concat("http://mrsistemas.net/grupo_mr_api/api/", "Login/RetornaLogin?user=" + usuario + "&pass=" + senha));
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content) || content.ToString().ToLower().Equals("null"))
                {
                    await DisplayAlert("Alerta!", "Usuário ou Senha inválidos.", "OK");
                    return;
                }

                JObject output = JObject.Parse(content);
                string user = output.SelectToken("CnpjCpf").ToString().Trim();
                string pwd = output.SelectToken("Senha").ToString().Trim();

                if (user.Equals(usuario) && pwd.Equals(senha))
                {

                    await Navigation.PushModalAsync(new MainPage());
                }
                else
                {
                    await DisplayAlert("Alerta!", "Usuário ou Senha inválidos.", "OK");
                    return;
                }
            }
            else
            {
                await DisplayAlert("Alerta!", "Falha na conexão, verifique sua conexão <br> e tente novamente.", "OK");
                return;
            }
        }
        async void LoginVendedor()
        {
            string usuario = usernameEntry.Text.Trim();
            string senha = passwordEntry.Text.Trim();

            if (string.IsNullOrEmpty(usuario))
            {
                await DisplayAlert("Alerta!", "Usuário inválido.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(senha))
            {
                await DisplayAlert("Alerta!", "Senha inválida.", "OK");
                return;
            }

            this.IsBusy = true;
            LoginFuncService funcionarioLogado = new LoginFuncService();
            LoginFuncModel funcionario = new LoginFuncModel();
            funcionario = await funcionarioLogado.BuscaLoginFuncionario(usuario, senha);
            this.IsBusy = false;

            if (funcionario.IdFuncionario == 0)
            {
                await DisplayAlert("Alerta!", "Usuário ou Senha inválidos.", "OK");
                return;
            }
            if (funcionario.EmailFuncionario.Equals(usuario) && funcionario.SenhaFuncionario.Equals(senha))
            {
                GlobalVariables.GlobalFuncionarioLogado = funcionario;
                Util.saveSettings(usuario, senha);
                Util.SalvaMostraSenha(swtcSenha.IsToggled);

                await Navigation.PushModalAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Alerta!", "Usuário ou Senha inválidos.", "OK");
                return;
            }
        }
    }
}