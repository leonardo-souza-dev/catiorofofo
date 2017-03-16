using App4.Model;
using App4.View;
using App4.ViewModel;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace App4
{
    public class MainPage : TabbedPage
    {
        private Page1 explorar;
        private Up upload;
        private PerfilView perfil;

        public MainPage()
        {
            explorar = new Page1();
            upload = new Up(this);
            perfil = new PerfilView();

            Children.Add(explorar);
            Children.Add(upload);
            Children.Add(perfil);
        }

        protected override void OnCurrentPageChanged()
        {
            Debug.WriteLine(" ");
            Debug.WriteLine("***** this.CurrentPage.Title");
            Debug.WriteLine(this.CurrentPage.Title);
            Debug.WriteLine(" ");

            if (this.CurrentPage.Title.Equals("enviar catioro fofo"))
            {
                upload.EscolherFoto();
            }
        }
    }
}
