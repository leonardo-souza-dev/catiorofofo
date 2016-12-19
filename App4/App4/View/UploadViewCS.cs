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
    public partial class UploadViewCS : ContentPage
    {
        private PostViewModel _postViewModel;
        public ObservableCollection<Post> posts { get; set; }

        Button _acharButton = new Button();
        Image _fotoImage;
        Button _postarButton = new Button();
        Entry _legendaEntry = new Entry();
        Stream _stream = null;
        MediaFile _file;
        public string _arquivo = string.Empty;

        Button _criarPerfil = new Button();
        TabbedPage _mainPage;

        public UploadViewCS(PostViewModel postViewModel, TabbedPage mainPage)
        {
            _mainPage = mainPage;
            _postViewModel = postViewModel;
            this.Title = "enviar catioro fofo";
            CrossMedia.Current.Initialize();
            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo()
        {
            _acharButton = new Button
            {
                Text = "achar catioro fofo",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            _postarButton = new Button
            {
                Text = "postar!",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            _legendaEntry = new Entry
            {
                Placeholder = "entre com uma legenda",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            _fotoImage = new Image { WidthRequest = 100, HeightRequest = 100 };

            _postarButton.IsEnabled = false;
            _postarButton.IsVisible = false;

            _acharButton.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

                _file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    CompressionQuality = 50
                });

                if (_file == null)
                    return;

                _postarButton.IsEnabled = true;
                _postarButton.IsVisible = true;
                _acharButton.Text = "trocar catioro fofo";

                _fotoImage.Source = ImageSource.FromStream(ObterStream);
            };

            _postarButton.Clicked += async (sender, args) =>
            {
                Post post = new Post(_stream)
                {
                    Legenda = _legendaEntry.Text,
                    UsuarioId = 1 // TODO: MUDAR PARA RECEBER O ID DO USUARIO LOGADO
                };
                var postFinal = new Post();
                postFinal = await PostRepository.SalvarPost(post);
                _postViewModel.InserirPost(postFinal);
                var expViewCode = new ExpViewCS(_postViewModel);
                _mainPage.CurrentPage = _mainPage.Children[0];

                return;
            };

            return new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 0),
                Orientation = StackOrientation.Vertical,
                Children = {
                    _criarPerfil,
                    _acharButton,
                    _postarButton,
                    _fotoImage,
                    _legendaEntry
                }
            };
        }

        private Stream ObterStream()
        {
            Stream stream = _file.GetStream();
            _stream = _file.GetStream();
            _file.Dispose();

            return stream;
        }
    }
}
