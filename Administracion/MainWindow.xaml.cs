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
                Mostrar_auxiliares.Visibility = Visibility.Hidden;
                Mostrar_Doctores.Visibility = Visibility.Hidden;
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
            Data_Pacientes.Visibility = Visibility.Hidden;
            Data_Doctores.Visibility = Visibility.Hidden;
            Data_Auxiliares.Visibility = Visibility.Hidden;
            Data_Empleados.HeadersVisibility = DataGridHeadersVisibility.All;

            List<Entidades.Program.Empleados> empleados = Funciones.Program.ObtenerTodosLosEmpleados();

            if (empleados == null || !empleados.Any())
            {
                MessageBox.Show("No se han encontrado empleados.");
                return;
            }

            Data_Empleados.ItemsSource = empleados;
            Data_Empleados.Visibility = Visibility.Visible;
        }
        private void MostrarContraseña_Click(object sender, RoutedEventArgs e)
        {
            Entidades.Program.Empleados empleado = Funciones.Program.ObtenerDatosEmpleadoPorEmail(emailG);

            string token = empleado.Token;

            // Mostrar la token

            MessageBox.Show($"Su token es {token}", "token");
        }//Muestra el token del usuario si es que lo tiene.
        private void Mostrar_Doctores_Click(object sender, RoutedEventArgs e)
        {
            Data_Empleados.Visibility = Visibility.Hidden;
            Data_Auxiliares.Visibility = Visibility.Hidden;
            Data_Pacientes.Visibility = Visibility.Hidden;
            Data_Doctores.HeadersVisibility = DataGridHeadersVisibility.All;

            List<Doctores> doctores = Funciones.Program.ObtenerDoctores();

            if (doctores == null || !doctores.Any())
            {
                MessageBox.Show("No se han encontrado empleados.");
                return;
            }

            Data_Doctores.ItemsSource = doctores;
            Data_Doctores.Visibility = Visibility.Visible;
        }
        private void Mostrar_auxiliares_Click_1(object sender, RoutedEventArgs e)
        {
            // Ocultar otros DataGrids
            Data_Empleados.Visibility = Visibility.Hidden;
            Data_Doctores.Visibility = Visibility.Hidden;
            Data_Pacientes.Visibility = Visibility.Hidden;

            // Configurar el DataGrid de auxiliares
            Data_Auxiliares.HeadersVisibility = DataGridHeadersVisibility.All;

            // Obtener la lista de auxiliares
            List<EmpleadosAux> auxiliar = Funciones.Program.ObtenerAuxiliares();

            // Verificar si la lista contiene datos
            if (auxiliar == null || !auxiliar.Any())
            {
                MessageBox.Show("No se han encontrado empleados auxiliares.");
                return;
            }

            // Asignar la lista al DataGrid
            Data_Auxiliares.ItemsSource = auxiliar;

            // Hacer visible el DataGrid
            Data_Auxiliares.Visibility = Visibility.Visible;
        }

        private void Mostrar_Pacientes_Click(object sender, RoutedEventArgs e)
        {
            
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
