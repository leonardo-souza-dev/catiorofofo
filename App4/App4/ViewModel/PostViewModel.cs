using App4.Model;
using App4.Repositoy;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace App4.ViewModel
{
    public class PostViewModel
    {
        public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();

        public PostViewModel()
        {
            CarregarPosts();
        }

        public async void CarregarPosts()
        {
            var lstModulos = new List<Post>();

            lstModulos = await PostRepository.ObterPostsNuvem();
            for (int index = 0; index < lstModulos.Count; index++)
            {
                var item = lstModulos[index];
                if (index + 1 > Posts.Count || Posts[index].Equals(item))
                    Posts.Insert(index, item);
            }
        }

        public void InserirPost(Post post)
        {
            int[] indicesARetirar = new int[Posts.Count];
            for (int i = 0; i < Posts.Count; i++)
            {
                indicesARetirar[i] = i;
            }

            Posts.Insert(Posts.Count, post);

            var max = Posts.Count - 1;
            for (int i = 0; i < max; i++)
            {
                Posts.Insert(Posts.Count, Posts[i]);
            }

            for (int i = indicesARetirar.Count() - 1; i >= 0; i--)
            {
                Posts.Remove(Posts[indicesARetirar[i]]);
            }
        }
    }
}
