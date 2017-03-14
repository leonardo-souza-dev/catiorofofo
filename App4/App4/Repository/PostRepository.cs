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

            var respostaPosts = await Resposta<List<RespostaPost>>(null, "obterposts");

            ListaPosts = new List<PostModel>();

            foreach (var respostaPost in respostaPosts)
            {
                List<CurtidaModel> curtidas = new List<CurtidaModel>();

                UsuarioModel usuario = new UsuarioModel();
                usuario.NomeArquivoAvatar = respostaPost.usuario.nomeArquivoAvatar;
                usuario.UsuarioId = respostaPost.usuario.usuarioId;
                usuario.Email = respostaPost.usuario.email;
                usuario.NomeUsuario = respostaPost.usuario.nomeUsuario;


                PostModel post = new PostModel()
                {
                    PostId = respostaPost.postId,
                    Legenda = respostaPost.legenda,
                    UsuarioId = respostaPost.usuarioId,
                    NomeArquivo = respostaPost.nomeArquivo,
                    Curtidas = curtidas,
                    Usuario = usuario
                };
                foreach (var c in respostaPost.curtidas )
                {
                    curtidas.Add(new CurtidaModel { UsuarioId = c.usuarioId, PostId = c.postId });
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
                var response = await httpClient.PostAsync(ObterUrlBaseWebApi() + "api/" + metodo, contentPost);
                var stream = await response.Content.ReadAsStreamAsync();
                var ser = new DataContractJsonSerializer(typeof(T));
                stream.Position = 0;
                T t = (T)ser.ReadObject(stream);
                return t;
            }
            else
            {
                var response = await httpClient.GetAsync(ObterUrlBaseWebApi() + "api/" + metodo);
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
            var urlUpload = ObterUrlBaseWebApi() + "api/uploadfoto";
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
            var clientt = new HttpClient();
            var json = JsonConvert.SerializeObject(post);
            var contentPost = new StringContent(json, Encoding.UTF8, "application/json");

            var response3 = await client.PostAsync(ObterUrlBaseWebApi() + "api/salvarpost", contentPost);
            var stream3 = await response3.Content.ReadAsStreamAsync();

            var ser3 = new DataContractJsonSerializer(typeof(RespostaSalvarPost));
            stream3.Position = 0;
            var resposta3 = (RespostaSalvarPost)ser3.ReadObject(stream3);

            return post;
        }

        private static string ObterUrlBaseWebApi()
        {
            bool usarCloud = true;
            bool debugarAndroid = false;

            string enderecoBase = string.Empty;

            if (usarCloud)
                enderecoBase = "https://cfwebapi.herokuapp.com/";
            else
            {
                enderecoBase += "http://";
                if (debugarAndroid)
                    enderecoBase += "10.0.2.2";
                else
                    enderecoBase += "localhost";
                enderecoBase += ":8084/";
            }
            return enderecoBase;
        }
    }
}
