using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ICBFApp.Pages.Jardines;


namespace ICBFApp.Pages.Niños
{
    public class CrearNiñoModel : PageModel
    {
        public List<JardinInfo> jardinInfo { get; set; } = new List<JardinInfo>();
        public List<PersonaInfo> personaInfo { get; set; } = new List<PersonaInfo>();
        public NiñoInfo niñoInfo = new NiñoInfo();
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
            string TipoSangre = Request.Form["TipoSangre"];
            string CiudadNacimiento = Request.Form["CiudadNacimiento"];
            string Telefono = Request.Form["Telefono"];
            string Edad = Request.Form["Edad"];
            string Direccion = Request.Form["Direccion"];
            string EPS = Request.Form["EPS"];
            string Identidad = Request.Form["Identidad"];
            string Jardin = Request.Form["Jardin"];
            string Persona = Request.Form["Persona"];

            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(TipoSangre) || string.IsNullOrEmpty(CiudadNacimiento) ||
                string.IsNullOrEmpty(Telefono) || string.IsNullOrEmpty(Edad) || string.IsNullOrEmpty(Direccion) || string.IsNullOrEmpty(EPS) ||
                string.IsNullOrEmpty(Identidad) ||  string.IsNullOrEmpty(Jardin) ||
                string.IsNullOrEmpty(Persona))
            {
                errorMessage = "Todos los campos son obligatorios";
                return;
            }
            // Validación de la edad
            if (int.TryParse(Edad, out int parsedEdad))
            {
                if (parsedEdad > 6)
                {
                    errorMessage = "La edad del niño no puede superar los 6 años";
                    return;
                }
            }
            else
            {
                errorMessage = "La edad debe ser un número válido";
                return;
            }
            try
            {
                String connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sqlExists = "SELECT COUNT(*) FROM Niño WHERE Identidad = @Identidad";
                    using (SqlCommand commandCheck = new SqlCommand(sqlExists, connection))
                    {
                        commandCheck.Parameters.AddWithValue("@Identidad", Identidad);
                        int count = (int)commandCheck.ExecuteScalar();
                        if (count > 0)
                        {
                            errorMessage = "El niño con identificación " + Identidad + " ya existe. Verifique la información e intente de nuevo";
                            return;
                        }
                    }

                    String sqlInsert = "INSERT INTO Niño (Nombre, TipoSangre, CiudadNacimiento, Telefono, Edad, Direccion, EPS, Identidad, fkJardin, fkPersona) " +
                                       "VALUES (@Nombre, @TipoSangre, @CiudadNacimiento, @Telefono, @Edad, @Direccion, @EPS, @Identidad, @Jardin, @Persona)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
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

                successMessage = "Niño creado exitosamente";
                RedirectToPage("/Niños/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }

}
