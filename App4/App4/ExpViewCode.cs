using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4
{
    public partial class ExpViewCode : ContentPage
    {
        public ObservableCollection<PostViewModel> posts { get; set; }

        public ExpViewCode()
        {
            this.Title = "Catioro Fofo";

            Content = ObterConteudo();
        }

        private StackLayout ObterConteudo()
        {
            posts = new ObservableCollection<PostViewModel>();

            ListView lstView = new ListView { HasUnevenRows = true };
            lstView.ItemTemplate = new DataTemplate(typeof(CustomPostCell));

            posts.Add(new PostViewModel()
            {
                FotoUrl = "http://lorempixel.com/250/250/",
                Descricao = "dogs forever",
                AvatarUrl = "http://lorempixel.com/40/40/",
                Largura = 150,
                Altura = 150
            });
            posts.Add(new PostViewModel()
            {
                FotoUrl = "http://lorempixel.com/250/200/",
                Descricao = "cachorro passeando!",
                AvatarUrl = "http://lorempixel.com/40/40/",
                Largura = 150,
                Altura = 150
            });

            lstView.ItemsSource = posts;

            return new StackLayout
            {
                Padding = new Thickness(0, 20, 0, 0),
                Children = {
                    new Label {
                        Text = "Explorar catioros fofos",
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    lstView
                }
            }; ;
        }

        public class CustomPostCell : ViewCell
        {
            public CustomPostCell()
            {
                var foto = new Image { Margin = new Thickness(5, 15, 5, 5) };
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
                descricao.SetBinding(Label.TextProperty, new Binding("Descricao"));

                View = principalLayout;
            }
        }
    }
}
