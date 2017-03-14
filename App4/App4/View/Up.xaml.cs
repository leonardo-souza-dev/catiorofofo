using App4.Model;
using App4.Model.Resposta;
using App4.Repository;
using App4.ViewModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Up : ContentPage
    {
        private PostViewModel PostViewModel;
        public ObservableCollection<PostModel> posts { get; set; }

        //Image FotoImage;
        Stream Stream = null;
        MediaFile File;
        public string Arquivo = string.Empty;

        TabbedPage MainPage;

        private PostViewModel _postViewModel;

        public Up(PostViewModel postViewModel, TabbedPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
            PostViewModel = postViewModel;
            this.Title = "enviar catioro fofo";
            CrossMedia.Current.Initialize();
            //FotoImage = new Image { WidthRequest = 100, HeightRequest = 100 };

            //PostarButton.IsEnabled = false;
            //PostarButton.IsVisible = false;
            UploadViewModel uvm = new UploadViewModel();
            uvm.UploadModel = new UploadModel(){PostarButtonIsEnabled = false};
            BindingContext = uvm.UploadModel;
        }

        protected async void AcharButtonClicked(object o, EventArgs args)
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

            PostarButton.IsEnabled = true;
            PostarButton.IsVisible = true;
            AcharButton.Text = "trocar catioro fofo";

            FotoImage.Source = ImageSource.FromStream(ObterStream);
        }

        protected async void PostarButtonClicked(object o, EventArgs args)
        {
            PostModel post = new PostModel(Stream)
            {
                Legenda = LegendaEntry.Text,
                UsuarioId = PostViewModel.Usuario.UsuarioId,
                Usuario = PostViewModel.Usuario
            };
            //var postFinal = new PostModel();
            //postFinal = await PostRepository.SalvarPost(post);
            //PostViewModel.InserirPost(postFinal);
            var resposta = await PostViewModel.Salvar(post);

            if (resposta == RespostaStatus.ErroGenerico)
            {
                await DisplayAlert("erro", "ocorreu um erro","cancelar");
                return;
            }
            var expViewCode = new ExpViewCS(PostViewModel);
            MainPage.CurrentPage = MainPage.Children[0];
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
