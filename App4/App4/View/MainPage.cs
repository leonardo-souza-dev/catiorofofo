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
            var upView = new NavigationPage(new UploadViewCode());
            upView.Title = "upload";

            Children.Add(new ExpViewCode());
            Children.Add(upView);
        }
    }
}
