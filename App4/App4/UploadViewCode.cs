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

        public UploadViewCode()
        {
            this.Title = "enviar catioro fofo";

            Content = ObterConteudo();
        }

        private View ObterConteudo()
        {
            Label label = new Label
            {
                Text = "upload do catioro",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            return label;
        }
    }
}
