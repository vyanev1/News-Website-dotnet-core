using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using News_website_DTT.Models;

namespace News_website_DTT.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {   
            string conString = _configuration.GetConnectionString("UserLoginConnection");

            using (MySqlConnection sqlCon = new MySqlConnection(conString))
            {
                using (MySqlCommand cmd = new MySqlCommand("", sqlCon))
                {
                    sqlCon.Open();
                    cmd.CommandText = $"SELECT * FROM Users WHERE UserName='{GetMd5Hash(user.UserName)}' AND Password='{GetMd5Hash(user.Password)}'";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Admin = Convert.ToInt32(reader["Admin"]);
                            HttpContext.Session.SetString("login", user.UserName);
                            HttpContext.Session.SetString("admin", user.Admin.ToString());

                            if (user.Admin == 1)
                            {
                                return RedirectToAction("AdminHome", "Home");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        sqlCon.Close();
                    }
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("login");
            HttpContext.Session.Remove("admin");
            return RedirectToAction("Index", "Home");
        }
    }
}
