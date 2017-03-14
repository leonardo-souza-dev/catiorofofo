using App4.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using App4.Model.Resposta;

namespace App4.Repository
{
    public static class UsuarioRepository
    {
        public static async Task<RespostaFetch> TesteConexao()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(ObterUrlBaseWebApi() + "fetch");
            var stream = await response.Content.ReadAsStreamAsync();
            var ser = new DataContractJsonSerializer(typeof(RespostaFetch));
            stream.Position = 0;
            RespostaFetch t = (RespostaFetch)ser.ReadObject(stream);
            return t;
        }


        public static async Task<RespostaAtualizarUsuario> Atualizar(UsuarioModel usuario)
        {
            if (usuario.EditouAvatar())
            {
                //upload da foto
                var urlUpload = ObterUrlBaseWebApi() + "api/uploadavatar";
                byte[] byteArray = usuario.ObterByteArrayAvatar();

                var requestContent = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(byteArray);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                requestContent.Add(imageContent, "av", usuario.UsuarioId.ToString().PadLeft(6, '0') + ".jpg");
                requestContent.Add(new StringContent(usuario.UsuarioId.ToString()), "usuarioId");

                var client = new HttpClient();
                var response = await client.PostAsync(urlUpload, requestContent);
                var stream = await response.Content.ReadAsStreamAsync();
                var ser = new DataContractJsonSerializer(typeof(RespostaUpload));
                stream.Position = 0;
                var respostaUpload = (RespostaUpload)ser.ReadObject(stream);
                var request = new
                {
                    nomeUsuario = usuario.NomeUsuario,
                    usuarioId = usuario.UsuarioId,
                    email = usuario.Email,
                    nomeArquivoAvatar = respostaUpload.nomeArquivo
                };
                var resposta = await Resposta<RespostaAtualizarUsuario>(request, "atualizarusuario");
                return resposta;
            }
            else
            {
                var request = new
                {
                    nomeUsuario = usuario.NomeUsuario,
                    usuarioId = usuario.UsuarioId,
                    email = usuario.Email
                };
                var resposta = await Resposta<RespostaAtualizarUsuario>(request, "atualizarusuario");
                return resposta;
            }

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

        public static async Task<RespostaCadastro> Cadastro(string emailDigitado, string senhaDigitada, string nomeUsuarioDigitado)
        {
            var resposta = await Resposta<RespostaCadastro>(new { email = emailDigitado, senha = senhaDigitada, nomeUsuario = nomeUsuarioDigitado }, "cadastro");

            return resposta;
        }

        public static async Task<RespostaLogin> Login(string emailDigitado, string senhaDigitada)
        {
            var resposta = await Resposta<RespostaLogin>(new { email = emailDigitado, senha = senhaDigitada }, "login");

            return resposta;
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
        
    }
}
