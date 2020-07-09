using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometría
{
    class ListaNoMetales
    {
        private List<Elemento> nometales = new Lector_elementos().leerXML(tipoElemento.nometal);

        internal List<Elemento> Nometales { get => nometales; set => nometales = value; }
    }
}
