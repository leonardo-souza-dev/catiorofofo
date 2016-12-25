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
        public ObservableCollection<Post> posts { get; set; }

        Image AvatarImage;
        Label UsuarioLabel;
        Label EmailLabel;

        Button AcharButton = new Button();
        Button PostarButton = new Button();
        Entry LegendaEntry = new Entry();
        Stream Stream = null;
        MediaFile File;
        public string Arquivo = string.Empty;

        Button CriarPerfil = new Button();
        TabbedPage MainPage;

        public PerfilViewCS(PostViewModel postViewModel)
        {
            PostViewModel = postViewModel;

            this.Title = "perfil";

            CrossMedia.Current.Initialize();

            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo()
        {
            AvatarImage = new Image
            {
                WidthRequest = 30,
                HeightRequest = 30
            };
            UsuarioLabel = new Label
            {
                FontSize = 10,
                Margin = new Thickness(5, 5, 5, 5)
            };
            EmailLabel = new Label
            {
                FontSize = 10,
                Margin = new Thickness(5, 5, 5, 5)
            };

            //AcharButton = new Button
            //{
            //    Text = "achar catioro fofo",
            //    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            //    VerticalOptions = LayoutOptions.CenterAndExpand
            //};
            //PostarButton = new Button
            //{
            //    Text = "postar!",
            //    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            //    VerticalOptions = LayoutOptions.CenterAndExpand
            //};
            //LegendaEntry = new Entry
            //{
            //    Placeholder = "entre com uma legenda",
            //    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            //    VerticalOptions = LayoutOptions.CenterAndExpand
            //};

            //PostarButton.IsEnabled = false;
            //PostarButton.IsVisible = false;

            //AcharButton.Clicked += async (sender, args) =>
            //{
            //    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
            //    {
            //        await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
            //        return;
            //    }

            //    File = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
            //    {
            //        CompressionQuality = 50
            //    });

            //    if (File == null)
            //        return;

            //    PostarButton.IsEnabled = true;
            //    PostarButton.IsVisible = true;
            //    AcharButton.Text = "trocar catioro fofo";

            //    FotoImage.Source = ImageSource.FromStream(ObterStream);
            //};

            //PostarButton.Clicked += async (sender, args) =>
            //{
            //    Post post = new Post(Stream)
            //    {
            //        Legenda = LegendaEntry.Text,
            //        UsuarioId = PostViewModel.UsuarioId
            //    };
            //    var postFinal = new Post();
            //    postFinal = await PostRepository.SalvarPost(post);
            //    PostViewModel.InserirPost(postFinal);
            //    var expViewCode = new ExpViewCS(PostViewModel);
            //    MainPage.CurrentPage = MainPage.Children[0];

            //    return;
            //};

            return new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 0),
                Orientation = StackOrientation.Vertical,
                Children = {
                    AvatarImage,
                    UsuarioLabel,
                    EmailLabel
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
