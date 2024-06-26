using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ICBFApp.Pages.Personas
{
    public class IndexModel : PageModel
    {
        public List<PersonaInfo> listPersona = new List<PersonaInfo>();
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlSelect = "SELECT p.Identificacion, p.Nombre, p.Cedula, p.Telefono, p.Direccion, p.Correo, r.Rol " +
                                       "FROM Persona p " +
                                       "JOIN Rol r ON p.fkRol = r.IdRol";

                    using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PersonaInfo persona = new PersonaInfo();
                                persona.Identificacion = reader.GetInt32(0);
                                persona.Nombre = reader.GetString(1);
                                persona.Cedula = reader.GetString(2);
                                persona.Telefono = reader.GetString(3);
                                persona.Direccion = reader.GetString(4);
                                persona.Correo = reader.GetString(5);
                                persona.Rol = reader.GetString(6);

                                listPersona.Add(persona);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
                Console.WriteLine(ErrorMessage);
            }
        }
    }

    public class PersonaInfo
    {
        public int Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
    }
}
