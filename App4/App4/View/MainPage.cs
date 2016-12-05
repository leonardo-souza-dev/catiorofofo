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
            //var expView = new NavigationPage(new ExpViewCode());
            //expView.Title = "exp";

            var upView = new NavigationPage(new UploadViewCode());
            upView.Title = "up";

            Children.Add(new ExpViewCode());
            Children.Add(upView);
        }
    }
}
