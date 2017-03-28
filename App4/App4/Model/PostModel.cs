using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class PostModel : INotifyPropertyChanged
    {

        [DataMember(Name = "legenda")]
        public string Legenda { get; set; }


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
        private UsuarioModel usuario;

        [DataMember(Name = "nomeArquivo")]
        public string NomeArquivo
        {
            get
            {
                return nomeArquivo;
            }
            set
            {
                nomeArquivo = value;
                OnPropertyChanged("NomeArquivo");
                
            }
        }
        private string nomeArquivo;









        private string nomeArquivoAvatar;




        


        [DataMember(Name = "postId")]
        public int PostId { get; set; }

        //[DataMember(Name = "usuarioId")]
        //public int UsuarioId { get; set; }

        [DataMember(Name = "curtidas")]
        public List<CurtidaModel> Curtidas { get; set; }


        public string NomeArquivoAvatar
        {
            get
            {
                return App.Config.ObterUrlBaseWebApi() + "api/foto?na=" + this.Usuario.NomeArquivoAvatar;
            }
            set
            {
                this.Usuario.NomeArquivoAvatar = value;
                OnPropertyChanged("NomeArquivoAvatar");
            }
        }




        private Stream FotoStream { get; set; }
        public bool CurtidaHabilitada { get; set; } = true;
        public string CurtidaTexto { get { return CurtidaHabilitada ? "curtir" : "descurtir"; } }
        public int NumCurtidas { get { return Curtidas.Count; } }
        public string FotoUrl { get { return App.Config.ObterUrlBaseWebApi() + "api/foto?na=" + NomeArquivo; } }
        public string QuemPostou { get { return this.Usuario.NomeUsuario; } }

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
