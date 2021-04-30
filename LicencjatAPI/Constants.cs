using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicencjatAPI
{
    public class Constants
    {
        public static string connectionString = "server=185.243.53.232;port=3306;uid=wurf;pwd=GoxFxCRul8N0p2dl;database=wurf";

        #region MYSQL INSERT NEW USER QUERY

        public static string InsertNewUserQuery = $"insert into users (username, password_salt, password, email, profile_photo_base64, description, sex, city) values (@username, @password_salt, @password, @email, @profile_photo_base64, @description, @sex, @city)";

        #endregion

        #region MYSQL CHECK IF EMAIL ALREADY REGISTERED

        public static string checkIfAlreadyRegisteredQuery = $"select * from users WHERE email LIKE @email";

        #endregion

        #region MYSQL CHECK LOGIN CRDENTIALS

        public static string checkLoginCredentials = $"select * from users WHERE email = @email";

        #endregion

        #region MYSQL ADD NEW POST TO DB

        public static string insertNewPostQuery = $"insert into posts (ownerId, ownerName, postName, postDescription, postCity) values (@ownerId, @ownerName, @postName, @postDescription, @postCity)";
        #endregion

        #region MYSQL ADD POST TO FOLLOWED BY USER

        public static string insertNewFollowedPost = $"INSERT into posts_to_user (userId, postId, acceptanceStatus) values (@userId, @postId, @acceptanceStatus)";
        #endregion

        #region MYSQL SELECT POSTS WHICH LOGGED USER FOLLOWS
        public static string selectFollowedPosts = $"SELECT * FROM posts JOIN posts_to_user ON posts.postId = posts_to_user.postId WHERE posts_to_user.userId = @userid";
        #endregion

        #region MYSQL SELECT LOGGED USER POSTS
        public static string select_user_posts = $"SELECT * FROM posts WHERE ownerId = @ownerId";
        #endregion

        #region MYSQL SELECT INTERESTED USERS
        public static string select_interested_users = $"SELECT * FROM users JOIN posts_to_user ON users.id = posts_to_user.userId WHERE posts_to_user.postId = @postId";
        #endregion

        #region MYSQL CHANGE FOLLOWED ACCEPTANCESTATUS
        public static string change_followed_status = $"UPDATE posts_to_user SET acceptanceStatus = @acceptanceStatus WHERE postId = @postId AND userId = @userId";
        #endregion
    }
}
