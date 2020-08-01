using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometria
{
    class ListaMetales
    {
        private List<Elemento> metales = new Lector_elementos().leerXML(tipoElemento.metal);

        internal List<Elemento> Metales { get => metales; set => metales = value; }
    }
}
