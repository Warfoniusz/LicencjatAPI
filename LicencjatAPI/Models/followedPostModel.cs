using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicencjatAPI.Models
{
    public class followedPostModel
    {
        public string fullName { get; set; }
        public string postTitle { get; set; }
        public string postDescription { get; set; }
        public string postCity { get; set; }
        public int userId { get; set; }
        public int postId { get; set; }
        public string acceptanceStatus { get; set; }
    }
}
