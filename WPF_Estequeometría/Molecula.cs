using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace WPF_Estequeometria
{
    public enum tipoMolécula { oxido, anhidrido, acido, hidroxido, sal, agua, oxidrilo, parteHidroxido, noValida };
    class Molecula
    {
        List<ElementoEnUso> elementosMolécula = new List<ElementoEnUso>();
        tipoMolécula tipo;
        string nombre = "";
        string cadenaMoléculaOrdenada = "";
        int cantidadMolecula = 1;

        public List<ElementoEnUso> ElementosMolécula { get => elementosMolécula; }
        public tipoMolécula Tipo { get => tipo; }
        public string Nombre { get => nombre; }
        public string CadenaMolécula { get => cadenaMoléculaOrdenada; }
        public int CantidadMolécula { get => cantidadMolecula; }




        public Molecula(string cadenaFormula, tipoMolécula t)
        {
            tipo = t;
            elementosMolécula = separarElmentos(cadenaFormula, t);
            nombre = deducirNombreMolecula(elementosMolécula);
            cadenaMoléculaOrdenada = ordenarCadenaMolécula(elementosMolécula);
        }

        public Molecula(tipoMolécula t)
        {
            tipo = t;
        }

        private List<ElementoEnUso> separarElmentos(string cadenaFormula, tipoMolécula t)
        {
            string auxcadena = "";
            string auxCantMol = "";
            List<string> auxElem = new List<string>();
            int auxPos = 0;
            bool swPrimerLetra = true;
            foreach (char c in cadenaFormula.Trim())
            {
                auxPos++;
                if (new ValidadorCadenas().esCaracterValido(c, false, t))
                {

                    if (char.IsUpper(c) && !swPrimerLetra)
                    {
                        //break;
                        auxElem.Add(auxcadena);
                        auxcadena = "";
                    }
                    if (swPrimerLetra && char.IsDigit(c))
                    {
                        auxCantMol += c;
                    }
                    else
                        auxcadena += c;

                    if (swPrimerLetra && !char.IsDigit(c) && !char.IsWhiteSpace(c))
                        swPrimerLetra = false;

                    if (auxPos == cadenaFormula.Trim().Length || (t == tipoMolécula.oxidrilo && auxPos == cadenaFormula.Trim().Length - 1)) //si no es un oxidrilo se añade cuando es el último elemento, si es oxidrilo, se añade cuando sólo falta el paréntesis
                        auxElem.Add(auxcadena);

                    if (!swPrimerLetra) //si se alcanza la primer letra
                        if (auxCantMol != "")
                            cantidadMolecula = int.Parse(auxCantMol);

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
                tipoElemento tipoEl = tipoElemento.noValido;
                //char num;
                int cont = 0;

                

                foreach (char caract in elem)
                    if (char.IsDigit(caract))
                        cont++;

                //num = elem.Substring(0, elem.Length - cont); //se busca la valencia usada
                if (cont > 0)
                {
                    cantAtomos = int.Parse(elem.Substring(elem.Length - cont, cont)); //int.Parse(num.ToString());
                    cadenaSimbolo = elem.Substring(0, elem.Length - cont);
                }

                bool swEsElemVálido = false;

                if (cadenaSimbolo == "H") //si es oxígeno
                {
                    cadenaNombre = "Hidrógeno";
                    swEsElemVálido = true;
                    tipoEl = tipoElemento.hidrogeno;
                }

                if (cadenaSimbolo == "O") //si es oxígeno
                {
                    cadenaNombre = "Oxígeno";
                    swEsElemVálido = true;
                    tipoEl = tipoElemento.oxigeno;
                }
                else
                {
                    bool swEsMetal = false;
                    foreach (Elemento e in new ListaMetales().Metales) //se chequea si es metal válido
                    {
                        if (e.Simbolo == cadenaSimbolo)
                        {
                            cadenaNombre = e.Nombre;
                            swEsElemVálido = true;
                            tipoEl = tipoElemento.metal;
                            swEsMetal = true;
                            break;
                        }
                    }

                    if (!swEsMetal) //si no es metal, vemos si es no metal
                    {
                        swEsElemVálido = false;
                        foreach (Elemento e in new ListaNoMetales().Nometales) //se chequea si es no metal válido
                        {
                            if (e.Simbolo == cadenaSimbolo)
                            {
                                cadenaNombre = e.Nombre;
                                swEsElemVálido = true;
                                tipoEl = tipoElemento.nometal;
                                break;
                            }
                        }
                    }
                }

                if (cantAtomos == 0) cantAtomos = 1;

                ElementoEnUso el = new ElementoEnUso(cadenaNombre, cadenaSimbolo, cantAtomos, swEsElemVálido);
                el.TipodeAtomo = tipoEl; //se le carga la propiedad si es metal o no metal
                elementos.Add(el);
            }


            return elementos;
        }

        private string deducirNombreMolecula(List<ElementoEnUso> elementosIntervienen)
        {
            string strNombreFormula = "";
            switch (elementosIntervienen.Count)
            {
                case 0:
                    return "Error, no hay átomos en la molécula";
                case 2:
                    foreach (ElementoEnUso e in elementosIntervienen)
                    {
                        if (e.TipodeAtomo == tipoElemento.metal)
                        {
                            // <---------- hacer función que nombre bien cada compuesto según átomo y valencia
                            //tipo = tipoMolécula.oxido;
                            return "óxido de " + e.Nombre;
                        }

                        if (e.TipodeAtomo == tipoElemento.nometal)
                        {
                            // <---------- hacer función que nombre bien cada compuesto según átomo y valencia
                            //tipo = tipoMolécula.anhidrido;
                            return "anhidrido de " + e.Nombre;
                        }
                    }
                    break;
                case 3:
                    //hacer nombre cuando son ácidos o anhidridos
                    break;
                default:
                    return "Error. Por ahora este programa sólo puede trabajar máximo con tres átomos a la vez";
            }

            return strNombreFormula;
        }

        private string ordenarCadenaMolécula(List<ElementoEnUso> elementosDeLaFormula)
        {
            string cadenaOrdenada = "";
            foreach (ElementoEnUso e in elementosDeLaFormula)
            {
                if (e.TipodeAtomo == tipoElemento.oxigeno)
                {
                    if (tipo != tipoMolécula.oxidrilo)
                    {
                        if (e.CantAtomos > 1)
                            cadenaOrdenada += e.Simbolo + e.CantAtomos.ToString();
                        else
                            cadenaOrdenada += e.Simbolo;
                    }
                    else
                    {
                        if (e.CantAtomos > 1)
                            cadenaOrdenada = e.Simbolo + e.CantAtomos.ToString() + cadenaOrdenada; //en oxidrilos que quede OH
                        else
                            cadenaOrdenada = e.Simbolo + cadenaOrdenada;
                    }
                }
                else if (e.TipodeAtomo == tipoElemento.hidrogeno)
                {
                    if (tipo != tipoMolécula.oxidrilo)
                    {
                        if (e.CantAtomos > 1)
                            cadenaOrdenada = e.Simbolo + e.CantAtomos.ToString() + cadenaOrdenada;
                        else
                            cadenaOrdenada = e.Simbolo + cadenaOrdenada;
                    }
                    else
                    {
                        if (e.CantAtomos > 1)
                            cadenaOrdenada += e.Simbolo + e.CantAtomos.ToString(); //en oxidrilos el H al final
                        else
                            cadenaOrdenada += e.Simbolo;
                    }
                }
                else
                {
                    if (e.CantAtomos > 1)
                        cadenaOrdenada = e.Simbolo + e.CantAtomos.ToString() + cadenaOrdenada;
                    else
                        cadenaOrdenada = e.Simbolo + cadenaOrdenada;

                }
            }

            return cadenaOrdenada;
        }


        public bool esCadenaDeMoleculaValida(string cadenaMolecula, tipoMolécula tipoMol) //refactorizar
        {
            try
            {
                string auxcadena = "";
                List<string> auxElem = new List<string>();
                int auxPos = 0;
                bool swPrimerLetra = true;
                foreach (char c in cadenaMolecula.Trim())
                {
                    auxPos++;
                    if (new ValidadorCadenas().esCaracterValido(c, false, tipo))
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

                        if (auxPos == cadenaMolecula.Trim().Length)
                            auxElem.Add(auxcadena);

                    }
                    else
                        return false;
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
                    //char num;
                    int cont = 0;

                    foreach (char caract in elem)
                        if (char.IsDigit(caract))
                            cont++;

                    //num = elem.Substring(0, elem.Length - cont); //se busca la valencia usada
                    if (cont > 0)
                    {
                        cantAtomos = int.Parse(elem.Substring(elem.Length - cont, cont)); //int.Parse(num.ToString());
                        cadenaSimbolo = elem.Substring(0, elem.Length - cont);
                    }

                    bool swEsElemVálido = false;
                    if (cadenaSimbolo == "H") //si es oxígeno
                    {
                        cadenaNombre = "Hidrógeno";
                        swEsElemVálido = true;
                        t = tipoElemento.hidrogeno;
                    }

                    if (cadenaSimbolo == "O") //si es oxígeno
                    {
                        cadenaNombre = "Oxígeno";
                        swEsElemVálido = true;
                        t = tipoElemento.oxigeno;
                    }
                    else
                    {
                        bool swEsMetal = false;
                        foreach (Elemento e in new ListaMetales().Metales) //se chequea si es metal válido
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
                            foreach (Elemento e in new ListaNoMetales().Nometales) //se chequea si es no metal válido
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

                if (tipoMol == tipoMolécula.hidroxido)
                {
                    //int lugarPrimerParentesis = cadenaMolecula.IndexOf('(');
                    //int lugarSegundoParentesis = cadenaMolecula.IndexOf(')');

                    //if (lugarPrimerParentesis < 0 || lugarSegundoParentesis < 0) return false; //si no hay paréntesis en hidróxido, no es válida

                    if (cadenaMolecula.IndexOf("OH") < 0) return false; //si no hay oxidrilo, no es hidróxido válido
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
