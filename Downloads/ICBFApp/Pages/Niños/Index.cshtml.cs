using ICBFApp.Pages.Jardines;
using ICBFApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Fluent;
using System.Data.SqlClient;

namespace ICBFApp.Pages.Niños
{
    public class IndexModel : PageModel
    {
        private readonly IGeneratePdfService _generatePdfService;
        private readonly IWebHostEnvironment _host;

        public IndexModel(IGeneratePdfService generatePdfService, IWebHostEnvironment host)
        {
            _generatePdfService = generatePdfService;
            _host = host;
        }
        public List<NiñoInfo> listNiño = new List<NiñoInfo>();
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlSelect = "SELECT n.NIUP, n.Nombre, n.TipoSangre, n.CiudadNacimiento, n.Telefono, n.Edad, n.Direccion, n.EPS, n.Identidad, j.Nombre AS JardinNombre, p.Nombre AS PersonaNombre " +
                      "FROM Niño n " +
                      "JOIN Jardin j ON n.fkJardin = j.Identificador " +
                      "JOIN Persona p ON n.fkPersona = p.Identificacion";


                    using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NiñoInfo niño = new NiñoInfo();
                                niño.NIUP = reader.GetInt32(0);
                                niño.Nombre = reader.GetString(1);
                                niño.TipoSangre = reader.GetString(2);
                                niño.CiudadNacimiento = reader.GetString(3);
                                niño.Telefono = reader.GetString(4);
                                niño.Edad = reader.GetString(5);
                                niño.Direccion = reader.GetString(6);
                                niño.EPS = reader.GetString(7);
                                niño.Identidad = reader.GetInt32(8);
                                niño.JardinNombre = reader.GetString(9);
                                niño.PersonaNombre = reader.GetString(10);

                                listNiño.Add(niño);
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

        public IActionResult OnPostDownloadPdf()
        {
            var report = _generatePdfService.GeneratePdfQuest();
            byte[] pdfBytes = report.GeneratePdf();
            var mimeType = "application/pdf";
            //return File(pdfBytes, mimeType, "Reporte.pdf"); // si le quito el nombre del archivo, no lo descarga auto
            return File(pdfBytes, mimeType); // si le quito el nombre del archivo, no lo descarga auto
        }
    }


    public class NiñoInfo
    {
        public int NIUP { get; set; }
        public string Nombre { get; set; }
        public string TipoSangre { get; set; }
        public string CiudadNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Edad { get; set; }
        public string Direccion { get; set; }
        public string EPS { get; set; }
        public int Identidad { get; set; } 
        public string JardinNombre { get; set; }
        public string PersonaNombre { get; set; }

        public PersonaInfo personaInfo { get; set; }
		public JardinInfo jardinInfo { get; set; }

	}
}
