using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
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
using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class UsuarioModel
    {
        #region Campos

        private int usuarioId;
        private string nomeArquivoAvatar;
        private string email;
        private string nomeUsuario;
        private Stream AvatarStream { get; set; }

        #endregion

        #region Propriedades

        [DataMember]
        public int UsuarioId { get { return usuarioId; } set { usuarioId = value; } }
        [DataMember]
        public string NomeArquivoAvatar { get { return nomeArquivoAvatar; } set { nomeArquivoAvatar = value; } }
        [DataMember]
        public string Email { get { return email; } set { email = value; } }
        [DataMember]
        public string NomeUsuario { get { return nomeUsuario; } set { nomeUsuario = value; } }
        public string AvatarUrl { get { return App.Config.ObterUrlBaseWebApi() + "api/foto?na=" + NomeArquivoAvatar; } }
        public bool NomeUsuarioEntry { get; set; }

        #endregion

        #region Metodos

        /*protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        */
        public byte[] ObterByteArrayAvatar()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                AvatarStream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public bool EditouAvatar()
        {
            return AvatarStream != null;
        }
        public void SetarAvatarStream(Stream stream)
        {
            AvatarStream = stream;
        }

        #endregion

    }

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

        public static async Task<RespostaAtualizarUsuario> Atualizar(UsuarioModel usuario)
        {
            if (usuario.EditouAvatar())
            {
                //upload da foto
                var urlUpload = App.Config.ObterUrlBaseWebApi() + "api/uploadavatar";
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
