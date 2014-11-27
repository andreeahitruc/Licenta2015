using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlickrNet;
using System.Xml;
using System.Threading.Tasks;
using System.IO;

namespace Friends_Service
{
    //public class CreateLinkFriends
    //{
    //    flickr_tablesEntities db = new flickr_tablesEntities();
    //    public static void getList(Flickr f,  list_of_friends, string userId)
    //    {

    //        List<string> tags = new List<string>();
    //        Dictionary<string, List<string>> tagsList = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>();

    //        foreach (var user in list_of_friends)
    //        {
    //           // FlickrNet.PhotoCollection photoCollection = f.PeopleGetPublicPhotos(user.UserId);
    //            tags = f.TagsGetListUser(user.UserId).GroupBy(x => x.TagName).OrderByDescending(g => g.Count()).Take(5).Select(gg => gg.Key).ToList();
    //            tagsList.Add(user.UserId, tags);
    //        }
    //        //createXml(userId, tagsList);
    //        createHtml(userId, tagsList);
    //    }
    //    private static void createHtml(string userId, Dictionary<string, List<string>> tags)
    //    {
    //        string line = "<html><body><div id=" + '"' + "friends" + '"' + ">"; int contor = 1;
    //        using (FileStream fs = new FileStream(@"C:\xampp\htdocs\Licenta\siteJS\files\friends.html", FileMode.Create))
    //        {
    //            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
    //            {
    //                w.WriteLine(line);
    //                foreach (var item in tags)
    //                {
    //                    foreach (var tag in item.Value)
    //                    {
    //                        line = "<span class=" + '"' + "friend" + contor + " " + tag + '"' + "></span>" + item.Key + "<br/>";
    //                        w.WriteLine(line);
    //                    }
    //                    contor++;
    //                }
    //                w.WriteLine("</div></body></html>");
    //            }
    //        } 
    //    }

    //    private static void createXml(string userId, Dictionary<string, List<string>> tags)
    //    {
    //        XmlDocument doc = new XmlDocument();
    //        doc.LoadXml("<friends><userId>" + userId + "</userId></friends>");

    //        foreach (var item in tags)
    //        {
    //            XmlElement friendElement = doc.CreateElement("friend");
    //            friendElement.SetAttribute("Id", item.Key);
    //            foreach (var tag in item.Value)
    //            {
    //                XmlElement friendTag = doc.CreateElement("tag");
    //                friendTag.InnerText = tag.ToString();
    //                friendElement.AppendChild(friendTag);
    //            }
    //            doc.DocumentElement.AppendChild(friendElement);
    //        }
    //        doc.PreserveWhitespace = true;
    //        XmlWriterSettings settings = new XmlWriterSettings();
    //        settings.Indent = true;
    //        // Save the document to a file and auto-indent the output.
    //        XmlWriter writer = XmlWriter.Create(@"C:\xampp\htdocs\Licenta\siteJS\files\data.xml", settings);
    //        doc.Save(writer);

    //    }

    //}
}
