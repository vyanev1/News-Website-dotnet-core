using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

namespace News_website_DTT.Models
{
    public class Article
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }

        public void Add()
        {
            var article = this;

            string conString = Startup._configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection sqlCon = new MySqlConnection(conString))
            {
                using (MySqlCommand cmd = new MySqlCommand("", sqlCon))
                {
                    sqlCon.Open();
                    cmd.CommandText = $"INSERT INTO Articles (Title, Subtitle, Content, Date) VALUES('{article.Title}','{article.Subtitle}','{article.Content}','{article.Date}')";
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                }
}
        }

        public void Update()
        {
            var article = this;

            string conString = Startup._configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection sqlCon = new MySqlConnection(conString))
            {
                using (MySqlCommand cmd = new MySqlCommand("", sqlCon))
                {
                    sqlCon.Open();
                    cmd.CommandText = $"UPDATE Articles SET Title='{article.Title}',Subtitle='{article.Subtitle}',Content='{article.Content}',Date='{article.Date}' WHERE ID={article.ID}";
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                }
            }
        }

        public void Delete()
        {
            var article = this;

            string conString = Startup._configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection sqlCon = new MySqlConnection(conString))
            {
                using (MySqlCommand cmd = new MySqlCommand("", sqlCon))
                {
                    sqlCon.Open();
                    cmd.CommandText = $"DELETE FROM Articles WHERE ID={article.ID}";
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}