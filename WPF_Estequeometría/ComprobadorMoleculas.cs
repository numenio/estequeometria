using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometria
{
    class ComprobadorMoleculas
    {
        Molecula moleculaEnviada;
        Molecula moleculaResueltaEnApp;
        MoleculaCompuesta moleculaCompuestaEnviada;
        MoleculaCompuesta moleculaCompuestaResueltaEnApp;
        SumadorAtomos sumador;
        tipoMolécula tipoMolParaHacer;
        public bool swMoleculaBienResulta = false;
        public bool swHayQueSimplificar = false;
        public string cadenaSimplificacion = "";
        //public string cadenaExplicaciónError = "";

        public ComprobadorMoleculas(Molecula m, ElementoEnUso e, tipoMolécula t)
        {
            moleculaEnviada = m;
            tipoMolParaHacer = t;

            sumador = new SumadorAtomos(e, tipoMolParaHacer);
            swHayQueSimplificar = sumador.swTieneQSimplificar;
            cadenaSimplificacion = sumador.registroSimplificación;
            moleculaResueltaEnApp = sumador.molFinal;

            switch (t)
            {
                case tipoMolécula.oxido: //ya sea óxido o anhidrido se hace lo mismo
                case tipoMolécula.anhidrido:
                    
                     if (moleculaEnviada.CadenaMolécula == moleculaResueltaEnApp.CadenaMolécula)
                        swMoleculaBienResulta = true;
                    else
                    {
                        //cadenaExplicaciónError = new BuscadorErroresEnFormula(moleculaEnviada, moleculaResueltaEnApp)
                        swMoleculaBienResulta = false;
                    }
                            
                    break;
                case tipoMolécula.acido: //aquí iría el código para sumar comprobar ácidos
                    swMoleculaBienResulta = comprobarAcidos(m, moleculaResueltaEnApp);
                    break;
            }
        }

        public ComprobadorMoleculas(MoleculaCompuesta m, ElementoEnUso e, tipoMolécula t) //hacer: revisar todo este constructor
        {
            moleculaCompuestaEnviada = m;
            tipoMolParaHacer = t;

            sumador = new SumadorAtomos(e, tipoMolParaHacer);
            
            moleculaCompuestaResueltaEnApp = sumador.molFinalCompuesta;

            switch (t)
            {
                case tipoMolécula.hidroxido:
                    if (moleculaCompuestaEnviada.cadenaMoléculaOrdenada == moleculaCompuestaResueltaEnApp.cadenaMoléculaOrdenada)
                        swMoleculaBienResulta = true;
                    else
                    {
                        //cadenaExplicaciónError = new BuscadorErroresEnFormula(moleculaEnviada, moleculaResueltaEnApp)
                        swMoleculaBienResulta = false;
                    }

                    break;
                case tipoMolécula.sal:
                    //swMoleculaBienResulta = comprobarAcidos(m, moleculaResueltaEnApp);
                    break;
            }
        }

        private bool comprobarAcidos(Molecula mEnviadaPorUsuario, Molecula mResultaEnApp)
        {
            bool swEstaBien = true;

            try //se intenta pasar por los elementos de ambas moleculas, si no son los mismos átomos se produce error
            {
                if (mEnviadaPorUsuario.ElementosMolécula.Count() == 0) return false;
                if (mEnviadaPorUsuario.ElementosMolécula.Count() != mResultaEnApp.ElementosMolécula.Count()) return false; //si no tienen la misma cantidad, hay error

                bool swEsta = false;
                foreach (ElementoEnUso elUsuario in mEnviadaPorUsuario.ElementosMolécula)
                {
                    swEsta = false;
                    foreach (ElementoEnUso elApp in mResultaEnApp.ElementosMolécula)
                    {
                        if (elUsuario.Simbolo == elApp.Simbolo)
                        {
                            if (elUsuario.CantAtomos == elApp.CantAtomos)
                            {
                                swEsta = true;
                                break;
                            }
                        }
                    }
                    if (!swEsta) break;
                }

                if (!swEsta) //si no están todos los elementos
                    return false;
                //else
                //    return true;
            }
            catch
            {
                swEstaBien = false;
            }
            return swEstaBien;
        }
    }
}
