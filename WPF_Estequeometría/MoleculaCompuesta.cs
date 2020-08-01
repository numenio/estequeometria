using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometria
{
    class MoleculaCompuesta //para hidróxidos y sales. El grupo oxidrilo y el grupo ácido tienen que ser moléculas inseparables
    {
        public List<Molecula> elementosMolécula = new List<Molecula>();
        public tipoMolécula tipo = tipoMolécula.noValida;
        public string nombre = "";
        public string cadenaMoléculaOrdenada = "";
        public int CantidadMolécula = 1;

        public MoleculaCompuesta (string cadenaFormula, tipoMolécula t)
        {
            tipo = t;
            elementosMolécula = separarElmentos(cadenaFormula, t);
            //nombre = deducirNombreMolecula(elementosMolécula);
            cadenaMoléculaOrdenada = ordenarCadenaMolécula(elementosMolécula);
        }

        private List<Molecula> separarElmentos(string cadenaFormula, tipoMolécula t)
        {
            
            List<Molecula> elementos = new List<Molecula>();

            if (t == tipoMolécula.hidroxido)
                elementos = separarHidróxido(cadenaFormula);

            //if (t == tipoMolécula.sal)
            //    elementos = separarSal(cadenaFormula);

            return elementos;
        }

        private List<Molecula> separarHidróxido (string cadenaFormula)
        {
            string auxcadena = "";
            string auxCantMol = "";
            List<string> auxElem = new List<string>();
            int auxPos = 0;
            bool swPrimerLetra = true;

            int lugarParéntesis = cadenaFormula.IndexOf('(');
            List<Molecula> elementos = new List<Molecula>();

            if (lugarParéntesis == 0) //si no hay paréntesis, no es hidróxido válido
            {
                tipo = tipoMolécula.noValida;
                return elementos;
            }

            
            cadenaFormula = new ValidadorCadenas().quitarEspaciosEncadena(cadenaFormula);
            string cadenaOxidrilo = cadenaFormula.Substring(lugarParéntesis, 4);

            if (!(cadenaOxidrilo == "(OH)")) //si no está el grupo oxidrilo, no es hidróxido válido
            {
                tipo = tipoMolécula.noValida;
                return elementos;
            }

            string auxformulaDespuesOxidrilo = cadenaFormula.Substring(lugarParéntesis + "(OH)".Length, cadenaFormula.Length - (lugarParéntesis + "(OH)".Length));
            string auxCantOxidrilo = "";
            foreach (char c in auxformulaDespuesOxidrilo) //se busca las valencias del oxidrilo si las hay
            {
                if (char.IsDigit(c))
                    auxCantOxidrilo += c;
                else
                    break;
            }


            elementos.Add(new Molecula(auxCantOxidrilo + cadenaOxidrilo, tipoMolécula.oxidrilo)); //se añade el oxidrilo a los elementos del hidróxido
            //------se quita el oxidrilo de la cadena------------
            auxcadena = cadenaFormula.Substring(0, lugarParéntesis); //desde el commienzo hasta el paréntesis
            auxcadena += cadenaFormula.Substring(lugarParéntesis + "(OH)".Length, cadenaFormula.Length- (lugarParéntesis + "(OH)".Length)-auxCantOxidrilo.Length); // desde el final del oxidrilo al final de la fórmmula
            cadenaFormula = auxcadena;
            auxcadena = "";
            //----------------------------------------------------

            foreach (char c in cadenaFormula)
            {
                auxPos++;
                if (new ValidadorCadenas().esCaracterValido(c, false, tipoMolécula.hidroxido))
                {

                    if (char.IsUpper(c) && !swPrimerLetra)
                    {
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

                    if (auxPos == cadenaFormula.Trim().Length)
                        auxElem.Add(auxcadena);

                    if (!swPrimerLetra) //si se alcanza la primer letra
                        if (auxCantMol != "")
                            CantidadMolécula = int.Parse(auxCantMol);

                }
            }


            //se cargan los elementos - hacer: ver si no hace falta comprobar que sean elementos válidos. 
            //Quizás no haga falta porque de eso se encarga el BuscadorErrores
            
            foreach (String elem in auxElem)
                elementos.Add(new Molecula(elem, tipoMolécula.parteHidroxido));

            return elementos; //
        }

        //private List<Molecula> separarSal(string cadenaFormula)
        //{
        //    string auxcadena = "";
        //    string auxCantMol = "";
        //    List<string> auxElem = new List<string>();
        //    int auxPos = 0;
        //    bool swPrimerLetra = true;

        //    foreach (char c in cadenaFormula.Trim())
        //    {
        //        auxPos++;
        //        if (new ValidadorCadenas().esCaracterValido(c, false, t))
        //        {

        //            if (char.IsUpper(c) && !swPrimerLetra)
        //            {
        //                //break;
        //                auxElem.Add(auxcadena);
        //                auxcadena = "";
        //            }
        //            if (swPrimerLetra && char.IsDigit(c))
        //            {
        //                auxCantMol += c;
        //            }
        //            else
        //                auxcadena += c;

        //            if (swPrimerLetra && !char.IsDigit(c) && !char.IsWhiteSpace(c))
        //                swPrimerLetra = false;

        //            if (auxPos == cadenaFormula.Trim().Length)
        //                auxElem.Add(auxcadena);

        //            if (!swPrimerLetra) //si se alcanza la primer letra
        //                if (auxCantMol != "")
        //                    cantidadMolecula = int.Parse(auxCantMol);

        //        }
        //    }


        //    //chequear que sean elementos válidos los que se encontraron arriba
        //    List<ElementoEnUso> elementos = new List<ElementoEnUso>();
        //    foreach (String elem in auxElem)
        //    {
        //        int cantAtomos = 1;
        //        //int cantMol = 1;
        //        string cadenaSimbolo = elem;
        //        string cadenaNombre = "";
        //        tipoElemento tipoEl = tipoElemento.noValido;
        //        //char num;
        //        int cont = 0;

        //        foreach (char caract in elem)
        //            if (char.IsDigit(caract))
        //                cont++;

        //        //num = elem.Substring(0, elem.Length - cont); //se busca la valencia usada
        //        if (cont > 0)
        //        {
        //            cantAtomos = int.Parse(elem.Substring(elem.Length - cont, cont)); //int.Parse(num.ToString());
        //            cadenaSimbolo = elem.Substring(0, elem.Length - cont);
        //        }

        //        bool swEsElemVálido = false;

        //        if (cadenaSimbolo == "H") //si es oxígeno
        //        {
        //            cadenaNombre = "Hidrógeno";
        //            swEsElemVálido = true;
        //            tipoEl = tipoElemento.hidrogeno;
        //        }

        //        if (cadenaSimbolo == "O") //si es oxígeno
        //        {
        //            cadenaNombre = "Oxígeno";
        //            swEsElemVálido = true;
        //            tipoEl = tipoElemento.oxigeno;
        //        }
        //        else
        //        {
        //            bool swEsMetal = false;
        //            foreach (Elemento e in new ListaMetales().Metales) //se chequea si es metal válido
        //            {
        //                if (e.Simbolo == cadenaSimbolo)
        //                {
        //                    cadenaNombre = e.Nombre;
        //                    swEsElemVálido = true;
        //                    tipoEl = tipoElemento.metal;
        //                    swEsMetal = true;
        //                    break;
        //                }
        //            }

        //            if (!swEsMetal) //si no es metal, vemos si es no metal
        //            {
        //                swEsElemVálido = false;
        //                foreach (Elemento e in new ListaNoMetales().Nometales) //se chequea si es no metal válido
        //                {
        //                    if (e.Simbolo == cadenaSimbolo)
        //                    {
        //                        cadenaNombre = e.Nombre;
        //                        swEsElemVálido = true;
        //                        tipoEl = tipoElemento.nometal;
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        if (cantAtomos == 0) cantAtomos = 1;

        //        ElementoEnUso el = new ElementoEnUso(cadenaNombre, cadenaSimbolo, cantAtomos, swEsElemVálido);
        //        el.TipodeAtomo = tipoEl; //se le carga la propiedad si es metal o no metal
        //        elementos.Add(el);


        //    }
        //    return new List<Molecula>(); //hacer
        //}


        //private string deducirNombreMolecula(List<Molecula> elementosIntervienen)
        //{
        //    string strNombreFormula = "";
        //    switch (elementosIntervienen.Count)
        //    {
        //        case 0:
        //            return "Error, no hay átomos en la molécula";
        //        case 2:
        //            foreach (ElementoEnUso e in elementosIntervienen)
        //            {
        //                if (e.TipodeAtomo == tipoElemento.metal)
        //                {
        //                    // <---------- hacer función que nombre bien cada compuesto según átomo y valencia
        //                    tipo = tipoMolécula.oxido;
        //                    return "óxido de " + e.Nombre;
        //                }

        //                if (e.TipodeAtomo == tipoElemento.nometal)
        //                {
        //                    // <---------- hacer función que nombre bien cada compuesto según átomo y valencia
        //                    tipo = tipoMolécula.anhidrido;
        //                    return "anhidrido de " + e.Nombre;
        //                }
        //            }
        //            break;
        //        case 3:
        //            //hacer nombre cuando son ácidos o anhidridos
        //            break;
        //        default:
        //            return "Error. Por ahora este programa sólo puede trabajar máximo con tres átomos a la vez";
        //    }

        //    return strNombreFormula;
        //}

        private string ordenarCadenaMolécula(List<Molecula> elementosDeLaFormula)
        {
            string cadenaOrdenada = "";
            foreach (Molecula m in elementosDeLaFormula)
            {
                

                if (m.Tipo != tipoMolécula.oxidrilo) //si no es oxidrilo, osea el elemento, va primero 
                {
                    if (m.CantidadMolécula > 1)
                        cadenaOrdenada = m.CantidadMolécula + cadenaOrdenada;

                    string aux = "";
                    foreach (ElementoEnUso e in m.ElementosMolécula)
                    {
                        aux = e.Simbolo;
                        if (e.CantAtomos > 1) aux += e.CantAtomos;
                        
                        cadenaOrdenada = aux + cadenaOrdenada;
                    }
                }
                else
                {
                    cadenaOrdenada += "(";

                    string aux = "";
                    foreach (ElementoEnUso e in m.ElementosMolécula) //si es el oxidrilo, va luego de los elementos
                    {
                        aux = e.Simbolo;
                        if (e.CantAtomos > 1) aux += e.CantAtomos;

                        cadenaOrdenada += aux;
                    }

                    cadenaOrdenada += ")";

                    if (m.CantidadMolécula > 1) //la cantidad del elemento al final
                        cadenaOrdenada += m.CantidadMolécula;
                }
            }

            return cadenaOrdenada;
        }


        public bool esCadenaDeMoleculaValida(string cadenaMolecula) //refactorizar
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
                            CantidadMolécula = int.Parse(c.ToString());
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
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
