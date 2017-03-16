using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaLogin
    {
        [DataMember]
        internal UsuarioModel usuario;

        [DataMember]
        internal string mensagem;
    }
}
