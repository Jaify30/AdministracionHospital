using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Program
    {
        static void Main(string[] args)
        {
            
        }

        //Se crean todas la entidades de las BBDD

        public class Empleados
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Nacionalidad { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public DateTime FechaIngreso { get; set; }
            public string Telefono { get; set; }
            public string Domicilio { get; set; }
            public string Localidad { get; set; }
            public int Documento { get; set; }
            public bool inhibido {  get; set; }
            public Empleados() { }

            public Empleados(int Id, string Nombre,string Apellido, string Password,string Email, string Nacionalidad, DateTime FechaNacimiento,DateTime FechaIngreso,
                string Telefono, string Domicilio, string Localidad, int Documento, bool inhibido)
            {
                this.Id = Id;
                this.Nombre = Nombre;
                this.Apellido = Apellido;
                this.Password = Password;
                this.Email = Email;
                this.Nacionalidad = Nacionalidad;
                this.FechaNacimiento = FechaNacimiento;
                this.FechaIngreso = FechaIngreso;
                this.Telefono = Telefono;
                this.Domicilio = Domicilio;
                this.Localidad = Localidad;
                this.Documento = Documento;
                this.inhibido = inhibido;
            }
        }

        public class EmpleadosAux: Empleados
        {
            public string Cargo { get; set; }

            public EmpleadosAux() : base() { }

            public EmpleadosAux(int Id, string Nombre, string Apellido, string Password, string Email, string Nacionalidad, DateTime FechaNacimiento, DateTime FechaIngreso,
                 string Telefono, string Domicilio, string Localidad, int Documento, string Cargo, bool inhibido) : base(Id, Nombre, Apellido,
                    Password,Email,Nacionalidad, FechaNacimiento, FechaIngreso,
                    Telefono, Domicilio, Localidad, Documento, inhibido)
            {
                this.Cargo = Cargo;
            }

        }

        public class Administradores : Empleados
        {
            public DateTime FechaAsignacion {  get; set; }
            public string Unique_pass {  get; set; }

            public Administradores() : base() { }

            public Administradores(int Id, string Nombre, string Apellido, string Password, string Email, string Nacionalidad, DateTime FechaNacimiento, DateTime FechaIngreso,
                string Telefono, string Domicilio, string Localidad, int Documento, DateTime FechaAsignacion, string Unique_Pass,bool inhibido) : base(Id,Nombre,Apellido,Password,Email,Nacionalidad,FechaNacimiento,FechaIngreso
                    ,Telefono,Domicilio,Localidad,Documento, inhibido)
            {
                this.FechaAsignacion = FechaAsignacion;
                this.Unique_pass = Unique_pass;
            }
        }

        public class Doctores : Empleados
        {
            public string Cargo { get; set; }
            public string Matricula { get; set; }

            public Doctores() : base() { }

            public Doctores(int Id, string Nombre, string Apellido, string Password, string Email, string Nacionalidad, DateTime FechaNacimiento, DateTime FechaIngreso,
                string Telefono, string Domicilio, string Localidad, int Documento, string Cargo, string Matricula, bool inhibido) : base(Id, Nombre, Apellido,
                    Password, Email, Nacionalidad, FechaNacimiento, FechaIngreso,
                    Telefono, Domicilio, Localidad, Documento, inhibido)
            {
                this.Cargo = Cargo;
                this.Matricula = Matricula;
            }
        }

        public class Pacientes
        {
            public int IdPacientes { get; set; }
            public string Email { get; set; }
            public int Documento {  get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Telefono { get; set; }
            public string Legajo {  get; set; }
            public DateTime FechaNacimiento { get;set; }
            public DateTime FechaIngreso {  get; set; }
            public int IdDoctor { get; set; }
            public string Historial { get; set; }
            public bool inhibido {  get; set; }

            public Pacientes() { }
            public Pacientes(int IdPacientes, string Email, int Documento, string Nombre, string Apellido, string Telefono, string Legajo, DateTime FechaNacimiento,
                DateTime FechaIngreso, int IdDoctor, string Historial, bool inhibido)
            {
                this.IdPacientes = IdPacientes;
                this.Email = Email;
                this.Documento = Documento;
                this.Nombre = Nombre;
                this.Apellido = Apellido;
                this.Telefono = Telefono;
                this.Legajo = Legajo;
                this.FechaIngreso = FechaIngreso;
                this.FechaNacimiento = FechaNacimiento;
                this.IdDoctor = IdDoctor;
                this.Historial = Historial;
                this.inhibido = inhibido;
            }
        }

    }
}
