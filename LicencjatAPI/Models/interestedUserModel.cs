using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicencjatAPI.Models
{
    public class interestedUserModel
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string profile_photo_base64 { get; set; }
    }
}
