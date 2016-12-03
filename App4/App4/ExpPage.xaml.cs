using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App4
{
    public partial class ExpPage : ContentPage
    {
        public ObservableCollection<PostViewModel> posts { get; set; }

        public ExpPage()
        {
            InitializeComponent();

            posts = new ObservableCollection<PostViewModel>();
            ListView lstView = new ListView();
            lstView.RowHeight = 60;
            this.Title = "code sample";
            lstView.ItemTemplate = new DataTemplate(typeof(CustomPostCell));
            posts.Add(new PostViewModel() { Descricao = "cao" });
            posts.Add(new PostViewModel() { Descricao = "dog" });
            lstView.ItemsSource = posts;

            Content = lstView;
        }

        public class CustomPostCell : ViewCell
        {
            public CustomPostCell()
            {
                var descricaoLabel = new Label();
                var verticaLayout = new StackLayout();
                var horizontalLayout = new StackLayout() { BackgroundColor = Color.Olive };


                //set bindings
                descricaoLabel.SetBinding(Label.TextProperty, new Binding("Descricao"));

                //set properties for desired design
                horizontalLayout.Orientation = StackOrientation.Horizontal;
                horizontalLayout.HorizontalOptions = LayoutOptions.Fill;
                descricaoLabel.FontSize = 24;

                //add views to the view hierarchy
                verticaLayout.Children.Add(descricaoLabel);
                horizontalLayout.Children.Add(verticaLayout);

                // add to parent view
                View = horizontalLayout;
            }
        }
    }
}
