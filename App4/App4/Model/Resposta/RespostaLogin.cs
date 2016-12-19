using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaLogin
    {
        [DataMember]
        internal int usuarioId;

        [DataMember]
        internal string avatar;

        [DataMember]
        internal string senha;
    }
}
