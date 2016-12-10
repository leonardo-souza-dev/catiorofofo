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
    public partial class ExpViewCode : ContentPage
    {
        private PostViewModel _postsVM;

        public ExpViewCode(PostViewModel postsVM)
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
                        Text = "Explorar catioros fofos",
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
                var foto = new Image { Margin = new Thickness(5, 15, 5, 5), VerticalOptions = LayoutOptions.CenterAndExpand };
                var avatar = new Image { Margin = new Thickness(5, 5, 5, 5) };
                var descricao = new Label { FontSize = 14 };
                var curtir = new Label { FontSize = 10, Text = "Curtir", Margin = new Thickness(5, 5, 5, 5) };
                var numCurtidas = new Label { FontSize = 10, Text = "22", Margin = new Thickness(5, 5, 5, 5) };

                var principalLayout = new StackLayout()
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = new LayoutOptions { Alignment = LayoutAlignment.Center },
                    //BackgroundColor = Color.Olive,
                    Margin = 0,
                    Children =
                        {
                            foto,
                            new StackLayout()
                            {
                                Padding = new Thickness(0, 0, 0, 0),
                                Orientation = StackOrientation.Horizontal,
                                //BackgroundColor = Color.Red,
                                Margin = 0,
                                Children = { avatar, descricao }
                            },
                            new StackLayout()
                            {
                                Padding = new Thickness(0, 0, 0, 0),
                                Orientation = StackOrientation.Horizontal,
                                //BackgroundColor = Color.Blue,
                                Margin = 0,
                                Children = { curtir, numCurtidas }
                            }
                        }
                };

                foto.SetBinding(Image.SourceProperty, new Binding("FotoUrl"));
                avatar.SetBinding(Image.SourceProperty, new Binding("AvatarUrl"));
                descricao.SetBinding(Label.TextProperty, new Binding("Legenda"));

                View = principalLayout;
            }
        }
    }
}
