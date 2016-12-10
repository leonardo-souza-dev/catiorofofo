using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace App4.Model
{
    [DataContract]
    public class Resposta
    {
        [DataMember]
        internal bool sucesso;

        [DataMember]
        internal string mensagem;

        //[DataMember]
        //internal object objeto;
    }

    [DataContract]
    public class RespostaUpload : Resposta
    {
        [DataMember]
        internal string nomeArquivo;
    }

    [DataContract]
    public class RespostaSalvarPost : Resposta
    {
        [DataMember]
        internal int postId;
    }

    [DataContract]
    public class RespostaObterPost : Resposta
    {
        [DataMember]
        internal RespostaPost posts;
    }

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
        internal string nomeArquivo;
    }
}
