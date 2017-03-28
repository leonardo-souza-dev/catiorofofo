using App4.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Diagnostics;

namespace App4.Repository
{
    public static class PostRepository
    {
        public static async Task<List<PostModel>> ObterPosts()
        {
            var respostaPosts = await Resposta<List<PostModel>>(null, "obterposts");

            Debug.WriteLine("<DEBUG>");
            foreach (var item in respostaPosts)
            {
                Debug.WriteLine(item.PostId);
                Debug.WriteLine(item.Usuario.UsuarioId);
                Debug.WriteLine(item.Usuario.AvatarUrl);
            }
            Debug.WriteLine("</DEBUG>");
            return respostaPosts;
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
            var uri = App.Config.ObterUrlBaseWebApi() + "api/" + metodo;

            if (conteudo != null)
            {
                var retorno = await new ClienteHttp().PostAsync<T>(uri, conteudo);

                return retorno;
            }
            else
            {
                var retorno = await new ClienteHttp().GetAsync<T>(uri);

                return retorno;
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
            requestContent.Add(imageContent, "cf", post.Usuario.UsuarioId.ToString().PadLeft(6,'0') + ".jpg");
            requestContent.Add(new StringContent(post.Usuario.UsuarioId.ToString()), "usuarioId");

            var client = new HttpClient();
            var response2 = await client.PostAsync(urlUpload, requestContent);
            var stream = await response2.Content.ReadAsStreamAsync();
            var ser = new DataContractJsonSerializer(typeof(RespostaUploadAvatar));
            stream.Position = 0;
            var resposta = (RespostaUploadAvatar)ser.ReadObject(stream);

            //PostModel postFinal = new PostModel() { Legenda = post.Legenda, NomeArquivo = resposta.nomeArquivo, UsuarioId = post.UsuarioId };
            post.NomeArquivo = resposta.nomeArquivo;

            //salva post
            var httpRequest = new HttpClient();
            //var json = JsonConvert.SerializeObject(post);
            //var contentPost = new StringContent(json, Encoding.UTF8, "application/json");

            string userJson = Newtonsoft.Json.JsonConvert.SerializeObject(post);
            var response = await httpRequest.PostAsync(App.Config.ObterUrlBaseWebApi() + "api/salvarpost",
                new StringContent(userJson, System.Text.Encoding.UTF8, "application/json"));

            var streamm = await response.Content.ReadAsStreamAsync();
            var x = new DataContractJsonSerializer(typeof(PostModel));
            streamm.Position = 0;
            var respostaUpload = (PostModel)x.ReadObject(streamm);


            return respostaUpload;
        }

        private static PostModel Convert(System.IO.Stream stream)
        {
            var ser3 = new DataContractJsonSerializer(typeof(PostModel));
            stream.Position = 0;
            PostModel resposta3 = (PostModel)ser3.ReadObject(stream);
            Debug.WriteLine("PostModel");
            Debug.WriteLine(resposta3.PostId);
            Debug.WriteLine(resposta3.Legenda);
            return resposta3;
        }
    }
}
