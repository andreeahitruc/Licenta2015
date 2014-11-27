using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlickrNet;

namespace Back_F.Models
{
    public class FlickrManager
    {
        public const string ApiKey = "33f3c09ec1f1baa63a45dfa6b72f5043";
        public const string SharedSecret = "049b6cb6525d5d9b";
        public static Flickr instance;
        public static Flickr GetInstance()
        {
            if(instance == null)
                instance = new Flickr(ApiKey, SharedSecret);
            return instance;
        }
    }
    
}