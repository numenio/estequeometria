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
    /// Lógica de interacción para VentanaInicio.xaml
    /// </summary>
    public partial class VentanaInicio : Window
    {
        //tipoMolécula tipoMoleculaResultado;

        public bool swVolviendo { get; set; }

        public VentanaInicio()
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

            listEjercicios.Items.Add("Trabajar con óxidos o anhidridos");
            listEjercicios.Items.Add("Trabajar con ácidos");
            listEjercicios.Items.Add("Trabajar con hidróxidos");
            listEjercicios.Items.Add("Trabajar con sales");
            
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
                tipoMolécula t = tipoMolécula.oxido;
                switch (listEjercicios.SelectedIndex)
                {
                    case 0: //óxidos o anhidridos
                        t = tipoMolécula.oxido;
                        break;
                    case 1: //ácidos
                        t = tipoMolécula.acido;
                        break;
                    case 2: //hidróxidos
                        t = tipoMolécula.hidroxido;
                        break;
                    case 3: //sales
                        t = tipoMolécula.sal;
                        break;
                }

                if (t == tipoMolécula.sal) //hacer: quitar cuando estén hechas las funciones
                {
                    Voz.hablarAsync("Todavía no me hacen esta función");
                    return;
                }

                VentanaSeleccionTipoEjercicio ventanaSeleccion = new VentanaSeleccionTipoEjercicio(t);
                ventanaSeleccion.Show();
                this.Close();
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
                Voz.hablarAsync("Volviendo a la lista de selección de molécula a realizar. Elegí con flecha abajo qué tipo querés hacer y aceptá con enter");
            else
                Voz.hablarAsync("Bienvenida o bienvenido a este programa de fórmulas químicas. Elegí con las flechas y aceptá con enter el tipo de molécula con que querés trabajar");
        }
    }
}
