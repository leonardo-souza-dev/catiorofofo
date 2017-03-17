using App4.Model;
using App4.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using App4.Model.Resposta;
using System.IO;

namespace App4.ViewModel
{
    public class PostViewModel
    {
        public ObservableCollection<PostModel> Posts { get; set; } = new ObservableCollection<PostModel>();

        public async void CarregarPosts()
        {
            var listaPosts = new List<PostModel>();

            listaPosts = await PostRepository.ObterPosts();

            for (int index = 0; index < listaPosts.Count; index++)
            {
                var post = listaPosts[index];

                if (App.UsuarioVM.Usuario.UsuarioId == post.Usuario.UsuarioId)
                {
                    post.Usuario = App.UsuarioVM.Usuario;
                }

                if (index + 1 > Posts.Count || Posts[index].Equals(post))
                {
                    foreach (var curtida in post.Curtidas)
                    {
                        if (curtida.UsuarioId == App.UsuarioVM.Usuario.UsuarioId)
                        {
                            post.CurtidaHabilitada = false;
                        }
                    }
                    post.NomeArquivoAvatar = post.Usuario.NomeArquivoAvatar;
                    Posts.Insert(index, post);
                }
            }
        }


        public async Task<RespostaStatus> Curtir(PostModel post)
        {
            var resposta = await PostRepository.Curtir(App.UsuarioVM.Usuario.UsuarioId, post.PostId);

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
            var resposta = await PostRepository.Descurtir(App.UsuarioVM.Usuario.UsuarioId, post.PostId);

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
                post.Curtidas.Remove(post.Curtidas.FirstOrDefault(x => x.UsuarioId == App.UsuarioVM.Usuario.UsuarioId));
            }
            else
            {
                post.Curtidas.Add(new CurtidaModel { UsuarioId = App.UsuarioVM.Usuario.UsuarioId, PostId = post.PostId });
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

        public async Task<RespostaStatus> Salvar(Stream stream, string legenda, UsuarioModel usuario)
        {
            try
            {
                PostModel post = new PostModel(stream)
                {
                    Legenda = legenda,
                    Usuario = usuario
                };

                PostModel postSalvo = await PostRepository.SalvarPost(post);
                InserirPost(postSalvo);

                return RespostaStatus.Sucesso;
            }
            catch (System.Exception)
            {
                return RespostaStatus.ErroGenerico;
            }
        }
    }
}
