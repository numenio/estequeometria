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
    /// Lógica de interacción para VentanaSoloResultado.xaml
    /// </summary>
    public partial class VentanaSoloResultado : Window
    {
        ElementoEnUso atomoSeleccionadoParaElUsuario;
        List<string> voces = Voz.listarVocesPorIdioma("Español");
        bool swSeManejoElEventoEnWindow = false;
        RandomizadorElementos ran = new RandomizadorElementos();

        public VentanaSoloResultado(tipoMolécula t)
        {
            InitializeComponent();

            try
            {
                atomoSeleccionadoParaElUsuario = ran.elegirAtomoAleatorio();

                txtInfo.Text = "Enter: Revisa la fórmula escrita. F1: lee el ejercicio a realizar.F2: cambia el átomo para hacer una fórmula nueva. F5, F6 y F7 modifican la voz. Control: callar la voz. Escape: volver\nAutor: Guillermo Toscani (guillermo.toscani@gmail.com)";
                txtPedido.Text = "Tenés que escribir sólo el óxido o anhidrido que se forma usando el átomo:" + "\n" + atomoSeleccionadoParaElUsuario.Nombre + " con la valencia " + atomoSeleccionadoParaElUsuario.CantAtomos.ToString();
                txtFormula.Focus();
                Voz.hablarAsync(txtPedido.Text + ". En este ejercicio no tenés que escribir los átomos que se suman, sólo el óxido o anhidrido resultado");
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n"+ ex.StackTrace);
            }
        }

        private void txtFormula_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                VentanaSeleccionTipoEjercicio v = new VentanaSeleccionTipoEjercicio();
                v.swVolviendo = true;
                v.Show();
                this.Close();
                return;
            }

            if (e.Key == Key.F1) //F1 informa lo que hay que hacer
            {
                Voz.hablarAsync("Efe uno: " + txtPedido.Text);
                return;
            }

            if (e.Key == Key.F2) //F2 cambia el átomo y la valencia a resolver
            {

                atomoSeleccionadoParaElUsuario = ran.elegirAtomoAleatorio();

                txtPedido.Text = "Tenés que escribir sólo el óxido o ahidrido que se forma usando el átomo:" + "\n" + atomoSeleccionadoParaElUsuario.Nombre + " con la valencia " + atomoSeleccionadoParaElUsuario.CantAtomos.ToString();
                txtFormula.Text = "";
                txtResultado.Text = "";
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
                        Voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true));
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


            if (!swSeManejoElEventoEnWindow)
            {
                int posCursor = txtFormula.SelectionStart;
                posCursor--;
                if (posCursor < 0) posCursor = 0;
                if (txtFormula.Text.Length != 0)
                    Voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[posCursor].ToString(), true));
            }

            swSeManejoElEventoEnWindow = false; //se resetea

        }

        private void txtFormula_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Back)
            //{
            //    int pos = txtFormula.SelectionStart;
            //    pos--;
            //    if (pos < 0) pos = 0;
            //    if (txtFormula.Text.Length != 0)
            //        Voz.hablarAsync("Borrando " + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[pos].ToString(), true));
            //    else
            //        Voz.hablarAsync("Borraste todo");

            //    return;
            //}
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    swSeManejoElEventoEnWindow = true;
            //    string cadena = "";
            //    //Formula f = new Formula(txtFormula.Text.Trim());
            //    if (new Molecula().esCadenaDeMoleculaValida(txtFormula.Text))
            //    {
            //        Molecula m = new Molecula(txtFormula.Text);
            //        ComprobadorMoleculas c = new ComprobadorMoleculas(m, atomoSeleccionadoParaElUsuario, tipoMolécula.oxido);

                    
            //        if (!c.swMoleculaBienResulta)
            //            cadena = "Error. Revisá la molécula que escribiste. Recordá que apretando F1 se lee de nuevo el ejercicio a realizar.";
            //        else
            //            cadena = "Todo está perfecto. Felicitaciones! Ahora podés apretar F2 para hacer un nuevo ejercicio.";
            //    }
            //    else
            //    {
            //        cadena = "Error. Revisá la molécula que escribiste. Recordá que apretando F1 se lee de nuevo el ejercicio a realizar.";
            //    }
            //    txtResultado.Text = cadena;
            //    Voz.hablarAsync(cadena);
            //    return;
            //}

            //if (e.Key == Key.Back)
            //{
            //    swSeManejoElEventoEnWindow = true;
            //    int pos = txtFormula.SelectionStart;
            //    pos--;
            //    if (pos < 0) pos = 0;
            //    if (txtFormula.Text.Length != 0)
            //        Voz.hablarAsync("Borrando " + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[pos].ToString(), true));
            //    else
            //        Voz.hablarAsync("Borraste todo");

            //    return;
            //}
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Voz.hablarAsync("no se puede usar la tecla suprimir");
                e.Handled = true;
            }

            if (e.Key == Key.Back)
            {
                swSeManejoElEventoEnWindow = true;
                string cadena = "";
                int posCursor = txtFormula.SelectionStart;
                posCursor--;
                if (posCursor < 0) posCursor = 0;

                if (txtFormula.Text == "")
                    cadena = "Borraste todo";
                else
                    cadena = "Borrando " + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[posCursor].ToString(), true);

                Voz.hablarAsync(cadena);
                return;
            }

            if (e.Key == Key.Enter)
            {
                swSeManejoElEventoEnWindow = true;
                string cadena = "";
                //Formula f = new Formula(txtFormula.Text.Trim());
                if (new Molecula().esCadenaDeMoleculaValida(txtFormula.Text))
                {
                    Molecula m = new Molecula(txtFormula.Text);
                    ComprobadorMoleculas c = new ComprobadorMoleculas(m, atomoSeleccionadoParaElUsuario, tipoMolécula.oxido);


                    if (!c.swMoleculaBienResulta)
                    {
                        SumadorAtomos s = new SumadorAtomos(atomoSeleccionadoParaElUsuario);
                        if (s.molOriginal.CadenaMolécula == new ValidadorCadenas().validarCadena(txtFormula.Text, false)) //si escribió bien la fórmula pero sólo le faltó simplificar
                            cadena = "Error, faltó simplificar. " + c.cadenaSimplificacion;
                        else
                            cadena = "Error. Revisá la molécula que escribiste. Recordá que apretando F1 se lee de nuevo el ejercicio a realizar.";
                    }
                    else
                    {
                        if (m.CantidadMolécula != 1)
                            cadena = "Error. Escribiste un número antes del óxido o anhidrido. Este ejercicio es sólo para practicar el óxido o anhidrido final, no la estequeometría.";
                        else
                            cadena = "Todo está perfecto. Felicitaciones! Ahora podés apretar F2 para hacer un nuevo ejercicio o ESCAPE para volver al menú inicial.";
                    }
                }
                else
                {
                    cadena = "Error. Revisá la molécula que escribiste. Recordá que apretando F1 se lee de nuevo el ejercicio a realizar.";
                }
                txtResultado.Text = cadena;
                Voz.hablarAsync(cadena);
                return;
            }
        }
    }
}
