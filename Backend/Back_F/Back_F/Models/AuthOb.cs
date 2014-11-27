using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlickrNet;

namespace Back_F.Models
{
    public class AuthOb
    {
        public string token { get; set; }
        public string secret { get; set; }
        public string url { get; set; }
        public string email { get; set; }
        public static OAuthRequestToken FullToken { get; set; }
        public static string auth_verifier { get; set; }
        public string a_ver { get; set; }
        public string UserId { get; set; }

    }
}