using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaObterPost //: Resposta
    {
        [DataMember]
        internal RespostaPost posts;
    }
}
