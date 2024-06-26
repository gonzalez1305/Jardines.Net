using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static ICBFApp.Pages.Roles.IndexModel;

namespace ICBFApp.Pages.Personas
{
    public class CrearPersonaModel : PageModel
    {
        public List<RolInfo> rolInfo { get; set; } = new List<RolInfo>();
        public PersonaInfo personaInfo = new PersonaInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT IdRol, Rol FROM Rol";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
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
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            string Nombre = Request.Form["Nombre"];
            string Cedula = Request.Form["Cedula"];
            string Telefono = Request.Form["Telefono"];
            string Direccion = Request.Form["Direccion"];
            string Correo = Request.Form["Correo"];
            string rolIdString = Request.Form["Rol"];
            int fkRol;

            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Cedula) || string.IsNullOrEmpty(Telefono)
              || string.IsNullOrEmpty(Direccion)
                || string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(rolIdString))
            {
                errorMessage = "Todos los campos son obligatorios";
                return;
            }

            if (!int.TryParse(rolIdString, out fkRol))
            {
                errorMessage = "Rol inválido seleccionado";
                return;
            }

            try
            {
                String connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sqlExists = "SELECT COUNT(*) FROM Persona WHERE Cedula = @Cedula";
                    using (SqlCommand commandCheck = new SqlCommand(sqlExists, connection))
                    {
                        commandCheck.Parameters.AddWithValue("@Cedula", Cedula);
                        int count = (int)commandCheck.ExecuteScalar();
                        if (count > 0)
                        {
                            errorMessage = "El usuario con identificación " + Cedula + " ya existe. Verifique la información e intente de nuevo";
                            return;
                        }
                    }

                    String sqlInsert = "INSERT INTO Persona (Nombre, Cedula, Telefono, Direccion, Correo, fkRol) " +
                                       "VALUES (@Nombre, @Cedula, @Telefono, @Direccion, @Correo, @fkRol)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", Nombre);
                        command.Parameters.AddWithValue("@Cedula", Cedula);
                        command.Parameters.AddWithValue("@Telefono", Telefono);
                        command.Parameters.AddWithValue("@Direccion", Direccion);
                        command.Parameters.AddWithValue("@Correo", Correo);
                        command.Parameters.AddWithValue("@fkRol", fkRol);

                        command.ExecuteNonQuery();
                    }
                }

                successMessage = "Persona creada exitosamente";
                RedirectToPage("/Personas/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
public class PersonaInfo
{
    public string Identificacion { get; set; }
    public string Nombre { get; set; }
    public string Cedula { get; set;}
    public string Telefono { get; set;}
    public string Correo { get; set;}
    public string FkRol { get; set;}
    public string Direccion { get;set;}

}