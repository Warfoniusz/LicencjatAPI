using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicencjatAPI.Models
{
    public class ChangeFollowedStatusModel
    {
        public int userId { get; set; }
        public int postId { get; set; }
        public string acceptanceStatus { get; set; }
    }
}
