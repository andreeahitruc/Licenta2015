using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_F.Models
{
    public class LinkUserFriendGraph
    {
        public string CategoryName { get; set; }
        public Dictionary<string, string> friends { get; set; } 
    }
}