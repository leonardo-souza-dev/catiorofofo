using App4.View;

using Xamarin.Forms;

namespace App4
{
    public class App : Application
    {
        public App()
        {
            ConfiguracaoApp config = new ConfiguracaoApp();
            MainPage = new LoginViewCS(config);
        }
    }

    public class ConfiguracaoApp
    {
        bool DebugarAndroid = false;
        bool UsarCloud = true;

        public string ObterUrlBaseWebApi()
        {

            string enderecoBase = string.Empty;

            if (UsarCloud)
                enderecoBase = "https://cfwebapi.herokuapp.com/";
            else
            {
                enderecoBase += "http://";
                if (DebugarAndroid)
                    enderecoBase += "10.0.2.2";
                else
                    enderecoBase += "localhost";
                enderecoBase += ":8084/";
            }
            return enderecoBase;
        }
    }
}
