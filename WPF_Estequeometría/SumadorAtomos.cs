using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometría
{
    class SumadorAtomos
    {
        private ElementoEnUso ele;
        private ElementoEnUso oxigeno = new ElementoEnUso("Oxígeno", "O", 2, true);
        List<int> primos = new List<int>();
        public string registroSimplificación = "";
        public Molecula molOriginal; //sin la simplificaci{on
        public Molecula molFinal; //con la simplificación
        public bool swTieneQSimplificar = false;

        public SumadorAtomos(ElementoEnUso e)
        {
            ele = e;
            primos.Add(2);
            primos.Add(3);
            primos.Add(5);
            primos.Add(7);
            primos.Add(11);

            molOriginal = new Molecula(ele.Simbolo + oxigeno.CantAtomos + oxigeno.Simbolo + ele.CantAtomos);
            molFinal = simplificarMolecula(molOriginal);

            registroSimplificación = cargarRegistroAccionesSimplificación();
        }

        public SumadorAtomos()
        {
            
        }

        private string cargarRegistroAccionesSimplificación ()
        {
            string cadena = "";
            int cont = 0;

            cadena += "Al unir el elemento " + ele.Nombre + " con valencia " + ele.CantAtomos + " con el Oxígeno, que siempre tiene valencia 2, nos da como resultado la molécula " + 
                molOriginal.CadenaMolécula + ". Como";
            foreach (ElementoEnUso e in molOriginal.ElementosMolécula) //se carga en la cadena cada átomo
            {

                cadena += " el elemento " + e.Nombre + " tiene " + e.CantAtomos +
                    " átomos ";
                cont++;
                if (cont != molOriginal.ElementosMolécula.Count) //si no es el último elemento de la lista se agrega el y
                    cadena += " y ";
            }

            if (!molOriginal.CadenaMolécula.Equals(molFinal.CadenaMolécula)) //si hubo simplificación
            {
                cadena += " se pueden simplificar entre sí. Por lo que la fórmula entonces queda ";
                swTieneQSimplificar = true;
            }
            else
            {
                cadena += " no se pueden simplificar entre sí. Por lo que la fórmula entonces queda sin simplificar, igual que antes, osea ";
                swTieneQSimplificar = false;
            }

            cadena += molFinal.CadenaMolécula; //se añade al final la molécula completa

            return cadena;
        }

        private Molecula simplificarMolecula (Molecula m)
        {
            int cantAtomos = m.ElementosMolécula.Count;
            List<int> listaCantAtomosOriginales = new List<int>();
            List<int> listaCantAtomosSimplificados = new List<int>();
            int cont = 0;

            foreach (ElementoEnUso e in m.ElementosMolécula)
                listaCantAtomosOriginales.Add(e.CantAtomos);//se carga la lista con las cantidades de átomos

            listaCantAtomosSimplificados = simplificarLista(listaCantAtomosOriginales);
            
            string auxCadenaMolecula = "";
            cont = 0;
            foreach (ElementoEnUso e in m.ElementosMolécula)
            {
                auxCadenaMolecula += e.Simbolo + listaCantAtomosSimplificados[cont]; 
                cont++;
            }

            return new Molecula(auxCadenaMolecula);
        }

        private bool esMultiplo (int numero, int multiplo)
        {
            if (numero % multiplo == 0)
                return true;
            else
                return false;
        }

        private List<int> simplificarLista (List<int>listaParaSimplificar)
        {
            bool swSonTodosDivisibles = false;
            List<int> listaSimplificada = new List<int>();
            
            foreach (int i in listaParaSimplificar) //copiamos una lista en la otra
                listaSimplificada.Add(i);

            foreach (int i in primos)
            {

                //for (int i = 0; i <= cantAtomos; i++)
                foreach (int num in listaParaSimplificar)
                {
                    if (esMultiplo(num, i))
                        swSonTodosDivisibles = true;
                    else
                    {
                        swSonTodosDivisibles = false;
                        break;
                    }
                    //}
                }

                
                if (swSonTodosDivisibles)
                {
                    listaSimplificada.Clear();
                    foreach (int num in listaParaSimplificar)
                        listaSimplificada.Add(num / i);

                    simplificarLista(listaSimplificada);// primoDivisor);
                    break;
                }
            }

            return listaSimplificada;
        }

        
    }
}
