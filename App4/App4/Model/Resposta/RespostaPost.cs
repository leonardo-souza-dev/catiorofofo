using System.Collections.Generic;
using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaPost
    {
        [DataMember]
        internal int postId;

        [DataMember]
        internal string legenda;

        [DataMember]
        internal int usuarioId;

        [DataMember]
        internal RespostaUsuario usuario;

        [DataMember]
        internal string nomeArquivo;

        [DataMember]
        internal string avatarUrl;

        [DataMember]
        internal List<RespostaCurtida> curtidas;
    }    
}
