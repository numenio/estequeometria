using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometria
{
    class Formula
    {
        public string cadenaFormulaEnviada = "";
        public string cadenaFormulaMejorada = "";
        public List<Molecula> atomosEnFormula = new List<Molecula>(); //los antecedentes de la fórmula
        public Molecula molEnviadaenResult; //el resultado en la fórmula
        public bool swEsFormulaValida = false;
        tipoMolécula tipoMoleculaPedida;
        

        public Formula(string formulaEnviada, tipoMolécula t)
        {
            tipoMoleculaPedida = t;
            cadenaFormulaEnviada = formulaEnviada;
            cadenaFormulaMejorada = new ValidadorCadenas().quitarEspaciosEncadena(cadenaFormulaEnviada); //new ValidadorCadenas().validarCadena(cadenaFormulaEnviada, false);
            
            if (esFormulaValida(cadenaFormulaMejorada, t))
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
                listaAtomos.Add(new Molecula(s, tipoMoleculaPedida));

            return listaAtomos;
        }

        private Molecula separarMolResultEnFormula (string formula)
        {
            string aux = formula.Substring(formula.IndexOf('=')+1).Trim();
            string auxcadenaFormula = "";

            auxcadenaFormula = new ValidadorCadenas().validarCadena(aux, false, tipoMoleculaPedida); //se quitan todos los caracteres que no sean válidos

            return new Molecula(auxcadenaFormula, tipoMoleculaPedida);
        }

        private bool esFormulaValida (string formula, tipoMolécula t) 
        {
            formula = new ValidadorCadenas().quitarEspaciosEncadena(formula);
            if (new ValidadorCadenas().esCadenaValida(formula, false, t)) //si los caracteres son válidos
                if (formula.Contains('+')) //tiene dos elementos que se suman
                    if (formula.Contains('=')) //tiene un igual
                        if (formula.IndexOf('+') < formula.IndexOf('=')) //y el signo + está antes que el =
                            return true; //entonces es válida

            return false;
        }
    }
}
