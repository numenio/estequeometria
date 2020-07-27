using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WPF_Estequeometría
{
    class ValidadorCadenas
    {
        public bool esCaracterValido(char c, bool swIncluirEspacios)
        {
            string caracteresValidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+=";
            if (swIncluirEspacios)
                caracteresValidos += " ";
            
            if (caracteresValidos.Contains(c))
                return true;
            else
                return false;
            
        }

        public bool esCadenaValida(string cadenaValida, bool swIncluirEspacios)
        {
            //bool swEsCadenaValida = true;

            foreach (char c in cadenaValida) //se quitan todos los caracteres que no sean válidos
                if (!new ValidadorCadenas().esCaracterValido(c, swIncluirEspacios))
                    return false;

            return true;
        }

        public string validarCadena (string cadenaAvalidar, bool swIncluirEspacios)
        {
            string auxcadenaValidada = "";

            foreach (char c in cadenaAvalidar) //se quitan todos los caracteres que no sean válidos
                if (new ValidadorCadenas().esCaracterValido(c, swIncluirEspacios))
                    auxcadenaValidada += c;

            return auxcadenaValidada;
        }

        public string quitarEspaciosEncadena(string cadena)
        {
            string auxcadenaValidada = "";

            foreach (char c in cadena) //se quitan todos los caracteres que no sean válidos
                if (!char.IsWhiteSpace(c))
                    auxcadenaValidada += c;

            return auxcadenaValidada;
        }

        public string OrdenarCadena(string cadenaParaOrdenar)
        {
            ////generate a Random instance
            //var rnd = new Random();
            ////get the count of items in the list
            //var i = list.Count();
            ////do we have a reference type or a value type
            //T val = default(T);

            ////we will loop through the list backwards
            //while (i >= 1)
            //{
            //    //decrement our counter
            //    i--;
            //    //grab the next random item from the list
            //    var nextIndex = rnd.Next(i, list.Count());
            //    val = list[nextIndex];
            //    //start swapping values
            //    list[nextIndex] = list[i];
            //    list[i] = val;
            //}

            return String.Concat(cadenaParaOrdenar.OrderBy(c => c));
        }

        public string tipoMoleculaTraducida (tipoMolécula t)
        {
            string cadena = "";
            switch (t)
            {
                case tipoMolécula.oxido:
                    cadena = "óxido o anhidrido";
                    break;
                case tipoMolécula.anhidrido:
                    cadena = "óxido o anhidrido";
                    break;
                case tipoMolécula.acido:
                    cadena = "ácido";
                    break;
                case tipoMolécula.hidroxido:
                    cadena = "hidróxido";
                    break;
                case tipoMolécula.sal:
                    break;
                case tipoMolécula.noValida:
                    break;
            }
            return cadena;
        }

        public bool EsDiatomico(ElementoEnUso el)
        {
            
            switch (el.Simbolo)
            {
                case "S":
                case "O":
                case "I":
                case "Cl":
                case "Br":
                case "F":
                case "N":
                    return true;
                default:
                    return false;
            }
        }

        public string separarCadenaconEspacios (string cadenaParaSeparar)
        {
            string aux = "";

            foreach (char c in cadenaParaSeparar)
                aux += c.ToString() + ' ';

            return traducirCadenaParaLeer(aux, false);
        }

        public string traducirCadenaParaLeer (string cadenaParaTraducir, bool swLeerEspacio)
        {
            string aux = "";
            foreach (char c in cadenaParaTraducir)
            {
                if (!char.IsWhiteSpace(c))
                    aux += traducirCaracterParaLeer(c);
                else
                {
                    if (swLeerEspacio)
                        aux += traducirCaracterParaLeer(c);
                    else
                        aux += ' ';
                }

                if (esEnMayúscula(c))
                    aux += " mayúsculas ";
            }

            return aux;
        }


        private string traducirCaracterParaLeer (char c)
        {
            string aux = c.ToString();
            switch (c)
            {
                case '´':
                    aux = "acento";
                    break;
                case ' ':
                    aux = "espacio";
                    break;
                case '{':
                    aux = "abre llave";
                    break;
                case '}':
                    aux = "ciera llave";
                    break;
                case '¨':
                    aux = "diéresis";
                    break;
                case '[':
                    aux = "abre corchetes";
                    break;
                case ']':
                    aux = "cierra corchetes";
                    break;
                case '*':
                    aux = "asterisco";
                    break;
                case ',':
                    aux = "coma";
                    break;
                case '.':
                    aux = "punto";
                    break;
                case '-':
                    aux = "guión";
                    break;
                case '^':
                    aux = "acentro circunflejo";
                    break;
                case 'ç':
                    aux = "ce cerillas";
                    break;
                case '`':
                    aux = "acentro grave";
                    break;
                case ';':
                    aux = "punto y coma";
                    break;
                case ':':
                    aux = "dos puntos";
                    break;
                case '_':
                    aux = "guión bajo";
                    break;
                case 'º':
                    aux = "grados";
                    break;
                case 'ª':
                    aux = "a volada";
                    break;
                case '!':
                    aux = "cierra admiración";
                    break;
                case '¿':
                    aux = "abre pregunta";
                    break;
                case '?':
                    aux = "cirra pregunta";
                    break;
                case '|':
                    aux = "bara vertical";
                    break;
                case '¬':
                    aux = "signo especial";
                    break;
                case '\'':
                    aux = "comilla simple";
                    break;
                case '"':
                    aux = "comillas";
                    break;
                case '¡':
                    aux = "abre admiración";
                    break;
                case '\\':
                    aux = "barra inclinada a la izquierda";
                    break;
                case '~':
                    aux = "tilde";
                    break;
                case '$':
                    aux = "pesos";
                    break;
                case '%':
                    aux = "porcentaje";
                    break;
                case '#':
                    aux = "numeral";
                    break;
                case '&':
                    aux = "unión";
                    break;
                case '/':
                    aux = "barra inclinada";
                    break;
                case '@':
                    aux = "arroba";
                    break;
                case 'a':
                case 'A':
                    aux = "a";
                    break;
                case 'b':
                case 'B':
                    aux = "be";
                    break;
                case 'c':
                case 'C':
                    aux = "se";
                    break;
                case 'd':
                case 'D':
                    aux = "de";
                    break;
                case 'e':
                case 'E':
                    aux = "e";
                    break;
                case 'f':
                case 'F':
                    aux = "efe";
                    break;
                case 'g':
                case 'G':
                    aux = "je";
                    break;
                case 'h':
                case 'H':
                    aux = "ache";
                    break;
                case 'i':
                case 'I':
                    aux = "i";
                    break;
                case 'j':
                case 'J':
                    aux = "jota";
                    break;
                case 'k':
                case 'K':
                    aux = "ca";
                    break;
                case 'l':
                case 'L':
                    aux = "ele";
                    break;
                case 'm':
                case 'M':
                    aux = "eme";
                    break;
                case 'n':
                case 'N':
                    aux = "ene";
                    break;
                case 'ñ':
                case 'Ñ':
                    aux = "eñe";
                    break;
                case 'o':
                case 'O':
                    aux = "o";
                    break;
                case 'p':
                case 'P':
                    aux = "pe";
                    break;
                case 'q':
                case 'Q':
                    aux = "cu";
                    break;
                case 'r':
                case 'R':
                    aux = "erre";
                    break;
                case 's':
                case 'S':
                    aux = "ese";
                    break;
                case 't':
                case 'T':
                    aux = "te";
                    break;
                case 'u':
                case 'U':
                    aux = "u";
                    break;
                case 'v':
                case 'V':
                    aux = "be corta";
                    break;
                case 'w':
                case 'W':
                    aux = "doble be";
                    break;
                case 'x':
                case 'X':
                    aux = "equis";
                    break;
                case 'y':
                case 'Y':
                    aux = "i griega";
                    break;
                case 'z':
                case 'Z':
                    aux = "seta";
                    break;
            }



            return aux;
        }

        private bool esEnMayúscula(char c)
        {
            string aux = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
            if (aux.Contains(c))
                return true;

            return false;
        }
    }
}
