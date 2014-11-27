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

namespace Back_F.ControllersApi
{
    public class FriendsController : ApiController
    {

        flickr_tablesEntities db = new flickr_tablesEntities();
        [GET("api/GetFriends/{userId}")]
        public FlickrNet.ContactCollection GetFriends(string userId)
        {
            linkfriend ob = null;
            try
            {
                try
                {
                    ob = (from t in db.linkfriends
                                  where t.IdUser == userId
                                  select t).FirstOrDefault();
                }
                catch (Exception) { ob = null; }
                if (ob == null)
                {
                    if (Models.User.FlickerObj.UserFl.ContainsKey(userId))
                    {
                        Flickr f = Models.User.FlickerObj.UserFl[userId];
                        FlickrNet.ContactCollection friends = f.ContactsGetPublicList(userId);
                        string realName = "";string description = "";
                        foreach (var item in friends)
                        {
                            var person = f.PeopleGetInfo(item.UserId);
                            if (person.RealName != null)
                                realName = person.RealName;
                            else
                                realName = "";
                            if(person.Description.Length > 50)
                                description = Models.StringTool.Truncate(person.Description,50)+"...";
                            else
                                description = person.Description;

                            item.PathAlias = item.BuddyIconUrl;
                            friend fr = new friend
                            {

                                FullName = realName,
                                IdFriend = item.UserId,
                                UserName = item.UserName,
                                PhotoUrl = item.BuddyIconUrl,
                                Description = description
                            };
                            linkfriend link = new linkfriend
                            {
                                IdUser = userId,
                                IdFriend = item.UserId
                            };
                            try
                            {
                                db.friends.Add(fr);
                                db.linkfriends.Add(link);
                            }
                            catch (Exception)
                            { 
                            
                            }
                        }
                        db.SaveChanges();
                        Models.ListOfFriends.coll = friends;
                        return friends;
                    }
                    else
                    {
                        ContactCollection coll = new ContactCollection();
                        Contact con = new Contact();
                        var friends = db.linkfriends.Where(idx => idx.IdUser == userId).ToList();
                        foreach (var item in friends)
                        {
                            var people = db.friends.Where(idx => idx.IdFriend == item.IdFriend).FirstOrDefault();
                            con.UserId = people.IdFriend;
                            con.RealName = people.FullName;
                            con.UserName = people.UserName;
                            con.PathAlias = people.PhotoUrl;
                            con.IconServer = people.Description;//we use some variable already existent so we not create another model
                            coll.Add(con);
                        }
                        return coll;
                    }
                }
                else
                {
                    ContactCollection coll = new ContactCollection();
                    var friends = db.linkfriends.Where(idx => idx.IdUser == userId).ToList();
                    foreach (var item in friends)
                    {
                        Contact con = new Contact();
                        var people = db.friends.Where(idx => idx.IdFriend == item.IdFriend).FirstOrDefault();
                        con.UserId = people.IdFriend;
                        con.RealName = people.FullName;
                        con.UserName = people.UserName;
                        con.PathAlias = people.PhotoUrl;
                        con.IconServer = people.Description;
                        coll.Add(con);
                    }
                    return coll;
                }
            }
            catch (Exception)
            {
                db.Dispose();
                return null;
            } 
        }
        [HttpGet]
        [GET("api/GetAllFriends/{id}")]
        public FlickrNet.ContactCollection GetAllFriends(string id)
        {
            try {
                var result = (from t1 in db.friends
                             join t2 in db.linkfriends on t1.IdFriend equals t2.IdFriend
                             where t2.IdUser == id
                             select t1).ToList();
                Contact returnVal = null;
                ContactCollection coll = new ContactCollection();
                foreach (var item in result)
                {
                    returnVal = new Contact();
                    returnVal.UserId = item.IdFriend;
                    returnVal.UserName = item.UserName;
                    returnVal.PathAlias = item.PhotoUrl;
                    returnVal.IconFarm = (item.FullName == "")?item.UserName:item.FullName ;
                    coll.Add(returnVal);
                }
                return coll;
            }
            catch(Exception){
                return null;
            }
        }
        [HttpGet]
        [GET("api/CreateList/{userId}")]
        public bool CreateList(string userId)
        {
            try
            {
                List<linkfriend> friends = db.linkfriends.Where(fr => fr.IdUser == userId).ToList();
                var listFriends = (from f1 in db.linkfriends
                                   join f2 in db.friends on f1.IdFriend equals f2.IdFriend
                                   where f1.IdUser == userId
                                   select f2).ToList();
                if (listFriends.FirstOrDefault().Tags == null)// we are entering in this section when the user use for the first time the application
                {
                    Flickr f = Models.FlickrManager.GetInstance();
                    List<string> tags = new List<string>();
                    Dictionary<string, List<string>> tagsList = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>();

                    foreach (var user in listFriends)
                    {
                        tags = f.TagsGetListUser(user.IdFriend).GroupBy(x => x.TagName).OrderByDescending(g => g.Count()).Take(10).Select(gg => gg.Key).ToList();
                        tagsList.Add(user.IdFriend, tags);
                        foreach(string tafg in tags)
                        {
                            friendtag ftag = new friendtag()
                            {
                                FriendId = user.IdFriend,
                                Tag = tafg,
                            };
                            db.friendtags.Add(ftag);
                        }
                        user.Tags = 1;
                    }
                    db.SaveChanges();

                    try {
                        foreach (var item in tagsList)//select list of tags foreach firends
                        {
                            foreach (var tagItem in item.Value)//selcet all tags from the list for iterated friend
                            {
                                var result = (from wordP in db.words
                                             where wordP.Word1 == tagItem
                                             select wordP).ToList();//check if for the iterated friend we have some tags like in ourdatabase
                                {
                                    foreach (var itemRes in result)//check the above result...if Yes then we add the friend to the specific category of tags
                                    {
                                        linkfriendcategory catLink = new linkfriendcategory()
                                        {
                                            IdCategory = itemRes.CategoryId,
                                            IdFriend = item.Key,
                                            Tag = tagItem
                                        };
                                        db.linkfriendcategories.Add(catLink);
                                    }
                                }
                                                
                            }
                        }
                        db.SaveChanges();
                    }
                    catch (Exception) {
                    }
                }

              //  Models.CreateGraph.getList(Models.FlickrManager.instance, friends, userId);
                return true;
            }
            catch (Exception) {
                db.Dispose();
                return false;
            }
            
        } 

      
    }

   
}
