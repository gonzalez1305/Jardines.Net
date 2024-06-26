using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static ICBFApp.Pages.Roles.IndexModel;

namespace ICBFApp.Pages.Roles
{
    public class CrearRolModel : PageModel
    {
        public RolInfo rolInfo = new RolInfo();
        public string errorMessage = "";
        public string succesMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            rolInfo.Rol = Request.Form["Rol"];


            if (rolInfo.Rol.Length == 0 )
            {
                errorMessage = "Debe completar todos los campos";
                return;
            }

            try
            {
                string connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Verificar si ya existe un registro con el rol especificado
                    if (rolInfo.Rol == "Madre Comunitaria" || rolInfo.Rol == "Administrador" || rolInfo.Rol == "Acudiente")
                    {
                        string sqlSelect = "SELECT COUNT(*) FROM Rol WHERE Rol = @Rol";
                        using (SqlCommand selectCommand = new SqlCommand(sqlSelect, connection))
                        {
                            selectCommand.Parameters.AddWithValue("@Rol", rolInfo.Rol);
                            int count = (int)selectCommand.ExecuteScalar();
                            if (count > 0)
                            {
                                errorMessage = $"El rol {rolInfo.Rol} solo puede ser registrado una sola vez.";
                                return;
                            }
                        }
                    }
                    string sqlInsert = "INSERT INTO Rol (Rol) VALUES (@Rol)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        command.Parameters.AddWithValue("@Rol", rolInfo.Rol);
                       

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            rolInfo.Rol = ""; 
            succesMessage = "Rol Agregado Exitosamente!";
            Response.Redirect("/Roles/Index");
        }
    }

    public class JardinInfo
    {
        public string IdRol { get; set; }
        public string Rol { get; set; }
      

    }
}
