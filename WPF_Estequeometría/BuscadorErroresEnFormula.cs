using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometria
{
    enum tipoError
    {
        atomosIncorrectosEnAntecedentes, valenciasIncorrectasEnAntecedentes, estequeometriaIncorrecta,
        atomosIncorrectosEnResult, valenciasIncorrectasEnResult, caracteresNoPermitidos, noSeSimplifico, noEspecifico,
        formulaIncompleta, moleculasIncorrectasEnAntecedentes
    }

    class BuscadorErroresEnFormula
    {
        Molecula mEnviadaxUsuario;
        Molecula mResultaEnApp;
        tipoError tipoErrorEnFormula;
        public string cadenaExplicacionError = "";
        Formula formulaEnviada;
        tipoMolécula tipo;
        ElementoEnUso elementoPedidoEnApp;
        
        public BuscadorErroresEnFormula( Molecula molEnviada, Molecula molBienResuelta, Formula formula, ElementoEnUso e, tipoMolécula t)
        {
            mEnviadaxUsuario = molEnviada;
            mResultaEnApp = molBienResuelta;
            formulaEnviada = formula;
            tipo = t;
            elementoPedidoEnApp = e;
            tipoErrorEnFormula = chequearTipoError();
            if (tipoErrorEnFormula != tipoError.estequeometriaIncorrecta) //si es error de estequiometría lo cargamos en la misma función de esteq
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
            if (chequearCaracteresNoPermitidos(tipo)) return tipoError.caracteresNoPermitidos;
            if (chequearFormulaIncompleta()) return tipoError.formulaIncompleta;
            if (chequearAtomosEnAntecedentes()) return tipoError.atomosIncorrectosEnAntecedentes;
            if (tipo == tipoMolécula.oxido || tipo == tipoMolécula.anhidrido)
            {
                if (chequearValenciaEnAntecedentes())
                    return tipoError.valenciasIncorrectasEnAntecedentes;
            }
            if (tipo == tipoMolécula.acido)
            {
                if (chequearMoleculasEnAntecedentes())
                    return tipoError.moleculasIncorrectasEnAntecedentes;
            }
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

        private bool chequearMoleculasEnAntecedentes() //para ácidos e hidróxidos
        {
            bool swHayError = false;

            try 
            {
                if (tipo == tipoMolécula.acido)
                {
                    Molecula oxido = new SumadorAtomos(elementoPedidoEnApp, tipoMolécula.oxido).molFinal;//se hace el oxido inicial
                    Molecula agua = new Molecula("H2O", tipoMolécula.agua);
                    bool swHayOxido = false;
                    bool swHayAgua = false;
                    foreach (Molecula m in formulaEnviada.atomosEnFormula) //cada uno de las moleculas de los antecedentes
                    {
                        if (m.CadenaMolécula == oxido.CadenaMolécula) swHayOxido = true; //si está el óxido
                        if (m.CadenaMolécula == agua.CadenaMolécula) swHayAgua = true; //si está el óxido
                    }

                    if (!(swHayAgua && swHayOxido))//si no están los dos elementos, hay error
                        return true;
                }
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

                //---------------ÓXIDOS Y ANHIDRIDOS ------------------------
                if (tipo == tipoMolécula.anhidrido || tipo == tipoMolécula.oxido)
                {
                    

                    foreach (ElementoEnUso el in mResultaEnApp.ElementosMolécula)
                        cadenaSimbolosResultado += el.Simbolo;

                    
                }

                //-------------------------ACIDOS--------------------------
                if (tipo == tipoMolécula.acido)
                {
                    cadenaSimbolosResultado += "HO"; //sumamos los símbolos del agua
                    Molecula m = new SumadorAtomos(elementoPedidoEnApp, tipoMolécula.oxido).molFinal;
                    foreach (ElementoEnUso el in m.ElementosMolécula)
                        cadenaSimbolosResultado += el.Simbolo;
                }

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
            List<ElementoEnUso> listaElementosEnAntecedentes = new List<ElementoEnUso>(); //para sumar los átomos que están en distintas moléculas
            List<string> listaSimbolosEnAntecedentes = new List<string>();
            //El problema de esta función es que supone que los átomos en las moléculas que suman sólo están repetidos 1 vez, funciona para ácidos y óxidos

            //-------Se hace una lista de elementos en los antecedentes, sin repetirlos, y con las valencias
            //-------multiplicadas por la cantidad de moléculas en la estequiometría
            foreach (Molecula mol in formulaEnviada.atomosEnFormula) //cada parte de los antecedentes
            { 
                foreach (ElementoEnUso elAntec in mol.ElementosMolécula) //cada elementos de cada parte
                {
                    int aux = 0;
                    
                    if (listaSimbolosEnAntecedentes.Contains(elAntec.Simbolo))
                    {
                        ElementoEnUso elAux = elAntec; //se inicializa con cualquier valor
                        foreach (ElementoEnUso el in listaElementosEnAntecedentes)
                        {
                            if (el.Simbolo == elAntec.Simbolo)
                            {
                                aux = el.CantAtomos + (elAntec.CantAtomos * mol.CantidadMolécula);
                                elAux = el;
                                break;
                            }
                        }
                        
                        listaElementosEnAntecedentes.Remove(elAux);
                        
                    }
                    else
                    {
                        aux = elAntec.CantAtomos * mol.CantidadMolécula;
                        listaSimbolosEnAntecedentes.Add(elAntec.Simbolo);
                    }

                    ElementoEnUso e = new ElementoEnUso(elAntec.Nombre, elAntec.Simbolo, aux, true);
                    listaElementosEnAntecedentes.Add(e);
                }
            }


            //-------se chequea que las cantidades de la lista anterior coincidan con las que están
            //-------en la molécula resultado
            foreach (ElementoEnUso elAntec in listaElementosEnAntecedentes)
            {
                foreach (ElementoEnUso elRes in mEnviadaxUsuario.ElementosMolécula) //cada parte del resultado
                {
                    if (elAntec.Simbolo == elRes.Simbolo)
                        if (elAntec.CantAtomos != mEnviadaxUsuario.CantidadMolécula * elRes.CantAtomos)
                        {
                            //int aux = mol.CantidadMolécula * elAntec.CantAtomos;
                            cadenaExplicacionError = "Error en la estequiometría. Hay " + elAntec.CantAtomos;
                            if (elAntec.CantAtomos == 1)
                                cadenaExplicacionError += " átomo ";
                            else
                                cadenaExplicacionError += " átomos ";

                            cadenaExplicacionError += "de " + elAntec.Nombre + " en la suma, pero hay " + (mEnviadaxUsuario.CantidadMolécula * elRes.CantAtomos) +
                                " del mismo átomo en el resultado.";
                            return true;
                        }

                }
            }

            return false;
            
        }

        private bool chequearAtomosEnResultado()
        {
            bool swHayError = false;
            try //se intenta pasar por los elementos de ambas moleculas, si no son los mismos átomos se produce error
            {
                if (mEnviadaxUsuario.ElementosMolécula.Count() == 0) return true;
                if (mEnviadaxUsuario.ElementosMolécula.Count() != mResultaEnApp.ElementosMolécula.Count()) return true;

                bool swEsta = false;
                foreach (ElementoEnUso elUsuario in mEnviadaxUsuario.ElementosMolécula)
                {
                    swEsta = false;
                    foreach (ElementoEnUso elApp in mResultaEnApp.ElementosMolécula)
                        if (elUsuario.Simbolo == elApp.Simbolo)
                        {
                            swEsta = true;
                            break;
                        }
                }

                if (!swEsta) //si no están todos los elementos
                    return true;

                //for (int i = 0; i < mEnviadaxUsuario.ElementosMolécula.Count(); i++)
                //{
                //    if (mEnviadaxUsuario.ElementosMolécula[i].Simbolo != mResultaEnApp.ElementosMolécula[i].Simbolo)
                //    {
                //        swHayError = true;
                //        break;
                //    }
                //}
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
                if (mEnviadaxUsuario.ElementosMolécula.Count() != mResultaEnApp.ElementosMolécula.Count()) return true;

                bool swEstaBien = false;
                foreach (ElementoEnUso elUsuario in mEnviadaxUsuario.ElementosMolécula)
                {
                    swEstaBien = false;
                    foreach (ElementoEnUso elApp in mResultaEnApp.ElementosMolécula)
                    { 
                        if (elUsuario.Simbolo == elApp.Simbolo)
                        {
                            if (elUsuario.CantAtomos == elApp.CantAtomos)
                            {
                                swEstaBien = true;
                                break;
                            }
                        }
                    }
                    if (!swEstaBien)
                        return true;
                }

                if (!swEstaBien) //si no están todos los elementos
                    return true;


                //for (int i = 0; i < mEnviadaxUsuario.ElementosMolécula.Count(); i++)
                //{
                //    if (mEnviadaxUsuario.ElementosMolécula[i].CantAtomos != mResultaEnApp.ElementosMolécula[i].CantAtomos)
                //    {
                //        swHayError = true;
                //        break;
                //    }
                //}
            }
            catch
            {
                swHayError = true;
            }

            return swHayError;
        }

        private bool chequearCaracteresNoPermitidos(tipoMolécula t)
        {
            bool swHayError = false;
            if (!new ValidadorCadenas().esCadenaValida(new ValidadorCadenas().quitarEspaciosEncadena(formulaEnviada.cadenaFormulaEnviada), false, t))
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

            if (tipo == tipoMolécula.oxido || tipo == tipoMolécula.anhidrido)
            {
                switch (t)
                {
                    case tipoError.atomosIncorrectosEnAntecedentes:
                        cadena = "Escribiste mal los átomos que estás sumando, hay escritos átomos incorrectos a los pedidos antes de la fórmula del resultado";
                        break;
                    case tipoError.valenciasIncorrectasEnAntecedentes:
                        cadena = "Escribiste mal las valencias de los átomos que estás sumando, acordate que el elemento metal o no metal va sin ningún subíndice excepto que sea diatómico, y a eso se le suma el oxígeno, al que siempre se le escribe subíndice 2 porque él sí es diatómico";
                        break;
                    case tipoError.estequeometriaIncorrecta:
                        //cadena = "Está mal la estequiometría de la operación. La fórmula escrita está bien tanto en los átomos que se están sumando como en la molécula resultado, pero no está bien equilibrada";
                        break;
                    case tipoError.atomosIncorrectosEnResult:
                        cadena = "Escribiste mal los átomos en la molécula resultado. Recordá que apretando F1 se te lee el elemento que tenés que usar, y el oxígeno se escribe siempre";
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
                    case tipoError.moleculasIncorrectasEnAntecedentes:
                        //if (tipo == tipoMolécula.oxido)
                        //    cadena += "óxido";
                        //if (tipo == tipoMolécula.anhidrido)
                        //    cadena += "anhidrido";
                        cadena = "Escribiste mal las moléculas que estás sumando. O está mal el óxido o anhidrido, o el agua";
                        break;
                }
            }

            if (tipo == tipoMolécula.acido)
            {
                switch (t)
                {
                    case tipoError.atomosIncorrectosEnAntecedentes:
                        cadena = "Error, hay escritos átomos incorrectos en las moléculas que estás sumando";
                        break;
                    case tipoError.valenciasIncorrectasEnAntecedentes:
                        cadena = "Escribiste mal las valencias de las moléculas que estás sumando, acordate que un ácido se forma sumando un anhidrido y agua";
                        break;
                    case tipoError.estequeometriaIncorrecta:
                        //cadena = "Está mal la estequiometría de la operación. La fórmula escrita está bien tanto en los átomos que se están sumando como en la molécula resultado, pero no está bien equilibrada";
                        break;
                    case tipoError.atomosIncorrectosEnResult:
                        cadena = "Escribiste mal los átomos en la ácido resultado";
                        break;
                    case tipoError.valenciasIncorrectasEnResult:
                        cadena = "Están mal los subíndices de la molécula resultado. Recordá que si la valencia a usar es impar el Hidrógeno va con subíndice 1 y si es par con subíndice 2. " +
                            "Para saber el subíndice del Oxígeno, tenés que sumar la valencia del elemento que se te pide, junto con la cantidad de Hidrógenos, y a eso lo dividís por 2";
                        break;
                    case tipoError.caracteresNoPermitidos:
                        cadena = "Revisá lo que escribiste porque hay caracteres no permitidos. Sólo se pueden usar números, letras mayúsculas, minúsculas, y los signos igual y suma";
                        break;
                    case tipoError.noSeSimplifico:
                        cadena = "Falta simplificar la molécula del resultado. Está bien resuelta pero no se simplificaron los subíndices";
                        break;
                    case tipoError.formulaIncompleta:
                        cadena = "Estás usando enter, pero la fórmula está incompleta. Tiene que estar el anhidrido que estás sumando, el signo suma, el agua, el signo igual y el ácido resultado";
                        break;
                    case tipoError.noEspecifico:
                        cadena = "Hay un error pero no puedo determinal cuál es. Por favor revisá todo lo que escribiste";
                        break;
                    case tipoError.moleculasIncorrectasEnAntecedentes:
                        //if (tipo == tipoMolécula.oxido)
                        //    cadena += "óxido";
                        //if (tipo == tipoMolécula.anhidrido)
                        //    cadena += "anhidrido";
                        cadena = "Escribiste mal las moléculas que estás sumando. O está mal resuelto el anhidrido, o el agua";
                        break;
                }
            }

            return cadena;
        }

        
    }
}