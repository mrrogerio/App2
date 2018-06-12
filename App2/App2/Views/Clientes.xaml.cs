using App2.Model;
using App2.Services;
using App2.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
    public partial class Clientes : ContentPage
    {
        static bool isCnpj = false;
        public Clientes()
        {
            InitializeComponent();
            this.BindingContext = this;
            this.IsBusy = false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private bool ValidaCampos()
        {
            if (isCnpj)
            {
                if (string.IsNullOrEmpty(lblRazaoSocial.Text)) return false;
                if (string.IsNullOrEmpty(lblNomeFantasia.Text)) return false;
            }
            else
            {
                if (string.IsNullOrEmpty(lblRazaoSocial.Text) || string.IsNullOrEmpty(lblNomeFantasia.Text)) return false;
            }
            if (string.IsNullOrEmpty(lblTelefone.Text)) return false;
            if (string.IsNullOrEmpty(lblEmail.Text)) return false;
            if (string.IsNullOrEmpty(lblEndereco.Text)) return false;
            if (string.IsNullOrEmpty(lblNumero.Text)) return false;
            if (string.IsNullOrEmpty(lblBairro.Text)) return false;
            if (string.IsNullOrEmpty(lblMunicipio.Text)) return false;
            if (string.IsNullOrEmpty(lblEstado.Text)) return false;
            if (string.IsNullOrEmpty(lblCep.Text)) return false;
            if (isCnpj && string.IsNullOrEmpty(lblIE.Text)) return false;
            return true;
        }
        private void LimpaCampos()
        {
            lblRazaoSocial.Text = string.Empty;
            lblNomeFantasia.Text = string.Empty;
            lblTelefone.Text = string.Empty;
            lblEmail.Text = string.Empty;
            lblEndereco.Text = string.Empty;
            lblNumero.Text = string.Empty;
            lblBairro.Text = string.Empty;
            lblMunicipio.Text = string.Empty;
            lblEstado.Text = string.Empty;
            lblCep.Text = string.Empty;
            lblIE.Text = string.Empty;
        }
        async Task ConsultaCNPJReceita(string cnpj)
        {
            // Inicia consulta do cnpj
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.IsBusy = true;
            // Procura CNPJ na Receita
            var uri = new Uri(string.Concat("https://www.receitaws.com.br/v1/cnpj/" + cnpj));
            var response = await client.GetAsync(uri);
            this.IsBusy = false;
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content) || content.ToString().ToLower().Equals("null"))
                {
                    //btnCadastraCliente.IsEnabled = false;
                    LimpaCampos();
                    await DisplayAlert("Alerta!", "CNPJ não encontrado.", "OK");
                    return;
                }
                // Preenche tela com dados obtidos
                JObject output = JObject.Parse(content);
                string fantasia = output.SelectToken("fantasia").ToString().Trim();
                string juridica = output.SelectToken("nome").ToString().Trim();
                string telefone = output.SelectToken("telefone").ToString().Trim();
                string email = output.SelectToken("email").ToString().Trim();
                string enderecoNumero = output.SelectToken("logradouro").ToString().Trim() + " " + output.SelectToken("numero").ToString().Trim();
                string endereco = output.SelectToken("logradouro").ToString().Trim();
                string numero = output.SelectToken("numero").ToString().Trim();
                string bairro = output.SelectToken("bairro").ToString().Trim();
                string cidade = output.SelectToken("municipio").ToString().Trim();
                string estado = output.SelectToken("uf").ToString().Trim();
                string cep = output.SelectToken("cep").ToString().Trim();
                string situacao = output.SelectToken("situacao").ToString().Trim();
                string complemento = output.SelectToken("complemento").ToString().Trim();

                lblRazaoSocial.Text = juridica;
                lblNomeFantasia.Text = fantasia;
                lblTelefone.Text = telefone;
                lblEmail.Text = email;
                lblEndereco.Text = endereco;
                lblNumero.Text = numero;
                lblBairro.Text = bairro;
                lblMunicipio.Text = cidade;
                lblEstado.Text = estado;
                lblCep.Text = cep;
                lblIE.Text = situacao;

                btnCadastraCliente.IsEnabled = true;
                return;
            }
            else
            {
                LimpaCampos();
                btnCadastraCliente.IsEnabled = true;
                await DisplayAlert("Alerta!", "O CNPJ " + cnpj + " não existe na base da Receita Federal.", "OK");
                return;
            }
        }
        async Task ConsultaCliente()
        {
            if (string.IsNullOrEmpty(userCodeEntry.Text))
            {
                btnCadastraCliente.IsEnabled = false;
                btnCadastraCliente.Opacity = 0.5;
                await DisplayAlert("Alerta!", "Preencha o CPF ou CNPJ.", "OK");
                userCodeEntry.Focus();
                return;
            }

            string buscaCliente = Util.RemoveSpecialCharacters(userCodeEntry.Text.Trim());
            if (buscaCliente.Length != 11 && buscaCliente.Length != 14)
            {
                LimpaCampos();
                btnCadastraCliente.IsEnabled = false;
                btnCadastraCliente.Opacity = 0.5;
                await DisplayAlert("Alerta!", "O CPF ou CNPJ não é válido.", "OK");
                return;
            }
            if (buscaCliente.Length == 11)
            {
                isCnpj = false;
                if (!Util.IsCpf(buscaCliente))
                {
                    LimpaCampos();
                    btnCadastraCliente.IsEnabled = false;
                    btnCadastraCliente.Opacity = 0.5;
                    await DisplayAlert("Alerta!", "O CPF não é válido.", "OK");
                    return;
                }
            }
            if (buscaCliente.Length == 14)
            {
                isCnpj = true;
                if (!Util.IsCnpj(buscaCliente))
                {
                    LimpaCampos();
                    btnCadastraCliente.Opacity = 0.5;
                    btnCadastraCliente.IsEnabled = false;
                    await DisplayAlert("Alerta!", "O CNPJ não é válido.", "OK");
                    return;
                }
            }
            //Verifica se já existe CNPJ cadastrado
            IsBusy = true;
            ClienteService clienteLogado = new ClienteService();
            ClienteModel cliente = new ClienteModel();
            cliente = await clienteLogado.BuscaClientePorCnpj(buscaCliente);
            IsBusy = false;

            if (cliente != null && cliente.IdCliente > 0)
            {
                GlobalVariables.GlobalClientePedido = cliente;
                lblRazaoSocial.Text = cliente.RazaoSocial.ToUpper();
                lblNomeFantasia.Text = cliente.NomeFantasia.ToUpper();
                lblTelefone.Text = cliente.Fone;
                lblEmail.Text = cliente.Email;
                lblEndereco.Text = cliente.Endereco;
                lblNumero.Text = cliente.Numero;
                lblBairro.Text = cliente.Bairro;
                lblMunicipio.Text = cliente.Cidade;
                lblEstado.Text = cliente.Estado;
                lblCep.Text = cliente.Cep;
                if (isCnpj) { lblIE.Text = cliente.IE; }
                else { lblIE.Text = ""; }
                btnCadastraCliente.IsEnabled = false;
            }
            else
            {
                if (isCnpj)
                {
                    var answer = await DisplayAlert("Cliente não cadastrado!", "Deseja consultar o CNPJ na base da Receita Federal?", "Sim", "Não");
                    if (answer)
                    {
                        await ConsultaCNPJReceita(buscaCliente);
                    }
                    else
                    {
                        btnCadastraCliente.Opacity = 1;
                        btnCadastraCliente.IsEnabled = true;
                    }
                }
                else
                {
                    await DisplayAlert("Info", "Cliente não cadastrado!", "OK");
                    btnCadastraCliente.Opacity = 1;
                    btnCadastraCliente.IsEnabled = true;
                    return;
                }
            }
        }
        async Task<bool> CadastraCliente()
        {
            if (!ValidaCampos())
            {
                await DisplayAlert("Alerta!", "Preencha todos os campos.", "OK");
                return false;
            }
            string cnpj = userCodeEntry.Text.Trim();
            if (!Util.IsCnpj(cnpj))
            {
                await DisplayAlert("Alerta!", "O CNPJ não é válido.", "OK");
                return false;
            }
            ClienteModel cliente = new ClienteModel();
            cliente.IdCategoria = 6;
            cliente.IdIndicante = 0;
            cliente.CnpjCpf = cnpj;
            cliente.Fone = lblTelefone.Text;
            cliente.Email = lblEmail.Text;
            cliente.Cep = lblCep.Text;
            cliente.Codigo = null;
            cliente.RazaoSocial = lblRazaoSocial.Text;
            cliente.NomeFantasia = lblNomeFantasia.Text;
            cliente.Endereco = lblEndereco.Text;
            cliente.Numero = lblNumero.Text;
            cliente.Bairro = lblBairro.Text;
            cliente.Complemento = "";
            cliente.Cidade = lblMunicipio.Text;
            cliente.Estado = lblEstado.Text;
            cliente.IE = lblIE.Text;

            var answer = await DisplayAlert("Info!", "Confirma cadastro?", "Sim", "Não");
            if (answer)
            {
                var cad = await CadastraCliente(cliente);
                if (cad == true)
                {
                    //btnCancelaCliente.IsEnabled = true;
                    return true;
                }
                else
                {
                    LimpaCampos();
                    //btnCancelaCliente.IsEnabled = false;
                    return false;
                }
            }
            else
            {
                cliente = null;
                GlobalVariables.GlobalClientePedido = null;
                return false;
            }
        }
        async Task<bool> CadastraCliente(ClienteModel cliente)
        {
            IsBusy = true;
            ClienteService clienteCad = new ClienteService();
            cliente = await clienteCad.CadastraCliente(cliente);
            IsBusy = false;

            if (cliente == null || cliente.IdCliente == 0)
            {
                await DisplayAlert("Alerta!", "Falha ao cadastrar cliente.", "OK");
                //btnCancelaCliente.IsEnabled = false;
                return false;
            }
            else
            {
                GlobalVariables.GlobalClientePedido = cliente;
                //btnCancelaCliente.IsEnabled = true;
                return true;
            }
        }
        protected void CancelaCliente()
        {
            btnCadastraCliente.Opacity = 1;
            LimpaCampos();
        }

        private void ButtonCallClicked(object sender, EventArgs e)
        {
            string phoneNumber = lblTelefone.Text;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return;
            }
            // Following line used to display given phone number in dialer  
            Device.OpenUri(new Uri(String.Format("tel:{0}", phoneNumber)));
        }

        async Task BuscaCep()
        {
            if (string.IsNullOrWhiteSpace(lblCep.Text))
            {
                await DisplayAlert("Alerta!", "Digite o Cep para procurar.", "OK");
                return;
            }
            IsBusy = true;
            CepService srvCep = new CepService();
            CepModel cep = new CepModel();
            cep = await srvCep.BuscaCep(lblCep.Text);
            IsBusy = false;
            if (cep.Cep == null)
            {
                await DisplayAlert("Alerta!", "Falha ao procurar o Cep.", "OK");
                return;
            }
            else
            {
                lblEndereco.Text = cep.Logradouro;
                lblMunicipio.Text = cep.Localidade;
                lblEstado.Text = cep.UF;
                lblBairro.Text = cep.Bairro;
                lblComplemento.Text = cep.Complemento;
            }
        }
    }
}