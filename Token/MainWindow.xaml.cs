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

namespace Unique_pass
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
        private void Button_Click(object sender, RoutedEventArgs e)//btn de cerrar
        {
            this.Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)//btn de minimizar
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Ingresar_Click(object sender, RoutedEventArgs e)
        {
            Entidades.Program.Administradores admin = new Entidades.Program.Administradores();

            admin.Unique_pass=Unique_passG.Password;

            if (Funciones.Program.IngresoUnique_pass(admin.Unique_pass) >0)
            {
                SeleccionRegistro.MainWindow seleccionRegistro = new SeleccionRegistro.MainWindow();
                this.Close();
                seleccionRegistro.ShowDialog();
            }
            else
            {
                MessageBox.Show("El Unique_pass no es válido", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
