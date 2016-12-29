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
        public ObservableCollection<PostModel> Posts { get; set; } = new ObservableCollection<PostModel>();
        public UsuarioModel Usuario;

        /// <summary>
        /// Construtor que recebe o id do usuário logado ou recém-cadastrado
        /// </summary>
        /// <param name="usuarioId">Id do usuário logado ou recém-cadastrado</param>
        public PostViewModel(UsuarioModel usuario, ConfiguracaoApp config)
        {
            PostRepository.SetarConfiguracao(config);
            Usuario = usuario;
            ObterPosts();
        }

        public async void ObterPosts()
        {
            var listaPosts = new List<PostModel>();

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
                    item.NomeArquivoAvatar = item.Usuario.NomeArquivoAvatar;
                    Posts.Insert(index, item);
                }
            }
        }


        public async Task<RespostaStatus> Curtir(PostModel post)
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


        public async Task<RespostaStatus> Descurtir(PostModel post)
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


        private void AdicionaCurtidaNoPost(PostModel post, int posicao, bool curtidaHabilitada)
        {
            if (curtidaHabilitada)
            {
                post.Curtidas.Remove(post.Curtidas.FirstOrDefault(x => x.UsuarioId == Usuario.UsuarioId));
            }
            else
            {
                post.Curtidas.Add(new CurtidaModel { UsuarioId = Usuario.UsuarioId, PostId = post.PostId });
            }
            
            post.CurtidaHabilitada = curtidaHabilitada;

            Posts.RemoveAt(posicao);
            Posts.Insert(posicao, post);
        }


        private int ObterPosicao(PostModel post)
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

        public void InserirPost(PostModel post)
        {
            Posts.Insert(0, post);
        }
    }
}
