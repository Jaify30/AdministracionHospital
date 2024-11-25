using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;
using static Entidades.Program;

namespace RegistroPacientes
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int IdDoctor;
        public MainWindow(int Id)
        {
            InitializeComponent();
            IdDoctor = Id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }//Boton para volver

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Entidades.Program.Pacientes pacienteNuevo = new Entidades.Program.Pacientes();
            int numero;

            if (!Funciones.Program.ValidarEmail(EmailG.Text))
            {
                MessageBox.Show("Por favor, ingrese un email válido.", "Email Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                pacienteNuevo.Email = EmailG.Text;
            }

            if (!Funciones.Program.ValidarSoloNumeros(DocumentoG.Text, out numero, "Documento"))
            {
                MessageBox.Show("El documento ingresado no es válido.", "Documento Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                pacienteNuevo.Documento = numero;
            }

            if (!Funciones.Program.ValidarLongitudTexto(NombreG, 1, 50, "Nombre", pacienteNuevo.Nombre))
            {
                MessageBox.Show("El nombre es inválido.", "Nombre Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                pacienteNuevo.Nombre = NombreG.Text;
            }

            if (!Funciones.Program.ValidarLongitudTexto(ApellidoG, 1, 50, "Apellido", pacienteNuevo.Apellido))
            {
                MessageBox.Show("El apellido es inválido.", "Apellido Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                pacienteNuevo.Apellido = ApellidoG.Text;
            }

            string patron1 = @"^\d{10}$";
            string patron2 = @"^\d{3}[-\s]?\d{3}[-\s]?\d{4}$";
            string patron3 = @"^\+?\d{1,3}?[-.\s]?\(?\d{1,4}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$";

            if (!Regex.IsMatch(TelefonoG.Text, patron1) && !Regex.IsMatch(TelefonoG.Text, patron2) && !Regex.IsMatch(TelefonoG.Text, patron3))
            {
                MessageBox.Show("Por favor, ingrese un número de teléfono válido.", "Teléfono Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                pacienteNuevo.Telefono = TelefonoG.Text;
            }

            if (!Funciones.Program.VerificarFecha(FechaIngreso.SelectedDate, "Ingreso"))
            {
                return;
            }
            else
            {
                pacienteNuevo.FechaIngreso = FechaIngreso.SelectedDate.Value;
            }

            if (!Funciones.Program.VerificarFecha(FechaNacimiento.SelectedDate, "Nacimiento"))
            {
                return;
            }
            else
            {
                pacienteNuevo.FechaNacimiento = FechaNacimiento.SelectedDate.Value;
            }

            if (!Funciones.Program.ValidarLongitudTexto(LegajoG, 1, 50, "Legajo", pacienteNuevo.Legajo))
            {
                MessageBox.Show("El Legajo es inválido.", "Legajo Invalido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                pacienteNuevo.Legajo = LegajoG.Text;
            }

            pacienteNuevo.Historial = Historial.Text;
            pacienteNuevo.IdDoctor = IdDoctor;

            int agregar = Funciones.Program.RegistrarPacientes(pacienteNuevo);

            if (agregar > 0)
            {
                EmailG.Text = "";
                DocumentoG.Text = "";
                NombreG.Text = "";
                ApellidoG.Text = "";
                TelefonoG.Text = "";
                FechaIngreso.SelectedDate = null;
                FechaNacimiento.SelectedDate = null;
                LegajoG.Text = "";
                Historial.Text = "";
                MessageBox.Show("Paciente agregado correctamente");
            }
        }//Btn para registrar los pacientes
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
