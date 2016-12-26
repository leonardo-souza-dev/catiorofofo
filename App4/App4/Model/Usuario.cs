using System;
using System.ComponentModel;

namespace App4.Model
{
    public class Usuario : INotifyPropertyChanged
    {
        public int UsuarioId { get; set; }

        private string email;
        public string Email { get { return email; } set { email = value; OnPropertyChanged("Email"); } }

        public string AvatarUrl { get; set; }
        public string NomeUsuario { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
