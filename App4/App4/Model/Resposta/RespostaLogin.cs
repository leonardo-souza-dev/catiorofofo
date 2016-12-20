﻿using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaLogin
    {
        [DataMember]
        internal RespostaUsuario usuario;

        [DataMember]
        internal string mensagem;
    }
}
