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
    /// Lógica de interacción para VentanaSeleccionTipoEjercicio.xaml
    /// </summary>
    public partial class VentanaSeleccionTipoEjercicio : Window
    {
        tipoMolécula tipoMoleculaResultado = tipoMolécula.oxido;
        //string aux = "";

        public bool swVolviendo { get; set; }

        public VentanaSeleccionTipoEjercicio()
        {
            InitializeComponent();
            if (Voz.listarVocesPorIdioma("Español").Count <= 0) //si no hay voces en español instaladas
            {
                MessageBox.Show("Este programa necesita que en la computadora haya instalada una voz en Español, por favor instale una.", "Error");
                this.Close();
                return;
            }

            Voz.cambiarVoz(Voz.listarVocesPorIdioma("Español")[0]);

            txtInfo.Text = "Autor: Guillermo Tosccani (guillermo.toscani@gmail.com";
            List<string> listaEjercicios = new List<string>();
            switch (tipoMoleculaResultado)
            {
                case tipoMolécula.oxido:
                    //aux = "óxidos o anhidridos";
                    listaEjercicios.Add("Practicar sólo hacer óxidos o anhidridos");
                    listaEjercicios.Add("Practicar escribir una fórmula sin estequeometría");
                    listaEjercicios.Add("Practicar sólo la estequeometría de una fórmula");
                    listaEjercicios.Add("Practicar una fórmula completa, incluyendo la estequeometría");
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

            foreach (string s in listaEjercicios)
                listEjercicios.Items.Add(s);

            listEjercicios.Focus();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            abrirEjercicio();
        }

        private void abrirEjercicio()
        {
            if (listEjercicios.SelectedItem != null)
            {
                switch (listEjercicios.SelectedIndex)
                {
                    case 0: //sólo resultado
                        VentanaSoloResultado ventanaResult = new VentanaSoloResultado(tipoMoleculaResultado);
                        ventanaResult.Show();
                        this.Close();
                        break;
                    case 1: //fórmula sin estequeometría
                        VentanaFormulaSinEsteq ventanaFormSin = new VentanaFormulaSinEsteq(tipoMoleculaResultado);
                        ventanaFormSin.Show();
                        this.Close();
                        break;
                    case 2: //estequeometría
                        VentanaEstequeometria ventEsteq = new VentanaEstequeometria(tipoMoleculaResultado);
                        ventEsteq.Show();
                        this.Close();
                        break;
                    case 3: //fórmula completa
                        VentanaFormulaTotal ventTotal = new VentanaFormulaTotal(tipoMoleculaResultado);
                        ventTotal.Show();
                        this.Close();
                        break;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Voz.hablarAsync("Estás en la pantalla de inicio. Usá las flechas y enter para elegir qué tipo de ejercicio hacer");

            if (e.Key == Key.Return)
            {
                abrirEjercicio();
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)//si usa las flechas
            {
                if (listEjercicios.Items.Count != 0)
                {
                    if (listEjercicios.SelectedItem != null)
                    {
                        Voz.hablarAsync(listEjercicios.SelectedItem.ToString());
                    }
                }
            }

            if (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)//callar con control
            {
                Voz.callar();
            }
        }

        private void listEjercicios_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            abrirEjercicio();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (swVolviendo)
                Voz.hablarAsync("Volviendo a la lista de ejercicios. Elegí con flecha abajo qué tipo querés hacer y aceptá con enter");
            else
                Voz.hablarAsync("Bienvenida o bienvenido a este programa para practicar óxidos y anhidridos. Elegí con las flechas y aceptá con enter el tipo de ejercicio de que querés hacer");
        }
    }
}