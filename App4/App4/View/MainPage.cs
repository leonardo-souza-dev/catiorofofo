using App4.ViewModel;
using Xamarin.Forms;

namespace App4.View
{
    public class MainPage : TabbedPage
    {
        public MainPage(int usuarioId)
        {
            var postViewModel = new PostViewModel(usuarioId);
            Children.Add(new ExpViewCS(postViewModel));
            
            var uploadView = new UploadViewCS(postViewModel, this);
            uploadView.Title = "upload";
            Children.Add(uploadView);
        }
    }
}
