using App4.Model;
using App4.Model.Resposta;
using App4.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4.View
{
    public partial class LoginViewCS : ContentPage
    {
        //UsuarioViewModel UsuarioViewModel;

        Label CatioroFofoLabel;
        Image LogoImage;

        Label EmailLabel;
        Entry EmailEntry;

        Label SenhaLabel;
        Entry SenhaEntry;

        Button LoginButton;
        Button CadastroButton;
        Button EsqueciButton;

        public LoginViewCS()
        {
            this.Title = "login";

            var scroll = new ScrollView();
            scroll.Content = ObterConteudo();
            Content = scroll;

            //temp
            EmailEntry.Text = "qwe";
            SenhaEntry.Text = "qwe";

            LoginBotao_Clicked(null, null);
        }



        private StackLayout ObterConteudo()
        {
            CatioroFofoLabel = new Label
            {
                Text = "catioro fofo",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            LogoImage = new Image
            {
                Source = ImageSource.FromResource("puppy150.png"),
                HorizontalOptions = LayoutOptions.Center
            };

            EmailLabel = new Label
            {
                Text = "email",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            EmailEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Entry)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            SenhaLabel = new Label
            {
                Text = "senha",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            SenhaEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Entry)),
                VerticalOptions = LayoutOptions.CenterAndExpand
                ,IsPassword = true
            };

            LoginButton = new Button
            {
                Text = "login",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            CadastroButton = new Button
            {
                Text = "cadastro",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            EsqueciButton = new Button
            {
                Text = "esqueci a senha",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };


            LoginButton.Clicked += LoginBotao_Clicked;
            CadastroButton.Clicked += CadastroButton_Clicked;
            EsqueciButton.Clicked += EsqueciButton_Clicked;

            return new StackLayout
            {
                Children = {
                    CatioroFofoLabel,
                    LogoImage,

                    EmailLabel,
                    EmailEntry,

                    SenhaLabel,
                    SenhaEntry,

                    LoginButton,
                    CadastroButton,
                    EsqueciButton
                }
            };
        }

        async void CadastroButton_Clicked(object sender, EventArgs e)
        {
            ValidaEntradas();

            string email = EmailEntry.Text;
            string senha = SenhaEntry.Text;

            var resposta = await App.UsuarioVM.CadastrarELogar(email, senha);

            if (resposta != null && resposta.Item1 == RespostaStatus.Sucesso)
            {
                var mainPage = new MainPage();
                await Navigation.PushModalAsync(mainPage);
            }
        }

        async void ValidaEntradas()
        {
            if (SenhaEntry == null || string.IsNullOrEmpty(SenhaEntry.Text))
            {
                if (EmailEntry == null || string.IsNullOrEmpty(EmailEntry.Text))
                {
                    await DisplayAlert("ops", "tem que colocar um email e senha, migx", "volta lá");
                    EmailEntry.Focus();
                    return;
                }
                await DisplayAlert("ops", "tem que colocar uma senha, migx", "volta lá");
                EmailEntry.Focus();
                return;
            }
        }

        private void TesteConexao()
        {
            var resposta = App.UsuarioVM.TesteConexao();

            if (resposta == RespostaStatus.Inexistente)
            {
                DisplayAlert("ops", "parece que nao tem internet", "volta lá");
            }

        }

        async void LoginBotao_Clicked(object sender, EventArgs e)
        {
            //TesteConexao();

            ValidaEntradas();

            string email = EmailEntry.Text;
            string senha = SenhaEntry.Text;
            bool logou = await App.UsuarioVM.Login(email, senha);

            if (logou)
            {
                var mainPage = new MainPage();
                await Navigation.PushModalAsync(mainPage);
            }
            else
            {
                await DisplayAlert("ops", "não tem esse email", "volta lá");
                EmailEntry.Focus();
            }
        }

        async void EsqueciButton_Clicked(object sender, EventArgs e)
        {
            if (EmailEntry == null || string.IsNullOrEmpty(EmailEntry.Text))
            {
                await DisplayAlert("ops", "tem que colocar um email, migx", "volta lá");
                EmailEntry.Focus();
                return;
            }

            string email = EmailEntry.Text;
            var resultado = await App.UsuarioVM.EsqueciSenha(email);

            switch (resultado)
            {
                case RespostaStatus.Sucesso:
                    await DisplayAlert("oi", "mandei um email, ve lá", "volta lá");
                    break;
                case RespostaStatus.Inexistente:
                    await DisplayAlert("ih", "nao existe usuario com esse email manolx", "volta lá");
                    break;
                case RespostaStatus.ErroGenerico:
                    await DisplayAlert("ih", "deu um erro, mals", "volta lá");
                    break;
                default:
                    break;
            }
        }
    }
}
