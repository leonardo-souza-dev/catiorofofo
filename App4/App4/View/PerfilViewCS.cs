using App4.Model;
using App4.Model.Resposta;
using App4.ViewModel;

using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;

using Xamarin.Forms;

namespace App4.View
{
    public partial class PerfilViewCS : ContentPage
    {
        private PostViewModel PostViewModel;
        private UsuarioViewModel UsuarioViewModel;
        private UsuarioModel Usuario;
        MediaFile File;
        Stream Stream = null;
        bool EditouAvatar;

        string NomeUsuarioValorInicial;

        public PerfilViewCS(PostViewModel postViewModel, ConfiguracaoApp config)
        {
            UsuarioViewModel = new UsuarioViewModel();

            PostViewModel = postViewModel;

            Title = "perfil";

            CrossMedia.Current.Initialize();

            Usuario = postViewModel.Usuario;

            BindingContext = Usuario;                

            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo(){

            bool modoEdicao = false;
            var tituloLabel = new Label
            {
                Text = "perfil",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            var avatarImage = new Image
            {
                WidthRequest = 30,
                HeightRequest = 30,
                IsEnabled = false
            };
            var nomeUsuarioLabel = new Label
            {
                Text = "nome do catioro",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = false
            };
            var nomeUsuarioEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Entry)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = false
            };
            var emailLabel = new Label
            {
                Text = "email",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = false
            };
            var emailEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Entry)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = false
            };
            var cancelarButton = new Button
            {
                FontSize = 10,
                Text = "cancelar",
                IsVisible = false
            };
            var salvarButton = new Button
            {
                FontSize = 10,
                Text = "salva",
                IsVisible = false
            };
            var editarButton = new Button
            {
                FontSize = 10,
                Text = "editar"
            };

            avatarImage.SetBinding(Image.SourceProperty, new Binding("AvatarUrl"));
            nomeUsuarioEntry.SetBinding(Entry.TextProperty, new Binding("NomeUsuario"));
            emailEntry.SetBinding(Entry.TextProperty, new Binding("Email"));

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
                
                if (modoEdicao)
                {
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                        return;
                    }

                    File = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                    {
                        CompressionQuality = 50
                    });

                    if (File == null)
                        return;

                    EditouAvatar = true;
                    avatarImage.Source = ImageSource.FromStream(ObterStream);
                }
            };
            avatarImage.GestureRecognizers.Add(tapGestureRecognizer);

            cancelarButton.Clicked += (object sender, EventArgs e) =>
            {
                nomeUsuarioEntry.IsEnabled = false;
                emailEntry.IsEnabled = false;
                cancelarButton.IsVisible = false;
                salvarButton.IsVisible = false;
                editarButton.IsVisible = true;

                nomeUsuarioEntry.Text = NomeUsuarioValorInicial;
                modoEdicao = false;
            };
            salvarButton.Clicked += async (object sender, EventArgs e) =>
            {
                nomeUsuarioEntry.IsEnabled = false;
                emailEntry.IsEnabled = false;
                cancelarButton.IsVisible = false;
                salvarButton.IsVisible = false;
                editarButton.IsVisible = true;

                if (EditouAvatar)
                {
                    Usuario.SetarAvatarStream(Stream);
                }

                var resultado = await UsuarioViewModel.AtualizarCadastro(Usuario);
                switch (resultado)
                {
                    case RespostaStatus.Sucesso:
                        break;
                    case RespostaStatus.Inexistente:
                        await DisplayAlert("ops", "erro estranho", "volta lá");
                        break;
                    case RespostaStatus.JaExiste:
                        await DisplayAlert("ops", "ja existe nome de usuario", "volta lá");
                        nomeUsuarioEntry.Focus();
                        break;
                }
                modoEdicao = false;
                return;
            };
            editarButton.Clicked += (object sender, EventArgs e) =>
            {
                modoEdicao = true;
                NomeUsuarioValorInicial = nomeUsuarioEntry.Text;

                nomeUsuarioEntry.IsEnabled = true;
                emailEntry.IsEnabled = true;
                cancelarButton.IsVisible = true;
                salvarButton.IsVisible = true;
                editarButton.IsVisible = false;
            };

            return new StackLayout
            {
                Padding = new Thickness(0, 10, 0, 0),
                Children = {
                    tituloLabel,
                    avatarImage,
                    nomeUsuarioLabel,
                    nomeUsuarioEntry,
                    emailLabel,
                    emailEntry,
                    cancelarButton,
                    salvarButton,
                    editarButton
                }
            };
        }

        private Stream ObterStream()
        {
            Stream stream = File.GetStream();
            Stream = File.GetStream();
            File.Dispose();

            return stream;
        }

    }
}
