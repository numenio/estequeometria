using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometría
{
    class Formula
    {
        public string cadenaFormulaEnviada = "";
        public string cadenaFormulaMejorada = "";
        public List<Molecula> atomosEnFormula = new List<Molecula>(); //los antecedentes de la fórmula
        public Molecula molEnviadaenResult; //el resultado en la fórmula
        public bool swEsFormulaValida = false;
        

        public Formula(string formulaEnviada)
        {
            cadenaFormulaEnviada = formulaEnviada;
            cadenaFormulaMejorada = new ValidadorCadenas().validarCadena(cadenaFormulaEnviada);
            // <------- hacer función que valide la fórmula enviada
            if (esFormulaValida(cadenaFormulaMejorada))
            {
                atomosEnFormula = separarAtomosenFormula(cadenaFormulaMejorada);
                molEnviadaenResult = separarMolResultEnFormula(cadenaFormulaMejorada);
                swEsFormulaValida = true;
                //Voz.hablar("esto nunca lo diré");
            }
            else
                swEsFormulaValida = false;
        }

        private List<Molecula> separarAtomosenFormula (string formula)
        {
            List<Molecula> listaAtomos = new List<Molecula>();
            string aux = formula.Substring(0,formula.IndexOf('=')).Trim();
            List<string> listaElementos = aux.Split('+').ToList();

            foreach(string s in listaElementos)
                listaAtomos.Add(new Molecula(s));

            return listaAtomos;
        }

        private Molecula separarMolResultEnFormula (string formula)
        {
            string aux = formula.Substring(formula.IndexOf('=')+1).Trim();
            string auxcadenaFormula = "";

            auxcadenaFormula = new ValidadorCadenas().validarCadena(aux); //se quitan todos los caracteres que no sean válidos

            return new Molecula(auxcadenaFormula);
        }

        private bool esFormulaValida (string formula) 
        {
            formula = new ValidadorCadenas().quitarEspaciosEncadena(formula);
            if (new ValidadorCadenas().esCadenaValida(formula)) //si los caracteres son válidos
                if (formula.Contains('+')) //tiene dos elementos que se suman
                    if (formula.Contains('=')) //tiene un igual
                        if (formula.IndexOf('+') < formula.IndexOf('=')) //y el signo + está antes que el =
                            return true; //entonces es válida

            return false;
        }
    }
}
