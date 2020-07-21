using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_Estequeometría
{
    /// <summary>
    /// Lógica de interacción para VentanaEstequeometria.xaml
    /// </summary>
    public partial class VentanaEstequeometria : Window
    {
        ElementoEnUso atomoSeleccionadoParaElUsuario;
        List<string> voces = Voz.listarVocesPorIdioma("Español");
        bool swSeManejoElEventoEnWindowKeyDown = false;
        List<int> listaPosicionesPermitidasParaBorrar = new List<int>();
        public VentanaEstequeometria(tipoMolécula t)
        {
            InitializeComponent();


            string aux = "";
            atomoSeleccionadoParaElUsuario = new SumadorAtomos().elegirAtomoAleatorio();
            string formula = atomoSeleccionadoParaElUsuario.Simbolo;
            if (new ValidadorCadenas().EsDiatomico(atomoSeleccionadoParaElUsuario))
                formula += "2";

            formula += "+O2=" + new SumadorAtomos(atomoSeleccionadoParaElUsuario).molFinal.CadenaMolécula;

            switch (t)
            {
                case tipoMolécula.oxido:
                    aux = "óxido o anhidrido";
                    break;
                case tipoMolécula.anhidrido:
                    break;
                case tipoMolécula.acido:
                    break;
                case tipoMolécula.hidroxido:
                    break;
                case tipoMolécula.sal:
                    break;
                case tipoMolécula.noValida:
                    break;
            }
            txtInfo.Text = "Enter: Revisa la fórmula escrita. F1: lee el ejercicio a realizar.F2: cambia el átomo para hacer una fórmula nueva. F5, F6 y F7 modifican la voz. Control: callar la voz. Escape: volver\nAutor: Guillermo Toscani (guillermo.toscani@gmail.com)";
            txtPedido.Text = "Tenés que hacer sólo la estequeometría del " + aux + " que se forma usando el átomo:" + "\n" + atomoSeleccionadoParaElUsuario.Nombre + " con la valencia " + atomoSeleccionadoParaElUsuario.CantAtomos.ToString();
            txtFormula.Text = formula;
            txtFormula.Focus();
            Voz.hablarAsync("En este ejercicio " + txtPedido.Text + ". La fórmula ya está escrita y no se puede borrar, sólo hay que hacer la estequeometría");
        }

        private bool esPosicionPermitida (List<int> lista, int posParaChequear)
        {
            if (lista.Contains(posParaChequear)) return true;
            else return false;
        }

        private List<int> modificarListaPermitidos (List<int> lista, int posValorACambiar, bool swAñadirOSacar)
        {
            if (swAñadirOSacar) //true = añadir, false = quitar
            {
                lista.Add(posValorACambiar);
            }
            else //sacar
            {
                if (lista.Contains(posValorACambiar)) //si tiene el valor
                {
                    //int indexPos = lista.IndexOf(posValorACambiar);
                    lista.Remove(posValorACambiar);
                }
            }

            return lista;
        }

        private bool esNumero(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    return true;
                default:
                    return false;
            }
        }


        private bool esLetra(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                case Key.B:
                case Key.C:
                case Key.D:
                case Key.E:
                case Key.F:
                case Key.G:
                case Key.H:
                case Key.I:
                case Key.J:
                case Key.K:
                case Key.L:
                case Key.M:
                case Key.N:
                case Key.O:
                case Key.P:
                case Key.Q:
                case Key.R:
                case Key.S:
                case Key.T:
                case Key.U:
                case Key.V:
                case Key.W:
                case Key.X:
                case Key.Y:
                case Key.Z:
                    return true;
                default:
                    return false;
            }
        }


        private int traducirKeyANumero(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D0:
                    return 0;
                case Key.D1:
                    return 1;
                case Key.D2:
                    return 2;
                case Key.D3:
                    return 3;
                case Key.D4:
                    return 4;
                case Key.D5:
                    return 5;
                case Key.D6:
                    return 6;
                case Key.D7:
                    return 7;
                case Key.D8:
                    return 8;
                case Key.D9:
                    return 9;
                case Key.NumPad0:
                    return 0;
                case Key.NumPad1:
                    return 1;
                case Key.NumPad2:
                    return 2;
                case Key.NumPad3:
                    return 3;
                case Key.NumPad4:
                    return 4;
                case Key.NumPad5:
                    return 5;
                case Key.NumPad6:
                    return 6;
                case Key.NumPad7:
                    return 7;
                case Key.NumPad8:
                    return 8;
                case Key.NumPad9:
                    return 9;
                default:
                    return 0;
            }

            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                VentanaSeleccionTipoEjercicio v = new VentanaSeleccionTipoEjercicio();
                v.swVolviendo = true;
                v.Show();
                this.Close();
                return;
            }


            //if (e.Key == Key.Enter)
            //{
            //    //ElementoEnUso atomo = new ElementoEnUso("Azufre", "S", 4, true);
            //    Formula f = new Formula(txtFormula.Text.Trim());
            //    bool swEscribioEstequeometria = false;

            //    if (f.swEsFormulaValida)
            //    {
            //        foreach (Molecula m in f.atomosEnFormula)
            //            if (m.CantidadMolécula != 1)
            //                swEscribioEstequeometria = true;

            //        if (f.molEnviadaenResult.CantidadMolécula != 1)
            //            swEscribioEstequeometria = true;
            //    }

            //    int atomosAntecedentes = 0;
            //    int atomosMoleculaFinal = 0;


            //    foreach (Molecula mol in f.atomosEnFormula)
            //        foreach (ElementoEnUso el in mol.ElementosMolécula)
            //            atomosAntecedentes += mol.CantidadMolécula * el.CantAtomos;


            //    foreach (ElementoEnUso el in f.molEnviadaenResult.ElementosMolécula)
            //        atomosMoleculaFinal += f.molEnviadaenResult.CantidadMolécula * el.CantAtomos;

            //    string cadena = "";
            //    ComprobadorFormulaTotal compr = new ComprobadorFormulaTotal(f, atomoSeleccionadoParaElUsuario, tipoMolécula.oxido, true);

            //    if (!swEscribioEstequeometria)
            //    {
            //        if (!compr.swFormulaBienHecha)
            //            cadena = compr.cadenaError;
            //            //cadena = "Error. Comprobá la estequeometría porque hay " + atomosAntecedentes + " átomos en la suma, pero hay " + atomosMoleculaFinal + " en el resultado";
            //        else
            //            cadena = "Todo está perfecto. Felicitaciones! Ahora podés apretar F2 para hacer un nuevo ejercicio.";
            //    }
            //    else
            //    {


            //        //if (atomosMoleculaFinal != atomosAntecedentes)
            //        //    swHayError = true;
            //        cadena = compr.cadenaError;
            //        //cadena = "Error. Comprobá la estequeometría porque hay " + atomosAntecedentes + " átomos en la suma, pero hay " + atomosMoleculaFinal + " en el resultado"; 
            //    }
            //    txtResultado.Text = cadena;
            //    Voz.hablarAsync(cadena);

            //    swSeManejoElEventoEnWindowKeyDown = true;
            //    return;
            //}



            //if (esNumero(e)) //si es un número se deja escribir, si es letra se cancela. Sólo funcionan los caracteres que están arriba
            //{
            //    int pos = txtFormula.SelectionStart;
            //    if (!listaPosicionesPermitidasParaBorrar.Contains(pos))
            //        listaPosicionesPermitidasParaBorrar.Add(pos);
            //    List<int> aux = new List<int>();
            //    listaPosicionesPermitidasParaBorrar.ForEach(p => aux.Add(p)); //se carga el auxiliar
            //    foreach (int i in aux)
            //    {
            //        if (i > pos)
            //        {
            //            listaPosicionesPermitidasParaBorrar.Remove(i);
            //            if (!listaPosicionesPermitidasParaBorrar.Contains(i))
            //                listaPosicionesPermitidasParaBorrar.Add(i + 1);
            //        }
            //    }
            //}
            //else
            //{
            //    Voz.hablarAsync("sólo se pueden escribir números. En este ejercicio sólo se practica la estequeometría");
            //    e.Handled = true;
            //}
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                VentanaSeleccionTipoEjercicio v = new VentanaSeleccionTipoEjercicio();
                v.swVolviendo = true;
                v.Show();
                this.Close();
            }

            if (e.Key == Key.F1) //F1 informa lo que hay que hacer
            {
                Voz.hablarAsync("Efe uno: " + txtPedido.Text);
                return;
            }

            if (e.Key == Key.F2) //F2 cambia el átomo y la valencia a resolver
            {
                atomoSeleccionadoParaElUsuario = new SumadorAtomos().elegirAtomoAleatorio();
                string formula = atomoSeleccionadoParaElUsuario.Simbolo;
                if (new ValidadorCadenas().EsDiatomico(atomoSeleccionadoParaElUsuario))
                    formula += "2";
                
                formula += "+O2=" + new SumadorAtomos(atomoSeleccionadoParaElUsuario).molFinal.CadenaMolécula;

                txtPedido.Text = "Tenés que hacer sólo la estequeometría del óxido o anhidrido que se forma usando el átomo:" + "\n" + atomoSeleccionadoParaElUsuario.Nombre + " con la valencia " + atomoSeleccionadoParaElUsuario.CantAtomos.ToString();
                txtFormula.Text = formula;
                txtResultado.Text = "";
                txtFormula.Focus();
                Voz.hablarAsync("Efe dos, Ejercicio nuevo: " + txtPedido.Text);
                return;
            }

            if (e.Key == Key.Down) //flecha abajo lee lo que ya está escrito en el cuadro de texto
            {
                if (txtFormula.Text.Trim() != "")
                    Voz.hablarAsync(new ValidadorCadenas().separarCadenaconEspacios(txtFormula.Text));
                else
                    Voz.hablarAsync("No hay nada escrito");

                return;
            }

            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) //control calla la voz
            {
                Voz.callar();
                return;
            }

            if (e.Key == Key.F5) //F5 más lento
            {
                Voz.cambiarVelocidad(Voz.velocidadVozActual() - 1);
                Voz.hablarAsync("más lento");
                return;
            }

            if (e.Key == Key.F6) //F6 más rápido
            {
                Voz.cambiarVelocidad(Voz.velocidadVozActual() + 1);
                Voz.hablarAsync("más rápido");
                return;
            }

            if (e.Key == Key.Left)
            {
                if (txtFormula.Text.Trim() == "") //si el cuadro está vacío
                    Voz.hablarAsync("No hay nada escrito");
                else
                {
                    if (txtFormula.SelectionStart == 0) //si está al principio del cuadro
                        Voz.hablarAsync("Estás en el comienzo del cuadro para escribir tu ejercicio");
                    //else if (txtFormula.SelectionStart == txtFormula.Text.Length)
                    //    Voz.hablarAsync("")
                    else
                    {
                        Voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true));
                    }
                }
                return;
            }

            if (e.Key == Key.Right)
            {
                if (txtFormula.Text.Trim() == "") //si el cuadro está vacío
                    Voz.hablarAsync("No hay nada escrito");
                else
                {
                    if (txtFormula.SelectionStart == txtFormula.Text.Length) //si está al principio del cuadro
                        Voz.hablarAsync("Última letra: " + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true));
                    //else if (txtFormula.SelectionStart == txtFormula.Text.Length)
                    //    Voz.hablarAsync("")
                    else
                    {
                        //if ()
                        int pos = txtFormula.SelectionStart - 1;
                        if (pos < 0) pos = 0;
                        Voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[pos].ToString(), true));
                    }
                }
                return;
            }

            if (e.Key == Key.F7)
            {
                int pos = voces.IndexOf(Voz.vozActual());
                if (pos >= voces.Count - 1) pos = -1;
                pos++;
                Voz.cambiarVoz(voces[pos]);
                Voz.hablarAsync("elegiste mi voz para hablarte");
                return;
            }

            if (!swSeManejoElEventoEnWindowKeyDown)
            {
                if (esNumero(e)) //si es un número se deja escribir, si es letra se cancela. Sólo funcionan los caracteres que están arriba
                {
             //       if (!swSeManejoElEventoEnWindowKeyDown)
             //       {
                        int posCursor = txtFormula.SelectionStart;
                        posCursor--;
                        if (posCursor < 0) posCursor = 0;
                        if (txtFormula.Text.Length != 0)
                            Voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[posCursor].ToString(), true));
            //        }
                }
            }

            swSeManejoElEventoEnWindowKeyDown = false; //se resetea

        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
       {
            

            if (e.Key == Key.Back)
            {
                swSeManejoElEventoEnWindowKeyDown = true;
                string numeros = "0123456789";
                int posCursor = txtFormula.SelectionStart;
                posCursor--;
                if (posCursor < 0) posCursor = 0;

                if (!numeros.Contains(txtFormula.Text[posCursor]))
                {
                    Voz.hablarAsync("no se puede borrar la fórmula, sólo se puede escribir números para la estequeometría");
                    e.Handled = true;
                }
                else

                {
                    if (listaPosicionesPermitidasParaBorrar.Contains(posCursor))
                    {
                        //char borrar = txtFormula.Text[txtFormula.SelectionStart - 1]; //borrar
                        swSeManejoElEventoEnWindowKeyDown = true;
                        //int pos = txtFormula.SelectionStart; //para la voz
                        //pos--;
                        if (posCursor < 0) posCursor = 0;
                        if (txtFormula.Text.Length != 0)
                        {
                            //int pos2 = txtFormula.SelectionStart;
                            
                            listaPosicionesPermitidasParaBorrar.Remove(posCursor);
                            List<int> aux = new List<int>();
                            listaPosicionesPermitidasParaBorrar.ForEach(p => aux.Add(p)); //se carga el auxiliar
                            foreach (int i in aux)
                            {
                                if (i > posCursor)
                                {
                                    listaPosicionesPermitidasParaBorrar.Remove(i);
                                    if (!listaPosicionesPermitidasParaBorrar.Contains(i -1))
                                        listaPosicionesPermitidasParaBorrar.Add(i - 1);
                                }
                            }
                                                        
                            Voz.hablarAsync("Borrando " + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[posCursor].ToString(), true));
                        }
                        else
                            Voz.hablarAsync("Borraste todo");

                        return;
                    }
                    else
                    {
                        Voz.hablarAsync("estás queriendo borrar un elemento de la fórmula. Sólo podés borrar la estequeometría que vos escribas.");
                        e.Handled = true;
                    }
                }
            }

            if (e.Key == Key.Enter)
            {
                //ElementoEnUso atomo = new ElementoEnUso("Azufre", "S", 4, true);
                Formula f = new Formula(txtFormula.Text.Trim());
                bool swEscribioEstequeometria = false;

                if (f.swEsFormulaValida)
                {
                    foreach (Molecula m in f.atomosEnFormula)
                        if (m.CantidadMolécula != 1)
                            swEscribioEstequeometria = true;

                    if (f.molEnviadaenResult.CantidadMolécula != 1)
                        swEscribioEstequeometria = true;


                    int atomosAntecedentes = 0;
                    int atomosMoleculaFinal = 0;


                    foreach (Molecula mol in f.atomosEnFormula)
                        foreach (ElementoEnUso el in mol.ElementosMolécula)
                            atomosAntecedentes += mol.CantidadMolécula * el.CantAtomos;


                    foreach (ElementoEnUso el in f.molEnviadaenResult.ElementosMolécula)
                        atomosMoleculaFinal += f.molEnviadaenResult.CantidadMolécula * el.CantAtomos;

                }

                string cadena = "";
                ComprobadorFormulaTotal compr = new ComprobadorFormulaTotal(f, atomoSeleccionadoParaElUsuario, tipoMolécula.oxido, true);

                if (swEscribioEstequeometria)
                {
                    if (!compr.swFormulaBienHecha)
                        cadena = compr.cadenaError;
                    //cadena = "Error. Comprobá la estequeometría porque hay " + atomosAntecedentes + " átomos en la suma, pero hay " + atomosMoleculaFinal + " en el resultado";
                    else
                        cadena = "Todo está perfecto. Felicitaciones! Ahora podés apretar F2 para hacer un nuevo ejercicio o ESCAPE para volver al menú inicial.";
                }
                else
                {


                    //if (atomosMoleculaFinal != atomosAntecedentes)
                    //    swHayError = true;
                    cadena = compr.cadenaError;
                    //cadena = "Error. Comprobá la estequeometría porque hay " + atomosAntecedentes + " átomos en la suma, pero hay " + atomosMoleculaFinal + " en el resultado"; 
                }
                txtResultado.Text = cadena;
                Voz.hablarAsync(cadena);

                swSeManejoElEventoEnWindowKeyDown = true;
                return;
            }

            if (esNumero(e))
            {
                if (!esLugarPermitidoParaEscribir(txtFormula.SelectionStart, txtFormula.Text))
                {
                    swSeManejoElEventoEnWindowKeyDown = true;
                    Voz.hablarAsync("no se puede escribir dentro de la fórmula, sólo delante de los elementos de la fórmula");
                    e.Handled = true;
                    return;
                }
            }

            if (esNumero(e)) //si es un número se deja escribir, si es letra se cancela. Sólo funcionan los caracteres que están arriba
            {
                int pos = txtFormula.SelectionStart;
                if (!listaPosicionesPermitidasParaBorrar.Contains(pos))
                    listaPosicionesPermitidasParaBorrar.Add(pos);
                List<int> aux = new List<int>();
                listaPosicionesPermitidasParaBorrar.ForEach(p => aux.Add(p)); //se carga el auxiliar
                foreach (int i in aux)
                {
                    if (i > pos)
                    {
                        listaPosicionesPermitidasParaBorrar.Remove(i);
                        if (!listaPosicionesPermitidasParaBorrar.Contains(i))
                            listaPosicionesPermitidasParaBorrar.Add(i + 1);
                    }
                }
            }
            else
            {
                //string c = e.Key.ToString();
                if (esLetra(e))
                {
                    Voz.hablarAsync("sólo se pueden escribir números. En este ejercicio sólo se practica la estequeometría");
                    e.Handled = true;
                }
            }


            
        
        }

        private bool esLugarPermitidoParaEscribir (int pos, string formula)
        {
            const int posComienzo = 0;
            int posPrimeraLetra = 0;
            int posIgual = 0;
            int posPrimeraLetraPostIgual = 0;
            int posSignoSuma = 0;
            int posPrimeraLetraPostSuma = 0;

            for(int i=0; i< formula.Length; i++)
            {
                char caracter = formula[i];
                if (char.IsLetter(caracter) || caracter == '+' || caracter == '=')
                {
                    if (posPrimeraLetra == 0) posPrimeraLetra = i; //se obtienen el valor de la primer letra
                    if (posPrimeraLetraPostSuma == 0 && posSignoSuma != 0) posPrimeraLetraPostSuma = i;
                    if (caracter == '+') posSignoSuma = i;
                    if (posPrimeraLetraPostIgual == 0 && posIgual != 0) posPrimeraLetraPostIgual = i;
                    if (caracter == '=') posIgual = i;
                }
            }

            if (pos >= posComienzo && pos <= posPrimeraLetra) return true;
            if (pos > posIgual && pos <= posPrimeraLetraPostIgual) return true;
            if (pos > posSignoSuma && pos <= posPrimeraLetraPostSuma) return true;

            return false;
        }
    }
}
