using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FlickrNet;
using AttributeRouting.Web.Http;
using System.Xml;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Text;


namespace Back_F.Models
{
    public class SaveImageFromUrl
    {
        //public static void ImageFromUrl(string id)
        //{
        //    string urlToDownload = "https://farm4.staticflickr.com/3701/buddyicons/"+id+".jpg";
        //    // May be you need to add some code to determine the type of the image
        //    string pathToSave = System.Web.Hosting.HostingEnvironment.MapPath("~/") + "/"+id+".jpg";
        //    // Download it
        //    WebClient client = new WebClient();
        //    client.DownloadFile(urlToDownload, pathToSave);
        //}
    }
}