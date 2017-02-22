using App4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App4.View
{
    public partial class Page1 : ContentPage
    {
        public Page1(PostViewModel postVM)
        {
            InitializeComponent();
            BindingContext = postVM;
        }
    }
}
