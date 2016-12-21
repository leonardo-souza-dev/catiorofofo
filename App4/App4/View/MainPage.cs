using App4.ViewModel;
using Xamarin.Forms;

namespace App4.View
{
    public class MainPage : TabbedPage
    {
        /// <summary>
        /// Construtor que recebe o id do usuário logado ou recém-cadastrado
        /// </summary>
        /// <param name="usuarioId">Id do usuário logado ou recém-cadastrado</param>
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
