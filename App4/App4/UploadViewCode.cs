using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4
{
    public partial class UploadViewCode : ContentPage
    {
        public ObservableCollection<PostViewModel> posts { get; set; }

        Button acharBotao = new Button();
        Image image = new Image();
        Button enviarBotao = new Button();

        public UploadViewCode()
        {
            this.Title = "enviar catioro fofo";
            //Media.Plugin.MediaImplementation.OnFilesPicked(args);
            CrossMedia.Current.Initialize();
            Content = ObterConteudo();
        }

        private View ObterConteudo()
        {
            acharBotao = new Button
            {
                Text = "achar catioro fofo",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            enviarBotao = new Button
            {
                Text = "fechou!",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            enviarBotao.IsEnabled = false;
            enviarBotao.IsVisible = false;

            acharBotao.Clicked += async (sender, args) =>
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

                //await DisplayAlert("File Location", file.Path, "OK");
                enviarBotao.IsEnabled = true;
                enviarBotao.IsVisible = true;

                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            };

            return new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 0),
                Orientation = StackOrientation.Vertical,
                Children = {
                    acharBotao,
                    enviarBotao,
                    image
                }
            };
        }
    }
}
