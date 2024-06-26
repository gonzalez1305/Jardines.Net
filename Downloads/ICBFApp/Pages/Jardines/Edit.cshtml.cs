using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ICBFApp.Pages.Jardines
{
    public class EditModel : PageModel
    {
        public JardinInfo jardinInfo = new JardinInfo();
        public string errorMessage = "";
        public string succesMessage = "";

        public void OnGet()
        {
            string Identificador = Request.Query["Identificador"];

            try
            {
                string connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Jardin WHERE Identificador = @Identificador";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Identificador", Identificador);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                jardinInfo.Identificador = reader["Identificador"].ToString();
                                jardinInfo.Nombre = reader["Nombre"].ToString();
                                jardinInfo.Direccion = reader["Direccion"].ToString();
                                jardinInfo.Estado = reader["Estado"].ToString();
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
            jardinInfo.Identificador = Request.Form["jardinInfo.Identificador"];
            jardinInfo.Nombre = Request.Form["jardinInfo.Nombre"];
            jardinInfo.Direccion = Request.Form["jardinInfo.Direccion"];
            jardinInfo.Estado = Request.Form["jardinInfo.Estado"];

            if (jardinInfo.Identificador.Length == 0 || jardinInfo.Nombre.Length == 0 || jardinInfo.Direccion.Length == 0 || jardinInfo.Estado.Length == 0)
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
                    string sqlUpdate = "UPDATE Jardin SET Nombre = @Nombre, Direccion = @Direccion, Estado = @Estado WHERE Identificador = @Identificador";
                    using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@Identificador", jardinInfo.Identificador);
                        command.Parameters.AddWithValue("@Nombre", jardinInfo.Nombre);
                        command.Parameters.AddWithValue("@Direccion", jardinInfo.Direccion);
                        command.Parameters.AddWithValue("@Estado", jardinInfo.Estado);

                        command.ExecuteNonQuery();
                    }
                }
                succesMessage = "Jardín actualizado exitosamente.";
                Response.Redirect("/Jardines/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
