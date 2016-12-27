using System;
using System.ComponentModel;

namespace App4.Model
{
    public class Usuario : INotifyPropertyChanged
    {
        public int UsuarioId { get; set; }

        private string email;
        public string Email { get { return email; } set { email = value; OnPropertyChanged("Email"); } }

        public string NomeArquivoAvatar { get; set; }
        public string NomeUsuario { get; set; }



        public string AvatarUrl
        {
            get
            {
                return ObterUrlBaseWebApi() + "api/foto?na=" + NomeArquivoAvatar;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
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
    }
}
