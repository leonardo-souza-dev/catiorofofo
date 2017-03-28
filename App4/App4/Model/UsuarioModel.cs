using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class UsuarioModel : INotifyPropertyChanged
    {

        [DataMember(Name = "nomeUsuario")]
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
        private string nomeUsuario;

        private string nomeArquivoAvatar;
        private Stream AvatarStream { get; set; }




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


        public string AvatarUrl
        {
            get
            {
                return App.Config.ObterUrlBaseWebApi() + "api/foto?na=" + NomeArquivoAvatar;
            }
            set
            {
                avatarUrl = value;
                OnPropertyChanged("AvatarUrl");
            }
        }
        private string avatarUrl;


        public bool NomeUsuarioEntry { get; set; }




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
    }
}
