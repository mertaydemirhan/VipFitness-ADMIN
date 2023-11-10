using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VipFitness_ADMIN.Controllers;
using VipFitness_ADMIN.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using NuGet.Common;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace VipFitness_ADMIN.Controllers
{
    public class LoginController : Controller
    {
        string connstring = "";
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View("Login");
        }
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            string hashedInput = HashPassword(inputPassword);
            return string.Equals(hashedInput, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }

        public ActionResult ChekingUser(string username, string password)
        {
            /* bu alt 3 satırda login model içerisine  girişte yazılan usrname pw bilgilerini girip atama yapıyoruz buna göre token vereceğiz.*/
            LoginRequest model = new LoginRequest();
            model.Username = username;
            model.Password = password;

            /*  ALT 3 SATIRDA appsettings.json içerisinde sakladığımız connectionstring bilsini çekiyoruz.  */
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfiguration configuration = builder.Build();
            connstring = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

            string hashedPassword = HashPassword(model.Password);
            /*BU ALANDA  USER KONTROL GERÇEKLEŞECEK*/
            // 0 = Super Admin,  1 = Admin,  2 = User
            SqlConnection sqlConnection = new SqlConnection(connstring);
            sqlConnection.ConnectionString = connstring;
            var query = "SELECT * FROM tblUsers WHERE username = @Username AND password = @Password AND UType IN(0,1)";
            var parameters = new { Username = model.Username, Password = hashedPassword };
            var CheckedUser = sqlConnection.Query<UserModel>(query, parameters);
            bool Credential = true; 


            if (CheckedUser != null && CheckedUser.Count()>0)
            {
                // Kullanıcı doğrulandıysa token oluştur
                // var token = GenerateToken(model.Username);
                /* TOKEN BÖLÜMÜ KAPATILDI. ŞU ANLIK İHTİYAÇ YOK.*/
                Credential = true;
                var response = new
                {
                    mahmut = Credential, // Eğer token varsa
                    Message = "Kullancı Doğrulandı."
                };
                return Json(response);
            }
            else
            {
                Credential = false;
                var response = new
                {
                    mahmut = Credential, // Eğer token varsa
                    Message = "Lütfen Kullanıcı adı veya şifrenizi kontrol ediniz. Eğer bilgiler doğru ise yetkiniz bulunmamaktadır."
                };
                return Json(response);
            }
            
        }

     /*   private string GenerateToken(string username)  // TOKEN BÖLÜMÜ İHTİYAÇ OLMADIĞI İÇİN KAPATILDI.
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "User") // Kullanıcının rolü (örneğin, "User")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Token'ın geçerlilik süresi (örneğin, 30 dakika)
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/




    }

    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

}

