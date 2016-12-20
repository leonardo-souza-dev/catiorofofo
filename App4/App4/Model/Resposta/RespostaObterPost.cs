using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaObterPost 
    {
        [DataMember]
        internal RespostaPost posts;
    }
}
