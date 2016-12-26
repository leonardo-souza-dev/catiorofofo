using App4.Model;
using App4.Repository;
using App4.ViewModel;

using Plugin.Media;
using Plugin.Media.Abstractions;

using System.Collections.ObjectModel;
using System.IO;

using Xamarin.Forms;

namespace App4.View
{
    public partial class PerfilViewCS : ContentPage
    {
        private PostViewModel PostViewModel;
        public Usuario posts { get; set; }

        //Image AvatarImage;
        //Label UsuarioLabel;
        //Label EmailLabel;

        //Button AcharButton = new Button();
        //Button PostarButton = new Button();
        //Entry LegendaEntry = new Entry();
        //Stream Stream = null;
        //MediaFile File;
        //public string Arquivo = string.Empty;

        Button CriarPerfil = new Button();
        TabbedPage MainPage;

        public PerfilViewCS(PostViewModel postViewModel)
        {
            PostViewModel = postViewModel;

            Title = "perfil";

            CrossMedia.Current.Initialize();

            BindingContext =
                new Usuario
                {
                    AvatarUrl = @"http://icon-icons.com/icons2/108/PNG/128/males_male_avatar_man_people_faces_18362.png",
                    Email = "asdasdasd@asdasd.xom",
                    NomeUsuario = "rex"
                };

            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo(){
            var TituloLabel = new Label
            {
                FontSize = 14,
                Text = "perfil",
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(5, 5, 5, 5)
            };
            var AvatarImage = new Image
            {
                WidthRequest = 30,
                HeightRequest = 30,
                Margin = new Thickness(5, 5, 5, 5)
            };
            var UsuarioLabel = new Label
            {
                FontSize = 10,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(5, 5, 5, 5)
            };
            var EmailLabel = new Label
            {
                FontSize = 10,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(5, 5, 5, 5)
            };
            var EditarButton = new Button
            {
                FontSize = 10,
                Text = "editar",
                Margin = new Thickness(5, 5, 5, 5)
            };
            
            AvatarImage.SetBinding(Image.SourceProperty, new Binding("AvatarUrl"));
            UsuarioLabel.SetBinding(Label.TextProperty, new Binding("NomeUsuario"));
            EmailLabel.SetBinding(Label.TextProperty, new Binding("Email"));

            return new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 0),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = new LayoutOptions { Alignment = LayoutAlignment.Center },
                Margin = 0,
                Children = {
                    TituloLabel,
                    AvatarImage,
                    UsuarioLabel,
                    EmailLabel,
                    EditarButton
                }
            };
        }
    }
}
