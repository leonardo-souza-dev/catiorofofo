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
            Posts.Insert(Posts.Count, post);
        }
    }
}
