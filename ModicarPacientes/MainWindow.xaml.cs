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

namespace ModicarPacientes
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int idPaciente;
        public MainWindow(Entidades.Program.Pacientes pacientes)
        {       
            
            InitializeComponent();
            idPaciente = pacientes.IdPacientes;
            EmailG.Text = pacientes.Email;
            DocumentoG.Text = pacientes.Documento.ToString();
            NombreG.Text = pacientes.Nombre;
            ApellidoG.Text = pacientes.Apellido;
            TelefonoG.Text = pacientes.Telefono;
            FechaIngreso.Text = pacientes.FechaIngreso.ToString();
            FechaNacimiento.Text = pacientes.FechaNacimiento.ToString();
            LegajoG.Text = pacientes.Legajo;
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult resultado = MessageBox.Show(
                "¿Está seguro de que desea modificar los datos del paciente?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (resultado != MessageBoxResult.Yes) return;

            string[] camposObligatorios = { EmailG.Text,DocumentoG.Text,NombreG.Text,ApellidoG.Text,TelefonoG.Text,FechaNacimiento.Text,FechaIngreso.Text,LegajoG.Text};

            if (Funciones.Program.CamposVacios(camposObligatorios))
            {
                MessageBox.Show("No puede dejar ningún campo en blanco...", "ATENCIÓN");
                return;
            }

            if (!Funciones.Program.ValidarEmail(EmailG.Text))
            {
                Funciones.Program.MostrarError("Por favor, ingrese un email válido.", "Email Inválido");
                return;
            }
            if (!Funciones.Program.ValidarSoloNumeros(DocumentoG.Text, out int numero, "Documento"))
            {
                Funciones.Program.MostrarError("El documento ingresado no es válido.", "Documento Inválido");
                return;
            }
            if (!Funciones.Program.ValidarTelefono(TelefonoG.Text))
            {
                Funciones.Program.MostrarError("Por favor, ingrese un número de teléfono válido.", "Teléfono Inválido");
                return;
            }
            if (!Funciones.Program.VerificarFecha(FechaIngreso.SelectedDate, "Ingreso") ||
                !Funciones.Program.VerificarFecha(FechaNacimiento.SelectedDate, "Nacimiento"))
            {
                return;
            }
            if (NombreG.Text.Length < 1 || NombreG.Text.Length > 50)
            {
                MessageBox.Show("Ingrese entre 1 a 50 caracteres en Nombre.");
                return ;
            }
            if (ApellidoG.Text.Length < 1 || ApellidoG.Text.Length > 50)
            {
                MessageBox.Show("Ingrese entre 1 a 50 caracteres en Apellido.");
                return;
            }
            if (LegajoG.Text.Length < 1 || LegajoG.Text.Length > 10)
            {
                MessageBox.Show("Ingrese entre 1 a 10 caracteres en Legajo.");
                return;
            }
            if (NombreG.Text.Length < 1 || NombreG.Text.Length > 8)
            {
                MessageBox.Show("Ingrese un documento valido.");
                return;
            }

            Funciones.Program.ModificarPaciente(idPaciente,EmailG.Text,int.Parse(DocumentoG.Text),NombreG.Text,ApellidoG.Text,TelefonoG.Text,DateTime.Parse(FechaIngreso.Text),DateTime.Parse(FechaNacimiento.Text),LegajoG.Text);
            MessageBox.Show("Paciente modificado correctamente.");

        }

        private void FechaIngreso_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!FechaIngreso.SelectedDate.HasValue)
            {
                ErrorTextBlock1.Text = "Debe seleccionar una fecha.";
                ErrorTextBlock1.Visibility = Visibility.Visible;
            }
            else
            {
                DateTime selectedDate = FechaIngreso.SelectedDate.Value;
                DateTime currentDate = DateTime.Now;
                DateTime minimumDate = currentDate.AddYears(-150);

                if (selectedDate > currentDate || selectedDate < minimumDate)
                {
                    ErrorTextBlock1.Text = "La fecha seleccionada no es válida.";
                    ErrorTextBlock1.Visibility = Visibility.Visible;
                    FechaIngreso.SelectedDate = null; // Opcional: Limpia la selección
                }
                else
                {
                    ErrorTextBlock1.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void FechaNacimiento_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!FechaNacimiento.SelectedDate.HasValue)
            {
                ErrorTextBlock.Text = "Debe seleccionar una fecha.";
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                DateTime selectedDate = FechaNacimiento.SelectedDate.Value;
                DateTime currentDate = DateTime.Now;
                DateTime minimumDate = currentDate.AddYears(-150);

                int edad = currentDate.Year - selectedDate.Year;
                if (currentDate.Month < selectedDate.Month ||
                (currentDate.Month == selectedDate.Month && currentDate.Day < selectedDate.Day))
                {
                    edad--;
                }

                if (selectedDate > currentDate || selectedDate < minimumDate)
                {
                    ErrorTextBlock.Text = "La fecha seleccionada no es válida.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    FechaNacimiento.SelectedDate = null; // Opcional: Limpia la selección
                }
                else
                {
                    ErrorTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
