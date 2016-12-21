using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaCurtir
    {
        [DataMember]
        internal string mensagem;

        [DataMember]
        internal RespostaCurtida curtida;
    }

    [DataContract]
    public class RespostaCurtida
    {
        [DataMember]
        internal int postId;

        [DataMember]
        internal int usuarioId;
    }
}
