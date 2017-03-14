using App4.Model;
using App4.Model.Resposta;
using App4.ViewModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilView : ContentPage
    {
        private PostViewModel PostViewModel;
        private UsuarioViewModel UsuarioViewModel;
        private UsuarioModel Usuario;
        MediaFile File;
        Stream Stream = null;
        bool EditouAvatar;
        bool modoEdicao = false;
        string NomeUsuarioValorInicial;

        public PerfilView(PostViewModel postViewModel, ConfiguracaoApp c)
        {
            InitializeComponent();

            UsuarioViewModel = new UsuarioViewModel();

            PostViewModel = postViewModel;

            CrossMedia.Current.Initialize();

            Usuario = postViewModel.Usuario;
            Usuario.NomeUsuarioEntry = false;
            BindingContext = Usuario;

            //avatarImage.GestureRecognizers.Add(tapGestureRecognizer);
        }

        protected async void OnAvatarImageTapped(object sender, EventArgs e)
        {
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
        }

        private Stream ObterStream()
        {
            Stream stream = File.GetStream();
            Stream = File.GetStream();
            File.Dispose();

            return stream;
        }



        protected void EditarClicked(object sender, EventArgs e)
        {
            modoEdicao = true;
            NomeUsuarioValorInicial = nomeUsuarioEntry.Text;

            Usuario.NomeUsuarioEntry = true;
            emailEntry.IsEnabled = true;
            nomeUsuarioEntry.IsEnabled = true;

            cancelarButton.IsVisible = true;
            cancelarButton.IsEnabled = true;

            salvarButton.IsVisible = true;
            salvarButton.IsEnabled = true;

            editarButton.IsVisible = false;
            editarButton.IsEnabled = false;
        }

        protected void CancelarClicked(object sender, EventArgs e)
        {
            nomeUsuarioEntry.IsEnabled = false;
            emailEntry.IsEnabled = false;

            cancelarButton.IsVisible = false;
            cancelarButton.IsEnabled = false;

            salvarButton.IsVisible = false;
            salvarButton.IsEnabled = false;

            editarButton.IsVisible = true;
            editarButton.IsEnabled = true;

            nomeUsuarioEntry.Text = NomeUsuarioValorInicial;
            modoEdicao = false;
        }

        protected async void SalvarClicked(object sender, EventArgs e)
        {

            nomeUsuarioEntry.IsEnabled = false;
            emailEntry.IsEnabled = false;

            cancelarButton.IsVisible = false;
            cancelarButton.IsEnabled = false;

            salvarButton.IsVisible = false;
            salvarButton.IsEnabled = false;

            editarButton.IsVisible = true;
            editarButton.IsEnabled = true;

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
        }
    }
}
