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
using System.Net;
using System.Net.Mail;

namespace RegistroMed
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LlenarComboBox();
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
        }//Llena con datos el combo box con las diferentes especialidades
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }//boton para cerrar la ventana
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }//boton para minimizar la ventana
        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
            
        }//boton para volver al inicio de sesion

        private void Button_Click(object sender, RoutedEventArgs e)//Btn que registra a los medicos
        {
            int numero;
            Entidades.Program.Doctores doctor = new Entidades.Program.Doctores();
            Entidades.Program.Empleados empleado = new Entidades.Program.Empleados();

            // Validar Email
            if (!Funciones.Program.ValidarEmail(EmailG.Text))
            {
                MessageBox.Show("Por favor, ingrese un email válido.", "Email Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            empleado.Email = EmailG.Text;

            // Validar Contraseña
            if (!Funciones.Program.ValidarContrasena(PassG.Password, Pass2G.Password))
            {
                MessageBox.Show("Las contraseñas no coinciden o no son válidas.", "Contraseña Inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            empleado.Password = PassG.Password;

            // Validar Documento
            if (!Funciones.Program.ValidarSoloNumeros(Documento.Text, out numero, "Documento"))
            {
                MessageBox.Show("El documento ingresado no es válido.", "Documento Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!Funciones.Program.ValidarSoloNumeros(Documento.Text, out numero, "Documento"))
            {
                MessageBox.Show("El documento ingresado no es válido.", "Documento Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            empleado.Documento = numero;

            // Validar Teléfono
            string patron1 = @"^\d{10}$";
            string patron2 = @"^\d{3}[-\s]?\d{3}[-\s]?\d{4}$";
            string patron3 = @"^\+?\d{1,3}?[-.\s]?\(?\d{1,4}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$";

            Telefono.Text = Telefono.Text.Replace(" ", "");

            if (!Regex.IsMatch(Telefono.Text, patron1) && !Regex.IsMatch(Telefono.Text, patron2) && !Regex.IsMatch(Telefono.Text, patron3))
            {
                MessageBox.Show("Por favor, ingrese un número de teléfono válido.", "Teléfono Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            empleado.Telefono = Telefono.Text;

            // Verificar Fecha de Ingreso
            if (!Funciones.Program.VerificarFecha(FechaIngreso.SelectedDate, "Ingreso"))
            {
                return;
            }
            empleado.FechaIngreso = FechaIngreso.SelectedDate.Value;

            // Verificar Fecha de Nacimiento
            if (!Funciones.Program.VerificarFecha(FechaNacimiento.SelectedDate, "Nacimiento"))
            {
                return;
            }
            empleado.FechaNacimiento = FechaNacimiento.SelectedDate.Value;

            // Validar Nacionalidad
            if (!Funciones.Program.ValidarLongitudTexto(NacionalidadG, 1, 50, "Nacionalidad",empleado.Nacionalidad))
            {
                MessageBox.Show("La nacionalidad es inválida.", "Nacionalidad Inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            empleado.Nacionalidad = NacionalidadG.Text;

            // Validar Nombre
            if (!Funciones.Program.ValidarLongitudTexto(NombreG, 1, 50, "Nombre",empleado.Nombre))
            {
                MessageBox.Show("El nombre es inválido.", "Nombre Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            empleado.Nombre = NombreG.Text;

            // Validar Apellido
            if (!Funciones.Program.ValidarLongitudTexto(Apellido, 1, 50, "Apellido", empleado.Apellido))
            {
                MessageBox.Show("El apellido es inválido.", "Apellido Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            empleado.Apellido = Apellido.Text;

            // Validar Domicilio
            if (!Funciones.Program.ValidarLongitudTexto(Domicilio, 1, 50, "Domicilio",empleado.Domicilio))
            {
                MessageBox.Show("El domicilio es inválido.", "Domicilio Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            empleado.Domicilio = Domicilio.Text;

            // Validar Localidad
            if (!Funciones.Program.ValidarLongitudTexto(Localidad, 1, 50, "Localidad", empleado.Localidad))
            {
                MessageBox.Show("La localidad es inválida.", "Localidad Inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            empleado.Localidad = Localidad.Text;

            // Asignar Cargo
            doctor.Cargo = cmbCargos.Text;

            if (Funciones.Program.ValidarLongitudTexto(Matricula, 1, 20, "Matricula", doctor.Matricula))//Validacion matricula
            {
                doctor.Matricula = Matricula.Text;
            }


            // Intentar registrar el empleado y doctor
            int agregar = Funciones.Program.RegistrarDoctores(doctor, empleado);

            if (agregar > 0)
            {
                MessageBox.Show("El registro se ha realizado con éxito.", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                EmailG.Text = "";
                PassG.Password = "";
                Pass2G.Password = "";
                Documento.Text = "";
                NacionalidadG.Text = "";
                NombreG.Text = "";
                Apellido.Text = "";
                cmbCargos.Text = "";
                Domicilio.Text = "";
                Telefono.Text = "";
                Matricula.Text = "";
                Localidad.Text = "";
                FechaIngreso.SelectedDate = null;
                FechaNacimiento.SelectedDate = null;
            }
            else
            {
                MessageBox.Show("Ocurrió un error al registrar los datos.", "Error de Registro", MessageBoxButton.OK, MessageBoxImage.Error);
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
