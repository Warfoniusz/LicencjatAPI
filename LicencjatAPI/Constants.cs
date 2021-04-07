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

        public static string InsertNewUserQuery = $"insert into users (username, password_salt, password, email, created_at) values (@username, @password_salt, @password, @email, @timestamp)";

        #endregion

        #region MYSQL CHECK IF EMAIL ALREADY REGISTERED

        public static string checkIfAlreadyRegisteredQuery = $"select * from users WHERE email LIKE @email";

        #endregion

        #region MYSQL CHECK LOGIN CRDENTIALS

        public static string checkLoginCredentials = $"select * from users WHERE email = @email AND password = @password";

        #endregion

        #region MYSQL ADD NEW POST TO DB

        public static string insertNewPostQuery = $"insert into posts (ownerId, ownerName, postName, postDescription, postCity) values (@ownerId, @ownerName, @postName, @postDescription, @postCity)";
        #endregion
    }
}
