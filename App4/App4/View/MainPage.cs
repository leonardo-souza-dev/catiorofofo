using App4.Model;
using App4.ViewModel;
using Xamarin.Forms;

namespace App4.View
{
    public class MainPage : TabbedPage
    {
        public MainPage(UsuarioModel usuario)
        {
            var postViewModel = new PostViewModel(usuario);
            Children.Add(new ExpViewCS(postViewModel));
            
            var uploadView = new UploadViewCS(postViewModel, this);
            uploadView.Title = "upload";
            Children.Add(uploadView);

            Children.Add(new PerfilViewCS(postViewModel));
        }
    }
}
