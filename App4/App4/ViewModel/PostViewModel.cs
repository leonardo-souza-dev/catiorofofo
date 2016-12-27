using App4.Model;
using App4.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using App4.Model.Resposta;

namespace App4.ViewModel
{
    public class PostViewModel
    {
        public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();
        //public int UsuarioId;
        public Usuario Usuario;

        /// <summary>
        /// Construtor que recebe o id do usuário logado ou recém-cadastrado
        /// </summary>
        /// <param name="usuarioId">Id do usuário logado ou recém-cadastrado</param>
        public PostViewModel(Usuario usuario, ConfiguracaoApp config)
        {
            PostRepository.SetarConfiguracao(config);
            Usuario = usuario;
            ObterPosts();
        }

        public async void ObterPosts()
        {
            var listaPosts = new List<Post>();

            listaPosts = await PostRepository.ObterPosts();
            for (int index = 0; index < listaPosts.Count; index++)
            {
                var item = listaPosts[index];
                if (index + 1 > Posts.Count || Posts[index].Equals(item))
                {
                    foreach (var curtida in item.Curtidas)
                    {
                        if (curtida.UsuarioId == Usuario.UsuarioId)
                        {
                            item.CurtidaHabilitada = false;
                        }
                    }
                    Posts.Insert(index, item);
                }
            }
        }


        public async Task<RespostaStatus> Curtir(Post post)
        {
            var resposta = await PostRepository.Curtir(Usuario.UsuarioId, post.PostId);

            int posicao = ObterPosicao(post);

            AdicionaCurtidaNoPost(post, posicao, false);

            if (resposta.mensagem == "SUCESSO")
            {
                return RespostaStatus.Sucesso;
            }

            return RespostaStatus.ErroGenerico;
        }


        public async Task<RespostaStatus> Descurtir(Post post)
        {
            var resposta = await PostRepository.Descurtir(Usuario.UsuarioId, post.PostId);

            int posicao = ObterPosicao(post);

            AdicionaCurtidaNoPost(post, posicao, true);

            if (resposta.mensagem == "SUCESSO")
            {
                return RespostaStatus.Sucesso;
            }

            return RespostaStatus.ErroGenerico;
        }


        private void AdicionaCurtidaNoPost(Post post, int posicao, bool curtidaHabilitada)
        {
            if (curtidaHabilitada)
            {
                post.Curtidas.Remove(post.Curtidas.FirstOrDefault(x => x.UsuarioId == Usuario.UsuarioId));
            }
            else
            {
                post.Curtidas.Add(new Curtida { UsuarioId = Usuario.UsuarioId, PostId = post.PostId });
            }
            
            post.CurtidaHabilitada = curtidaHabilitada;

            Posts.RemoveAt(posicao);
            Posts.Insert(posicao, post);
        }


        private int ObterPosicao(Post post)
        {
            int posicao = -1;
            for (int i = 0; i < Posts.Count; i++)
            {
                if (Posts[i].PostId == post.PostId)
                {
                    posicao = i;
                    break;
                }
            }

            return posicao;
        }

        public void InserirPost(Post post)
        {
            Posts.Insert(0, post);
        }
    }
}
