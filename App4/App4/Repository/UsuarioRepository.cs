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
            var response = await httpClient.GetAsync(App.Config.ObterUrlBaseWebApi() + "fetch");
            var stream = await response.Content.ReadAsStreamAsync();
            var ser = new DataContractJsonSerializer(typeof(RespostaFetch));
            stream.Position = 0;
            RespostaFetch t = (RespostaFetch)ser.ReadObject(stream);
            return t;
        }

        public static async Task<RespostaUploadAvatar> UploadAvatar()
        {
            var urlUpload = App.Config.ObterUrlBaseWebApi() + "api/uploadavatar";
            byte[] byteArray = App.UsuarioVM.Usuario.ObterByteArrayAvatar();
            var requestContent = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(byteArray);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent, "av", App.UsuarioVM.Usuario.UsuarioId.ToString().PadLeft(6, '0') + ".jpg");
            requestContent.Add(new StringContent(App.UsuarioVM.Usuario.UsuarioId.ToString()), "usuarioId");

            var client = new HttpClient();
            var response = await client.PostAsync(urlUpload, requestContent);//upload da foto do usuario
            var stream = await response.Content.ReadAsStreamAsync();
            var ser = new DataContractJsonSerializer(typeof(RespostaUploadAvatar));
            stream.Position = 0;

            var respostaUpload = (RespostaUploadAvatar)ser.ReadObject(stream);//retornando nome do arquivo uploadado

            return respostaUpload;
        }

        public static async Task<UsuarioModel> Atualizar()
        {
            var request = new
            {
                nomeUsuario = App.UsuarioVM.Usuario.NomeUsuario,
                usuarioId = App.UsuarioVM.Usuario.UsuarioId,
                email = App.UsuarioVM.Usuario.Email,
                nomeArquivoAvatar = App.UsuarioVM.Usuario.NomeArquivoAvatar
            };
            var resposta = await Resposta<UsuarioModel>(request, "atualizarusuario");

            return resposta;            
        }

        public static async Task<RespostaCadastro> Cadastro(string emailDigitado, string senhaDigitada, string nomeUsuarioDigitado)
        {
            var resposta = await Resposta<RespostaCadastro>(new { email = emailDigitado, senha = senhaDigitada, nomeUsuario = nomeUsuarioDigitado }, "cadastro");

            return resposta;
        }

        public static async Task<UsuarioModel> Login(string emailDigitado, string senhaDigitada)
        {
            var resposta = await Resposta<UsuarioModel>(new { email = emailDigitado, senha = senhaDigitada }, "login");

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
            var response = await httpClient.PostAsync(App.Config.ObterUrlBaseWebApi() + "api/" + metodo, contentPost);
            var stream = await response.Content.ReadAsStreamAsync();
            var ser = new DataContractJsonSerializer(typeof(T));
            stream.Position = 0;
            T t = (T)ser.ReadObject(stream);

            return t;
        }

    }
}
