using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaUpload
    {
        [DataMember]
        internal bool sucesso;

        [DataMember]
        internal string mensagem;

        [DataMember]
        internal string nomeArquivo;
    }
}
