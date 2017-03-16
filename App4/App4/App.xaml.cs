using App4.View;
using App4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4
{
    public partial class App : Application
    {
        public static PostViewModel PostVM { get; set; }
        public static UsuarioViewModel UsuarioVM { get; set; }
        public static ConfiguracaoApp Config { get; set; }

        public App()
        {
            InitializeComponent();

            UsuarioVM = new UsuarioViewModel();
            PostVM = new PostViewModel();
            Config = new ConfiguracaoApp();


            MainPage = new LoginViewCS();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
