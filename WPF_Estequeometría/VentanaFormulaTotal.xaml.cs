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
    /// Lógica de interacción para VentanaFormulaTotal.xaml
    /// </summary>
    public partial class VentanaFormulaTotal : Window
    {
        ElementoEnUso atomoSeleccionadoParaElUsuario;
        List<string> voces = Voz.listarVocesPorIdioma("Español");
        bool swSeManejoElEventoEnWindow = false;

        public VentanaFormulaTotal(tipoMolécula t)
        {
            InitializeComponent();
            //ElementoEnUso atomo = new ElementoEnUso("Azufre", "S", 4, true);
            atomoSeleccionadoParaElUsuario = new SumadorAtomos().elegirAtomoAleatorio();

            txtInfo.Text = "Enter: Revisa la fórmula escrita. F1: lee el ejercicio a realizar.F2: cambia el átomo para hacer una fórmula nueva. F5, F6 y F7 modifican la voz. Control: callar la voz. Escape: volver\nAutor: Guillermo Toscani (guillermo.toscani@gmail.com)";
            txtPedido.Text = "Tenés que usar el átomo:" + "\n" + atomoSeleccionadoParaElUsuario.Nombre + " con la valencia " + atomoSeleccionadoParaElUsuario.CantAtomos.ToString() + " para hacer la fórmula completa";
            txtFormula.Focus();
            Voz.hablarAsync("Este es el ejercicio más difícil. Aquí " + txtPedido.Text + ". Acordate de poner los átomos que suman, el resultado, y la estequeometría");

        }

        //private void elegirAtomoAleatorio()
        //{
        //    ListaMetales listamet = new ListaMetales();
        //    ListaNoMetales listanomet = new ListaNoMetales();
        //    Random rnd = new Random();
        //    int cantMetales = listamet.Metales.Count;
        //    int cantNoMetales = listanomet.Nometales.Count;
        //    Elemento el;
        //    int valenciaAleatoria = 1;

        //    int i = rnd.Next(cantMetales + cantNoMetales);

        //    if (i >= cantMetales) //como se suman las listas de metales y no metales, si el número es superior al total de metales, quiere decir que es no metal
        //    {
        //        i -= cantMetales; //se resta la lista de los metales
        //        el = listanomet.Nometales[i];
        //    }
        //    else
        //    {
        //        el = listamet.Metales[i];
        //    }

        //    i = rnd.Next(el.Valencias.Count);
        //    valenciaAleatoria = int.Parse(el.Valencias[i]);

        //    atomoSeleccionadoParaElUsuario = new ElementoEnUso(el.Nombre, el.Simbolo, valenciaAleatoria, true);

        //}

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

        private void txtFormula_KeyUp(object sender, KeyEventArgs e)
        {
            

            if (e.Key == Key.F1) //F1 informa lo que hay que hacer
            {
                Voz.hablarAsync("Efe uno: " + txtPedido.Text);
                return;
            }

            if (e.Key == Key.F2) //F2 cambia el átomo y la valencia a resolver
            {
                atomoSeleccionadoParaElUsuario = new SumadorAtomos().elegirAtomoAleatorio();

                txtPedido.Text = "Tenés que escribir la fórmula usando el átomo:" + "\n" + atomoSeleccionadoParaElUsuario.Nombre + " con la valencia " + atomoSeleccionadoParaElUsuario.CantAtomos.ToString();
                txtFormula.Text = "";
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

                //return;
            }

            swSeManejoElEventoEnWindow = false; //se resetea
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

            if (e.Key == Key.Enter)
            {
                //ElementoEnUso atomo = new ElementoEnUso("Azufre", "S", 4, true);
                Formula f = new Formula(txtFormula.Text.Trim());

                string cadena = "";
                if (!new ComprobadorFormulaTotal(f, atomoSeleccionadoParaElUsuario, tipoMolécula.oxido, true).swFormulaBienHecha)
                    cadena = new BuscadorErroresEnFormula(f.molEnviadaenResult, new SumadorAtomos(atomoSeleccionadoParaElUsuario).molFinal, f).cadenaExplicacionError;
                else
                    cadena = "Todo está perfecto. Felicitaciones! Pudiste resolver el ejercicio más difícil. Ahora podés apretar F2 para hacer un ejercicio nuevo o ESCAPE para volver al menú inicial.";

                txtResultado.Text = cadena;
                Voz.hablarAsync(cadena);

                swSeManejoElEventoEnWindow = true;
                return;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                int pos = txtFormula.SelectionStart;
                pos--;
                if (pos < 0) pos = 0;
                if (txtFormula.Text.Length != 0)
                    Voz.hablarAsync("Borrando " + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[pos].ToString(), true));
                else
                    Voz.hablarAsync("Borraste todo");

                swSeManejoElEventoEnWindow = true;
                return;
            }
        }
    }
}
