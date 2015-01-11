using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_F.Models.Friends
{
    public class CommentsLinking
    {
        public string PhotoId { get; set; }
        public Dictionary<string, string> friends { get; set; } 
    }
}