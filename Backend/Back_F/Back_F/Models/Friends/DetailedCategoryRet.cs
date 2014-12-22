using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlickrNet;

namespace Back_F.Models.Friends
{
    public class DetailedCategoryRet
    {
        public string friendName { get; set; }
        public string friendPhoto { get; set; }
        public List<string> tagsName { get; set; }
        public List<string[]> photos { get; set; }
    }
}