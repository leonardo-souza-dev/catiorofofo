using System;
using System.ComponentModel;

namespace App4.Model
{
    public class Usuario : INotifyPropertyChanged
    {
        public int UsuarioId { get; set; }
        public string Email { get; set; }
        public string AvatarHash { get; set; }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }
    }
}
