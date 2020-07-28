using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace News_website_DTT.Models
{
    public class ArticleList
    {
        public List<Article> List = new List<Article>();

        public ArticleList()
        {
            string conString = Startup._configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection sqlCon = new MySqlConnection(conString))
            {
                using (MySqlCommand cmd = new MySqlCommand("",sqlCon))
                {
                    sqlCon.Open();
                    cmd.CommandText = "SELECT * FROM Articles";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var article = new Article() { ID = Convert.ToInt32(reader["ID"]), Title = Convert.ToString(reader["Title"]), Subtitle = Convert.ToString(reader["Subtitle"]), Content = Convert.ToString(reader["Content"]), Date = Convert.ToString(reader["Date"]) };
                            List.Add(article);
                        }
                        sqlCon.Close();
                    }
                }
            }
        }

    }
}