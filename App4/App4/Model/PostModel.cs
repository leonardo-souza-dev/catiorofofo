using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class PostModel : INotifyPropertyChanged
    {
        #region Campos

        private UsuarioModel usuario;
        private string nomeArquivoAvatar;

        #endregion


        #region Propriedades

        #region Binds

        [DataMember(Name = "postId")]
        public int PostId { get; set; }

        [DataMember(Name = "legenda")]
        public string Legenda { get; set; }

        [DataMember(Name = "nomeArquivo")]
        public string NomeArquivo { get; set; }

        //[DataMember(Name = "usuarioId")]
        //public int UsuarioId { get; set; }

        [DataMember(Name = "curtidas")]
        public List<CurtidaModel> Curtidas { get; set; }

        [DataMember(Name = "usuario")]
        public UsuarioModel Usuario
        {
            get
            {
                return usuario;
            }
            set
            {
                usuario = value;
                OnPropertyChanged("Usuario");
            }
        }


        public string NomeArquivoAvatar
        {
            get
            {
                return App.Config.ObterUrlBaseWebApi() + "api/foto?na=" + this.Usuario.NomeArquivoAvatar;
            }
            set
            {
                nomeArquivoAvatar = value;
                OnPropertyChanged("NomeArquivoAvatar");
            }
        }

        /*public string AvatarUrl
        {
            get
            {
                return App.Config.ObterUrlBaseWebApi() + "api/foto?na=" + Usuario.NomeArquivoAvatar;
            }
        }*/

        #endregion

        private Stream FotoStream { get; set; }
        public bool CurtidaHabilitada { get; set; } = true;
        public string CurtidaTexto { get { return CurtidaHabilitada ? "curtir" : "descurtir"; } }
        public int NumCurtidas { get { return Curtidas.Count; } }
        public string FotoUrl { get { return App.Config.ObterUrlBaseWebApi() + "api/foto?na=" + NomeArquivo; } }
        public string QuemPostou { get { return this.Usuario.NomeUsuario; } }

        #endregion

        #region Metodos

        public PostModel(Stream stream)
        {
            this.FotoStream = stream;
            this.Curtidas = new List<CurtidaModel>();
        }

        public PostModel()
        {
            Curtidas = new List<CurtidaModel>();
        }

        public byte[] ObterByteArrayFoto()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                FotoStream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
    }
}
