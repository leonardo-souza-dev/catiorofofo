using App4.Model;
using App4.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App4.ViewModel
{
    public class PostViewModel
    {
        public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();
        public int UsuarioId;

        public PostViewModel(int usuarioId)
        {
            UsuarioId = usuarioId;
            CarregarPosts(usuarioId);
        }

        public async void CarregarPosts(int usuarioId)
        {
            var listaPosts = new List<Post>();

            listaPosts = await PostRepository.ObterPosts(usuarioId);
            for (int index = 0; index < listaPosts.Count; index++)
            {
                var item = listaPosts[index];
                if (index + 1 > Posts.Count || Posts[index].Equals(item))
                {
                    Posts.Insert(index, item);
                }
            }
        }

        /// <summary>
        /// Insere o post recém-uploadado em primeiro na página de explorar
        /// </summary>
        /// <param name="post">Post recém uploadado</param>
        public void InserirPost(Post post)
        {
            Posts.Insert(0, post);
        }
    }
}
