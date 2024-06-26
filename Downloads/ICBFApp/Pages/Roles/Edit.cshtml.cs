using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ICBFApp.Pages.Roles
{
    public class EditModel : PageModel
    {
        public RolInfo rolInfo { get; set; } = new RolInfo();
        public string errorMessage { get; set; } = "";
        public string successMessage { get; set; } = "";

        public void OnGet()
        {
            string IdRol = Request.Query["IdRol"];

            if (string.IsNullOrEmpty(IdRol))
            {
                errorMessage = "IdRol is missing.";
                return;
            }

            try
            {
                string connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Rol WHERE IdRol = @IdRol";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IdRol", IdRol);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                rolInfo.IdRol = reader["IdRol"].ToString();
                                rolInfo.Rol = reader["Rol"].ToString();
                            }
                            else
                            {
                                errorMessage = "Rol not found.";
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
            rolInfo.IdRol = Request.Form["rolInfo.IdRol"];
            rolInfo.Rol = Request.Form["rolInfo.Rol"];

            if (string.IsNullOrEmpty(rolInfo.IdRol) || string.IsNullOrEmpty(rolInfo.Rol))
            {
                errorMessage = "Todos los campos son obligatorios.";
                return;
            }

            try
            {
                string connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlUpdate = "UPDATE Rol SET Rol = @Rol WHERE IdRol = @IdRol";
                    using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@IdRol", rolInfo.IdRol);
                        command.Parameters.AddWithValue("@Rol", rolInfo.Rol);

                        command.ExecuteNonQuery();
                    }
                }
                successMessage = "Rol actualizado exitosamente.";
                Response.Redirect("/Roles/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }

    public class RolInfo
    {
        public string IdRol { get; set; }
        public string Rol { get; set; }
    }
}
