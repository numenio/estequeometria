using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometría
{
    enum tipoError
    {
        atomosIncorrectosEnAntecedentes, valenciasIncorrectasEnAntecedentes, estequeometriaIncorrecta,
        atomosIncorrectosEnResult, valenciasIncorrectasEnResult, caracteresNoPermitidos, noSeSimplifico, noEspecifico,
        formulaIncompleta
    }

    class BuscadorErroresEnFormula
    {
        Molecula mEnviadaxUsuario;
        Molecula mResultaEnApp;
        tipoError tipoErrorEnFormula;
        public string cadenaExplicacionError = "";
        Formula formulaEnviada;
        
        public BuscadorErroresEnFormula( Molecula molEnviada, Molecula molBienResuelta, Formula formula)
        {
            mEnviadaxUsuario = molEnviada;
            mResultaEnApp = molBienResuelta;
            formulaEnviada = formula;
            tipoErrorEnFormula = chequearTipoError();
            if (tipoErrorEnFormula != tipoError.estequeometriaIncorrecta) //si es error de estequeometría lo cargamos en la misma función de esteq
                cadenaExplicacionError = hacerCadenaDevolSegunError(tipoErrorEnFormula);
        }

        //public BuscadorErroresEnFormula(Molecula molEnviada, Molecula molBienResuelta)
        //{
        //    mEnviadaxUsuario = molEnviada;
        //    mResultaEnApp = molBienResuelta;
        //    //formulaEnviada = formula;
        //    tipoErrorEnFormula = chequearTipoError();
        //    cadenaExplicacionError = hacerCadenaDevolSegunError(tipoErrorEnFormula);
        //}

        private tipoError chequearTipoError()
        {
            if (chequearCaracteresNoPermitidos()) return tipoError.caracteresNoPermitidos;
            if (chequearFormulaIncompleta()) return tipoError.formulaIncompleta;
            if (chequearAtomosEnAntecedentes()) return tipoError.atomosIncorrectosEnAntecedentes;
            if (chequearValenciaEnAntecedentes()) return tipoError.valenciasIncorrectasEnAntecedentes;
            if (chequearAtomosEnResultado()) return tipoError.atomosIncorrectosEnResult;
            if (chequearValenciasEnResultado()) return tipoError.valenciasIncorrectasEnResult;
            if (chequearNoHaySimplificacion()) return tipoError.noSeSimplifico;
            if (chequearEstequeometria()) return tipoError.estequeometriaIncorrecta;

            return tipoError.noEspecifico;
        }

        private bool chequearFormulaIncompleta ()
        {
            bool swHayError = false;

            try //se intenta pasar por los elementos de ambas moleculas, si no son los mismos átomos se produce error
            {
                if (!formulaEnviada.swEsFormulaValida) return true;

                if (mEnviadaxUsuario.ElementosMolécula.Count() == 0) return true;
            }
            catch
            {
                swHayError = true;
            }

            return swHayError;
        }

        private bool chequearAtomosEnAntecedentes ()
        {
            bool swHayError = false;
            string cadenaSimbolosAntecedentes = "";
            string cadenaSimbolosResultado = "";
            string cadenaAntecedentes = "";
            string cadenaResultado = "";

            try //se arma una cadena con los elementos de la fórmula escrita y lo que deberían estar. Se comparan
            {
                foreach (Molecula mol in formulaEnviada.atomosEnFormula)
                    foreach (ElementoEnUso el in mol.ElementosMolécula)
                        cadenaSimbolosAntecedentes += el.Simbolo;

                foreach (ElementoEnUso el in mResultaEnApp.ElementosMolécula)
                    cadenaSimbolosResultado += el.Simbolo;

                cadenaAntecedentes = String.Concat(cadenaSimbolosAntecedentes.OrderBy(c => c));
                cadenaResultado = String.Concat(cadenaSimbolosResultado.OrderBy(c => c));

                if (cadenaAntecedentes != cadenaResultado)
                    swHayError = true;
            }
            catch
            {
                swHayError = true;
            }

            return swHayError;
        }

        private bool chequearValenciaEnAntecedentes()
        {
            bool swHayError = false;
            //busca los elementos que están en la fórmula final, ver si es diatómico
            foreach (Molecula m in formulaEnviada.atomosEnFormula)
            {
                foreach (ElementoEnUso el in m.ElementosMolécula)
                {
                    if (new ValidadorCadenas().EsDiatomico(el))
                    {
                        if (el.CantAtomos != 2)
                            return true;
                    }
                    else
                    {
                        if (el.CantAtomos != 1)
                        return true;
                    }
                }
            }

            return swHayError;
        }

        public bool chequearEstequeometria()
        {
            //bool swHayError = false;
            //int cantAtomosEnElementoAntecedente = 0;
            //int cantAtomosEnElementoResultado = 0;

            foreach (Molecula mol in formulaEnviada.atomosEnFormula) //cada parte de los antecedentes
                foreach (ElementoEnUso elAntec in mol.ElementosMolécula) //cada elementos de cada parte
                    foreach(ElementoEnUso elRes in mEnviadaxUsuario.ElementosMolécula) //cada parte del resultado
                    {
                        if (elAntec.Simbolo == elRes.Simbolo)
                            if (mol.CantidadMolécula * elAntec.CantAtomos != mEnviadaxUsuario.CantidadMolécula * elRes.CantAtomos)
                            {
                                int aux = mol.CantidadMolécula * elAntec.CantAtomos;
                                cadenaExplicacionError = "Error en la estequeometría. Hay " + aux;
                                if (aux == 1)
                                    cadenaExplicacionError += " átomo ";
                                else
                                    cadenaExplicacionError += " átomos ";

                                cadenaExplicacionError += "de " + elAntec.Nombre + " en la suma, pero hay " + (mEnviadaxUsuario.CantidadMolécula * elRes.CantAtomos) +
                                    " del mismo átomo en el resultado.";
                                return true;
                            }
                                
                    }

            return false;
            //foreach (Molecula mol in formulaEnviada.atomosEnFormula)
            //    foreach (ElementoEnUso el in mol.ElementosMolécula)
            //        cantAtomosEnElementoAntecedente += mol.CantidadMolécula * el.CantAtomos;


            //foreach (ElementoEnUso el in mEnviadaxUsuario.ElementosMolécula)
            //    cantAtomosEnElementoResultado += mEnviadaxUsuario.CantidadMolécula * el.CantAtomos;

            //if (cantAtomosEnElementoAntecedente != cantAtomosEnElementoResultado)
            //    swHayError = true;

            //return swHayError;
        }

        private bool chequearAtomosEnResultado()
        {
            bool swHayError = false;
            try //se intenta pasar por los elementos de ambas moleculas, si no son los mismos átomos se produce error
            {
                if (mEnviadaxUsuario.ElementosMolécula.Count() == 0) return true;

                for (int i = 0; i < mEnviadaxUsuario.ElementosMolécula.Count(); i++)
                {
                    if (mEnviadaxUsuario.ElementosMolécula[i].Simbolo != mResultaEnApp.ElementosMolécula[i].Simbolo)
                    {
                        swHayError = true;
                        break;
                    }
                }
            }
            catch
            {
                swHayError = true;
            }

            return swHayError;
        }

        private bool chequearValenciasEnResultado()
        {
            bool swHayError = false;
            try //se intenta pasar por los elementos de ambas moleculas, si no tienen la misma cantidad se produce error
            {
                if (mEnviadaxUsuario.ElementosMolécula.Count() == 0) return true;

                for (int i = 0; i < mEnviadaxUsuario.ElementosMolécula.Count(); i++)
                {
                    if (mEnviadaxUsuario.ElementosMolécula[i].CantAtomos != mResultaEnApp.ElementosMolécula[i].CantAtomos)
                    {
                        swHayError = true;
                        break;
                    }
                }
            }
            catch
            {
                swHayError = true;
            }

            return swHayError;
        }

        private bool chequearCaracteresNoPermitidos()
        {
            bool swHayError = false;
            if (!new ValidadorCadenas().esCadenaValida(new ValidadorCadenas().quitarEspaciosEncadena(formulaEnviada.cadenaFormulaEnviada), false))
                swHayError = true;

            return swHayError;
        }

        private bool chequearNoHaySimplificacion()
        {
            bool swHayError = false;


            return swHayError;
        }

        private string hacerCadenaDevolSegunError (tipoError t)
        {
            string cadena = "";
            switch (t)
            {
                case tipoError.atomosIncorrectosEnAntecedentes:
                    cadena = "Escribiste mal los átomos que estás sumando, hay escritos átomos incorrectos a los pedidos antes de la fórmula del resultado";
                    break;
                case tipoError.valenciasIncorrectasEnAntecedentes:
                    cadena = "Escribiste mal las valencias de los átomos que estás sumando, acordate que el elemento metal o no metal va sin ningún subíndice excepto que sea diatómico, y a eso se le suma el oxígeno, al que siempre se le escribe subíndice 2 porque él sí es diatómico";
                    break;
                case tipoError.estequeometriaIncorrecta:
                    //cadena = "Está mal la estequeometría de la operación. La fórmula escrita está bien tanto en los átomos que se están sumando como en la molécula resultado, pero no está bien equilibrada";
                    break;
                case tipoError.atomosIncorrectosEnResult:
                    cadena = "Escribiste mal los átomos en la molécula resultado. Recordá que tenés que intercambiar la valencia del átomo que sumás, con la valencia 2 que siempre tiene el oxígeno. Después si se puede tenés que simplificar";
                    break;
                case tipoError.valenciasIncorrectasEnResult:
                    cadena = "Están mal los subíndices de la molécula resultado. Puede ser que usaste valencias equivocadas, o que simplificaste mal";
                    break;
                case tipoError.caracteresNoPermitidos:
                    cadena = "Revisá lo que escribiste porque hay caracteres no permitidos. Sólo se pueden usar números, letras mayúsculas, minúsculas, y los signos igual y suma";
                    break;
                case tipoError.noSeSimplifico:
                    cadena = "Falta simplificar la molécula del resultado. Está bien resuelta pero no se simplificaron los subíndices";
                    break;
                case tipoError.formulaIncompleta:
                    cadena = "Estás usando enter, pero la fórmula está incompleta. Tienen que estar los átomos que estás sumando, el signo igual y la molécula resultado";
                    break;
                case tipoError.noEspecifico:
                    cadena = "Hay un error pero no puedo determinal cuál es. Por favor revisá todo lo que escribiste";
                    break;
            }

            return cadena;
        }

        
    }
}