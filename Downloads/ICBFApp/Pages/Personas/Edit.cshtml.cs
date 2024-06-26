using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static ICBFApp.Pages.Roles.IndexModel;

namespace ICBFApp.Pages.Personas
{
    public class EditModel : PageModel
    {
        public PersonaInfo personaInfo { get; set; } = new PersonaInfo();
        public List<RolInfo> rolInfo { get; set; } = new List<RolInfo>();
        public RolInfo rolinfoSelected { get; set; } = new RolInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String Identificacion = Request.Query["Identificacion"];
            try
            {
                string connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sqlPersona = "SELECT * FROM Persona WHERE Identificacion = @Identificacion";
                    using (SqlCommand command = new SqlCommand(sqlPersona, connection))
                    {
                        command.Parameters.AddWithValue("@Identificacion", Identificacion);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                personaInfo.Identificacion = reader.GetInt32(0);
                                personaInfo.Nombre = reader.GetString(1);
                                personaInfo.Cedula = reader.GetString(2);
                                personaInfo.Telefono = reader.GetString(3);
                                personaInfo.Direccion = reader.GetString(4);
                                personaInfo.Correo = reader.GetString(5);
                                personaInfo.Rol = reader.GetInt32(6).ToString();
                            }
                        }
                    }

                    String sqlRoles = "SELECT IdRol, Rol FROM Rol WHERE IdRol != @IdRol";
                    using (SqlCommand command = new SqlCommand(sqlRoles, connection))
                    {
                        command.Parameters.AddWithValue("@IdRol", personaInfo.Rol);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rolInfo.Add(new RolInfo
                                {
                                    IdRol = reader.GetInt32(0).ToString(),
                                    Rol = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            string Identificacion = Request.Form["personaInfo.Identificacion"];
            string Nombre = Request.Form["nombre"];
            string Cedula = Request.Form["cedula"];
            string Telefono = Request.Form["telefono"];
            string Direccion = Request.Form["direccion"];
            string Correo = Request.Form["correo"];
            string fkRol = Request.Form["rol"];

            if (string.IsNullOrEmpty(Identificacion) || string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Cedula) || string.IsNullOrEmpty(Telefono)
                 || string.IsNullOrEmpty(Direccion)
                || string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(fkRol))
            {
                errorMessage = "Todos los campos son obligatorios";
                return;
            }

            try
            {
                string connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sqlUpdate = "UPDATE Persona SET " +
                                        "Nombre = @nombre, Cedula = @cedula, Telefono = @telefono, " +
                                        "Direccion = @direccion, Correo = @correo, fkRol = @fkRol WHERE Identificacion = @Identificacion";

                    using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@Identificacion", Identificacion);
                        command.Parameters.AddWithValue("@nombre", Nombre);
                        command.Parameters.AddWithValue("@cedula", Cedula);
                        command.Parameters.AddWithValue("@telefono", Telefono);
                        command.Parameters.AddWithValue("@direccion", Direccion);
                        command.Parameters.AddWithValue("@correo", Correo);
                        command.Parameters.AddWithValue("@fkRol", fkRol);

                        command.ExecuteNonQuery();
                    }
                }

                successMessage = "Persona editada exitosamente";
                RedirectToPage("/Personas/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
