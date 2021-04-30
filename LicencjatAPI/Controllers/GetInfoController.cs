using LicencjatAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LicencjatAPI.Controllers
{
    public class Interested
    {
        public int userId { get; set; }
        public int postId { get; set; }
        public string acceptanceStatus { get; set; }
    }

    public class UserProfile
    {
        public string userName { get; set; }
        public string user_photo_base64 { get; set; }
        public string userCity { get; set; }
        public string userDescription { get; set; }
        public string userSex { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class GetInfoController : ControllerBase
    {
        //POST Return a post list to client
        [HttpPost("getPostList/{id}")]
        public IActionResult Post(int id)
        {
            List<PostModel> posts = new List<PostModel>();
            MySqlConnection conn = new MySqlConnection(Constants.connectionString);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM posts JOIN users ON posts.ownerId = users.id WHERE ownerId != @id", conn);
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
            MySqlDataReader reader;
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                PostModel post = new PostModel();
                post.fullName = (string) reader.GetValue(2);
                post.userId = (int) reader.GetValue(1);
                post.postCity = (string) reader.GetValue(5);
                post.postDescription = (string) reader.GetValue(4);
                post.postTitle = (string) reader.GetValue(3);
                post.postId = (int) reader.GetValue(0);
                post.user_photo_base64 = (string) reader.GetValue(13);
                posts.Add(post);
            }


            return Ok(posts);
        }

        [HttpGet("get_user_profile/{id}")]
        public IActionResult GetUserProfile(int id)
        {
            UserProfile userprofile = new UserProfile();
            MySqlConnection conn = new MySqlConnection((Constants.connectionString));
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE id = @id", conn);
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
            MySqlDataReader reader;
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                userprofile.userName = (string)reader.GetValue(1);
                userprofile.userCity = (string) reader.GetValue(9);
                userprofile.userSex = (string) reader.GetValue(7);
                userprofile.userDescription = (string) reader.GetValue(8);
                userprofile.user_photo_base64 = (string) reader.GetValue(6);
            }

            return Ok(userprofile);
        }

        [HttpGet("get_user_posts/{id}")]
        public IActionResult GetUserPosts(int id)
        {
            List<PostModel> posts = new List<PostModel>();
            MySqlConnection conn = new MySqlConnection(Constants.connectionString);
            MySqlCommand cmd = new MySqlCommand(Constants.select_user_posts, conn);
            cmd.Parameters.Add("@ownerId", MySqlDbType.Int32).Value = id;
            MySqlDataReader reader;
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                PostModel post = new PostModel();
                post.fullName = (string) reader.GetValue(2);
                post.userId = (int) reader.GetValue(1);
                post.postCity = (string) reader.GetValue(5);
                post.postDescription = (string) reader.GetValue(4);
                post.postTitle = (string) reader.GetValue(3);
                post.postId = (int) reader.GetValue(0);
                posts.Add(post);
            }


            return Ok(posts);
        }

        [HttpGet("get_interested_users/{id}")]
        public IActionResult GetInterestedUsers(int id)
        {
            List<interestedUserModel> users = new List<interestedUserModel>();
            MySqlConnection conn = new MySqlConnection(Constants.connectionString);
            MySqlCommand cmd = new MySqlCommand(Constants.select_interested_users, conn);
            cmd.Parameters.AddWithValue("@postId", MySqlDbType.String).Value = id;
            MySqlDataReader reader;
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                interestedUserModel user = new interestedUserModel();
                user.userId = (int) reader.GetValue(0);
                user.username = (string) reader.GetValue(1);
                user.profile_photo_base64 = (string) reader.GetValue(6);
                users.Add(user);
            }

            return Ok(users);
        }

        //POST Return a post list to client
        [HttpGet("getFollowedPosts/{id}")]
        public IActionResult GetFollowedPosts(int id)
        {
            List<followedPostModel> posts = new List<followedPostModel>();
            MySqlConnection conn = new MySqlConnection(Constants.connectionString);
            MySqlCommand cmd = new MySqlCommand(Constants.selectFollowedPosts, conn);
            cmd.Parameters.Add("@userId", MySqlDbType.Int32).Value = id;
            MySqlDataReader reader;
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                followedPostModel post = new followedPostModel();
                post.fullName = (string) reader.GetValue(2);
                post.userId = (int) reader.GetValue(1);
                post.postCity = (string) reader.GetValue(5);
                post.postDescription = (string) reader.GetValue(4);
                post.postTitle = (string) reader.GetValue(3);
                post.postId = (int) reader.GetValue(0);
                post.acceptanceStatus = (string) reader.GetValue(10);

                posts.Add(post);
            }


            return Ok(posts);
        }

        [HttpPost("change_followed_status")]
        public IActionResult ChangeFollowedStatus([FromBody] ChangeFollowedStatusModel changeFollowedStatusModel)
        {
            MySqlConnection conn = new MySqlConnection(Constants.connectionString);
            MySqlDataReader reader;
            MySqlCommand cmd = new MySqlCommand(Constants.change_followed_status, conn);
            cmd.Parameters.AddWithValue("@acceptanceStatus", MySqlDbType.String).Value =
                changeFollowedStatusModel.acceptanceStatus;
            cmd.Parameters.AddWithValue("@userId", MySqlDbType.Int32).Value = changeFollowedStatusModel.userId;
            cmd.Parameters.AddWithValue("@postId", MySqlDbType.Int32).Value = changeFollowedStatusModel.postId;
            conn.Open();
            cmd.ExecuteNonQueryAsync();
            conn.Close();

            return Ok();
        }

        //POST User clicks interested on a post
        [HttpPost("clickInterested")]
        public IActionResult AddInterestRealtion([FromBody] Interested interested)
        {
            MySqlConnection conn = new MySqlConnection(Constants.connectionString);
            MySqlDataReader reader;
            MySqlCommand addNewFollwedPost = new MySqlCommand(Constants.insertNewFollowedPost, conn);
            addNewFollwedPost.Parameters.Add("@acceptanceStatus", MySqlDbType.VarChar).Value =
                interested.acceptanceStatus;
            addNewFollwedPost.Parameters.Add("@userId", MySqlDbType.Int32).Value = interested.userId;
            addNewFollwedPost.Parameters.Add("@postId", MySqlDbType.Int32).Value = interested.postId;
            conn.Open();
            addNewFollwedPost.ExecuteNonQueryAsync();
            conn.Close();

            return Ok();
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