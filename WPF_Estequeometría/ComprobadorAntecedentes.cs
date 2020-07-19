﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Estequeometría
{
    class ComprobadorAntecedentes
    {
        Formula formulaEnviada;
        //Molecula moleculaResueltaEnApp;
        //SumadorAtomos sumador;
        public bool swAntecedentesBienEscritos = false;

        public ComprobadorAntecedentes(Formula f, ElementoEnUso e, tipoMolécula t)
        {
            formulaEnviada = f;

            switch (t)
            {
                case tipoMolécula.oxido: //ya sea óxido o anhidrido se hace lo mismo
                case tipoMolécula.anhidrido:
                    Molecula moleculaResueltaEnApp = new SumadorAtomos(e).molFinal;

                    if (sonAntecedentesCorrectos(f, moleculaResueltaEnApp))
                        swAntecedentesBienEscritos = true;
                    else
                        swAntecedentesBienEscritos = false;

                    break;
                case tipoMolécula.acido: //aquí iría el código para sumar comprobar ácidos
                    break;
            }
        }

        private bool sonAntecedentesCorrectos(Formula f, Molecula m)
        {
            bool swAntecedentesCorrectos = true;
            //bool swValenciaBienOxigeno = true;

            foreach (Molecula mol in f.atomosEnFormula)
            {
                foreach (ElementoEnUso el in m.ElementosMolécula)
                {
                    //foreach (ElementoEnUso ele in mol.ElementosMolécula)
                    //{
                        if (mol.ElementosMolécula[0].Simbolo == el.Simbolo)
                        {
                            if (new ValidadorCadenas().EsDiatomico(el))
                            //if (el.Simbolo == "O") //si el átomo es oxígeno, se chequea que esté escrito el subíndice 2
                            {
                                if (mol.ElementosMolécula[0].CantAtomos != 2)
                                {
                                    swAntecedentesCorrectos = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (mol.ElementosMolécula[0].CantAtomos != 1) //si no es diatómico tiene que tener valencia 1
                            {
                                swAntecedentesCorrectos = false;
                                break;
                            }
                        }

                        //if (el.Simbolo == "S") //si el átomo es oxígeno, se chequea que esté escrito el subíndice 2
                        //{
                        //    if (mol.ElementosMolécula[0].CantAtomos != 2)
                        //    {
                        //        swAntecedentesCorrectos = false;
                        //        break;
                        //    }
                        //}


                        swAntecedentesCorrectos = true; //si encuentra que el átomo está en las dos fórmulas, se deja de buscar
                        break;
                        }
                        else
                            swAntecedentesCorrectos = false;
                    //}
                }

                if (!swAntecedentesCorrectos) break; //si tan solo un átomo no está en ambas formulas, se deja de buscar y se da por incorrecta
            }

            return swAntecedentesCorrectos;
        }
    }
}
