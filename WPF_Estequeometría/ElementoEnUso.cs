using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometría
{
    class ElementoEnUso
    {
        private string nombre = "";
        private string simbolo = "";
        private int cantAtomos = 1;
        private bool swEsValido = false;

        //public enum tipoElemento { metal, nometal, oxidrilo, noValido}
        //private int cantMoleculas;
        private tipoElemento tipodeAtomo = tipoElemento.noValido;

        public string Nombre { get => nombre; set => nombre = value; }
        public string Simbolo { get => simbolo; set => simbolo = value; }
        public int CantAtomos { get => cantAtomos; set => cantAtomos = value; }
        public bool SwEsValido { get => swEsValido; set => swEsValido = value; }
        public tipoElemento TipodeAtomo { get => tipodeAtomo; set => tipodeAtomo = value; }

        //public int CantMoleculas { get => cantMoleculas; set => cantMoleculas = value; }

        public ElementoEnUso(String nombreElemento, String simboloElemento, int cantAtomosEnUso, bool swEsElemValido)
        {
            nombre = nombreElemento;
            simbolo = simboloElemento;
            cantAtomos = cantAtomosEnUso;
            swEsValido = swEsElemValido;
            //cantMoleculas = cantMoleculasenFormula;
        }
    }
}
