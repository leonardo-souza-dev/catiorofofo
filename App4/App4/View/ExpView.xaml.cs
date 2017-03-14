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
        private PostViewModel _postViewModel;

        public Page1(PostViewModel postVM)
        {
            InitializeComponent();
            _postViewModel = postVM;
            
            BindingContext = postVM;
        }

        protected async void CurtirButtonClicked(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            PostModel post = (PostModel)botao.BindingContext;

            if (post.CurtidaHabilitada)
            {
                var resultado = _postViewModel.Curtir(post);
            }
            else
            {
                var resultado = _postViewModel.Descurtir(post);
            }

            return;
        }

    }
}
