using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlickrNet;

namespace Back_F.Models
{
    public class Authentication
    {
        public static string token { get; set; }
        public static string secret { get; set; }
        public static string url { get; set; }
        public static OAuthRequestToken FullToken { get; set; }

    }
}