using App4.ViewModel;
using Xamarin.Forms;

namespace App4.View
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            var postViewModel = new PostViewModel();
            Children.Add(new ExpViewCode(postViewModel));

            var uploadView = new NavigationPage(new UploadViewCode(postViewModel));
            uploadView.Title = "upload";
            Children.Add(uploadView);
        }
    }
}
