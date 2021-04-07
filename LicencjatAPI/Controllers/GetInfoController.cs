using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LicencjatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GetInfoController : ControllerBase
    {
        // GET: api/<GetInfoController>
        [HttpPost("getPostList")]
        public IActionResult Post()
        {
            MySqlConnection conn = new MySqlConnection(Constants.connectionString);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM posts", conn);
            MySqlDataReader reader;
            conn.Open();
            reader = cmd.ExecuteReader();
            reader.Read();


            return Ok();
        }

        // GET api/<GetInfoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GetInfoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GetInfoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GetInfoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
