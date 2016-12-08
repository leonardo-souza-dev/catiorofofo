using App4.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
//using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Text;

namespace App4.Repositoy
{
    public static class PostRepository
    {
        private static List<Post> listaPosts;

        public static async Task<List<Post>> ObterPostsNuvem()
        {
            var httpRequest = new HttpClient();
            var stream = await httpRequest.GetStreamAsync(
                "http://aaaaamobapiwsrest20161109074638.azurewebsites.net/api/moduloes");

            var moduloSerializer = new DataContractJsonSerializer(typeof(List<Post>));
            listaPosts = (List<Post>)moduloSerializer.ReadObject(stream);

            return listaPosts;
        }

        public static void SalvarPostOld(Post post)
        {
            var httpRequest = new HttpClient();
            var response = httpRequest.PostAsJsonAsync(
                "https://cfwebapi.herokuapp.com/api/uploadfoto",
                post.ObterByteArrayFoto());

            //var moduloSerializer = new DataContractJsonSerializer(typeof(List<Post>));
            //var staPosts = moduloSerializer.ReadObject(response);
            int a = 1;
            a++;
        }

        public static List<Post> ObterPostsMock()
        {
            if (listaPosts == null)
            {
                listaPosts = new List<Post>();
                //@"C:\Users\Leonardo\Pictures\Camera Roll\WIN_20161203_122140.JPG"
                listaPosts.Add(new Post()
                {
                    //FotoHash = "http://lorempixel.com/300/300/",
                    Legenda = "dogs forever"
                    //,AvatarUrl = "http://lorempixel.com/40/40/"
                });
                //@"C:\Users\Leonardo\Pictures\Camera Roll\WIN_20161203_122147.JPG"
                listaPosts.Add(new Post()
                {
                    //FotoHash = "http://lorempixel.com/300/300/",
                    Legenda = "cachorro passeando!"
                    //,AvatarUrl = "http://lorempixel.com/40/40/"
                });
            }
            return listaPosts;

        }

        public static async void SalvarPost(Post post)
        {
            bool usarCloud = false;
            string endereco = usarCloud ? "https://cfwebapi.herokuapp.com/" : "http://localhost:8084/";

            var urlUpload = endereco + "api/uploadfoto";
            byte[] byteArray = post.ObterByteArrayFoto();

            var requestContent = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(byteArray);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent, "foto_de_catioro", "image.jpg");

            var client = new HttpClient();
            var response = await client.PostAsync(urlUpload, requestContent);



            var urlSalvarPost = endereco + "api/salvarpost";

            HttpClient clientt = new HttpClient();
            var json = JsonConvert.SerializeObject((new Post() { Legenda = post.Legenda }));
            HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");

            var res2 = await clientt.PostAsync(new Uri(urlSalvarPost), contentPost).ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());

        }
    }
}
