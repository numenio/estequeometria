using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometría
{
    public enum tipoElemento {metal, nometal, oxigeno, noValido};
    class Elemento
    {
        private string nombre = "";
        private string simbolo = "";
        private List<string> valencias;

        public string Nombre { get => nombre; set => nombre = value; }
        public string Simbolo { get => simbolo; set => simbolo = value; }
        public List<string> Valencias { get => valencias; set => valencias = value; }

        public Elemento(String nombreElemento, String simboloElemento, List<string> valenciasElemento)
        {
            nombre = nombreElemento;
            simbolo = simboloElemento;
            valencias = valenciasElemento;
        }

        public bool EsDiatomico (ElementoEnUso el)
        {
            switch(el.Simbolo)
            {
                case "S":
                case "O":
                    return true;
                default:
                    return false;
            }    
        }
    }
}
