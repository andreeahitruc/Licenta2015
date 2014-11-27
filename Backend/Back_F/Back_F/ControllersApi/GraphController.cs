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
    public class GraphController : ApiController
    {
        flickr_tablesEntities db = new flickr_tablesEntities();
        [GET("api/DrawLinks/{userId}")]
        public Dictionary<string, Models.Friends.Linking> GetFriends(string userId)
        {
            var friendsCat = (from f1 in db.linkfriends 
                          join f2 in db.friends on f1.IdFriend equals f2.IdFriend
                          join f3 in db.linkfriendcategories on f2.IdFriend equals f3.IdFriend
                          where f1.IdUser == userId
                          select f3).ToList();//retreive all the friends for the specific user and their categories+tag
            Dictionary<string, Models.Friends.Linking> dict = new Dictionary<string, Models.Friends.Linking>();
            foreach (var item in friendsCat)
            {
                //categoryName+categoryId+listOfFriends-tags
                int catId = item.IdCategory;
                if (!dict.ContainsKey(catId.ToString()))
                {
                    var resultLink = friendsCat.Where(idC => idC.IdCategory == catId).ToList();
                    Models.Friends.Linking obLink = new Models.Friends.Linking();
                    obLink.CategoryId = catId;
                    obLink.CategoryName = db.categories.Where(idc => idc.Id == catId).FirstOrDefault().Category1;
                    obLink.friends = new Dictionary<string, string>();
                    foreach (var itemL in resultLink)
                    {
                        if (obLink.friends.ContainsKey(itemL.IdFriend))
                        {
                            obLink.friends[itemL.IdFriend] = obLink.friends[itemL.IdFriend] +','+itemL.Tag;
                        }else
                        obLink.friends.Add(itemL.IdFriend, itemL.Tag);
                    }
                    dict.Add(catId.ToString(), obLink);
                }
            }
            return dict;

        }
        [GET("api/GetUserLinks/{friendId}/{userId}")]
        public Models.Friends.DetailedLink GetFriendsLinktags(string friendId,string userId)
        {
           var friendDetails = (from f1 in db.linkfriendcategories
                                join f2 in db.linkfriends on f1.IdFriend equals f2.IdFriend
                                 where f2.IdUser == userId && f1.IdFriend == friendId 
                                select f1).ToList();
            List<string> listCat = new List<string>();
            List<string> listTags = new List<string>();
            List<string> listFriends = new List<string>();
            List<string> listFriendsNames = new List<string>();
            foreach (var item in friendDetails) {
                var catName = db.categories.Where(idx => idx.Id == item.IdCategory).FirstOrDefault().Category1;
                listCat.Add(catName);
                listTags.Add(item.Tag);
                
                var links = (from f1 in db.linkfriendcategories
                             join f2 in db.linkfriends on f1.IdFriend equals f2.IdFriend
                             where f2.IdUser == userId && f1.IdCategory == item.IdCategory
                             select f1).ToList();
                foreach (var itemL in links)
                {
                    listFriends.Add(itemL.IdFriend);
                }
            }
            Models.Friends.DetailedLink listRet = new Models.Friends.DetailedLink();
            listRet.categories = listCat.Distinct().ToList();
            listRet.tags = listTags.Distinct().ToList();
            listFriends = listFriends.Distinct().ToList();
            foreach (string item in listFriends)
            {
                if (item != friendId)
                {
                    string namefriend = db.friends.Where(idx => idx.IdFriend == item).FirstOrDefault().FullName;
                    string name = (namefriend == "") ? db.friends.Where(idx => idx.IdFriend == item).FirstOrDefault().UserName : namefriend;
                    listFriendsNames.Add(name);
                }
                    
            }
            listRet.friends = listFriendsNames.Distinct().ToList();
            return listRet;

           //Dictionary<string, List<Models.Friends.DetailedLink>> dic = new Dictionary<string, List<Models.Friends.DetailedLink>>();
           //foreach(var item in friendDetails)
           //{
           //    var links = (from f1 in db.linkfriendcategories
           //                where f1.IdCategory == item.IdCategory
           //                select f1).ToList();
           //    int idCat = links.FirstOrDefault().IdCategory;
           //    List<Models.Friends.DetailedLink> list = new List<Models.Friends.DetailedLink>();
           //    var catName = db.categories.Where(idx => idx.Id == idCat).FirstOrDefault().Category1;
           //    if (!dic.ContainsKey(catName))
           //    {
           //        foreach (var itemL in links)
           //        {
           //            string namefriend = db.friends.Where(idx => idx.IdFriend == itemL.IdFriend).FirstOrDefault().FullName;
           //            var obj = new Models.Friends.DetailedLink();
           //            obj.nameFriend = (namefriend == "")?itemL.IdFriend:namefriend;
           //            obj.nameTag = itemL.Tag;
           //            list.Add(obj);
           //        }
           //        dic.Add(catName, list);//we add in dictionary <nameOfTheCategory,listOfFriendsAssociated>
           //    }
           //}
          //  return null;
        }

    }
   
}
