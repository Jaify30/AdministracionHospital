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

namespace Funciones
{
    public class Program
    {
        static void Main(string[] args)
        {
        }

        public static SqlConnection conexionBBDD() //Se crea la conexion a la base de datos
        {
            SqlConnection conexion = new SqlConnection("Server=DESKTOP-3MSS1LQ\\JAIFY3; Database=Hospital;" +
                " Trusted_Connection=true; Integrated Security=SSPI;Persist Security Info=False;");
            conexion.Open();

            return conexion;
        }
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
                INSERT INTO Empleados (Nombre, Apellido, Password, Email, Nacionalidad, FechaNacimiento, FechaIngreso, Permiso, Token, Telefono, Domicilio, Localidad, Documento)
                VALUES (@Nombre, @Apellido, @Password, @Email, @Nacionalidad, @FechaNacimiento, @FechaIngreso, @Permiso, @Token, @Telefono, @Domicilio, @Localidad, @Documento);
                SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdEmpleado = new SqlCommand(queryEmpleado, conexion, transaccion);
                        cmdEmpleado.Parameters.AddWithValue("@Nombre", Empleado.Nombre);
                        cmdEmpleado.Parameters.AddWithValue("@Apellido", Empleado.Apellido);
                        cmdEmpleado.Parameters.AddWithValue("@Password", Empleado.Password);
                        cmdEmpleado.Parameters.AddWithValue("@Email", Empleado.Email);
                        cmdEmpleado.Parameters.AddWithValue("@Nacionalidad", Empleado.Nacionalidad);
                        cmdEmpleado.Parameters.AddWithValue("@FechaNacimiento", Empleado.FechaNacimiento);
                        cmdEmpleado.Parameters.AddWithValue("@FechaIngreso", Empleado.FechaIngreso);
                        cmdEmpleado.Parameters.AddWithValue("@Permiso", Empleado.Permiso);
                        cmdEmpleado.Parameters.AddWithValue("@Token", Empleado.Token ?? (object)DBNull.Value);
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
                        INSERT INTO Empleados (Nombre, Apellido, Password, Email, Nacionalidad, FechaNacimiento, FechaIngreso, Permiso, Token, Telefono, Domicilio, Localidad, Documento)
                        VALUES (@Nombre, @Apellido, @Password, @Email, @Nacionalidad, @FechaNacimiento, @FechaIngreso, @Permiso, @Token, @Telefono, @Domicilio, @Localidad, @Documento);
                        SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdEmpleado = new SqlCommand(queryEmpleado, conexion, transaccion);
                        cmdEmpleado.Parameters.AddWithValue("@Nombre", Empleado.Nombre);
                        cmdEmpleado.Parameters.AddWithValue("@Apellido", Empleado.Apellido);
                        cmdEmpleado.Parameters.AddWithValue("@Password", Empleado.Password);
                        cmdEmpleado.Parameters.AddWithValue("@Email", Empleado.Email);
                        cmdEmpleado.Parameters.AddWithValue("@Nacionalidad", Empleado.Nacionalidad);
                        cmdEmpleado.Parameters.AddWithValue("@FechaNacimiento", Empleado.FechaNacimiento);
                        cmdEmpleado.Parameters.AddWithValue("@FechaIngreso", Empleado.FechaIngreso);
                        cmdEmpleado.Parameters.AddWithValue("@Permiso", Empleado.Permiso);
                        cmdEmpleado.Parameters.AddWithValue("@Token", Empleado.Token ?? (object)DBNull.Value);
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
                Regex regex = new Regex(patron,RegexOptions.IgnoreCase);
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
        public static int IngresoToken(Entidades.Program.Empleados Usuario)
        {
            int retorno = 0;

            using (SqlConnection conexion = conexionBBDD())
            {
                string query = "SELECT COUNT(*) FROM Empleados WHERE Token = @Token";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@Token", Usuario.Token);

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
                        Nombre=reader.GetString(1),
                        Apellido=reader.GetString(2),
                        Password=reader.GetString(3),
                        Email=reader.GetString(4),
                        Nacionalidad=reader.GetString(5),
                        FechaNacimiento=reader.GetDateTime(6),
                        FechaIngreso=reader.GetDateTime(7),
                        Permiso=reader.GetString(8),
                        Token = reader.IsDBNull(9) ? null : reader.GetString(9),
                        Telefono=reader.GetString(10),
                        Domicilio=reader.GetString(11),
                        Localidad=reader.GetString(12),
                        Documento=reader.GetInt32(13)
                    };
                }
            }

            return empleado;
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
                            Permiso = reader.GetString(8),
                            Token = reader.IsDBNull(9) ? null : reader.GetString(9),
                            Telefono = reader.GetString(10),
                            Domicilio = reader.GetString(11),
                            Localidad = reader.GetString(12),
                            Documento = reader.GetInt32(13)//Hacer dos botones, uno donde se muestre al empleadoaux y otro a los doctores
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
                            e.Permiso,
                            e.Telefono,
                            e.Domicilio,
                            e.Localidad,
                            e.Documento,
                            e.Token,
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
                            Permiso = reader.GetString(7),
                            Telefono = reader.GetString(8),
                            Domicilio = reader.GetString(9),
                            Localidad = reader.GetString(10),
                            Documento = reader.GetInt32(11),
                            Token = reader.IsDBNull(12) ? null : reader.GetString(12),
                            Password = reader.GetString(13),
                            Cargo = reader.GetString(14),
                            Matricula = reader.GetString(15)
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
                    e.Permiso,
                    e.Telefono,
                    e.Domicilio,
                    e.Localidad,
                    e.Documento,
                    e.Token,
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
                            Permiso = reader.GetString(7),
                            Telefono = reader.GetString(8),
                            Domicilio = reader.GetString(9),
                            Localidad = reader.GetString(10),
                            Documento = reader.GetInt32(11),
                            Token = reader.IsDBNull(12) ? null : reader.GetString(12),
                            Password = reader.GetString(13),
                            Cargo = reader.GetString(14)
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
    }
}
