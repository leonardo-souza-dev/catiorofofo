using App4.Model;
using App4.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4.View
{
    public partial class ExpViewCS : ContentPage
    {
        private PostViewModel _postsVM;

        public ExpViewCS(PostViewModel postsVM)
        {
            this.Title = "catioro fofo";

            _postsVM = postsVM;

            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo()
        {
            ListView postsListView = new ListView { HasUnevenRows = true };
            postsListView.ItemTemplate = new DataTemplate(typeof(CustomPostCell));

            postsListView.ItemsSource = _postsVM.Posts;

            return new StackLayout
            {
                Padding = new Thickness(0, 20, 0, 0),
                Children = {
                    new Label {
                        Text = "explorar catioros fofos",
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    postsListView
                }
            };
        }

        public class AspectRatioContainer : ContentView
        {
            protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
            {
                return new SizeRequest(new Size(widthConstraint, widthConstraint * this.AspectRatio));
            }

            public double AspectRatio { get; set; }
        }

        public class CustomPostCell : ViewCell
        {
            public CustomPostCell()
            {
                var fotoImage = new Image { Margin = new Thickness(5, 15, 5, 5), VerticalOptions = LayoutOptions.CenterAndExpand };
                var avatarImage = new Image { Margin = new Thickness(5, 5, 5, 5) };
                var descricaoLabel = new Label { FontSize = 14 };
                var curtirButton = new Button { FontSize = 10, Text = "Curtir", Margin = new Thickness(5, 5, 5, 5) };
                var numCurtidasLabel = new Label { FontSize = 10, Text = "22", Margin = new Thickness(5, 5, 5, 5) };

                var principalLayout = new StackLayout()
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = new LayoutOptions { Alignment = LayoutAlignment.Center },
                    Margin = 0,
                    Children =
                        {
                            fotoImage,
                            new StackLayout()
                            {
                                Padding = new Thickness(0, 0, 0, 0),
                                Orientation = StackOrientation.Horizontal,
                                Margin = 0,
                                Children = { avatarImage, descricaoLabel }
                            },
                            new StackLayout()
                            {
                                Padding = new Thickness(0, 0, 0, 0),
                                Orientation = StackOrientation.Horizontal,
                                Margin = 0,
                                Children = { curtirButton, numCurtidasLabel }
                            }
                        }
                };

                fotoImage.SetBinding(Image.SourceProperty, new Binding("FotoUrl"));
                avatarImage.SetBinding(Image.SourceProperty, new Binding("AvatarUrl"));
                descricaoLabel.SetBinding(Label.TextProperty, new Binding("Legenda"));

                View = principalLayout;
            }
        }
    }
}
