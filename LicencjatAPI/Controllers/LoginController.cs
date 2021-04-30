using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using MySqlConnector;
using LicencjatAPI.Models;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LicencjatAPI.Controllers
{
    public class User
    {
        public string fullName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int userId { get; set; }
        public string profile_photo_base64 { get; set; }
        public string description { get; set; }
        public string sex { get; set; }
        public string city { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        DateTime dtNow = DateTime.Now;

        // GET: api/<LoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // POST api/login/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            MySqlConnection conn = new MySqlConnection(Constants.connectionString);
            MySqlDataReader reader;
            MySqlCommand checkCredentials = new MySqlCommand(Constants.checkLoginCredentials, conn);
            checkCredentials.Parameters.Add("@email", MySqlDbType.VarChar).Value = user.email;
            conn.Open();
            reader = checkCredentials.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                string password = (string) reader.GetValue(3);
                string salt_string = (string) reader.GetValue(2);
                byte[] salt = Convert.FromBase64String(salt_string);
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: user.password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                if (password == hashed)
                {
                    reader.Close();
                    MySqlCommand selectUserId = new MySqlCommand("SELECT id FROM users WHERE email = @email", conn);
                    selectUserId.Parameters.Add("@email", MySqlDbType.VarChar).Value = user.email;
                    MySqlCommand selectUsername =
                        new MySqlCommand("SELECT username FROM users WHERE email = @email", conn);
                    selectUsername.Parameters.Add("@email", MySqlDbType.VarChar).Value = user.email;
                    MySqlCommand selectUserEmail =
                        new MySqlCommand("SELECT email FROM users WHERE email = @email", conn);
                    selectUserEmail.Parameters.Add("@email", MySqlDbType.VarChar).Value = user.email;
                    user.userId = (int) selectUserId.ExecuteScalar();
                    user.fullName = (string) selectUsername.ExecuteScalar();
                    user.email = (string) selectUserEmail.ExecuteScalar();
                    return Ok(user);
                }
                else
                {
                    reader.Close();
                    return Unauthorized();
                }
            }
            else
            {
                reader.Close();
                return Unauthorized();
            }
        }

        // POST api/login/test
        [HttpPost("test")]
        public IActionResult Register([FromBody] User user)
        {
            MySqlConnection conn = new MySqlConnection(Constants.connectionString);
            MySqlDataReader reader;
            MySqlCommand checkEmailCommand = new MySqlCommand(Constants.checkIfAlreadyRegisteredQuery, conn);
            checkEmailCommand.Parameters.Add("@email", MySqlDbType.VarChar).Value = user.email;
            conn.Open();
            reader = checkEmailCommand.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                return Unauthorized();
            }
            else
            {
                reader.Close();
            }

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            string salt_string = Convert.ToBase64String(salt);

            cmd.CommandText = Constants.InsertNewUserQuery;
            cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = user.fullName;
            cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = hashed;
            cmd.Parameters.Add("@password_salt", MySqlDbType.VarChar).Value = salt_string;
            cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = user.email;
            cmd.Parameters.Add("@profile_photo_base64", MySqlDbType.VarChar).Value = user.profile_photo_base64;
            cmd.Parameters.Add("@sex", MySqlDbType.VarChar).Value = user.sex;
            cmd.Parameters.Add("@description", MySqlDbType.VarChar).Value = user.description;
            cmd.Parameters.Add("@city", MySqlDbType.VarChar).Value = user.city;
            cmd.ExecuteNonQueryAsync();

            conn.Close();
            return Created(string.Empty, null);
        }

        //POST api/login/addPost
        [HttpPost("addPost")]
        public IActionResult addPost([FromBody] PostModel post)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(Constants.connectionString);
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                //$"insert into posts (ownerId, ownerName, postName, postDescription, postCity, created_at) values (@ownerId, @ownerName, @postName, @postDescription, @postCity)";
                cmd.CommandText = Constants.insertNewPostQuery;
                cmd.Parameters.Add("@ownerId", MySqlDbType.Int32).Value = post.userId;
                cmd.Parameters.Add("@ownerName", MySqlDbType.String).Value = post.fullName;
                cmd.Parameters.Add("@postName", MySqlDbType.String).Value = post.postTitle;
                cmd.Parameters.Add("@postDescription", MySqlDbType.String).Value = post.postDescription;
                cmd.Parameters.Add("@postCity", MySqlDbType.String).Value = post.postCity;
                conn.Open();
                cmd.ExecuteNonQueryAsync();
                conn.Close();
            }
            catch (Exception ex)
            {
            }

            return Created(string.Empty, null);
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}