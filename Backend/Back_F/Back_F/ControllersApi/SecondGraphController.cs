using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FlickrNet;
using AttributeRouting.Web.Http;
namespace Back_F.ControllersApi
{
    public class SecondGraphController : ApiController
    {
        flickr_tablesEntities db = new flickr_tablesEntities();
        [GET("api/DrawLinksComments/{userId}")]
        public List<Models.Friends.CommentsLinking> GetFriends(string userId)
        {
            try {

                var items = db.commentsusers.ToList().GroupBy(x => x.PhotoId);
                var ax = (from e1 in db.commentsusers
                         join e2 in db.friends on e1.CommentatorId equals e2.IdFriend
                         select e1).ToList().GroupBy(a => a.PhotoId);
                Dictionary<string, string> fr;
                Models.Friends.CommentsLinking ob;
                List<Models.Friends.CommentsLinking> obRet = new List<Models.Friends.CommentsLinking>();
                foreach (var item in items)
                {
                    var friends = db.commentsusers.Where(a => a.PhotoId == item.Key).ToList();
                    fr = new Dictionary<string, string>();
                    ob = new Models.Friends.CommentsLinking();
                    foreach(var friend in friends)
                    { 
                        if (!fr.ContainsKey(friend.CommentatorId) && db.friends.Where(a=>a.IdFriend == friend.CommentatorId).Count() == 1)
                        {
                            fr.Add(friend.CommentatorId,"");
                        }
                     }
                        ob.PhotoId = item.Key;
                        ob.friends = fr;
                        if (ob.friends.Count > 1)
                        {
                            obRet.Add(ob);
                        }
                }
                return obRet;
            }
            catch (Exception) { return null; }
        }

        [HttpPost]
        [POST("api/GetPageLinks")]
        public List<List<Models.FriendType>>  GetLinks(Models.LinksToShow ob)
        {
            try
            {
                List<List<Models.FriendType>> obRet = new List<List<Models.FriendType>>();
                string[] items = ob.links.Split(',');
                for (int i = 0; i < items.Length; i++)
                {
                    string link = items[i];
                    string[] friends = link.Split('+');
                    List<Models.FriendType> SecLink = new List<Models.FriendType>();
                    for (int j = 0; j < friends.Length; j++)
                    {
                        string id = friends[j];
                        var item = db.friends.Where(idx => idx.IdFriend == id).FirstOrDefault();
                        Models.FriendType obF = new Models.FriendType();
                        obF.image = item.PhotoUrl; obF.name = (item.FullName == "") ? item.UserName : item.FullName;
                        SecLink.Add(obF);
                    }
                    obRet.Add(SecLink);
                }
                return obRet;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [POST("api/GetCommentDetails")]
        public Models.CommentDetails GetCommentDetails(Models.Friends.DetailedCategory ob)
        {
            try
            {
                //ob.catName is the id of the photo, i used same model object so not to create another one :) 
                var count = ob.friends.Split(',').Length;
                Flickr f = Models.FlickrManager.GetInstance();
                var image = f.PhotosGetInfo(ob.catName);
                string Photo = image.LargeSquareUrl;
                string PhotoPath = image.WebUrl;
                List<friend> friends = new List<friend>();
                for (int i = 0; i < count; i++) 
                { 
                    string item = ob.friends.Split(',')[i];
                    friends.Add(db.friends.Where(id => id.IdFriend == item).FirstOrDefault());                    
                }
                Models.CommentDetails obRet = new Models.CommentDetails();
                obRet.photoUrl = Photo;
                List<Models.FriendType> items = new List<Models.FriendType>();
                foreach (var itm in friends)
                {
                    Models.FriendType ff = new Models.FriendType();
                    ff.name = (itm.FullName == "") ? itm.UserName : itm.FullName;
                    ff.image = itm.PhotoUrl;
                    items.Add(ff);
                }
                obRet.friends = items;
                obRet.pathUrl = PhotoPath;
                return obRet;
            }
            catch (Exception) { return null; }
        }

    }
   
}
