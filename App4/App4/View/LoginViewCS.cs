using App4.Model;
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
        UsuarioViewModel UsuarioViewModel;

        Entry EmailEntry;
        Button LoginButton;
        Entry SenhaEntry;
        Button EsqueciButton;

        public LoginViewCS()
        {
            UsuarioViewModel = new UsuarioViewModel();
            this.Title = "login";

            var scroll = new ScrollView();
            scroll.Content = ObterConteudo();
            Content = scroll;
        }

        private StackLayout ObterConteudo()
        {
            EmailEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(5, 5, 5, 5)
            };
            LoginButton = new Button
            {
                Text = "login",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(5, 5, 5, 5)
            };
            SenhaEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(5, 5, 5, 5),
                IsPassword = true
            };
            EsqueciButton = new Button
            {
                Text = "esqueci a senha",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(5, 5, 5, 5)                
            };


            LoginButton.Clicked += LoginBotao_Clicked;
            EsqueciButton.Clicked += EsqueciButton_Clicked;

            return new StackLayout
            {
                Padding = new Thickness(0, 20, 0, 0),
                Children = {
                    new Label {
                        Text = "catioro fofo",
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    new Image {
                        Source = ImageSource.FromResource("puppy150.png"),// FromUri(new Uri(@"http://download.seaicons.com/download/i34286/wackypixel/dogs-n-puppies/wackypixel-dogs-n-puppies-puppy-10.ico")),
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    new Label {
                        Text = "email",
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    EmailEntry,
                    new Label {
                        Text = "senha",
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    SenhaEntry,
                    LoginButton,
                    new Button {
                        Text = "Cadastro",
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    EsqueciButton
                }
            };
        }

        async void LoginBotao_Clicked(object sender, EventArgs e)
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

            string senha = SenhaEntry.Text;
            Usuario usuario = await UsuarioViewModel.Login(senha);

            if (usuario.UsuarioId > 0)
            {
                var mainPage = new MainPage(usuario.UsuarioId);
                await Navigation.PushModalAsync(mainPage);
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
            RespostaStatus resultado = await UsuarioViewModel.EsqueciSenha(email);

            switch (resultado)
            {
                case RespostaStatus.Sucesso:
                    await DisplayAlert("oi", "mandei um email, ve lá", "volta lá");
                    break;
                case RespostaStatus.EmailInexistente:
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
