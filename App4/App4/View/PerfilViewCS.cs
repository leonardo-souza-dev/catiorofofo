using App4.Model;
using App4.Repository;
using App4.ViewModel;

using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.IO;

using Xamarin.Forms;

namespace App4.View
{
    public partial class PerfilViewCS : ContentPage
    {
        private PostViewModel PostViewModel;
        private UsuarioViewModel UsuarioViewModel;
        public Usuario posts { get; set; }
        private Usuario Usuario;

        string NomeUsuarioValorInicial;

        public PerfilViewCS(PostViewModel postViewModel, ConfiguracaoApp config)
        {
            UsuarioViewModel = new UsuarioViewModel(config);
            PostViewModel = postViewModel;

            Title = "perfil";

            CrossMedia.Current.Initialize();
            Usuario = new Usuario
            {
                AvatarUrl = @"http://icon-icons.com/icons2/108/PNG/128/males_male_avatar_man_people_faces_18362.png",
                Email = "asdasdasd@asdasd.xom",
                NomeUsuario = "rex"
            };

            BindingContext = Usuario;
                

            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo(){
            var TituloLabel = new Label
            {
                Text = "perfil",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                //Margin = new Thickness(5, 5, 5, 5),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            var AvatarImage = new Image
            {
                WidthRequest = 30,
                HeightRequest = 30,
                //Margin = new Thickness(5, 5, 5, 5),
                IsEnabled = false
            };
            var NomeUsuarioEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //Margin = new Thickness(5, 5, 5, 5),
                IsEnabled = false
            };
            var EmailEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //Margin = new Thickness(5, 5, 5, 5),
                IsEnabled = false
            };
            var CancelarButton = new Button
            {
                FontSize = 10,
                Text = "cancelar",
                //Margin = new Thickness(5, 5, 5, 5),
                IsVisible = false

            };
            var SalvarButton = new Button
            {
                FontSize = 10,
                Text = "salva",
                //Margin = new Thickness(5, 5, 5, 5),
                IsVisible = false
            };
            var EditarButton = new Button
            {
                FontSize = 10,
                Text = "editar"
                //,Margin = new Thickness(5, 5, 5, 5)
            };

            AvatarImage.SetBinding(Image.SourceProperty, new Binding("AvatarUrl"));
            NomeUsuarioEntry.SetBinding(Entry.TextProperty, new Binding("NomeUsuario"));
            EmailEntry.SetBinding(Entry.TextProperty, new Binding("Email"));

            CancelarButton.Clicked += (object sender, EventArgs e) =>
            {
                NomeUsuarioEntry.IsEnabled = false;
                EmailEntry.IsEnabled = false;
                CancelarButton.IsVisible = false;
                SalvarButton.IsVisible = false;
                EditarButton.IsVisible = true;

                NomeUsuarioEntry.Text = NomeUsuarioValorInicial;
            };
            SalvarButton.Clicked += async (object sender, EventArgs e) =>
            {
                NomeUsuarioEntry.IsEnabled = false;
                EmailEntry.IsEnabled = false;
                CancelarButton.IsVisible = false;
                SalvarButton.IsVisible = false;
                EditarButton.IsVisible = true;

                var resultado2 = await UsuarioViewModel.AtualizarCadastro(Usuario);
                switch (resultado2)
                {
                    case RespostaStatus.Sucesso:
                        break;
                    case RespostaStatus.Inexistente:
                        await DisplayAlert("ops", "erro estranho", "volta lá");
                        break;
                    case RespostaStatus.JaExiste:
                        await DisplayAlert("ops", "ja existe nome de usuario", "volta lá");
                        EmailEntry.Focus();
                        break;
                }
                return;
            };
            EditarButton.Clicked += (object sender, EventArgs e) =>
            {
                NomeUsuarioValorInicial = NomeUsuarioEntry.Text;

                NomeUsuarioEntry.IsEnabled = true;
                EmailEntry.IsEnabled = true;
                CancelarButton.IsVisible = true;
                SalvarButton.IsVisible = true;
                EditarButton.IsVisible = false;
            };

            return new StackLayout
            {
                Padding = new Thickness(0, 10, 0, 0),
                Children = {
                    TituloLabel,
                    AvatarImage,
                    NomeUsuarioEntry,
                    EmailEntry,
                    CancelarButton,
                    SalvarButton,
                    EditarButton
                }
            };
        }
        
    }
}
