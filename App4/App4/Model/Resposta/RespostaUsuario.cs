using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaUsuario
    {
        [DataMember]
        internal int usuarioId;

        [DataMember]
        internal string email;

        [DataMember]
        internal string nomeArquivoAvatar;

        [DataMember]
        internal string nomeUsuario;
    }
}
