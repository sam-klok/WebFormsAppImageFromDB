using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data;

namespace WebFormsApp
{
    public partial class DisplayImageFromDB : System.Web.UI.Page
    {
        readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ImageToFromDatabase.Properties.Settings.LifelongLearningConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadImageFromDB();
                    return;
            }
        }

        private void LoadImageFromDB()
        {
            using (var connection = new SqlConnection(connectionString))
            using (var sqlCommand = new SqlCommand(
                @"SELECT
	                 [ID]
                    ,[Name]
                    ,[Type]
                    ,[Hash]
                    ,[PicData]
                FROM [dbo].[Picture]
                WHERE ID = @ImageID", connection))
            {
                sqlCommand.Parameters.Add("@ImageID", SqlDbType.Int);
                sqlCommand.Parameters["@ImageID"].Value = 2;

                connection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Debug.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}", reader[0], reader[1], reader[2], reader[3], reader[4]));

                        var blob = new Byte[(reader.GetBytes(4, 0, null, 0, int.MaxValue))];
                        reader.GetBytes(4, 0, blob, 0, blob.Length);
                        string base64String = Convert.ToBase64String(blob, 0, blob.Length);
                        string fileName = reader[1].ToString();

                        Image1.AlternateText = fileName;
                        Image1.Width = 800; // otherwise it will use image width/hight, which is probably too big
                        Image1.Height = 600;
                        Image1.ImageUrl = "data:image/png;base64," + base64String;
                    }
                }

            }
        }
    }
}