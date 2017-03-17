using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using App4.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
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

        private Stream AvatarStream { get; set; }

        #endregion


        #region Propriedades

        [DataMember]
        public int UsuarioId { get; set; }
        [DataMember]
        public string NomeArquivoAvatar { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string NomeUsuario { get; set; }
        public string AvatarUrl { get { return App.Config.ObterUrlBaseWebApi() + "api/foto?na=" + NomeArquivoAvatar; } }
        public bool NomeUsuarioEntry { get; set; }

        #endregion


        #region Metodos

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
}
