using App4.Model;
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
        public Page1()
        {
            InitializeComponent();
            
            BindingContext = App.PostVM;
        }

        protected async void CurtirButtonClicked(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            PostModel post = (PostModel)botao.BindingContext;

            if (post.CurtidaHabilitada)
            {
                var resultado = await App.PostVM.Curtir(post);
            }
            else
            {
                var resultado = await App.PostVM.Descurtir(post);
            }

            return;
        }

        protected override void OnAppearing()
        {
            App.PostVM.CarregarPosts();
            base.OnAppearing();
        }

    }
}
