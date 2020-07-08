using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WPF_Estequeometría
{
    class Molecula
    {
        public enum tipoMolécula { oxido, anhidrido, acido, hidroxido, sal, noValida };
        List<ElementoEnUso> elementosMolécula = new List<ElementoEnUso>();
        tipoMolécula tipo = tipoMolécula.noValida;
        string nombre = "";
        string cadenaMoléculaOrdenada = "";
        int cantidadMolecula = 1;
        private List<Elemento> metales = new Lector_elementos().leerXML(tipoElemento.metal);
        private List<Elemento> nometales = new Lector_elementos().leerXML(tipoElemento.nometal);

        public List<ElementoEnUso> ElementosMolécula { get => elementosMolécula; }
        public tipoMolécula Tipo { get => tipo; }
        public string Nombre {get => nombre;}
        public string CadenaMolécula { get => cadenaMoléculaOrdenada; }
        public int CantidadMolécula { get => cantidadMolecula; }

        private List<ElementoEnUso> separarElmentos (string cadenaFormula)
        {
            string auxcadena = "";
            List<string> auxElem = new List<string>();
            int auxPos = 0;
            bool swPrimerLetra = true;
            foreach (char c in cadenaFormula) 
            {
                auxPos++;
                if (!char.IsWhiteSpace(c))
                {
                    
                    if (char.IsUpper(c) && !swPrimerLetra)
                    {
                        //break;
                        auxElem.Add(auxcadena);
                        auxcadena = "";
                        //auxcadena += c;
                    }
                    if (swPrimerLetra && char.IsDigit(c))
                    {
                        cantidadMolecula = int.Parse(c.ToString());
                    }
                    else
                        auxcadena += c;

                    if (swPrimerLetra && !char.IsDigit(c) && !char.IsWhiteSpace(c))
                        swPrimerLetra = false;

                    if (auxPos == cadenaFormula.Length)
                        auxElem.Add(auxcadena);
                    
                }
            }


            //chequear que sean elementos válidos los que se encontraron arriba
            List<ElementoEnUso> elementos = new List<ElementoEnUso>();
            foreach (String elem in auxElem)
            {
                int cantAtomos = 1;
                //int cantMol = 1;
                string cadenaSimbolo = elem;
                string cadenaNombre = "";
                tipoElemento t = tipoElemento.noValido;

                char num = elem[elem.Length - 1]; //se busca la valencia usada
                if (char.IsDigit(num))
                {
                    cantAtomos = int.Parse(num.ToString());
                    cadenaSimbolo = elem.Substring(0, elem.Length - 1);
                }

                bool swEsElemVálido = false;
                if (cadenaSimbolo == "O") //si es oxígeno
                {
                    cadenaNombre = "Oxígeno";
                    swEsElemVálido = true;
                    t = tipoElemento.oxigeno;
                }
                else
                {
                    bool swEsMetal = false;
                    foreach (Elemento e in metales) //se chequea si es metal válido
                    {
                        if (e.Simbolo == cadenaSimbolo)
                        {
                            cadenaNombre = e.Nombre;
                            swEsElemVálido = true;
                            t = tipoElemento.metal;
                            swEsMetal = true;
                            break;
                        }
                    }

                    if (!swEsMetal) //si no es metal, vemos si es no metal
                    {
                        swEsElemVálido = false;
                        foreach (Elemento e in nometales) //se chequea si es no metal válido
                        {
                            if (e.Simbolo == cadenaSimbolo)
                            {
                                cadenaNombre = e.Nombre;
                                swEsElemVálido = true;
                                t = tipoElemento.nometal;
                                break;
                            }
                        }
                    }
                }

                ElementoEnUso el = new ElementoEnUso(cadenaNombre, cadenaSimbolo, cantAtomos, swEsElemVálido);
                el.TipodeAtomo = t; //se le carga la propiedad si es metal o no metal
                elementos.Add(el);
            }


            return elementos;
        }

        private string deducirNombreMolecula (List<ElementoEnUso> elementosIntervienen)
        {
            string strNombreFormula = "";


            return strNombreFormula;
        }

        private string ordenarCadenaMolécula (List<ElementoEnUso> elementosDeLaFormula)
        {
            string cadenaOrdenada = "";
            foreach (ElementoEnUso e in elementosDeLaFormula)
            {
                //if (e.SwEsValido && (e.TipodeAtomo == tipoElemento.metal || e.TipodeAtomo== tipoElemento.nometal))
                //cadenaOrdenada = e.Nombre + e.CantAtomos.ToString() + cadenaOrdenada;

                if (e.SwEsValido && e.TipodeAtomo == tipoElemento.oxigeno)
                    cadenaOrdenada += e.Simbolo + e.CantAtomos.ToString();
                else
                    cadenaOrdenada = e.Simbolo + e.CantAtomos.ToString() + cadenaOrdenada;
            }

            return cadenaOrdenada;
        }

        //función para deducir tipo de fómula (más adelante)

        public Molecula (string cadenaFormula, tipoMolécula tipoMolecula) //si se hace la función deducir tipo de formula este segundo parámetro no hace falta
        {
            elementosMolécula = separarElmentos(cadenaFormula);
            tipo = tipoMolecula;
            nombre = deducirNombreMolecula(elementosMolécula);
            cadenaMoléculaOrdenada = ordenarCadenaMolécula(elementosMolécula);
            //Voz.hablar("hola");
        }

        

    }
}
