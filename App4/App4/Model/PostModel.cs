using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class PostModel
    {
        #region Campos

        private int usuarioId;

        #endregion


        #region Propriedades

        #region Binds

        [DataMember]
        public int PostId { get; set; }
        [DataMember]
        public string Legenda { get; set; }
        [DataMember]
        public string NomeArquivo { get; set; }
        [DataMember]
        public int UsuarioId { get { return this.Usuario.UsuarioId; } set { usuarioId = value; } }
        [DataMember]
        public List<CurtidaModel> Curtidas { get; set; }

        public UsuarioModel Usuario { get; set; }
        public string AvatarUrl { get { return App.Config.ObterUrlBaseWebApi() + "api/foto?na=" + NomeArquivoAvatar; } }

        #endregion

        private Stream FotoStream { get; set; }
        public string NomeArquivoAvatar { get; set; }
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
    }
}
