using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using static Entidades.Program;
using System.Net;
using System.Net.Mail;
using System.Data;

namespace Funciones
{
    public class Program
    {
        static void Main(string[] args)
        {


        }

        
        //Conexion SQL
        public static SqlConnection conexionBBDD()
        {
            SqlConnection conexion = new SqlConnection("Server=JAIFY\\SQLEXPRESS; Database=Hospital;" +
                " Trusted_Connection=true; Integrated Security=SSPI;Persist Security Info=False;");
            conexion.Open();

            return conexion;
        }

        //Registros
        public static int RegistrarAdministrador(Entidades.Program.Administradores admin)
        {
            using (SqlConnection conexion = conexionBBDD())
            {
                string query = "insert into Administradores(IdAdmin,FechaAsignacion,Unique_pass) VALUES (@IdAdmin,@FechaAsignacion,@Unique_pass);";

                try
                {
                    using (SqlCommand cmd = new SqlCommand(query,conexion))
                    {
                        cmd.Parameters.Add("@IdAdmin", SqlDbType.Int).Value = admin.Id;
                        cmd.Parameters.Add("@Unique_pass", SqlDbType.NVarChar).Value = admin.Unique_pass;
                        cmd.Parameters.Add("@FechaAsignacion", SqlDbType.DateTime).Value = admin.FechaAsignacion;

                        int FilasAfectadas= cmd.ExecuteNonQuery();

                        return FilasAfectadas;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error al registrar el administrador");
                    return -1;
                }
            }
        }
        public static string ObtenerUnique_pass(int id)
        {
            string query = "SELECT Unique_pass FROM Administradores WHERE IdAdmin=@Id";

            using (SqlConnection conexion= conexionBBDD())
            {
                try { 
                    using (SqlCommand cmd= new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Id",id);

                        object resultado=cmd.ExecuteScalar();

                        if (resultado != null)
                        {
                            string uniquePass = resultado.ToString();
                            return uniquePass;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show($"Error al obtener Unique_pass: {ex.Message}");
                    return null; // Devolver -1 en caso de error
                }
            }
        }
        public static int RegistrarPacientes(Pacientes pacienteNuevo)
        {
            int retorno = 0;

            using (SqlConnection conexion = conexionBBDD())
            {
                using (SqlTransaction transaction = conexion.BeginTransaction())
                {
                    try
                    {
                        string query = @"INSERT INTO Pacientes (Email, Documento, Nombre, Apellido, Telefono, Legajo, FechaIngreso, FechaNacimiento, IdDoctor, Historial)
                        VALUES (@Email, @Documento, @Nombre, @Apellido, @Telefono, @Legajo, @FechaIngreso, @FechaNacimiento, @IdDoctor, @Historial)";

                        SqlCommand cmd = new SqlCommand(query, conexion,transaction);
                        cmd.Parameters.AddWithValue("@Email", pacienteNuevo.Email);
                        cmd.Parameters.AddWithValue("@Documento", pacienteNuevo.Documento);
                        cmd.Parameters.AddWithValue("@Nombre", pacienteNuevo.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", pacienteNuevo.Apellido);
                        cmd.Parameters.AddWithValue("@Telefono", pacienteNuevo.Telefono);
                        cmd.Parameters.AddWithValue("@Legajo", pacienteNuevo.Legajo);
                        cmd.Parameters.AddWithValue("@FechaIngreso", pacienteNuevo.FechaIngreso); // Usa la fecha actual como ejemplo
                        cmd.Parameters.AddWithValue("@FechaNacimiento", pacienteNuevo.FechaNacimiento); // Reemplaza con la fecha de nacimiento correcta
                        cmd.Parameters.AddWithValue("@IdDoctor", pacienteNuevo.IdDoctor); // Usar DBNull.Value si el campo es opcional y no tiene valor
                        cmd.Parameters.AddWithValue("@Historial", pacienteNuevo.Historial);
                        retorno=cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Ha ocurrido un error: " + e.Message);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return retorno;
        } //Agrega paciente
        public static int RegistrarDoctores(Entidades.Program.Doctores Doctores, Entidades.Program.Empleados Empleado)
        {
            int retorno = 0;

            using (SqlConnection conexion = conexionBBDD())
            {
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        string queryEmpleado = @"
                INSERT INTO Empleados (Nombre, Apellido, Password, Email, Nacionalidad, FechaNacimiento, FechaIngreso, Telefono, Domicilio, Localidad, Documento)
                VALUES (@Nombre, @Apellido, @Password, @Email, @Nacionalidad, @FechaNacimiento, @FechaIngreso, @Telefono, @Domicilio, @Localidad, @Documento);
                SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdEmpleado = new SqlCommand(queryEmpleado, conexion, transaccion);
                        cmdEmpleado.Parameters.AddWithValue("@Nombre", Empleado.Nombre);
                        cmdEmpleado.Parameters.AddWithValue("@Apellido", Empleado.Apellido);
                        cmdEmpleado.Parameters.AddWithValue("@Password", Empleado.Password);
                        cmdEmpleado.Parameters.AddWithValue("@Email", Empleado.Email);
                        cmdEmpleado.Parameters.AddWithValue("@Nacionalidad", Empleado.Nacionalidad);
                        cmdEmpleado.Parameters.AddWithValue("@FechaNacimiento", Empleado.FechaNacimiento);
                        cmdEmpleado.Parameters.AddWithValue("@FechaIngreso", Empleado.FechaIngreso);
                        cmdEmpleado.Parameters.AddWithValue("@Telefono", Empleado.Telefono);
                        cmdEmpleado.Parameters.AddWithValue("@Domicilio", Empleado.Domicilio);
                        cmdEmpleado.Parameters.AddWithValue("@Localidad", Empleado.Localidad);
                        cmdEmpleado.Parameters.AddWithValue("@Documento", Empleado.Documento);

                        int IdEmpleado = Convert.ToInt32(cmdEmpleado.ExecuteScalar());

                        if (Doctores != null)
                        {
                            string queryDoctor = @"
                    INSERT INTO Doctores (IdDoctor, Cargo, Matricula)
                    VALUES (@IdDoctor, @Cargo, @Matricula);";

                            SqlCommand cmdDoctor = new SqlCommand(queryDoctor, conexion, transaccion);
                            cmdDoctor.Parameters.AddWithValue("@IdDoctor", IdEmpleado);
                            cmdDoctor.Parameters.AddWithValue("@Cargo", Doctores.Cargo);
                            cmdDoctor.Parameters.AddWithValue("@Matricula", Doctores.Matricula);

                            cmdDoctor.ExecuteNonQuery();
                        }

                        //confirma la transaccion
                        transaccion.Commit();
                        retorno = IdEmpleado; // Devuelve el ID del empleado insertado
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
            return retorno;
        }
        public static int RegistrarAuxiliares(Entidades.Program.EmpleadosAux empleadosAux, Entidades.Program.Empleados Empleado)
        {
            int retorno = 0;

            using (SqlConnection conexion = conexionBBDD())
            {
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        string queryEmpleado = @"
                        INSERT INTO Empleados (Nombre, Apellido, Password, Email, Nacionalidad, FechaNacimiento, FechaIngreso, Telefono, Domicilio, Localidad, Documento)
                        VALUES (@Nombre, @Apellido, @Password, @Email, @Nacionalidad, @FechaNacimiento, @FechaIngreso, @Telefono, @Domicilio, @Localidad, @Documento);
                        SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdEmpleado = new SqlCommand(queryEmpleado, conexion, transaccion);
                        cmdEmpleado.Parameters.AddWithValue("@Nombre", Empleado.Nombre);
                        cmdEmpleado.Parameters.AddWithValue("@Apellido", Empleado.Apellido);
                        cmdEmpleado.Parameters.AddWithValue("@Password", Empleado.Password);
                        cmdEmpleado.Parameters.AddWithValue("@Email", Empleado.Email);
                        cmdEmpleado.Parameters.AddWithValue("@Nacionalidad", Empleado.Nacionalidad);
                        cmdEmpleado.Parameters.AddWithValue("@FechaNacimiento", Empleado.FechaNacimiento);
                        cmdEmpleado.Parameters.AddWithValue("@FechaIngreso", Empleado.FechaIngreso);
                        cmdEmpleado.Parameters.AddWithValue("@Telefono", Empleado.Telefono);
                        cmdEmpleado.Parameters.AddWithValue("@Domicilio", Empleado.Domicilio);
                        cmdEmpleado.Parameters.AddWithValue("@Localidad", Empleado.Localidad);
                        cmdEmpleado.Parameters.AddWithValue("@Documento", Empleado.Documento);

                        int IdEmpleado = Convert.ToInt32(cmdEmpleado.ExecuteScalar());

                        if (empleadosAux != null)
                        {
                            string queryDoctor = @"
                            INSERT INTO EmpleadosAux (IdEmpleado, Cargo)
                            VALUES (@IdEmpleado, @Cargo);";

                            SqlCommand cmdDoctor = new SqlCommand(queryDoctor, conexion, transaccion);
                            cmdDoctor.Parameters.AddWithValue("@IdEmpleado", IdEmpleado);
                            cmdDoctor.Parameters.AddWithValue("@Cargo", empleadosAux.Cargo);

                            cmdDoctor.ExecuteNonQuery();
                        }

                        //confirma la transaccion
                        transaccion.Commit();
                        retorno = IdEmpleado; // Devuelve el ID del empleado insertado
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
            return retorno;
        }
        //Validaciones
        public static bool DocumentoExiste(int NumeroDocumento)
        {
            // Variable para almacenar el resultado
            bool existe = false;
            using (SqlConnection conexion = conexionBBDD())
            {
                // Consulta SQL para verificar si el número de documento ya existe
                string query = "SELECT COUNT(1) FROM Empleados WHERE Documento=@Documento";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    // Agregar el parámetro de número de documento
                    cmd.Parameters.AddWithValue("@Documento", NumeroDocumento);

                    try
                    {
                        // Ejecutar la consulta y obtener el resultado
                        int count = (int)cmd.ExecuteScalar();
                        // Si el resultado es mayor a 0, significa que ya existe el número de documento
                        existe = count > 0;
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones en caso de que ocurra un error
                        MessageBox.Show("Error al consultar la base de datos: " + ex.Message);
                    }
                }
            }
            return existe;
        }
        public static bool VerificarFecha(DateTime? fecha, string nombreCampo)
        {
            // Verifica si la fecha ha sido seleccionada
            if (!fecha.HasValue)
            {
                MessageBox.Show($"Por favor, seleccione una fecha para {nombreCampo}.", "Fecha Requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Verifica que la fecha no sea mayor a la fecha actual
            if (fecha.Value > DateTime.Now)
            {
                MessageBox.Show($"La fecha de {nombreCampo} no puede ser mayor a la fecha actual.", "Fecha Inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Si todo es válido
            return true;
        }
        public static bool ValidarLongitudTexto(TextBox textBox, int minLongitud, int maxLongitud, string nombreCampo, string textoValidado)
        {
            textoValidado = string.Empty;

            if (textBox.Text.Length < minLongitud)
            {
                MessageBox.Show($"Ingrese un {nombreCampo}, por favor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (textBox.Text.Length > maxLongitud)
            {
                MessageBox.Show($"Se ha excedido del numero maximo de caracteres[{maxLongitud}] ({nombreCampo}).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            textoValidado = textBox.Text;
            return true;
        }
        public static bool ValidarEmail(string email)
        {
            // Expresión regular para validar direcciones de correo electrónico
            string patronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(patronEmail);

            // Verificar si el email coincide con el patrón
            if (regex.IsMatch(email))
            {
                return true;
            }
            else
            {
                MessageBox.Show("La dirección de correo electrónico no es válida. Tiene que tener formato Email pepe123@ejemplo.com", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public static bool ValidarContrasena(string contrasena, string confirmarContrasena)
        {
            // Verificar si la contraseña tiene al menos 8 caracteres
            if (contrasena.Length < 8 || confirmarContrasena.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Verificar si la confirmación de la contraseña coincide con la original
            if (contrasena != confirmarContrasena)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public static string GenerarContrasenaAleatoria(int longitud)
        {
            const string caracteresPermitidos = "1234567890";
            var random = new Random();
            return new string(Enumerable.Repeat(caracteresPermitidos, longitud)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static bool ValidarSoloNumeros(string numero, out int numeroValidado, string NombreCampo)
        {
            // Intentar convertir el texto a un número entero
            if (int.TryParse(numero, out numeroValidado))
            {
                return true;
            }
            else
            {
                MessageBox.Show($"Por favor, ingrese solo números enteros. ({NombreCampo})", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public static bool EsCorreoValido(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }

            try
            {
                string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                Regex regex = new Regex(patron, RegexOptions.IgnoreCase);
                return regex.IsMatch(Email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static int VerificacionLogin(Entidades.Program.Empleados usuario)
        {
            int retorno = 0;

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = "SELECT COUNT(1) FROM Empleados WHERE Email = @Email AND Password COLLATE SQL_Latin1_General_CP1_CS_AS = @Password";//Hacemos una query donde se seleccione solo email y contrasena

                SqlCommand command = new SqlCommand(query, conexion);

                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Password", usuario.Password);

                try
                {
                    retorno = (int)command.ExecuteScalar();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error.");
                }

                return retorno;
            }
        }
        public static int IngresoUnique_pass(string pass)
        {
            int retorno = 0;

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = "SELECT COUNT(*) FROM Administradores WHERE Unique_pass = @Unique_pass";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@Unique_pass", pass);

                try
                {
                    retorno = (int)cmd.ExecuteScalar();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e.Message);
                }

                return retorno;
            }
        }

        //Mostrar datos
        public static Empleados ObtenerDatosEmpleadoPorEmail(string email)
        {
            Empleados empleado = null;

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = "SELECT * FROM Empleados WHERE Email = @Email";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    empleado = new Empleados
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Password = reader.GetString(3),
                        Email = reader.GetString(4),
                        Nacionalidad = reader.GetString(5),
                        FechaNacimiento = reader.GetDateTime(6),
                        FechaIngreso = reader.GetDateTime(7),
                        Telefono = reader.GetString(8),
                        Domicilio = reader.GetString(9),
                        Localidad = reader.GetString(10),
                        Documento = reader.GetInt32(11)
                    };
                }
            }

            return empleado;
        }
        public static Pacientes ObtenerHistorialPacientePorEmail(string Email)
        {
            Pacientes pacientes = null;

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = @"SELECT Historial FROM Pacientes WHERE Email=@Email";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Email",Email);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    pacientes = new Pacientes
                    {
                        Historial = reader.GetString(0)
                    };
                }
            }
            return pacientes;
        }
        public static List<Empleados> ObtenerTodosLosEmpleados()
        {
            List<Empleados> empleados = new List<Empleados>();

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = "SELECT * FROM Empleados";

                SqlCommand command = new SqlCommand(query, conexion);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Empleados empleado = new Empleados
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Password = reader.GetString(3),
                            Email = reader.GetString(4),
                            Nacionalidad = reader.GetString(5),
                            FechaNacimiento = reader.GetDateTime(6),
                            FechaIngreso = reader.GetDateTime(7),
                            Telefono = reader.GetString(8),
                            Domicilio = reader.GetString(9),
                            Localidad = reader.GetString(10),
                            Documento = reader.GetInt32(11)//Hacer dos botones, uno donde se muestre al empleadoaux y otro a los doctores
                        };

                        empleados.Add(empleado);
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e.Message);
                }
            }

            return empleados;
        }
        public static List<Doctores> ObtenerDoctores()
        {
            List<Doctores> doctores = new List<Doctores>();

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = @"
                        SELECT 
                            e.Id,
                            e.Nombre,
                            e.Apellido,
                            e.Email,
                            e.Nacionalidad,
                            e.FechaNacimiento,
                            e.FechaIngreso,
                            e.Telefono,
                            e.Domicilio,
                            e.Localidad,
                            e.Documento,
                            e.Password,
                            d.Cargo,
                            d.Matricula
                        FROM Empleados e
                        INNER JOIN Doctores d ON e.Id = d.IdDoctor";

                SqlCommand command = new SqlCommand(query, conexion);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Doctores doctor = new Doctores
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Nacionalidad = reader.GetString(4),
                            FechaNacimiento = reader.GetDateTime(5),
                            FechaIngreso = reader.GetDateTime(6),
                            Telefono = reader.GetString(7),
                            Domicilio = reader.GetString(8),
                            Localidad = reader.GetString(9),
                            Documento = reader.GetInt32(10),
                            Password = reader.GetString(11),
                            Cargo = reader.GetString(12),
                            Matricula = reader.GetString(13)
                        };

                        doctores.Add(doctor);
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e.Message);
                }
            }
            return doctores;
        }
        public static List<EmpleadosAux> ObtenerAuxiliares()
        {
            List<EmpleadosAux> auxiliares = new List<EmpleadosAux>();

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = @"
                SELECT 
                    e.Id,
                    e.Nombre,
                    e.Apellido,
                    e.Email,
                    e.Nacionalidad,
                    e.FechaNacimiento,
                    e.FechaIngreso,
                    e.Telefono,
                    e.Domicilio,
                    e.Localidad,
                    e.Documento,
                    e.Password,
                    d.Cargo
                FROM Empleados e
                INNER JOIN EmpleadosAux d ON e.Id = d.IdEmpleado";

                SqlCommand command = new SqlCommand(query, conexion);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        EmpleadosAux auxiliar = new EmpleadosAux
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Nacionalidad = reader.GetString(4),
                            FechaNacimiento = reader.GetDateTime(5),
                            FechaIngreso = reader.GetDateTime(6),
                            Telefono = reader.GetString(7),
                            Domicilio = reader.GetString(8),
                            Localidad = reader.GetString(9),
                            Documento = reader.GetInt32(10),
                            Password = reader.GetString(11),
                            Cargo = reader.GetString(12)
                        };

                        auxiliares.Add(auxiliar);
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e.Message);
                }
            }
            return auxiliares;
        }
        public static string ObtenerMatriculaDoctor(string Email)
        {
            string matricula = null; // Inicializa la variable para almacenar la matrícula

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = @"SELECT 
                            d.Matricula
                         FROM Empleados e
                         INNER JOIN Doctores d ON e.Id = d.IdDoctor
                         WHERE e.Email = @Email";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Email", Email);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read()) // Lee solo la primera fila, ya que solo se necesita una matrícula
                    {
                        matricula = reader.GetString(0); // Obtiene el valor de la matrícula
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e.Message);
                }
            }

            return matricula; // Retorna la matrícula encontrada o null si no se encontró ninguna
        }
        public static List<Pacientes> MostrarPacientes(int idDoctor)
        {
            List<Pacientes> pacientes = new List<Pacientes>();

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = @"SELECT * FROM Pacientes WHERE IdDoctor = @IdDoctor";

                SqlCommand cmd = new SqlCommand(query, conexion);
                
                cmd.Parameters.AddWithValue("@IdDoctor", idDoctor);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pacientes paciente = new Pacientes
                        {
                            IdPacientes = Convert.ToInt32(reader["IdPacientes"]),
                            Email = reader["Email"].ToString(),
                            Documento = Convert.ToInt32(reader["Documento"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Legajo = reader["Legajo"].ToString(),
                            FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                            IdDoctor = Convert.ToInt32(reader["IdDoctor"]),
                            Historial = reader["Historial"].ToString()
                        };
                        pacientes.Add(paciente);
                    }
                }
            }
            return pacientes;
        }
        public static List<Pacientes> BuscarPaciente(int Documento, int idDoctor)
        {
            List<Pacientes> pacienteBuscado = new List<Pacientes>();

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = @"SELECT * FROM Pacientes WHERE Documento=@documento AND IdDoctor=@idDoctor ";
                
                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@documento",Documento);
                cmd.Parameters.AddWithValue("@idDoctor",idDoctor);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pacientes paciente = new Pacientes
                        {
                            IdPacientes = Convert.ToInt32(reader["IdPacientes"]),
                            Email = reader["Email"].ToString(),
                            Documento = Convert.ToInt32(reader["Documento"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Legajo = reader["Legajo"].ToString(),
                            FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                            IdDoctor = Convert.ToInt32(reader["IdDoctor"]),
                            Historial = reader["Historial"].ToString()
                        };
                        pacienteBuscado.Add(paciente);
                    }
                }
            }
            return pacienteBuscado;
        }
        public static List<Doctores> BuscarDoctores(int Documento)
        {
            List<Doctores> doctorBuscado = new List<Doctores>();

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = @"
                        SELECT 
                        e.Id,
                        e.Nombre,
                        e.Apellido,
                        e.Email,
                        e.Nacionalidad,
                        e.FechaNacimiento,
                        e.FechaIngreso,
                        e.Telefono,
                        e.Domicilio,
                        e.Localidad,
                        e.Documento,
                        e.Password,
                        d.Cargo,
                        d.Matricula
                    FROM Empleados e
                    INNER JOIN Doctores d ON e.Id = d.IdDoctor
                    WHERE e.Documento = @Documento;";

                SqlCommand command = new SqlCommand(query, conexion);

                command.Parameters.AddWithValue("@Documento",Documento);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Doctores doctor = new Doctores
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Nacionalidad = reader.GetString(4),
                            FechaNacimiento = reader.GetDateTime(5),
                            FechaIngreso = reader.GetDateTime(6),
                            Telefono = reader.GetString(7),
                            Domicilio = reader.GetString(8),
                            Localidad = reader.GetString(9),
                            Documento = reader.GetInt32(10),
                            Password = reader.GetString(11),
                            Cargo = reader.GetString(12),
                            Matricula = reader.GetString(13)
                        };

                        doctorBuscado.Add(doctor);
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e.Message);
                }
            }
            return doctorBuscado;
        }

        //Eliminar
        public static void EliminarDoctor(int idEmpleado)
        {

            using (SqlConnection conexion = conexionBBDD())
            {
                SqlTransaction transaction = conexion.BeginTransaction();

                try
                {
                    // Eliminar de Doctores
                    SqlCommand EliminarDoctores = new SqlCommand("DELETE FROM Doctores WHERE IdDoctor = @Id", conexion, transaction);
                    EliminarDoctores.Parameters.AddWithValue("@Id", idEmpleado);
                    EliminarDoctores.ExecuteNonQuery();

                    // Eliminar de Empleados
                    SqlCommand deleteEmpleadosCommand = new SqlCommand("DELETE FROM Empleados WHERE Id = @Id", conexion, transaction);
                    deleteEmpleadosCommand.Parameters.AddWithValue("@Id", idEmpleado);
                    deleteEmpleadosCommand.ExecuteNonQuery();

                    // Confirmar la transacción
                    transaction.Commit();
                    MessageBox.Show("Se ha eliminado el empleado.");
                }
                catch(Exception ex)
                {
                    // Si ocurre un error, revertir la transacción
                    transaction.Rollback();
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
            }

        }
        public static void EliminarAuxiliar(int idAuxiliar)
        {
            using (SqlConnection conexion = conexionBBDD())
            {
                SqlTransaction sqlTransaction = conexion.BeginTransaction();

                try
                {
                    // Eliminar de Auxiliar
                    SqlCommand deleteAuxiliaresCommand = new SqlCommand("DELETE FROM EmpleadosAux WHERE IdEmpleado = @Id", conexion, sqlTransaction);
                    deleteAuxiliaresCommand.Parameters.AddWithValue("@Id", idAuxiliar);
                    deleteAuxiliaresCommand.ExecuteNonQuery();

                    // Eliminar de Empleados
                    SqlCommand deleteEmpleadosCommand = new SqlCommand("DELETE FROM Empleados WHERE Id = @Id", conexion, sqlTransaction);
                    deleteEmpleadosCommand.Parameters.AddWithValue("@Id", idAuxiliar);
                    deleteEmpleadosCommand.ExecuteNonQuery();

                    sqlTransaction.Commit();
                    MessageBox.Show("Se ha eliminado el empleado");
                }
                catch(Exception ex)
                {
                    // Si ocurre un error, revertir la transacción
                    sqlTransaction.Rollback();
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
            }
        }
        public static void EliminarPaciente(int idPaciente)
        {
            using (SqlConnection conexion = conexionBBDD())
            {
                SqlTransaction sqlTransaction = conexion.BeginTransaction();

                try
                { 
                    // Eliminar de Empleados
                    SqlCommand deletePacienteCommand = new SqlCommand("DELETE FROM Pacientes WHERE IdPacientes = @Id", conexion, sqlTransaction);
                    deletePacienteCommand.Parameters.AddWithValue("@Id", idPaciente);
                    deletePacienteCommand.ExecuteNonQuery();

                    sqlTransaction.Commit();
                    MessageBox.Show("Se ha eliminado el paciente");
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, revertir la transacción
                    sqlTransaction.Rollback();
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
            }
        }

        //Actualizaciones o Modificaciones
        public static int ActualizarHistorialPaciente(int idPaciente, string nuevoHistorial)
        {
            int retorno = 0;

            using (SqlConnection conexion = conexionBBDD())
            {
                using (SqlTransaction transaction = conexion.BeginTransaction())
                {
                    try
                    {
                        // Actualiza solo el campo Historial del paciente con el id dado
                        string query = @"UPDATE Pacientes SET Historial = @Historial WHERE IdPacientes = @IdPacientes";

                        SqlCommand cmd = new SqlCommand(query, conexion, transaction);
                        cmd.Parameters.AddWithValue("@Historial", nuevoHistorial);
                        cmd.Parameters.AddWithValue("@IdPacientes", idPaciente);

                        retorno = cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Ha ocurrido un error: " + e.Message);
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return retorno;
        }
        public static void ModificarPaciente(int idPaciente, string emailPaciente, int documentoPaciente,
            string nombrePaciente, string apellidoPaciente, string telefonoPaciente, DateTime fechaingreso, DateTime fechanacimiento,
            string legajo)
        {
            using (SqlConnection conexion= conexionBBDD())
            {
                SqlTransaction transaction = conexion.BeginTransaction();
                string query = "UPDATE Pacientes " +
                "SET Email = @Email, Documento = @Documento, Nombre = @Nombre, Apellido = @Apellido, " +
                "Telefono = @Telefono, FechaIngreso = @FechaIngreso, FechaNacimiento = @FechaNacimiento, " +
                "Legajo = @Legajo " +
                "WHERE IdPacientes = @Id";

                try
                { 
                    SqlCommand updatecmd = new SqlCommand(query, conexion, transaction);
                    updatecmd.Parameters.AddWithValue("@Id", idPaciente);
                    updatecmd.Parameters.AddWithValue("@Email", emailPaciente);
                    updatecmd.Parameters.AddWithValue("@Documento", documentoPaciente);
                    updatecmd.Parameters.AddWithValue("@Nombre", nombrePaciente);
                    updatecmd.Parameters.AddWithValue("@Apellido", apellidoPaciente);
                    updatecmd.Parameters.AddWithValue("@Telefono", telefonoPaciente);
                    updatecmd.Parameters.AddWithValue("@FechaIngreso", fechaingreso);
                    updatecmd.Parameters.AddWithValue("@FechaNacimiento", fechanacimiento);
                    updatecmd.Parameters.AddWithValue("@Legajo", legajo);
                    // Ejecutar el comando
                    updatecmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show(e.Message);
                }
            }
        }
        //Terminar con las ventanas de modificaciones
        public static void ModificarDoctores(Entidades.Program.Doctores doctor)
        {
            using (SqlConnection conexion = conexionBBDD())
            {
                string queryEmpleado = @"UPDATE Empleados 
                             SET Nombre = @Nombre, 
                                 Apellido = @Apellido, 
                                 Email = @Email, 
                                 Nacionalidad = @Nacionalidad, 
                                 FechaNacimiento = @FechaNacimiento, 
                                 FechaIngreso = @FechaIngreso, 
                                 Telefono = @Telefono, 
                                 Domicilio = @Domicilio, 
                                 Localidad = @Localidad, 
                                 Documento = @Documento
                             WHERE Id = @IdEmpleado";

                // Query SQL para actualizar los datos en la tabla Doctores
                string queryDoctor = @"UPDATE Doctores 
                           SET Cargo = @Cargo, 
                               Matricula = @Matricula
                           WHERE IdDoctor = @IdEmpleado";
                using (SqlTransaction transaction = conexion.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmdEmpleados=new SqlCommand(queryEmpleado,conexion,transaction))
                        {
                            cmdEmpleados.Parameters.AddWithValue("@IdEmpleado", doctor.Id);
                            cmdEmpleados.Parameters.AddWithValue("@Nombre",doctor.Nombre);
                            cmdEmpleados.Parameters.AddWithValue("@Apellido",doctor.Apellido);
                            cmdEmpleados.Parameters.AddWithValue("@Email",doctor.Email);
                            cmdEmpleados.Parameters.AddWithValue("@Nacionalidad",doctor.Nacionalidad);
                            cmdEmpleados.Parameters.AddWithValue("@FechaNacimiento",doctor.FechaNacimiento);
                            cmdEmpleados.Parameters.AddWithValue("@FechaIngreso", doctor.FechaIngreso);
                            cmdEmpleados.Parameters.AddWithValue("@Telefono", doctor.Telefono);
                            cmdEmpleados.Parameters.AddWithValue("@Domicilio", doctor.Domicilio);
                            cmdEmpleados.Parameters.AddWithValue("@Localidad", doctor.Localidad);
                            cmdEmpleados.Parameters.AddWithValue("@Documento", doctor.Documento);
                            // Ejecuta el comando para actualizar la tabla Empleados
                            cmdEmpleados.ExecuteNonQuery();
                        }
                        using (SqlCommand cmdDoctor= new SqlCommand(queryDoctor,conexion,transaction))
                        {
                            cmdDoctor.Parameters.AddWithValue("@IdEmpleado", doctor.Id);
                            cmdDoctor.Parameters.AddWithValue("@Cargo", doctor.Cargo);
                            cmdDoctor.Parameters.AddWithValue("@Matricula", doctor.Matricula);
                            // Ejecuta el comando para actualizar la tabla Doctores
                            cmdDoctor.ExecuteNonQuery();
                        }

                        // Si ambos comandos se ejecutaron correctamente, confirma la transacción
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        // Si ocurre un error, revierte la transacción
                        transaction.Rollback();
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        public static void ModificarAuxiliares(Entidades.Program.EmpleadosAux Auxiliar)
        {
            using (SqlConnection conexion=conexionBBDD())
            {
                string queryEmpleado = @"UPDATE Empleados 
                             SET Nombre = @Nombre, 
                                 Apellido = @Apellido, 
                                 Email = @Email, 
                                 Nacionalidad = @Nacionalidad, 
                                 FechaNacimiento = @FechaNacimiento, 
                                 FechaIngreso = @FechaIngreso, 
                                 Telefono = @Telefono, 
                                 Domicilio = @Domicilio, 
                                 Localidad = @Localidad, 
                                 Documento = @Documento
                             WHERE Id = @IdEmpleado";

                string queryAux = @"UPDATE EmpleadosAux
                                SET Cargo=@Cargo
                                WHERE IdEmpleado=@IdEmpleado";

                using (SqlTransaction transaction = conexion.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmdEmpleados = new SqlCommand(queryEmpleado, conexion, transaction))
                        {
                            cmdEmpleados.Parameters.AddWithValue("@IdEmpleado", Auxiliar.Id);
                            cmdEmpleados.Parameters.AddWithValue("@Nombre", Auxiliar.Nombre);
                            cmdEmpleados.Parameters.AddWithValue("@Apellido", Auxiliar.Apellido);
                            cmdEmpleados.Parameters.AddWithValue("@Email", Auxiliar.Email);
                            cmdEmpleados.Parameters.AddWithValue("@Nacionalidad", Auxiliar.Nacionalidad);
                            cmdEmpleados.Parameters.AddWithValue("@FechaNacimiento", Auxiliar.FechaNacimiento);
                            cmdEmpleados.Parameters.AddWithValue("@FechaIngreso", Auxiliar.FechaIngreso);
                            cmdEmpleados.Parameters.AddWithValue("@Telefono", Auxiliar.Telefono);
                            cmdEmpleados.Parameters.AddWithValue("@Domicilio", Auxiliar.Domicilio);
                            cmdEmpleados.Parameters.AddWithValue("@Localidad", Auxiliar.Localidad);
                            cmdEmpleados.Parameters.AddWithValue("@Documento", Auxiliar.Documento);
                            // Ejecuta el comando para actualizar la tabla Empleados
                            cmdEmpleados.ExecuteNonQuery();
                        }
                        using (SqlCommand cmdAux = new SqlCommand(queryAux, conexion, transaction))
                        {
                            cmdAux.Parameters.AddWithValue("@IdEmpleado", Auxiliar.Id);
                            cmdAux.Parameters.AddWithValue("@Cargo", Auxiliar.Cargo);
                            // Ejecuta el comando para actualizar la tabla Doctores
                            cmdAux.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }

        //Ordenamientos
        public static List<Empleados> ObtenerEmpleadosOrdenados(string columnaOrden)
        {
            List<Empleados> empleados = new List<Empleados>();

            using (SqlConnection conexion = conexionBBDD())
            {
                // Usamos parámetro para especificar la columna de orden
                string query = $"SELECT * FROM Empleados ORDER BY {columnaOrden}";

                SqlCommand command = new SqlCommand(query, conexion);

                try
                {

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Empleados empleado = new Empleados
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Password = reader.GetString(3),
                            Email = reader.GetString(4),
                            Nacionalidad = reader.GetString(5),
                            FechaNacimiento = reader.GetDateTime(6),
                            FechaIngreso = reader.GetDateTime(7),
                            Telefono = reader.GetString(8),
                            Domicilio = reader.GetString(9),
                            Localidad = reader.GetString(10),
                            Documento = reader.GetInt32(11)
                        };

                        empleados.Add(empleado);
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e.Message);
                }
            }

            return empleados;
        }
        public static List<Doctores> ObtenerDoctoresOrdenados(string columnaOrden)
        {
            List<Doctores> doctores = new List<Doctores>();

            using (SqlConnection conexion = conexionBBDD())
            {
                
                string query = $@"
                SELECT 
                    e.Id,
                    e.Nombre,
                    e.Apellido,
                    e.Email,
                    e.Nacionalidad,
                    e.FechaNacimiento,
                    e.FechaIngreso,
                    e.Telefono,
                    e.Domicilio,
                    e.Localidad,
                    e.Documento,
                    e.Password,
                    d.Cargo,
                    d.Matricula
                FROM Empleados e
                INNER JOIN Doctores d ON e.Id = d.IdDoctor
                ORDER BY e.{columnaOrden}";

                SqlCommand command = new SqlCommand(query, conexion);

                try
                {

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Doctores doctor = new Doctores
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Nacionalidad = reader.GetString(4),
                            FechaNacimiento = reader.GetDateTime(5),
                            FechaIngreso = reader.GetDateTime(6),
                            Telefono = reader.GetString(7),
                            Domicilio = reader.GetString(8),
                            Localidad = reader.GetString(9),
                            Documento = reader.GetInt32(10),
                            Password = reader.GetString(11),
                            Cargo = reader.GetString(12),
                            Matricula = reader.GetString(13)
                        };

                        doctores.Add(doctor);
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e.Message);
                }
            }
            return doctores;
        }
        public static List<EmpleadosAux> ObtenerAuxiliaresOrdenados(string columnaOrden)
        {
            List<EmpleadosAux> auxiliares = new List<EmpleadosAux>();

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = $@"
        SELECT 
            e.Id,
            e.Nombre,
            e.Apellido,
            e.Email,
            e.Nacionalidad,
            e.FechaNacimiento,
            e.FechaIngreso,
            e.Telefono,
            e.Domicilio,
            e.Localidad,
            e.Documento,
            e.Password,
            d.Cargo
        FROM Empleados e
        INNER JOIN EmpleadosAux d ON e.Id = d.IdEmpleado
        ORDER BY {columnaOrden}";

                SqlCommand command = new SqlCommand(query, conexion);

                try
                {

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        EmpleadosAux auxiliar = new EmpleadosAux
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Nacionalidad = reader.GetString(4),
                            FechaNacimiento = reader.GetDateTime(5),
                            FechaIngreso = reader.GetDateTime(6),
                            Telefono = reader.GetString(7),
                            Domicilio = reader.GetString(8),
                            Localidad = reader.GetString(9),
                            Documento = reader.GetInt32(10),
                            Password = reader.GetString(11),
                            Cargo = reader.GetString(12)
                        };

                        auxiliares.Add(auxiliar);
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e.Message);
                }
            }

            return auxiliares;
        }
        public static List<Pacientes> MostrarPacientesOrdenados(int idDoctor, string columnaOrden)
        {
            List<Pacientes> pacientes = new List<Pacientes>();

            using (SqlConnection conexion = conexionBBDD())
            {
                
                string query = $@"
                SELECT * 
                FROM Pacientes 
                WHERE IdDoctor = @IdDoctor
                ORDER BY {columnaOrden}";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@IdDoctor", idDoctor);

                try
                {
                    

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pacientes paciente = new Pacientes
                            {
                                IdPacientes = Convert.ToInt32(reader["IdPacientes"]),
                                Email = reader["Email"].ToString(),
                                Documento = Convert.ToInt32(reader["Documento"]),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Legajo = reader["Legajo"].ToString(),
                                FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                                FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                                IdDoctor = Convert.ToInt32(reader["IdDoctor"]),
                                Historial = reader["Historial"].ToString()
                            };
                            pacientes.Add(paciente);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e.Message);
                }
            }
            return pacientes;
        }

    }
}