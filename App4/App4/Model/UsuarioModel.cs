using System;
using System.ComponentModel;
using System.IO;

namespace App4.Model
{
    public class UsuarioModel : INotifyPropertyChanged
    {
        public bool NomeUsuarioEntry { get; set; }

        public int UsuarioId { get; set; }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        private string nomeArquivoAvatar;
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

        private string nomeUsuario;
        public string NomeUsuario
        {
            get
            {
                return nomeUsuario;
            }
            set
            {
                nomeUsuario = value;
                OnPropertyChanged("NomeUsuario");
            }
        }

        private Stream AvatarStream { get; set; }

        public bool EditouAvatar()
        {
            return AvatarStream != null;
        }
        public void SetarAvatarStream(Stream stream)
        {
            AvatarStream = stream;
        }

        public string AvatarUrl
        {
            get
            {
                return ObterUrlBaseWebApi() + "api/foto?na=" + NomeArquivoAvatar;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public byte[] ObterByteArrayAvatar()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                AvatarStream.CopyTo(ms);
                return ms.ToArray();
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
    }
}
