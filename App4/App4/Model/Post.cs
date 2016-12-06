using Plugin.Media.Abstractions;

namespace App4.Model
{
    public class Post
    {
        public int PostId { get; set; }
        public MediaFile Foto { get; set; }
        public string Legenda { get; set; }
        public int UsuarioId { get; set; }
    }
}
