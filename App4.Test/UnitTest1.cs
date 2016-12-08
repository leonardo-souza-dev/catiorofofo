using App4.Model;
using App4.Repositoy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Drawing.Imaging;

namespace App4.Test
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void TestePost()
        {
            Image image = Image.FromFile(@"C:\Users\Leonardo\Pictures\golden.jpg");

            var stream = new System.IO.MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;
            

            PostRepository.SalvarPost(new Post(stream)
            {
                Legenda = "teste",
                UsuarioId = 1
            });
            Assert.Fail();
        }
    }
}
