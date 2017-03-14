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
        private PostViewModel PostViewModel;
        public ObservableCollection<PostModel> posts { get; set; }

        Button AcharButton = new Button();
        Image FotoImage;
        Button PostarButton = new Button();
        Entry LegendaEntry = new Entry();
        Stream Stream = null;
        MediaFile File;
        public string Arquivo = string.Empty;

        TabbedPage MainPage;

        public UploadViewCS(PostViewModel postViewModel, TabbedPage mainPage)
        {
            MainPage = mainPage;
            PostViewModel = postViewModel;
            this.Title = "enviar catioro fofo";
            CrossMedia.Current.Initialize();
            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo()
        {
            AcharButton = new Button
            {
                Text = "achar catioro fofo",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            PostarButton = new Button
            {
                Text = "postar!",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            LegendaEntry = new Entry
            {
                Placeholder = "entre com uma legenda",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            FotoImage = new Image { WidthRequest = 100, HeightRequest = 100 };

            PostarButton.IsEnabled = false;
            PostarButton.IsVisible = false;

            AcharButton.Clicked += async (sender, args) =>
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
            };

            PostarButton.Clicked += async (sender, args) =>
            {
                PostModel post = new PostModel(Stream)
                {
                    Legenda = LegendaEntry.Text,
                    UsuarioId = PostViewModel.Usuario.UsuarioId
                };
                var postFinal = new PostModel();
                postFinal = await PostRepository.SalvarPost(post);
                PostViewModel.InserirPost(postFinal);
                var expViewCode = new ExpViewCS(PostViewModel);
                MainPage.CurrentPage = MainPage.Children[0];

                return;
            };

            return new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 0),
                Orientation = StackOrientation.Vertical,
                Children = {
                    AcharButton,
                    PostarButton,
                    FotoImage,
                    LegendaEntry
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
