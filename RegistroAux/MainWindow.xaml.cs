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

namespace RegistroAux
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
                "Oficina Administrativa",
                "Asistente Medico Clinico",
                "Asistente Tencnico Medico",
                "Flebotomista",
                "Limpieza",
                "Camilleros",
                "Enfermeros/as",
                "Paramedicos",
                "Contadores",
                "Administrativo",
                "Tecnicos",
                "Recursos Humanos",
                "Otros"
            };
            cmbCargos.ItemsSource = cargos;
        }//Llena con datos el combo box con las diferentes especialidades

        private void Button_Click(object sender, RoutedEventArgs e)//Btn que registra a los medicos
        {
            int numero;
            Entidades.Program.EmpleadosAux empleadosAux = new Entidades.Program.EmpleadosAux();
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
            if (Funciones.Program.DocumentoExiste(int.Parse(Documento.Text)))//Verifica, si el empleados existe en la base de datos mediante el numero de documento.
            {
                MessageBox.Show("El empleado ingresado, ya se encuentra en la base de datos.");
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
            if (!Funciones.Program.ValidarLongitudTexto(NacionalidadG, 1, 50, "Nacionalidad", empleado.Nacionalidad))
            {
                MessageBox.Show("La nacionalidad es inválida.", "Nacionalidad Inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            empleado.Nacionalidad = NacionalidadG.Text;

            // Validar Nombre
            if (!Funciones.Program.ValidarLongitudTexto(NombreG, 1, 50, "Nombre", empleado.Nombre))
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
            if (!Funciones.Program.ValidarLongitudTexto(Domicilio, 1, 50, "Domicilio", empleado.Domicilio))
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
            empleadosAux.Cargo = cmbCargos.Text;

            

            // Intentar registrar el empleado y doctor
            int agregar = Funciones.Program.RegistrarAuxiliares(empleadosAux,empleado);

            if (agregar > 0)
            {
                
                if (PermisosSi.IsChecked == true)
                {
                    Entidades.Program.Administradores admin = new Entidades.Program.Administradores();
                    admin.Id = agregar; // Asignar el mismo ID
                    admin.Unique_pass = Funciones.Program.GenerarContrasenaAleatoria(10);
                    admin.FechaAsignacion = empleado.FechaIngreso;

                    int resultadoAdmin = Funciones.Program.RegistrarAdministrador(admin);

                    if (resultadoAdmin > 0)
                    {
                        MessageBox.Show("Administrador registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al registrar el administrador.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

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
                PermisosSi.IsChecked = false;
                PermisosNo.IsChecked = false;
                Localidad.Text = "";
                FechaIngreso.SelectedDate = null;
                FechaNacimiento.SelectedDate = null;
            }
            else
            {
                MessageBox.Show("Ocurrió un error al registrar los datos.", "Error de Registro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
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

                int edad= currentDate.Year - selectedDate.Year;
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
