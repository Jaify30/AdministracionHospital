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
using static Entidades.Program;

namespace Administracion
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string emailG;
        public MainWindow(string Email)
        {
            InitializeComponent();
            emailG = Email;
            Loaded += MainWindow_Loaded;

            Entidades.Program.Empleados empleados = Funciones.Program.ObtenerDatosEmpleadoPorEmail(emailG);
            if (empleados.Token == null)
            {
                token.Visibility = Visibility.Hidden;
                Mostrar_Empleados.Visibility = Visibility.Hidden;
            }
            if (empleados.Token != null)
            {
                Data_Empleados.Visibility = Visibility.Visible;
                Mostrar_Empleados.Visibility = Visibility.Visible;
            }
        }

        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Entidades.Program.Empleados empleado = Funciones.Program.ObtenerDatosEmpleadoPorEmail(emailG);

            NombreUsuario.Text = $"Bienvenido {empleado.Nombre} {empleado.Apellido}";
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Mostrar_Empleados_Click(object sender, RoutedEventArgs e)
        {
            Data_Empleados.HeadersVisibility = DataGridHeadersVisibility.All;
            List<Entidades.Program.Empleados> empleado = Funciones.Program.ObtenerTodosLosEmpleados();
            Data_Empleados.ItemsSource = empleado;
        }

        private void Mostrar_Pacientes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MostrarContraseña_Click(object sender, RoutedEventArgs e)
        {
            Entidades.Program.Empleados empleado = Funciones.Program.ObtenerDatosEmpleadoPorEmail(emailG);

            string token = empleado.Token;

            // Mostrar la token

            MessageBox.Show($"Su token es {token}", "token");
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Agregar_Paciente_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
