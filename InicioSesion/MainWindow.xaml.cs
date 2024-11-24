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

namespace InicioSesion
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

        private void IniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            if (Funciones.Program.EsCorreoValido(CorreG.Text) == false)
            {
                MessageBox.Show("Ingrese un formato tipo Email. Ejemplo: Ejemplo12345@ejemplo.com");
                return;
            }

            Entidades.Program.Empleados empleado = new Entidades.Program.Empleados()
            {
                Email = CorreG.Text,
                Password = PassG.Password
            };

            if (Funciones.Program.VerificacionLogin(empleado)>0)
            {
                //Crear el objeto de la pantalla de administracion y cerrar la catual y mostar la nueva
                Administracion.MainWindow administracion = new Administracion.MainWindow(CorreG.Text);
                this.Close();
                administracion.Show();
            }
            else
            {
                MessageBox.Show("Las credenciales no son validas.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Registrar_Click(object sender, RoutedEventArgs e)
        {
            Unique_pass.MainWindow Unique_pass = new Unique_pass.MainWindow();
            Unique_pass.ShowDialog();
        }
        private void Close_Click(object sender, RoutedEventArgs e)//Cierre De ventana
        {
            this.Close();
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}
