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

namespace ModificarAuxiliares
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int idAuxiliar;
        Entidades.Program.EmpleadosAux auxiliares;
        public MainWindow(Entidades.Program.EmpleadosAux auxiliar)
        {
            InitializeComponent();
            LlenarComboBox();
            idAuxiliar = auxiliar.Id;
            EmailG.Text= auxiliar.Email;
            DocumentoG.Text = auxiliar.Documento.ToString();
            NombreG.Text= auxiliar.Nombre;
            ApellidoG.Text= auxiliar.Apellido;
            TelefonoG.Text= auxiliar.Telefono;
            FechaIngreso.Text=auxiliar.FechaIngreso.ToString();
            FechaNacimiento.Text = auxiliar.FechaNacimiento.ToString();
            NacionalidadG.Text = auxiliar.Nacionalidad;
            Domicilio.Text= auxiliar.Domicilio;
            Localidad.Text = auxiliar.Localidad;
            cmbCargos.Text = auxiliar.Cargo;
            auxiliares= auxiliar;
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
            MessageBoxResult resultado = MessageBox.Show("¿Está seguro de que desea modificar los datos del paciente?",
          "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultado != MessageBoxResult.Yes) return;

            string[] camposObligatorios = {EmailG.Text, DocumentoG.Text, NombreG.Text, ApellidoG.Text, TelefonoG.Text, FechaIngreso.Text,
                                     NacionalidadG.Text, Domicilio.Text, Localidad.Text, cmbCargos.Text, FechaNacimiento.Text};
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
            var camposTexto = new (TextBox control, int min, int max, string nombre, string valorAsignar)[]//se crea una tupla
            {
                (NacionalidadG, 1, 50, "Nacionalidad", auxiliares.Nacionalidad),
                (NombreG, 1, 50, "Nombre", auxiliares.Nombre),
                (ApellidoG, 1, 50, "Apellido", auxiliares.Apellido),
                (Domicilio, 1, 50, "Domicilio", auxiliares.Domicilio),
                (Localidad, 1, 50, "Localidad", auxiliares.Localidad)
            };

            foreach (var campo in camposTexto)
            {
                // Pasar el control completo al método ValidarLongitudTexto
                if (!Funciones.Program.ValidarLongitudTexto(campo.control, campo.min, campo.max, campo.nombre, campo.valorAsignar))
                {
                    Funciones.Program.MostrarError($"El campo {campo.nombre} es inválido.", $"{campo.nombre} Inválido");
                    return;
                }
            }
            auxiliares.Id=idAuxiliar;
            auxiliares.Email = EmailG.Text;
            auxiliares.Documento = int.Parse(DocumentoG.Text); 
            auxiliares.Nombre= NombreG.Text;
            auxiliares.Apellido= ApellidoG.Text;
            auxiliares.Telefono= TelefonoG.Text;
            auxiliares.FechaNacimiento = DateTime.Parse(FechaNacimiento.Text);
            auxiliares.FechaIngreso = DateTime.Parse(FechaIngreso.Text);
            auxiliares.Nacionalidad= NacionalidadG.Text;
            auxiliares.Domicilio = Domicilio.Text;
            auxiliares.Localidad = Localidad.Text;
            auxiliares.Cargo= cmbCargos.Text;
            
            Funciones.Program.ModificarAuxiliares(auxiliares);
            MessageBox.Show("Auxiliar modificado correctamente.");

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
