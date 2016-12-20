using App4.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace App4.Repository
{
    public static class PostRepository
    {
        private static List<Post> listaPosts;

        public static async Task<List<Post>> ObterPosts(int usuarioIdPassado)
        {
            var listaRespPosts = await Resposta<List<RespostaPost>>(new { usuarioId = usuarioIdPassado });

            listaPosts = new List<Post>();

            foreach (var item in listaRespPosts)
            {
                listaPosts.Add( 
                    new Post() {
                        PostId = item.postId,
                        Legenda = item.legenda,
                        UsuarioId = item.usuarioId,
                        NomeArquivo = item.nomeArquivo
                    });
            }
            return listaPosts;
        }

        private static async Task<T> Resposta<T>(object conteudo, bool ehDownload = false)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(conteudo);
            var contentPost = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(ObterUrlBaseWebApi() + "api/obterposts", contentPost);
            var stream = await response.Content.ReadAsStreamAsync();
            var ser = new DataContractJsonSerializer(typeof(T));
            stream.Position = 0;
            return (T)ser.ReadObject(stream);
        }

        private static string ObterUrlBaseWebApi()
        {
            bool usarCloud = false;
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

        public static async Task<Post> SalvarPost(Post post)
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

            Post postFinal = new Post() { Legenda = post.Legenda, NomeArquivo = resposta.nomeArquivo, UsuarioId = post.UsuarioId };

            //salva post
            var clientt = new HttpClient();
            var json = JsonConvert.SerializeObject(postFinal);
            var contentPost = new StringContent(json, Encoding.UTF8, "application/json");

            var response3 = await client.PostAsync(ObterUrlBaseWebApi() + "api/salvarpost", contentPost);
            var stream3 = await response3.Content.ReadAsStreamAsync();

            var ser3 = new DataContractJsonSerializer(typeof(RespostaSalvarPost));
            stream3.Position = 0;
            var resposta3 = (RespostaSalvarPost)ser3.ReadObject(stream3);

            return postFinal;
        }
    }
}
