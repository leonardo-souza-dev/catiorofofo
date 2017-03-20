using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

    public class DebugHelper
    {
        public void Print<T>(IEnumerable<T> lista)
        {
            Debug.WriteLine(" ****** * * * * *  DEBUNG PRINT * * * ******** ");

            Type typeParameterType = typeof(T);

            foreach (var obj in lista)
            {
                PrintProperties(obj, 4);
                Debug.WriteLine("- ");
            }
        }

        public void PrintProperties(object obj, int indent)
        {
            try
            {
                if (obj == null) return;
                string indentString = new string(' ', indent);
                Type objType = obj.GetType();
                var properties = objType.GetRuntimeProperties();
                Debug.WriteLine("{0}*{1}", indentString, objType.Name);
                foreach (PropertyInfo property in properties)
                {
                    object propValue = null;
                    try
                    {
                        propValue = property.GetValue(obj, null);
                    }
                    catch
                    {
                        continue;
                    }

                    if (propValue != null && (propValue.GetType() == typeof(string) || propValue.GetType() == typeof(int) || propValue.GetType() == typeof(decimal) || propValue.GetType() == typeof(float)
                        || propValue.GetType() == typeof(byte) || propValue.GetType() == typeof(long) || propValue.GetType() == typeof(char) || propValue.GetType() == typeof(bool)
                        || propValue.GetType() == typeof(short) || propValue.GetType() == typeof(sbyte) || propValue.GetType() == typeof(uint) || propValue.GetType() == typeof(Guid)))
                    {
                        Debug.WriteLine("{0}*{1}: {2}", indentString, property.Name, propValue);
                    }
                    else
                    {
                        PrintProperties(propValue, indent * 2);
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
