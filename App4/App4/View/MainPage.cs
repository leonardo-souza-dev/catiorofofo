using App4.Model;
using App4.ViewModel;
using Xamarin.Forms;

namespace App4.View
{
    public class MainPage : TabbedPage
    {
        /// <summary>
        /// Construtor que recebe o id do usuário logado
        /// </summary>
        /// <param name="usuarioId">Id do usuário logado</param>
        public MainPage(UsuarioModel usuario, ConfiguracaoApp config)
        {
            var postViewModel = new PostViewModel(usuario, config);

            //layout via código CSharp
            //Children.Add(new ExpViewCS(postViewModel)); 

            //layout via XAML
            Children.Add(new Page1(postViewModel)); 

            var uploadView = new UploadViewCS(postViewModel, this);
            uploadView.Title = "upload";
            Children.Add(uploadView);

            Children.Add(new PerfilViewCS(postViewModel, config));
        }
    }
}
