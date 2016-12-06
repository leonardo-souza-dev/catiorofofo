using App4.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

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

        public static List<Post> ObterPostsMock()
        {
            if (listaPosts == null)
            {
                listaPosts = new List<Post>();

                listaPosts.Add(new Post
                {
                    //FotoHash = "http://lorempixel.com/300/300/",
                    Legenda = "dogs forever"
                    //,AvatarUrl = "http://lorempixel.com/40/40/"
                });
                listaPosts.Add(new Post
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
            var url = "http://cfwebapi.herokuapp.com/api/novopost";
            byte[] byteArray = new byte[post.Foto.GetStream().Length];
            using (var memoryStream = new MemoryStream())
            {
                post.Foto.GetStream().CopyTo(memoryStream);
                byteArray = memoryStream.ToArray();
            }

            var requestContent = new MultipartFormDataContent();
            //http://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload
            //    here you can specify boundary if you need---^
            var imageContent = new ByteArrayContent(byteArray);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent, "image", "image.jpg");

            await new HttpClient().PostAsync(url, requestContent);
        }
    }
}
