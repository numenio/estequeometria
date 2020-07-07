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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Estequeometría
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Elemento> metales = new List<Elemento>();
        private List<Elemento> nometales = new List<Elemento>();
        public MainWindow()
        {
            InitializeComponent();
            Voz.hablar("hola");
            metales = new Lector_elementos().leerXML(tipoElemento.metal);
            nometales = new Lector_elementos().leerXML(tipoElemento.nometal);
            Voz.hablar("lista de metales cargada");
        }
    }
}
