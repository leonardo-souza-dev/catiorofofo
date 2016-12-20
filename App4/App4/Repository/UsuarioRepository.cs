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

        public static async Task<Usuario> Cadastro(string emailDigitado, string senhaDigitada)
        {
            var resposta = await Resposta<RespostaCadastro>(new { email = emailDigitado, senha = senhaDigitada }, "cadastro");
            usuario = new Usuario();
            usuario.Email = resposta.usuario.email;
            usuario.UsuarioId = resposta.usuario.usuarioId;

            return usuario;
        }

        public static async Task<Usuario> Login(string senhaDigitada)
        {
            var resposta = await Resposta<RespostaLogin>(new { senha = senhaDigitada }, "login");
            usuario = new Usuario();
            usuario.UsuarioId = resposta.usuarioId;

            return usuario;
        }

        public static async Task<RespostaEsqueciSenha> EsqueciSenha(string emailDigitado)
        {
            var resposta = await Resposta<RespostaEsqueciSenha>(new { email = emailDigitado }, "esquecisenha");

            return resposta;
        }

        private static async Task<T> Resposta<T>(object conteudo, string metodo, bool ehDownload = false)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(conteudo);
            var contentPost = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(ObterUrlBaseWebApi() + "api/" + metodo, contentPost);
            var stream = await response.Content.ReadAsStreamAsync();
            var ser = new DataContractJsonSerializer(typeof(T));
            stream.Position = 0;
            T t = (T)ser.ReadObject(stream);

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
