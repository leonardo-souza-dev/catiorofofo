using App4.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System;

namespace App4.Repository
{
    public static class PostRepository
    {
        private static Guid idTemp;
        private static List<PostModel> ListaPosts;

        public static async Task<List<PostModel>> ObterPosts()
        {
            idTemp = Guid.NewGuid();

            var respostaPosts = await Resposta<List<PostModel>>(null, "obterposts");

            ListaPosts = new List<PostModel>();

            foreach (var respostaPost in respostaPosts)
            {
                List<CurtidaModel> curtidas = new List<CurtidaModel>();

                UsuarioModel usuario = new UsuarioModel();
                usuario.NomeArquivoAvatar = respostaPost.NomeArquivoAvatar;
                usuario.UsuarioId = respostaPost.UsuarioId;
                usuario.Email = respostaPost.Usuario.Email;
                usuario.NomeUsuario = respostaPost.Usuario.NomeUsuario;


                PostModel post = new PostModel()
                {
                    PostId = respostaPost.PostId,
                    Legenda = respostaPost.Legenda,
                    NomeArquivo = respostaPost.NomeArquivo,
                    Curtidas = curtidas,
                    Usuario = usuario
                };
                foreach (var c in respostaPost.Curtidas)
                {
                    curtidas.Add(new CurtidaModel { UsuarioId = c.UsuarioId, PostId = c.PostId });
                }
                ListaPosts.Add(post);
            }
            return ListaPosts;
        }

        public static async Task<RespostaCurtir> Descurtir(int usuarioIdPassado, int postIdPassado)
        {
            var resposta = await Resposta<RespostaCurtir>(new { usuarioId = usuarioIdPassado, postId = postIdPassado }, "descurtir");

            return resposta;
        }

        public static async Task<RespostaCurtir> Curtir(int usuarioIdPassado, int postIdPassado)
        {
            var resposta = await Resposta<RespostaCurtir>(new { usuarioId = usuarioIdPassado, postId = postIdPassado }, "curtir");

            return resposta;
        }

        private static async Task<T> Resposta<T>(object conteudo, string metodo, bool ehDownload = false)
        {
            var httpClient = new HttpClient();

            if (conteudo != null)
            {
                var json = JsonConvert.SerializeObject(conteudo);
                var contentPost = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(App.Config.ObterUrlBaseWebApi() + "api/" + metodo, contentPost);
                var stream = await response.Content.ReadAsStreamAsync();
                var ser = new DataContractJsonSerializer(typeof(T));
                stream.Position = 0;
                T t = (T)ser.ReadObject(stream);
                return t;
            }
            else
            {
                var response = await httpClient.GetAsync(App.Config.ObterUrlBaseWebApi() + "api/" + metodo);
                var stream = await response.Content.ReadAsStreamAsync();
                var ser = new DataContractJsonSerializer(typeof(T));
                stream.Position = 0;
                T t = (T)ser.ReadObject(stream);
                return t;
            }
        }

        public static async Task<PostModel> SalvarPost(PostModel post)
        {   
            //upload da foto
            var urlUpload = App.Config.ObterUrlBaseWebApi() + "api/uploadfoto";
            byte[] byteArray = post.ObterByteArrayFoto();

            var requestContent = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(byteArray);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent, "cf", post.UsuarioId.ToString().PadLeft(6,'0') + ".jpg");
            requestContent.Add(new StringContent(post.UsuarioId.ToString()), "usuarioId");

            var client = new HttpClient();
            var response = await client.PostAsync(urlUpload, requestContent);
            var stream = await response.Content.ReadAsStreamAsync();
            var ser = new DataContractJsonSerializer(typeof(RespostaUpload));
            stream.Position = 0;
            var resposta = (RespostaUpload)ser.ReadObject(stream);

            //PostModel postFinal = new PostModel() { Legenda = post.Legenda, NomeArquivo = resposta.nomeArquivo, UsuarioId = post.UsuarioId };
            post.NomeArquivo = resposta.nomeArquivo;

            //salva post
            //var clientt = new HttpClient();
            var json = JsonConvert.SerializeObject(post);
            var contentPost = new StringContent(json, Encoding.UTF8, "application/json");

            var response3 = await client.PostAsync(App.Config.ObterUrlBaseWebApi() + "api/salvarpost", contentPost);
            var stream3 = await response3.Content.ReadAsStreamAsync();

            var ser3 = new DataContractJsonSerializer(typeof(PostModel));
            stream3.Position = 0;
            var resposta3 = (PostModel)ser3.ReadObject(stream3);

            return post;
        }
    }
}
