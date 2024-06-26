using ICBFApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Fluent;
using System.Data.SqlClient;

namespace ICBFApp.Pages.Jardines
{
    public class IndexModel : PageModel
    {
	

		public List<JardinInfo> listJardin = new List<JardinInfo>();
		public string ErrorMessage { get; set; } = "";

		public void OnGet()
		{
			try
			{
				String connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sqlSelect = "SELECT * FROM Jardin";

					using (SqlCommand command = new SqlCommand(sqlSelect, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							//Validar si hay datos
							if (reader.HasRows)
							{
								while (reader.Read())
								{
									JardinInfo jardinInfo = new JardinInfo();
									jardinInfo.Identificador = reader.GetInt32(0).ToString();
									jardinInfo.Nombre = reader.GetString(1);
									jardinInfo.Direccion = reader.GetString(2);
									jardinInfo.Estado = reader.GetString(3);

									listJardin.Add(jardinInfo);
								}
							}

						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.ToString());
			}

		}
	

		public class JardinInfo
		{
			public string Identificador { get; set; }
			public string Nombre { get; set; }
			public string Direccion { get; set; }
			public string Estado { get; set; }
		}
	}

}
