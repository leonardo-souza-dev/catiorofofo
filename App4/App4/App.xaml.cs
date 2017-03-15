using App4.View;
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
        public App()
        {
            InitializeComponent();

            ConfiguracaoApp config = new ConfiguracaoApp();
            MainPage = new LoginViewCS(config);
        }
    }
}
