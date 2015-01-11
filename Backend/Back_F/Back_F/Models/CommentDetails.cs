using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_F.Models
{
    public class CommentDetails
    {
        public string photoUrl {get;set;}
        public string pathUrl { get; set; }
        public List<FriendType> friends { get; set; }
    }
    public class FriendType
    {
        public string name { get; set; }
        public string image { get; set; }
    }
}