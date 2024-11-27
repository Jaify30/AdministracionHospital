using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Input;
using System.ComponentModel;
using static Entidades.Program;
using GalaSoft.MvvmLight.Command;

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
            Entidades.Program.Administradores admin= new Entidades.Program.Administradores();
            admin.Id=empleados.Id;
            string unique_pass = Funciones.Program.ObtenerUnique_pass(admin.Id);
            if (unique_pass == null)
            {
                Unique_pass.Visibility = Visibility.Hidden;
                Mostrar_Empleados.Visibility = Visibility.Hidden;
                Mostrar_auxiliares.Visibility = Visibility.Hidden;
                Mostrar_Doctores.Visibility = Visibility.Hidden;
                Buscar_Doctor.Visibility = Visibility.Hidden;
                TextDoctor.Visibility = Visibility.Hidden;
                DocumentoDoctor.Visibility = Visibility.Hidden;
                DocumentoDoctorBorder.Visibility = Visibility.Hidden;
            }
            if (unique_pass != null)
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

        //Mostrar lista de models
        private void Mostrar_Empleados_Click(object sender, RoutedEventArgs e)
        {
            Data_Pacientes.Visibility = Visibility.Hidden;
            Data_Doctores.Visibility = Visibility.Hidden;
            Data_Auxiliares.Visibility = Visibility.Hidden;
            Data_Empleados.HeadersVisibility = DataGridHeadersVisibility.All;

            List<Entidades.Program.Empleados> empleados = Funciones.Program.ObtenerTodosLosEmpleados();

            int indice_seleccionado = cmbOrdenamiento.SelectedIndex;

            if (empleados == null || !empleados.Any())
            {
                MessageBox.Show("No se han encontrado empleados.");
                return;
            }
            if (indice_seleccionado == 1)
            {
                List<Entidades.Program.Empleados> empleadosOrdenadosPorNombre = Funciones.Program.ObtenerEmpleadosOrdenados("Nombre");
                Data_Empleados.ItemsSource = empleadosOrdenadosPorNombre;
            }
            else if (indice_seleccionado == 2)
            {
                List<Entidades.Program.Empleados> empleadosOrdenadosPorApellido = Funciones.Program.ObtenerEmpleadosOrdenados("Apellido");
                Data_Empleados.ItemsSource = empleadosOrdenadosPorApellido;
            }
            else
            {
                Data_Empleados.ItemsSource = empleados; // Por defecto, no ordenado
            }
            Data_Empleados.Visibility = Visibility.Visible;
            Eliminar.Visibility = Visibility.Hidden;
            Modificar.Visibility = Visibility.Hidden;


        }
        private void MostrarContraseña_Click(object sender, RoutedEventArgs e)
        {
            Entidades.Program.Empleados empleado = Funciones.Program.ObtenerDatosEmpleadoPorEmail(emailG);
            Entidades.Program.Administradores admin = new Entidades.Program.Administradores();
            admin.Id = empleado.Id;
            string unique_pass = Funciones.Program.ObtenerUnique_pass(admin.Id);
            

            // Mostrar la Unique_pass

            MessageBox.Show($"Su Contraseña Unica es {unique_pass}", "Contraseña Unica");
        }//Muestra el Unique_pass del usuario si es que lo tiene.
        private void Mostrar_Doctores_Click(object sender, RoutedEventArgs e)
        {
            Data_Empleados.Visibility = Visibility.Hidden;
            Data_Auxiliares.Visibility = Visibility.Hidden;
            Data_Pacientes.Visibility = Visibility.Hidden;
            Data_Doctores.HeadersVisibility = DataGridHeadersVisibility.All;

            List<Doctores> doctores = Funciones.Program.ObtenerDoctores();

            int indice_seleccionado = cmbOrdenamiento.SelectedIndex;

            if (doctores == null || !doctores.Any())
            {
                MessageBox.Show("No se han encontrado empleados.");
                return;
            }
            if (indice_seleccionado == 1)
            {
                List<Entidades.Program.Doctores> doctoresOrdenadosPorNombre = Funciones.Program.ObtenerDoctoresOrdenados("Nombre");
                Data_Doctores.ItemsSource = doctoresOrdenadosPorNombre;
            }
            else if (indice_seleccionado == 2)
            {
                List<Entidades.Program.Doctores> doctoresOrdenadosPorApellido = Funciones.Program.ObtenerDoctoresOrdenados("Apellido");
                Data_Doctores.ItemsSource = doctoresOrdenadosPorApellido;
            }
            else
            {
                Data_Doctores.ItemsSource = doctores; // Por defecto, no ordenado
            }

            Data_Doctores.Visibility = Visibility.Visible;
            Eliminar.Visibility = Visibility.Visible;
            Modificar.Visibility = Visibility.Visible;
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
            int indice_seleccionado = cmbOrdenamiento.SelectedIndex;

            // Verificar si la lista contiene datos
            if (auxiliar == null || !auxiliar.Any())
            {
                MessageBox.Show("No se han encontrado empleados auxiliares.");
                return;
            }
            if (indice_seleccionado == 1)
            {
                List<Entidades.Program.EmpleadosAux> auxiliarOrdenadosPorNombre = Funciones.Program.ObtenerAuxiliaresOrdenados("e.Nombre");
                Data_Auxiliares.ItemsSource = auxiliarOrdenadosPorNombre;
            }
            else if (indice_seleccionado == 2)
            {
                List<Entidades.Program.EmpleadosAux> auxiliarOrdenadosPorApellido = Funciones.Program.ObtenerAuxiliaresOrdenados("e.Apellido");
                Data_Auxiliares.ItemsSource = auxiliarOrdenadosPorApellido;
            }
            else
            {
                Data_Auxiliares.ItemsSource = auxiliar; // Por defecto, no ordenado
            }


            // Hacer visible el DataGrid
            Data_Auxiliares.Visibility = Visibility.Visible;
            Eliminar.Visibility = Visibility.Visible;
            Modificar.Visibility = Visibility.Visible;
        }
        private void Mostrar_Pacientes_Click(object sender, RoutedEventArgs e)
        {
            Empleados empleados = Funciones.Program.ObtenerDatosEmpleadoPorEmail(emailG);
            Data_Empleados.Visibility = Visibility.Hidden;
            Data_Doctores.Visibility = Visibility.Hidden;
            Data_Auxiliares.Visibility = Visibility.Hidden;

            Data_Pacientes.HeadersVisibility = DataGridHeadersVisibility.All;

            List<Pacientes> pacientes = Funciones.Program.MostrarPacientes(empleados.Id);
            int indice_seleccionado = cmbOrdenamiento.SelectedIndex;
            if (pacientes == null || !pacientes.Any())
            {
                MessageBox.Show("No se han encontrado Pacientes.");
                return;
            }
            if (indice_seleccionado == 1)
            {
                List<Entidades.Program.Pacientes> pacientesOrdenadosPorNombre = Funciones.Program.MostrarPacientesOrdenados(empleados.Id,"Nombre");
                Data_Pacientes.ItemsSource = pacientesOrdenadosPorNombre;
            }
            else if (indice_seleccionado == 2)
            {
                List<Entidades.Program.Pacientes> pacientesOrdenadosPorApellido = Funciones.Program.MostrarPacientesOrdenados(empleados.Id,"Apellido");
                Data_Pacientes.ItemsSource = pacientesOrdenadosPorApellido;
            }
            else
            {
                Data_Pacientes.ItemsSource = pacientes; // Por defecto, no ordenado
            }

            Data_Pacientes.Visibility= Visibility.Visible;
            Eliminar.Visibility = Visibility.Visible;
            Modificar.Visibility = Visibility.Visible;
        }
        //Modificar, eliminar y agregar
        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (Data_Doctores.SelectedItem is Doctores doctorSeleccionado)
            {
                if (doctorSeleccionado == null || doctorSeleccionado.Id == null)
                {
                    MessageBox.Show("Seleccione un empleado por favor");
                }
                else
                {
                    ModificarDoctores.MainWindow Doctores = new ModificarDoctores.MainWindow(doctorSeleccionado);
                    Doctores.ShowDialog();
                }
            }
            if (Data_Pacientes.SelectedItem is Pacientes pacienteSeleccionado)
            {
                if (pacienteSeleccionado == null || pacienteSeleccionado.IdPacientes == null)
                {
                    MessageBox.Show("Seleccione un paciente por favor");
                }
                else
                {
                    ModicarPacientes.MainWindow Paciente = new ModicarPacientes.MainWindow(pacienteSeleccionado);
                    Paciente.ShowDialog();
                }
            }
            if (Data_Auxiliares.SelectedItem is EmpleadosAux auxiliarSeleccionado)
            {
                if (auxiliarSeleccionado == null || auxiliarSeleccionado.Id == null)
                {
                    MessageBox.Show("Seleccione un empleado por favor");
                }
                else
                {
                    ModificarAuxiliares.MainWindow modificarAuxiliares =  new ModificarAuxiliares.MainWindow(auxiliarSeleccionado);
                    modificarAuxiliares.ShowDialog();
                }

            }
            else
            {
                MessageBox.Show("Seleccione una casilla para Modificar");
            }
        }
        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este registro?",
                                                  "Confirmar Eliminación",
                                                  MessageBoxButton.YesNo,
                                                  MessageBoxImage.Warning);

            if (Data_Doctores.SelectedItem is Doctores doctorSeleccionado) 
            {
                if (doctorSeleccionado == null || doctorSeleccionado.Id == null)
                {
                    MessageBox.Show("Seleccione un empleado por favor");
                }
                else
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        Funciones.Program.EliminarDoctor(doctorSeleccionado.Id);
                        List<Doctores> doctores = Funciones.Program.ObtenerDoctores();
                        Data_Doctores.ItemsSource = doctores;
                    }
                }
            }
            if (Data_Pacientes.SelectedItem is Pacientes pacienteSeleccionado)
            {
                if (pacienteSeleccionado == null || pacienteSeleccionado.IdPacientes == null)
                {
                    MessageBox.Show("Seleccione un paciente por favor");
                }
                else
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        Funciones.Program.EliminarPaciente(pacienteSeleccionado.IdPacientes);
                        Empleados empleados = Funciones.Program.ObtenerDatosEmpleadoPorEmail(emailG);
                        List<Pacientes> pacientes = Funciones.Program.MostrarPacientes(empleados.Id);
                        Data_Pacientes.ItemsSource = pacientes;
                    }
                }
            }
            if (Data_Auxiliares.SelectedItem is EmpleadosAux auxiliarSeleccionado)
            {
                if (auxiliarSeleccionado == null || auxiliarSeleccionado.Id == null)
                {
                    MessageBox.Show("Seleccione un empleado por favor");
                }
                else
                {
                    if (result == MessageBoxResult.Yes) // Asegúrate de que result esté definido
                    {
                        Funciones.Program.EliminarAuxiliar(auxiliarSeleccionado.Id);
                        List<EmpleadosAux> auxiliares = Funciones.Program.ObtenerAuxiliares();
                        Data_Auxiliares.ItemsSource = auxiliares;
                    }
                }

            }
            else
            {
                MessageBox.Show("Seleccione una casilla para eliminar");
            }

        }
        private void Agregar_Paciente_Click(object sender, RoutedEventArgs e)
        {
            string TieneMatricula = Funciones.Program.ObtenerMatriculaDoctor(emailG);
            Entidades.Program.Empleados empleado = Funciones.Program.ObtenerDatosEmpleadoPorEmail(emailG);

            if (TieneMatricula==null)
            {
                MessageBox.Show("No tiene permisos para agregar pacientes a su Base de datos.");
                return ;
            }
            else
            {
                RegistroPacientes.MainWindow AgregarPacientes = new RegistroPacientes.MainWindow(empleado.Id);
                AgregarPacientes.ShowDialog();
            }
        }
        private void Buscar_Paciente_Click(object sender, RoutedEventArgs e)
        {
            Empleados empleados = Funciones.Program.ObtenerDatosEmpleadoPorEmail(emailG);
            Data_Empleados.Visibility = Visibility.Hidden;
            Data_Doctores.Visibility = Visibility.Hidden;
            Data_Auxiliares.Visibility = Visibility.Hidden;

            int numero;
            int documentoPaciente;

            Data_Pacientes.HeadersVisibility = DataGridHeadersVisibility.All;
            if (!Funciones.Program.ValidarSoloNumeros(DocumentoPaciente.Text, out numero, "Documento"))
            {
                MessageBox.Show("El documento ingresado no es válido.", "Documento Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                documentoPaciente = numero;
            }

            List<Pacientes> pacientes = Funciones.Program.BuscarPaciente(documentoPaciente,empleados.Id);

            if (pacientes == null || !pacientes.Any())
            {
                MessageBox.Show("No se han encontrado el paciente con ese documento.");
                return;
            }

            Data_Pacientes.ItemsSource = pacientes;

            Data_Pacientes.Visibility = Visibility.Visible;
        }
        private void Buscar_Doctor_Click(object sender, RoutedEventArgs e)
        {
            Data_Empleados.Visibility = Visibility.Hidden;
            Data_Auxiliares.Visibility = Visibility.Hidden;
            Data_Pacientes.Visibility = Visibility.Hidden;
            Data_Doctores.HeadersVisibility = DataGridHeadersVisibility.All;
            int numero;
            int documentoDoctor;

            if (!Funciones.Program.ValidarSoloNumeros(DocumentoDoctor.Text, out numero, "Documento"))
            {
                MessageBox.Show("El documento ingresado no es válido.", "Documento Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                documentoDoctor = numero;
            }

            List<Doctores> doctores = Funciones.Program.BuscarDoctores(documentoDoctor);

            if (doctores == null || !doctores.Any())
            {
                MessageBox.Show("No se ha encontrado un doctor con ese documento.");
                return;
            }

            Data_Doctores.ItemsSource = doctores;
            Data_Doctores.Visibility = Visibility.Visible;
        }
        private void Ver_Historial_Click(object sender, RoutedEventArgs e)
        {
            if (Data_Pacientes.SelectedItem is Pacientes pacienteSeleccionado)
            {
                if (pacienteSeleccionado == null || pacienteSeleccionado.IdPacientes == null)
                {
                    MessageBox.Show("Seleccione un paciente por favor");
                }
                else
                {
                    VerHistorial.MainWindow Historial = new VerHistorial.MainWindow(pacienteSeleccionado.Email,pacienteSeleccionado.Nombre,pacienteSeleccionado.Apellido, pacienteSeleccionado.IdPacientes);
                    Historial.ShowDialog();
                }
            }
        }
        
    }

}
