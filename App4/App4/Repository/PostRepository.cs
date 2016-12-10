using App4.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace App4.Repositoy
{
    public static class PostRepository
    {
        private static List<Post> listaPosts;

        public static async Task<List<Post>> ObterPostsNuvem()
        {
            bool usarCloud = false;
            string endereco = usarCloud ? "https://cfwebapi.herokuapp.com/" : "http://localhost:8084/";

            var clientt = new HttpClient();
            var json = JsonConvert.SerializeObject((new { usuarioId = 1 }));
            var contentPost = new StringContent(json, Encoding.UTF8, "application/json");

            var response3 = await clientt.PostAsync(endereco + "api/obterposts", contentPost);
            var stream3 = await response3.Content.ReadAsStreamAsync();

            var ser3 = new DataContractJsonSerializer(typeof(List<RespostaPost>));
            stream3.Position = 0;
            var listaRespPosts = (List<RespostaPost>)ser3.ReadObject(stream3);
            listaPosts = new List<Post>();
            foreach (var item in listaRespPosts)
            {
                var clientt4 = new HttpClient();
                var json4 = JsonConvert.SerializeObject((new { nomeArquivo = item.nomeArquivo }));
                var contentPost4 = new StringContent(json4, Encoding.UTF8, "application/json");

                var response4 = await clientt.PostAsync(endereco + "api/downloadfoto", contentPost4);
                var stream4 = await response4.Content.ReadAsStreamAsync();

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

        public static async Task<Post> SalvarPost(Post post)
        {
            bool usarCloud = false;
            string endereco = usarCloud ? "https://cfwebapi.herokuapp.com/" : "http://localhost:8084/";

            //upload da foto
            var urlUpload = endereco + "api/uploadfoto";
            byte[] byteArray = post.ObterByteArrayFoto();

            var requestContent = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(byteArray);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent, "foto_de_catioro", "image.jpg");

            var client = new HttpClient();
            var response = await client.PostAsync(urlUpload, requestContent);
            var stream = await response.Content.ReadAsStreamAsync();
            var ser = new DataContractJsonSerializer(typeof(RespostaUpload));
            stream.Position = 0;
            var resposta = (RespostaUpload)ser.ReadObject(stream);

            Post postFinal = new Post() { Legenda = post.Legenda, NomeArquivo = resposta.nomeArquivo, UsuarioId = 1 };

            //salva post
            var clientt = new HttpClient();
            var json = JsonConvert.SerializeObject(postFinal);
            var contentPost = new StringContent(json, Encoding.UTF8, "application/json");

            var response3 = await client.PostAsync(endereco + "api/salvarpost", contentPost);
            var stream3 = await response3.Content.ReadAsStreamAsync();

            var ser3 = new DataContractJsonSerializer(typeof(RespostaSalvarPost));
            stream3.Position = 0;
            var resposta3 = (RespostaSalvarPost)ser3.ReadObject(stream3);

            return postFinal;
        }
    }
}
