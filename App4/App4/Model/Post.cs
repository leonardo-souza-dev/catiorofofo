using System.IO;

namespace App4.Model
{
    public class Post
    {
        private Stream FotoStream { get; set; }

        public int PostId { get; set; }

        public string Legenda { get; set; }

        public int UsuarioId { get; set; }

        public string NomeArquivo { get; set; }

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

        public Post(Stream stream)
        {
            FotoStream = stream;
        }

        public Post()
        {

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
}
