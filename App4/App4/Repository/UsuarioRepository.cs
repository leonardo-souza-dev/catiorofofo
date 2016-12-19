using App4.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;

namespace App4.Repository
{
    public static class UsuarioRepository
    {
        private static Usuario usuario;

        public static async Task<Usuario> Login(string senhaDigitada)
        {
            var respostaLogin = await Resposta<RespostaLogin>(new { senha = senhaDigitada });
            usuario = new Usuario();
            usuario.UsuarioId = respostaLogin.usuarioId;

            return usuario;
        }

        private static async Task<T> Resposta<T>(object conteudo, bool ehDownload = false)
        {
            Debug.WriteLine("UsuarioRepository > Login > Resposta > 0. INICIO");
            var httpClient = new HttpClient();
            Debug.WriteLine("UsuarioRepository > Login > Resposta > 1. httpClient ");
            var json = JsonConvert.SerializeObject(conteudo);
            Debug.WriteLine("UsuarioRepository > Login > Resposta > 2. json ");
            var contentPost = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine("UsuarioRepository > Login > Resposta > 3. StringContent ");
            var response = await httpClient.PostAsync(ObterUrlBaseWebApi() + "api/login", contentPost);
            Debug.WriteLine("UsuarioRepository > Login > Resposta > 4. response ");
            var stream = await response.Content.ReadAsStreamAsync();
            Debug.WriteLine("UsuarioRepository > Login > Resposta > 5. stream ");
            var ser = new DataContractJsonSerializer(typeof(T));
            Debug.WriteLine("UsuarioRepository > Login > Resposta > 6. ser ");
            stream.Position = 0;
            Debug.WriteLine("UsuarioRepository > Login > Resposta > 7. stream.Position = 0; ");
            T t = (T)ser.ReadObject(stream);
            Debug.WriteLine("UsuarioRepository > Login > Resposta > 8. t ");
            return t;
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
        
    }
}
