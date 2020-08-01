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

namespace WPF_Estequeometria
{
    /// <summary>
    /// Lógica de interacción para VentanaSeleccionTipoEjercicio.xaml
    /// </summary>
    public partial class VentanaSeleccionTipoEjercicio : Window
    {
        tipoMolécula tipoMoleculaResultado;

        public bool swVolviendo { get; set; }

        public VentanaSeleccionTipoEjercicio(tipoMolécula t)
        {
            InitializeComponent();
            
            tipoMoleculaResultado = t;

            txtInfo.Text = "Autor: Guillermo Tosccani (guillermo.toscani@gmail.com";
            List<string> listaEjercicios = new List<string>();
            switch (tipoMoleculaResultado)
            {
                case tipoMolécula.anhidrido:
                case tipoMolécula.oxido:
                    listaEjercicios.Add("Practicar sólo hacer óxidos o anhidridos");
                    listaEjercicios.Add("Practicar escribir una fórmula de un óxido sin estequiometría");
                    listaEjercicios.Add("Practicar sólo la estequiometría de una fórmula de un óxido");
                    listaEjercicios.Add("Practicar una fórmula completa de un óxido, incluyendo la estequiometría");
                    break;
                case tipoMolécula.acido:
                    listaEjercicios.Add("Practicar sólo hacer ácidos");
                    listaEjercicios.Add("Practicar escribir una fórmula de un ácido sin estequiometría");
                    listaEjercicios.Add("Practicar sólo la estequiometría de una fórmula de un ácido");
                    listaEjercicios.Add("Practicar una fórmula completa de un ácido, incluyendo la estequiometría");
                    break;
                case tipoMolécula.hidroxido:
                    listaEjercicios.Add("Practicar sólo hacer hidróxidos");
                    listaEjercicios.Add("Practicar escribir una fórmula de un hidróxido sin estequiometría");
                    listaEjercicios.Add("Practicar sólo la estequiometría de una fórmula de un hidróxido");
                    listaEjercicios.Add("Practicar una fórmula completa de un hidróxido, incluyendo la estequiometría");
                    break;
                case tipoMolécula.sal:
                    listaEjercicios.Add("Practicar sólo hacer sales");
                    listaEjercicios.Add("Practicar escribir una fórmula de una sal sin estequiometría");
                    listaEjercicios.Add("Practicar sólo la estequiometría de una fórmula de una sal");
                    listaEjercicios.Add("Practicar una fórmula completa de una sal, incluyendo la estequiometría");
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
                    case 1: //fórmula sin estequiometría
                        VentanaFormulaSinEsteq ventanaFormSin = new VentanaFormulaSinEsteq(tipoMoleculaResultado);
                        ventanaFormSin.Show();
                        this.Close();
                        break;
                    case 2: //estequiometría
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
            {
                VentanaInicio v = new VentanaInicio();
                v.swVolviendo = true;
                v.Show();
                this.Close();
                return;
            }

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
                Voz.hablarAsync("Abriendo la lista de ejercicios. Elegí con las flechas y aceptá con enter el tipo que querés hacer");
        }
    }
}