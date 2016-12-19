using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaUpload
    {
        [DataMember]
        internal string nomeArquivo;
    }
}
