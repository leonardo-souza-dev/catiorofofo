using App4.Model;
using App4.Repositoy;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4.View
{
    public partial class UploadViewCode : ContentPage
    {
        public ObservableCollection<Post> posts { get; set; }

        Button acharButton = new Button();
        Image fotoImage = new Image();
        Button postarButton = new Button();
        Entry legendaEntry = new Entry();

        public string caminho = string.Empty;

        public UploadViewCode()
        {
            this.Title = "enviar catioro fofo";
            CrossMedia.Current.Initialize();
            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo()
        {
            acharButton = new Button
            {
                Text = "achar catioro fofo",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            postarButton = new Button
            {
                Text = "postar!",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            legendaEntry = new Entry
            {
                Placeholder = "entre com uma legenda",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            postarButton.IsEnabled = false;
            postarButton.IsVisible = false;

            acharButton.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions()
                {
                    CompressionQuality = 50
                });

                if (file == null)
                    return;

                postarButton.IsEnabled = true;
                postarButton.IsVisible = true;
                acharButton.Text = "trocar catioro fofo";

                fotoImage.Source = ImageSource.FromStream(() =>
                {
                    caminho = file.AlbumPath;
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
                fotoImage.WidthRequest = 40;
                fotoImage.HeightRequest = 40;
            };

            postarButton.Clicked += (sender, args) =>
            {
                PostRepository.SalvarPost(new Post
                {
                    FotoUrl = "https://www.fillmurray.com/300/300",
                    AvatarUrl = "http://lorempixel.com/40/40/",
                    Legenda = legendaEntry.Text
                });
                //Navigation.PushAsync(new ExpViewCode(), true);
                Navigation.PushAsync(new ExpViewCode());

                return;
            };

            return new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 0),
                Orientation = StackOrientation.Vertical,
                Children = {
                    acharButton,
                    postarButton,
                    fotoImage,
                    legendaEntry
                }
            };
        }
    }
}
