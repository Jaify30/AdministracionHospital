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

namespace VerHistorial
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string emailG;
        public static string nombreG;
        public static string apellidoG;
        public static int IdG;
        public MainWindow(string Email, string Nombre, string Apellido, int Id)
        {
            InitializeComponent();
            emailG = Email;
            nombreG = Nombre;
            apellidoG = Apellido;
            IdG = Id;
            Loaded += MainWindow_Loaded;
        }
        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Entidades.Program.Pacientes paciente = Funciones.Program.ObtenerHistorialPacientePorEmail(emailG);
            Historial.Text = paciente.Historial;
            NombreUsuario.Text = $"{nombreG} {apellidoG}";
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirmar_Cambios_Click(object sender, RoutedEventArgs e)
        {
            int resultado = Funciones.Program.ActualizarHistorialPaciente(IdG,Historial.Text);
            if (resultado > 0)
            {
                MessageBox.Show("Historial actualizado correctamente");
                this.Close();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el historial");
            }
        }

        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
