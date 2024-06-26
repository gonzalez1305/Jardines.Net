using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ICBFApp.Pages.Roles
{
    public class IndexModel : PageModel
    {
		public List<RolInfo> listRol = new List<RolInfo>();

		public void OnGet()
        {
			try
			{
				String connectionString = "Data Source=WINDOW11-CAROLG\\SQLEXPRESS;Initial Catalog=icbf;Integrated Security=True;";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sqlSelect = "SELECT * FROM Rol";

					using (SqlCommand command = new SqlCommand(sqlSelect, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							//Validar si hay datos
							if (reader.HasRows)
							{
								while (reader.Read())
								{
									RolInfo rolInfo = new RolInfo();
									rolInfo.IdRol = reader.GetInt32(0).ToString();
									rolInfo.Rol = reader.GetString(1);


									listRol.Add(rolInfo);
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
				Console.WriteLine("Exception: " + ex.ToString());
			}
		}

		public class RolInfo
		{
			public string IdRol { get; set; }
			public string Rol { get; set; }
		
		}
	}
}
