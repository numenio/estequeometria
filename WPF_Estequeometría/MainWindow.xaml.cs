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
        
        public MainWindow()
        {
            InitializeComponent();
            Voz.hablar("hola");
            //Molecula borrame = new Molecula(" 2  O 3 3Cl24 ");
            ElementoEnUso atomo = new ElementoEnUso("Aluminio", "Al", 4, true);
            SumadorAtomos s = new SumadorAtomos(atomo);
            //Molecula borrame2 = s.combinarElemconOxigeno();
            Voz.hablar("lista de metales cargada");
        }
    }
}
