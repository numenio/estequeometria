using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometría
{
    class ComprobadorMoleculas
    {
        Molecula moleculaEnviada;
        Molecula moleculaResueltaEnApp;
        SumadorAtomos sumador;
        public bool swMoleculaBienResulta = false;
        public bool swHayQueSimplificar = false;
        public string cadenaSimplificacion = "";
        //public string cadenaExplicaciónError = "";

        public ComprobadorMoleculas(Molecula m, ElementoEnUso e, tipoMolécula t)
        {
            moleculaEnviada = m;
            

            switch (t)
            {
                case tipoMolécula.oxido: //ya sea óxido o anhidrido se hace lo mismo
                case tipoMolécula.anhidrido:
                    sumador = new SumadorAtomos(e);
                    swHayQueSimplificar = sumador.swTieneQSimplificar;
                    cadenaSimplificacion = sumador.registroSimplificación;
                    moleculaResueltaEnApp = sumador.molFinal;
                     if (moleculaEnviada.CadenaMolécula == moleculaResueltaEnApp.CadenaMolécula)
                        swMoleculaBienResulta = true;
                    else
                    {
                        //cadenaExplicaciónError = new BuscadorErroresEnFormula(moleculaEnviada, moleculaResueltaEnApp)
                        swMoleculaBienResulta = false;
                    }
                            
                    break;
                case tipoMolécula.acido: //aquí iría el código para sumar comprobar ácidos
                    break;
            }
        }
    }
}
