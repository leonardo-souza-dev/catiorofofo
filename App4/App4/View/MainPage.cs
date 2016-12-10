using App4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4.View
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {

            PostViewModel postViewModel = new PostViewModel();
            Children.Add(new ExpViewCode(postViewModel));
            Children.Add(new NavigationPage(new UploadViewCode(postViewModel) { Title = "uploadd" }));
        }
    }
}
