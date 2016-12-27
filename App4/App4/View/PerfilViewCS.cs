using App4.Model;
using App4.Model.Resposta;
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
        MediaFile File;
        Stream Stream = null;

        string NomeUsuarioValorInicial;

        public PerfilViewCS(PostViewModel postViewModel, ConfiguracaoApp config)
        {
            UsuarioViewModel = new UsuarioViewModel(config);

            PostViewModel = postViewModel;

            Title = "perfil";

            CrossMedia.Current.Initialize();

            Usuario = postViewModel.Usuario;

            BindingContext = Usuario;                

            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo(){

            bool modoEdicao = false;
            var TituloLabel = new Label
            {
                Text = "perfil",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            var AvatarImage = new Image
            {
                WidthRequest = 30,
                HeightRequest = 30,
                IsEnabled = false
            };
            var NomeUsuarioEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = false
            };
            var EmailEntry = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = false
            };
            var CancelarButton = new Button
            {
                FontSize = 10,
                Text = "cancelar",
                IsVisible = false

            };
            var SalvarButton = new Button
            {
                FontSize = 10,
                Text = "salva",
                IsVisible = false
            };
            var EditarButton = new Button
            {
                FontSize = 10,
                Text = "editar"
            };

            AvatarImage.SetBinding(Image.SourceProperty, new Binding("AvatarUrl"));
            NomeUsuarioEntry.SetBinding(Entry.TextProperty, new Binding("NomeUsuario"));
            EmailEntry.SetBinding(Entry.TextProperty, new Binding("Email"));

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
                // handle the tap
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

                    AvatarImage.Source = ImageSource.FromStream(ObterStream);
                }
            };
            AvatarImage.GestureRecognizers.Add(tapGestureRecognizer);
            //AvatarImage.Focused += AvatarImage_Focused;

            CancelarButton.Clicked += (object sender, EventArgs e) =>
            {
                NomeUsuarioEntry.IsEnabled = false;
                EmailEntry.IsEnabled = false;
                CancelarButton.IsVisible = false;
                SalvarButton.IsVisible = false;
                EditarButton.IsVisible = true;

                NomeUsuarioEntry.Text = NomeUsuarioValorInicial;
                modoEdicao = false;
            };
            SalvarButton.Clicked += async (object sender, EventArgs e) =>
            {
                
                NomeUsuarioEntry.IsEnabled = false;
                EmailEntry.IsEnabled = false;
                CancelarButton.IsVisible = false;
                SalvarButton.IsVisible = false;
                EditarButton.IsVisible = true;

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
                        NomeUsuarioEntry.Focus();
                        break;
                }
                modoEdicao = false;
                return;
            };
            EditarButton.Clicked += (object sender, EventArgs e) =>
            {
                modoEdicao = true;
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

        private Stream ObterStream()
        {
            Stream stream = File.GetStream();
            Stream = File.GetStream();
            File.Dispose();

            return stream;
        }

    }
}
