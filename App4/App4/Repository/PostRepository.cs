using App4.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
                    FotoUrl = "http://lorempixel.com/300/300/",
                    Legenda = "dogs forever",
                    AvatarUrl = "http://lorempixel.com/40/40/"
                });
                listaPosts.Add(new Post
                {
                    FotoUrl = "http://lorempixel.com/300/300/",
                    Legenda = "cachorro passeando!",
                    AvatarUrl = "http://lorempixel.com/40/40/"
                });
            }
            return listaPosts;

        }

        public static void SalvarPost(Post post)
        {
            if (listaPosts == null)
                listaPosts = new List<Post>();

            listaPosts.Add(post);

            //TODO: Implementar salvar post na nuvem
        }
    }
}
