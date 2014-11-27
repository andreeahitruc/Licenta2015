using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_F.Models.Friends
{
    public class DetailedLink
    {
       public List<string> tags { get; set; }
       public List<string> categories { get; set; }
       public List<string> friends { get; set; }
    }
}