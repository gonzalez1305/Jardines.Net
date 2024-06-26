using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using ICBFApp.Pages.Jardines;

namespace ICBFApp.Pages.Niños
{
    public class EditModel : PageModel
    {
        public List<JardinInfo> jardinInfo { get; set; } = new List<JardinInfo>();
        public List<PersonaInfo> personaInfo { get; set; } = new List<PersonaInfo>();
        public NiñoInfo niñoInfo { get; set; } = new NiñoInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet(int NIUP)
        {
            try
            {
                String connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sqlJardin = "SELECT Identificador, Nombre FROM Jardin";
                    using (SqlCommand command = new SqlCommand(sqlJardin, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    jardinInfo.Add(new JardinInfo
                                    {
                                        Identificador = reader.GetInt32(0).ToString(),
                                        Nombre = reader.GetString(1)
                                    });
                                }
                            }
                        }
                    }
                    String sqlPersona = "SELECT Identificacion, Nombre FROM Persona";
                    using (SqlCommand command = new SqlCommand(sqlPersona, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    personaInfo.Add(new PersonaInfo
                                    {
                                        Identificacion = reader.GetInt32(0).ToString(),
                                        Nombre = reader.GetString(1)
                                    });
                                }
                            }
                        }
                    }

                    string sqlSelect = "SELECT NIUP, Nombre, TipoSangre, CiudadNacimiento, Telefono, Edad, Direccion, EPS, Identidad, fkJardin, fkPersona " +
                      "FROM Niño WHERE NIUP = @NIUP";

                    using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                    {
                        command.Parameters.AddWithValue("@NIUP", NIUP);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                niñoInfo.NIUP = reader.GetInt32(0);
                                niñoInfo.Nombre = reader.GetString(1);
                                niñoInfo.TipoSangre = reader.GetString(2);
                                niñoInfo.CiudadNacimiento = reader.GetString(3);
                                niñoInfo.Telefono = reader.GetString(4);
                                niñoInfo.Edad = reader.GetString(5);
                                niñoInfo.Direccion = reader.GetString(6);
                                niñoInfo.EPS = reader.GetString(7);
                                niñoInfo.Identidad = reader.GetInt32(8);
                                niñoInfo.JardinNombre = reader.GetInt32(9).ToString();  // Convertir a string
                                niñoInfo.PersonaNombre = reader.GetInt32(10).ToString();  // Convertir a string
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
            string NIUP = Request.Form["niñoInfo.NIUP"];
            string Nombre = Request.Form["Nombre"];
            string TipoSangre = Request.Form["TipoSangre"];
            string CiudadNacimiento = Request.Form["CiudadNacimiento"];
            string Telefono = Request.Form["Telefono"];
            string Edad = Request.Form["Edad"];
            string Direccion = Request.Form["Direccion"];
            string EPS = Request.Form["EPS"];
            string Identidad = Request.Form["Identidad"];
            string Jardin = Request.Form["Jardin"];
            string Persona = Request.Form["Persona"];

            if (string.IsNullOrEmpty(NIUP) || string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(TipoSangre) || string.IsNullOrEmpty(CiudadNacimiento)
                || string.IsNullOrEmpty(Telefono) || string.IsNullOrEmpty(Edad) || string.IsNullOrEmpty(Direccion) || string.IsNullOrEmpty(EPS)
                || string.IsNullOrEmpty(Identidad) || string.IsNullOrEmpty(Jardin) || string.IsNullOrEmpty(Persona))
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

                    String sqlUpdate = "UPDATE Niño SET Nombre = @Nombre, TipoSangre = @TipoSangre, CiudadNacimiento = @CiudadNacimiento, Telefono = @Telefono, Edad = @Edad, Direccion = @Direccion, EPS = @EPS, Identidad = @Identidad, fkJardin = @Jardin, fkPersona = @Persona WHERE NIUP = @NIUP";

                    using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                    {
                        {
                            command.Parameters.AddWithValue("@NIUP", NIUP);
                            command.Parameters.AddWithValue("@Nombre", Nombre);
                            command.Parameters.AddWithValue("@TipoSangre", TipoSangre);
                            command.Parameters.AddWithValue("@CiudadNacimiento", CiudadNacimiento);
                            command.Parameters.AddWithValue("@Telefono", Telefono);
                            command.Parameters.AddWithValue("@Edad", Edad);
                            command.Parameters.AddWithValue("@Direccion", Direccion);
                            command.Parameters.AddWithValue("@EPS", EPS);
                            command.Parameters.AddWithValue("@Identidad", Identidad);
                            command.Parameters.AddWithValue("@Jardin", Jardin);
                            command.Parameters.AddWithValue("@Persona", Persona);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                successMessage = "Niño editado exitosamente";
                RedirectToPage("/Niños/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
