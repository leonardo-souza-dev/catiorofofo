﻿using System.Collections.Generic;
using System.IO;

namespace App4.Model
{
    public class PostModel
    {
        private Stream FotoStream { get; set; }

        public int PostId { get; set; }

        public string Legenda { get; set; }

        public int UsuarioId { get; set; }

        public string NomeArquivo { get; set; }

        public bool CurtidaHabilitada { get; set; } = true;

        public string CurtidaTexto { get { return CurtidaHabilitada ? "curtir" : "descurtir"; } }

        public int NumCurtidas { get { return Curtidas.Count; } }

        public List<Curtida> Curtidas { get; set; }

        public string FotoUrl
        {
            get
            {
                return ObterUrlBaseWebApi() + "api/foto?na=" + NomeArquivo;
            }
        }

        private static string ObterUrlBaseWebApi()
        {
            bool usarCloud = false;
            bool debugarAndroid = false;

            string enderecoBase = string.Empty;

            if (usarCloud)
                enderecoBase = "https://cfwebapi.herokuapp.com/";
            else
            {
                enderecoBase += "http://";
                if (debugarAndroid)
                    enderecoBase += "10.0.2.2";
                else
                    enderecoBase += "localhost";
                enderecoBase += ":8084/";
            }
            return enderecoBase;
        }

        public PostModel(Stream stream)
        {
            FotoStream = stream;
        }

        public PostModel()
        {
            Curtidas = new List<Curtida>();
        }

        public byte[] ObterByteArrayFoto()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                FotoStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }

    public class Curtida
    {
        public int PostId { get; set; }
        public int UsuarioId { get; set; }
    }
}