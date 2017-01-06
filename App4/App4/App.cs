using App4.View;

using Xamarin.Forms;

namespace App4
{
    public class App : Application
    {
        public App()
        {
            MainPage = new LoginViewCS();
        }
    }
    
    public static class Configuracao
    {
        private static string UrlLocalAndroid = "http://10.0.2.2:8084/";
        private static string UrlLocalWindows = "http://localhost:8084/";
        private static string UrlCloud = "https://cfwebapi.herokuapp.com/";

        public static string UrlWebApi = UrlCloud;
    }
}
