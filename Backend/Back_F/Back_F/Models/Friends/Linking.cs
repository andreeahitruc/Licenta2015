using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_F.Models.Friends
{
    public class Linking
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public Dictionary<string, string> friends { get; set; } 
    }
}