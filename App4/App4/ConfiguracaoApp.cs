using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App4
{
    public class ConfiguracaoApp
    {
        bool DebugarAndroid = false;
        public bool UsarCloud { get { return false; }  }

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
