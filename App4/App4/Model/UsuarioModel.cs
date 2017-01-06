using System;
using System.ComponentModel;
using System.IO;

namespace App4.Model
{
    public class UsuarioModel : INotifyPropertyChanged
    {
        public int UsuarioId { get; set; }

        private string email;
        public string Email { get { return email; } set { email = value; OnPropertyChanged("Email"); } }

        public string NomeArquivoAvatar { get; set; }
        public string NomeUsuario { get; set; }

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
                return Configuracao.UrlWebApi + "api/foto?na=" + NomeArquivoAvatar;
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
    }
}
