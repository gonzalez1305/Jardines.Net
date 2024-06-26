using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ICBFApp.Pages.Jardines
{
	public class CrearJardinModel : PageModel
	{
		public JardinInfo jardinInfo = new JardinInfo();
		public string errorMessage = "";
		public string succesMessage = "";

		public void OnGet()
		{
		}

		public void OnPost()
		{
			jardinInfo.Nombre = Request.Form["Nombre"];
			jardinInfo.Direccion = Request.Form["Direccion"];
			jardinInfo.Estado = Request.Form["Estado"];

			if (jardinInfo.Nombre.Length == 0 || jardinInfo.Direccion.Length == 0 || jardinInfo.Estado.Length == 0)
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
					string sqlInsert = "INSERT INTO Jardin (Nombre, Direccion, Estado) VALUES (@Nombre, @Direccion, @Estado)";

					using (SqlCommand command = new SqlCommand(sqlInsert, connection))
					{
						command.Parameters.AddWithValue("@Nombre", jardinInfo.Nombre);
						command.Parameters.AddWithValue("@Direccion", jardinInfo.Direccion);
						command.Parameters.AddWithValue("@Estado", jardinInfo.Estado);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			jardinInfo.Nombre = ""; jardinInfo.Direccion = ""; jardinInfo.Estado = "";
			succesMessage = "Jardin Agregado Exitosamente!";
			Response.Redirect("/Jardines/Index");
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
