﻿using System;
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

namespace WPF_Estequeometria
{
    /// <summary>
    /// Lógica de interacción para VentanaFormulaTotal.xaml
    /// </summary>
    public partial class VentanaFormulaTotal : Window
    {
        ElementoEnUso atomoSeleccionadoParaElUsuario;
        List<string> voces = Voz.listarVocesPorIdioma("Español");
        bool swSeManejoElEventoEnWindow = false;
        RandomizadorElementos ran;// = new RandomizadorElementos();
        tipoMolécula tipoMolParaHacer;


        public VentanaFormulaTotal(tipoMolécula t)
        {
            InitializeComponent();
            //ElementoEnUso atomo = new ElementoEnUso("Azufre", "S", 4, true);
            atomoSeleccionadoParaElUsuario = ran.elegirAtomoAleatorio();
            tipoMolParaHacer = t;
            ran = new RandomizadorElementos(t);

            txtInfo.Text = "Enter: Revisa la fórmula escrita. Flecha arriba o abajo: leen lo escrito con y sin mayúsculas. F1: lee el ejercicio a realizar.F2: cambia el átomo para hacer una fórmula nueva. F5, F6 y F7 modifican la voz. Control: callar la voz. Escape: volver\nAutor: Guillermo Toscani (guillermo.toscani@gmail.com)";
            txtPedido.Text = "Tenés que usar el átomo:" + "\n" + atomoSeleccionadoParaElUsuario.Nombre + " con la valencia " + atomoSeleccionadoParaElUsuario.CantAtomos.ToString() + " para hacer la fórmula completa";
            txtFormula.Focus();
            Voz.hablarAsync("Aquí " + txtPedido.Text + ". Acordate de poner los átomos que suman, el resultado, y la estequiometría.  Para escuchar la ayuda, apretá efe tres");

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
                atomoSeleccionadoParaElUsuario = ran.elegirAtomoAleatorio();

                txtPedido.Text = "Tenés que escribir la fórmula usando el átomo:" + "\n" + atomoSeleccionadoParaElUsuario.Nombre + " con la valencia " + atomoSeleccionadoParaElUsuario.CantAtomos.ToString();
                txtFormula.Text = "";
                txtResultado.Text = "";
                txtFormula.Focus();
                Voz.hablarAsync("Efe dos, Ejercicio nuevo: " + txtPedido.Text);
                return;
            }

            if (e.Key == Key.F3) //F3 lee la ayuda
            {
                Voz.hablarAsync("Efe tres, leer ayuda: " + txtInfo.Text);
                return;
            }

            if (e.Key == Key.Up) //flecha arriba lee lo que ya está escrito en el cuadro de texto diciendo las mayúsculas
            {
                if (txtFormula.Text.Trim() != "")
                    Voz.hablarAsync(new ValidadorCadenas().separarCadenaconEspacios(txtFormula.Text, true));
                else
                    Voz.hablarAsync("No hay nada escrito");

                return;
            }

            if (e.Key == Key.Down) //flecha abajo lee lo que ya está escrito en el cuadro de texto sin decir las mayúsculas
            {
                if (txtFormula.Text.Trim() != "")
                    Voz.hablarAsync(new ValidadorCadenas().separarCadenaconEspacios(txtFormula.Text, false));
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
                    else
                    {
                        Voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true, true));
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
                    if (txtFormula.SelectionStart == txtFormula.Text.Length) //si está al final del cuadro
                        Voz.hablarAsync("Última letra: " + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true, true));
                    else
                    {
                        Voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[txtFormula.SelectionStart - 1].ToString(), true, true));
                    }
                }
                return;
            }

            if (e.Key == Key.F7)
            {
                int pos = voces.IndexOf(Voz.vozActual());
                if (pos >= voces.Count - 1) pos = -1;
                pos++;
                if (voces.Count > 1) //si más de una voz una sola 
                {
                    Voz.cambiarVoz(voces[pos]);
                    Voz.hablarAsync("elegiste mi voz para hablarte");
                }
                else
                    Voz.hablarAsync("hay una sola voz instalada en la computadora. Si quiere cambiarla por favor instale otra");


                return;
            }


            if (!swSeManejoElEventoEnWindow)
            {
                
                int posCursor = txtFormula.SelectionStart;
                posCursor--;
                if (posCursor < 0) posCursor = 0;
                if (txtFormula.Text.Length != 0)
                    Voz.hablarAsync(new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[posCursor].ToString(), true, true));

                //return;
            }

            swSeManejoElEventoEnWindow = false; //se resetea
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                VentanaSeleccionTipoEjercicio v = new VentanaSeleccionTipoEjercicio(tipoMolParaHacer);
                v.swVolviendo = true;
                v.Show();
                this.Close();
                return;
            }

            if (e.Key == Key.Enter)
            {
                Formula f = new Formula(txtFormula.Text.Trim(), tipoMolParaHacer);

                string cadena = "";
                if (!new ComprobadorFormulaTotal(f, atomoSeleccionadoParaElUsuario, tipoMolParaHacer, true).swFormulaBienHecha)
                    cadena = new BuscadorErroresEnFormula(f.molEnviadaenResult, new SumadorAtomos(atomoSeleccionadoParaElUsuario, tipoMolParaHacer).molFinal, f, atomoSeleccionadoParaElUsuario, tipoMolParaHacer).cadenaExplicacionError;
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
            if (e.Key == Key.Delete)
            {
                Voz.hablarAsync("no se puede usar la tecla suprimir");
                e.Handled = true;
            }

            if (e.Key == Key.Back)
            {
                int pos = txtFormula.SelectionStart;
                pos--;
                if (pos < 0) pos = 0;
                if (txtFormula.Text.Length != 0)
                    Voz.hablarAsync("Borrando " + new ValidadorCadenas().traducirCadenaParaLeer(txtFormula.Text[pos].ToString(), true, true));
                else
                    Voz.hablarAsync("Borraste todo");

                swSeManejoElEventoEnWindow = true;
                return;
            }
        }
    }
}
