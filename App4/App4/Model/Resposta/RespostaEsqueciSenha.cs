using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaEsqueciSenha
    {
        [DataMember]
        internal string mensagem;

        [DataMember]
        internal bool sucesso;
    }
}
