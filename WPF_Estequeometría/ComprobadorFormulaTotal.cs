using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometria
{
    class ComprobadorFormulaTotal
    {
        public bool swFormulaBienHecha = true;
        public string cadenaError = "";
        public ComprobadorFormulaTotal(Formula f, ElementoEnUso e, tipoMolécula t, bool chequearEstequeometría)
        {
            BuscadorErroresEnFormula b = new BuscadorErroresEnFormula(f.molEnviadaenResult, new SumadorAtomos(e, t).molFinal, f, e, t);

            if (f.swEsFormulaValida)
            {
                if (!new ComprobadorAntecedentes(f, e, t).swAntecedentesBienEscritos) swFormulaBienHecha = false;
                if (!new ComprobadorMoleculas(f.molEnviadaenResult, e, t).swMoleculaBienResulta) swFormulaBienHecha = false;
                if (chequearEstequeometría && swFormulaBienHecha)
                    if (b.chequearEstequeometria())
                    {
                        cadenaError = b.cadenaExplicacionError;
                        swFormulaBienHecha = false;
                    }
            }
            else
            {
                swFormulaBienHecha = false;
                cadenaError = "Error. Revisá lo que escribiste porque no es una fórmula válida";
            }
        }
    }
}
