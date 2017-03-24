﻿using System;
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
    public class UsuarioModel : INotifyPropertyChanged
    {
        #region Campos

        private string nomeArquivoAvatar;
        private Stream AvatarStream { get; set; }

        #endregion


        #region Propriedades

        [DataMember(Name = "usuarioId")]
        public int UsuarioId { get; set; }

        [DataMember(Name = "nomeArquivoAvatar")]
        public string NomeArquivoAvatar
        {
            get
            {
                return nomeArquivoAvatar;
            }
            set
            {
                nomeArquivoAvatar = value;
                OnPropertyChanged("NomeArquivoAvatar");
            }
        }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "nomeUsuario")]
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #endregion
    }
}
