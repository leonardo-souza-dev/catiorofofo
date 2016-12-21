using App4.Model;
using App4.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace App4.ViewModel
{
    public class PostViewModel
    {
        public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();
        public int UsuarioId;

        /// <summary>
        /// Construtor que recebe o id do usuário logado ou recém-cadastrado
        /// </summary>
        /// <param name="usuarioId">Id do usuário logado ou recém-cadastrado</param>
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

        public async Task<RespostaStatus> Curtir(Post post)
        {
            var resposta = await PostRepository.Curtir(UsuarioId, post.PostId);

            if (resposta.mensagem == "SUCESSO")
            {
                return RespostaStatus.Sucesso;
            }

            return RespostaStatus.ErroGenerico;
        }

        public void InserirPost(Post post)
        {
            Posts.Insert(0, post);
        }
    }
}
