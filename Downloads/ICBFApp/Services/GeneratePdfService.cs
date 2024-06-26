using ICBFApp.Pages.Niños;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data.SqlClient;
using ICBFApp.Pages.Jardines;
using ICBFApp.Pages.Personas;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ICBFApp.Services
{
	public class GeneratePdfService : IGeneratePdfService
	{
		private readonly IWebHostEnvironment _host;

		public GeneratePdfService(IWebHostEnvironment host)
		{
			_host = host;
		}

		public List<NiñoInfo> listNiño = new List<NiñoInfo>();


		public void GetData()
		{
			try
			{
				string connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					string sqlSelect = "SELECT n.Nombre, n.TipoSangre, n.CiudadNacimiento, n.Edad, n.EPS, n.Identidad, j.Nombre AS JardinNombre, p.Nombre AS PersonaNombre " +
					  "FROM Niño n " +
					  "JOIN Jardin j ON n.fkJardin = j.Identificador " +
					  "JOIN Persona p ON n.fkPersona = p.Identificacion";

					using (SqlCommand command = new SqlCommand(sqlSelect, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							// Validar si hay datos
							if (reader.HasRows)
							{
								while (reader.Read())
								{
									NiñoInfo niño = new NiñoInfo();
									niño.Nombre = reader.GetString(0);
									niño.TipoSangre = reader.GetString(1);
									niño.CiudadNacimiento = reader.GetString(2);
									niño.Edad = reader.GetString(3);
									niño.EPS = reader.GetString(4);
									niño.Identidad = reader.GetInt32(5);

									JardinInfo jardin = new JardinInfo();
									jardin.Nombre = reader.GetString(6);

									PersonaInfo persona = new PersonaInfo();
									persona.Nombre = reader.GetString(7);

									niño.jardinInfo = jardin;
									niño.personaInfo = persona;

									listNiño.Add(niño);

								}
							}
							else
							{
								Console.WriteLine("No hay filas en el resultado");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex);
			}
		}


		public Document GeneratePdfQuest()
		{
			GetData();
			DateTime today = DateTime.Today;
			var report = Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Margin(30);
					page.Size(PageSizes.A4);
					page.PageColor(Colors.White);
					page.DefaultTextStyle(x => x.FontSize(12));

					page.Header().Row(row =>
					{
						row.RelativeItem().AlignRight().Column(col =>
						{
							col.Item().Height(20).Text("JARDIN ICBF").Bold().FontSize(14).AlignRight();
							col.Item().Height(20).Text("Reporte Diario").Bold().AlignRight();
							col.Item().Height(20).Text("Fecha de emisión: " + today.Date.ToShortDateString()).AlignRight();
						});
					});

					page.Content().PaddingVertical(10).Column(col =>
					{
						col.Item().PaddingVertical(10).AlignCenter()
						.Text("Listado de Niños Inscritos")
						.Bold().FontSize(24).FontColor("#39a900");

						col.Item().Table(table =>
						{
							table.ColumnsDefinition(columns =>
							{
								columns.ConstantColumn(50);
								columns.RelativeColumn(2);
								columns.ConstantColumn(70);
								columns.RelativeColumn(2);
								columns.RelativeColumn();
								columns.RelativeColumn();
								columns.RelativeColumn();
								columns.RelativeColumn();
							});

							table.Header(header =>
							{
								header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("Identidad").FontColor("#fff").AlignCenter();
								header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("Nombre").FontColor("#fff").AlignCenter();
								header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("TipoSangre").FontColor("#fff").AlignCenter();
								header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("CiudadNacimiento").FontColor("#fff").AlignCenter();
								header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("Edad").FontColor("#fff").AlignCenter();
								header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("EPS").FontColor("#fff").AlignCenter();
								header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("Jardin").FontColor("#fff").AlignCenter();
								header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("Acudiente").FontColor("#fff").AlignCenter();

							});

							foreach (var niño in listNiño)
							{
								table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(niño.Identidad.ToString()).AlignCenter();
								table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(niño.Nombre).AlignCenter();
								table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(niño.TipoSangre).AlignCenter();
								table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(niño.CiudadNacimiento).AlignCenter();
								table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(niño.Edad).AlignCenter();
								table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(niño.EPS).AlignCenter();
								table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(niño.jardinInfo.Nombre).AlignCenter();
								table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(niño.personaInfo.Nombre).AlignCenter();
							}
						});
					});

					page.Footer()
						.AlignRight()
						.Text(txt =>
						{
							txt.Span("Página ").FontSize(10);
							txt.CurrentPageNumber().FontSize(10);
							txt.Span(" de ").FontSize(10);
							txt.TotalPages().FontSize(10);
						});
				});
			});

			return report;
		}

	}
}
