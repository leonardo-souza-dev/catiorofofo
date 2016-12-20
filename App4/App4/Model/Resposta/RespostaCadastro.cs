﻿using System.Runtime.Serialization;

namespace App4.Model
{
    [DataContract]
    public class RespostaCadastro
    {
        [DataMember]
        internal bool sucesso;

        [DataMember]
        internal string mensagem;

        [DataMember]
        internal RespostaUsuario usuario;
    }

    [DataContract]
    public class RespostaUsuario
    {
        [DataMember]
        internal int usuarioId;

        [DataMember]
        internal string email;
    }
}
