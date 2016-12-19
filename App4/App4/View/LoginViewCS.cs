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
        public LoginViewCS()
        {
            this.Title = "login";

            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo()
        {
            Button loginBotao = new Button
            {
                Text = "Login",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(5, 5, 5, 5)
            };

            loginBotao.Clicked += LoginBotao_Clicked;

            return new StackLayout
            {
                Padding = new Thickness(0, 20, 0, 0),
                Children = {
                    new Label {
                        Text = "catioro fofo",
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    new Image {
                        Source = ImageSource.FromResource("puppy150.png"),// FromUri(new Uri(@"http://download.seaicons.com/download/i34286/wackypixel/dogs-n-puppies/wackypixel-dogs-n-puppies-puppy-10.ico")),
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    new Label {
                        Text = "login",
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    new Entry {
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    new Label {
                        Text = "senha",
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    new Entry {
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness (5, 5, 5, 5)
                    },
                    loginBotao,
                    new Button {
                        Text = "Cadastro",
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness (5, 5, 5, 5)
                    }

                }
            };
        }

        private int usuarioId;

        async void LoginBotao_Clicked(object sender, EventArgs e)
        {
            string senha = "123";
            UsuarioViewModel uvm = new UsuarioViewModel();
            Usuario usuario = await uvm.Login(senha);

            if (usuario.UsuarioId > 0)
            {
                var mainPage = new MainPage(usuario.UsuarioId);
                await Navigation.PushModalAsync(mainPage);
            }
        }
    }
}
