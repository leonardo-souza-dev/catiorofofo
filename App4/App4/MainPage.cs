using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Children.Add(new ExpViewCode());
            Children.Add(new UploadViewCode());
        }
    }
}
