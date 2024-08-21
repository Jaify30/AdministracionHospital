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

namespace SeleccionRegistro
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SeleccionMedicos_Click(object sender, RoutedEventArgs e)
        {
            RegistroMed.MainWindow registroMed = new RegistroMed.MainWindow();
            this.Close();
            registroMed.ShowDialog();
        }

        private void SeleccionAuxiliares_Click(object sender, RoutedEventArgs e)
        {
            RegistroAux.MainWindow registroAux = new RegistroAux.MainWindow();
            this.Close();
            registroAux.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
