using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using MySqlConnector;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LicencjatAPI.Controllers
{
    public class User
    {
        public string fullName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
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
            return new string[] { "value1", "value2" };
        }

        // POST api/login
        [HttpPost("login")]
        public IActionResult Login([FromBody]User user)
        {
            MySqlConnection conn = new MySqlConnection(Constants.connectionString);
            MySqlDataReader reader;
            MySqlCommand checkCredentials = new MySqlCommand(Constants.checkLoginCredentials, conn);
            checkCredentials.Parameters.Add("@email", MySqlDbType.VarChar).Value = user.email;
            checkCredentials.Parameters.Add("@password", MySqlDbType.VarChar).Value = user.password;
            conn.Open();
            reader = checkCredentials.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                reader.Close();
                return Accepted();
            }
            else
            {
                reader.Close();
                return Unauthorized();
            }
        }

        // POST api/register
        [HttpPost("test")]
        public IActionResult Register([FromBody]User user)
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


                cmd.CommandText = Constants.InsertNewUserQuery;
                cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = user.fullName;
                cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = user.password;
                cmd.Parameters.Add("@password_salt", MySqlDbType.VarChar).Value = user.password;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = user.email;
                cmd.Parameters.Add("@timestamp", MySqlDbType.DateTime).Value = dtNow;
                cmd.ExecuteNonQueryAsync();

                conn.Close();
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
