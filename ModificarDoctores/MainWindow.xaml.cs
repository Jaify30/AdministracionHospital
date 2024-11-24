using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace ModificarDoctores
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int idDoctor;
        Entidades.Program.Doctores Doctores;
        public MainWindow(Entidades.Program.Doctores doctor)
        {
            InitializeComponent();
            LlenarComboBox();
            idDoctor = doctor.Id;
            EmailG.Text = doctor.Email;
            DocumentoG.Text=doctor.Documento.ToString();
            NombreG.Text = doctor.Nombre;
            ApellidoG.Text = doctor.Apellido;
            TelefonoG.Text = doctor.Telefono;
            FechaIngreso.Text=doctor.FechaIngreso.ToString();
            FechaNacimiento.Text=doctor.FechaNacimiento.ToString();
            NacionalidadG.Text = doctor.Nacionalidad;
            Domicilio.Text = doctor.Domicilio;
            Localidad.Text = doctor.Localidad;
            cmbCargos.Text = doctor.Cargo;
            MatriculaG.Text = doctor.Matricula;
            Doctores = doctor;
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void LlenarComboBox()
        {
            List<string> cargos = new List<string>()
            {
                "Alergólogo",
                "Anestesiólogo",
                "Cardiólogo",
                "Cardiólogo intervencionista",
                "Cirujano",
                "Cirujano cardiovascular",
                "Cirujano de colon y recto",
                "Cirujano de mano",
                "Cirujano maxilofacial",
                "Cirujano neurólogo",
                "Cirujano ortopédico",
                "Cirujano plástico",
                "Cirujano torácico",
                "Cirujano vascular",
                "Dermatólogo",
                "Endocrinólogo",
                "Endocrinólogo especialista en reproducción",
                "Especialista en el manejo del dolor",
                "Especialista en electrofisiología cardíaca",
                "Especialista en enfermedades contagiosas",
                "Especialista en fisioterapia y rehabilitación",
                "Especialista en lesiones de la médula espinal",
                "Especialista en medicina de urgencias",
                "Especialista en medicina del deporte",
                "Especialista en medicina hiperbárica",
                "Especialista en medicina intensiva",
                "Especialista en medicina laboral",
                "Especialista en medicina nuclear",
                "Especialista en programas y cuidados paliativos",
                "Especialista en trastornos del sueño",
                "Gastroenterólogo",
                "Ginecólogo",
                "Hematólogo",
                "Hepatólogo",
                "Hospitalista",
                "Médico de medicina familiar",
                "Médico especialista en adolescentes",
                "Médico especialista en geriatría",
                "Médico especialista en medicina interna",
                "Médico forense",
                "Médico genético",
                "Nefrólogo",
                "Neonatólogo",
                "Neumólogo",
                "Neurólogo",
                "Obstetra",
                "Oftalmólogo",
                "Oncólogo",
                "Oncólogo ginecológico",
                "Oncólogo radiólogo",
                "Otorrinolaringólogo",
                "Patólogo",
                "Patólogo forense",
                "Pediatra",
                "Pediatra especialista en desarrollo",
                "Perinatólogo",
                "Psiquiatra",
                "Psiquiatra especialista en adicciones",
                "Radiólogo",
                "Reumatólogo",
                "Urólogo"
            };
            cmbCargos.ItemsSource = cargos;
        }
        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("¿Está seguro de que desea modificar los datos del paciente?",
          "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                if (string.IsNullOrWhiteSpace(EmailG.Text) || string.IsNullOrWhiteSpace(DocumentoG.Text) || string.IsNullOrWhiteSpace(NombreG.Text) || string.IsNullOrWhiteSpace(ApellidoG.Text) 
                    || string.IsNullOrWhiteSpace(TelefonoG.Text) || string.IsNullOrWhiteSpace(FechaIngreso.Text) || string.IsNullOrWhiteSpace(NacionalidadG.Text) || string.IsNullOrWhiteSpace(Domicilio.Text)
                    || string.IsNullOrWhiteSpace(Localidad.Text) || string.IsNullOrWhiteSpace(cmbCargos.Text) || string.IsNullOrWhiteSpace(MatriculaG.Text) || string.IsNullOrWhiteSpace(FechaNacimiento.Text))
                {
                    MessageBox.Show("No puede dejar ningun campo en blanco...", "ATENCION");
                    return;
                }
                else
                {
                    Doctores.Id = idDoctor;
                    Doctores.Email = EmailG.Text;
                    Doctores.Documento = int.Parse(DocumentoG.Text); // Asegúrate de que esto es un número válido
                    Doctores.Nombre = NombreG.Text;
                    Doctores.Apellido = ApellidoG.Text;
                    Doctores.Telefono = TelefonoG.Text;
                    Doctores.FechaIngreso = DateTime.Parse(FechaIngreso.Text); // Manejar posibles errores de formato
                    Doctores.FechaNacimiento = DateTime.Parse(FechaNacimiento.Text); // Manejar posibles errores de formato
                    Doctores.Nacionalidad = NacionalidadG.Text;
                    Doctores.Domicilio = Domicilio.Text;
                    Doctores.Localidad = Localidad.Text;
                    Doctores.Cargo = cmbCargos.Text;
                    Doctores.Matricula = MatriculaG.Text;

                    Funciones.Program.ModificarDoctores(Doctores);
                    MessageBox.Show("Paciente modificado correctamente.");
                    //funcion de modificar
                }
            }
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
