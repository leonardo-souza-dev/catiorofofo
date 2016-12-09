using Plugin.Media.Abstractions;
using System.IO;
using Xamarin.Forms;

namespace App4.Model
{
    public class Post
    {
        private Stream FotoStream { get; set; }

        public int PostId { get; set; }
        public string Legenda { get; set; }
        public int UsuarioId { get; set; }
        public string NomeArquivo { get; set; }

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
