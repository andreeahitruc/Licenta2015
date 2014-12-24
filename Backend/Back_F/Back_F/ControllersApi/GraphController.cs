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

        [GET("api/DrawLinksForUser/{userId}")]
        public Dictionary<string, Models.Friends.Linking> GetLinksUser(string userId)
        {
            try{

                var catUser = db.linkusercategories.Where(x => x.UserId == userId).ToList().GroupBy(p=>p.Category);
                Dictionary<string, Models.Friends.Linking> dictUser = new Dictionary<string, Models.Friends.Linking>();
                foreach (var item in catUser)
                {
                    var itemx = (from f1 in db.linkfriendcategories
                                join f2 in db.linkfriends on f1.IdFriend equals f2.IdFriend
                                where f2.IdUser == userId && f1.IdCategory == item.Key
                                select f1).ToList();

                    int catId = item.Key;
                    string cat = item.Key.ToString();
                    string nameCat = db.categories.Where(id => id.Id == catId).First().Category1;
                    Models.Friends.Linking obLink = new Models.Friends.Linking();
                    obLink.CategoryId = catId;
                    obLink.CategoryName = nameCat;
                    obLink.friends = new Dictionary<string, string>();
                    dictUser.Add(cat, obLink);
                    foreach (var itemNew in itemx)
                    {
                        if (dictUser[cat].friends.ContainsKey(itemNew.IdFriend))
                        {
                            dictUser[cat].friends[itemNew.IdFriend] = dictUser[cat].friends[itemNew.IdFriend] + ',' + itemNew.Tag;
                        }
                        else
                        {
                            dictUser[cat].friends.Add(itemNew.IdFriend, itemNew.Tag);
                        }
                    }
                }
            return dictUser;
            }
            catch(Exception){return null;}
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
        }

        [POST("api/GetCategoryDetails")]
        public List<Models.Friends.DetailedCategoryRet> categoryDetails(Models.Friends.DetailedCategory ob)
        {
            string idFriend0 = ob.friends.ToString().Split(',')[0];
            string idFriend1 = ob.friends.ToString().Split(',')[1];
            int idCat = 0;
            string[] arrayCat = ob.catName.Split(',');
            List<Models.Friends.DetailedCategoryRet> obRet = new List<Models.Friends.DetailedCategoryRet>();
            for (int i = 0; i < 2; i++)
            {
                var friend = "";List<string> tags = new List<string>();
                if(i == 0) friend = idFriend0; else friend = idFriend1;
                var itemFriend = db.friends.Where(id => id.IdFriend == friend).First();
                Models.Friends.DetailedCategoryRet obF = new Models.Friends.DetailedCategoryRet();
                obF.friendName = (itemFriend.FullName == "") ? itemFriend.UserName : itemFriend.FullName; obF.friendPhoto = itemFriend.PhotoUrl;
                for (int x = 0; x < arrayCat.Length; x++)
                {
                    string cat = arrayCat[x];
                    idCat = db.categories.Where(c => c.Category1 == cat).First().Id;
                    var tagsList = db.linkfriendcategories.Where(link => link.IdCategory == idCat && link.IdFriend == friend);
                    List<PhotoCollection> photoColl = new List<PhotoCollection>();
                    foreach (var item in tagsList)
                    {
                        tags.Add(item.Tag);
                    }
                }
                obF.tagsName = tags.Distinct().ToList();
                obRet.Add(obF);
           
           }
            return obRet;
        }
        [HttpPost]
        [POST("api/CommonPictures")]
        public List<Models.Friends.DetailedCategoryRet> GetCommonPicture(Models.Friends.DetailedCategory ob)
        {
            Flickr f = Models.FlickrManager.GetInstance();
            //Models.Authentification obpp = new Models.Authentification();
            //string auth = obpp.authMethod();
            //string oauth_token = auth.Split('=')[2].ToString().Split('&')[0];
            //string oauth_token_secret = auth.Split('=')[3].ToString();
            
            //OAuthRequestToken requestToken = Models.Authentication.FullToken as OAuthRequestToken;
            //f.OAuthAccessToken = requestToken.Token;
            //f.OAuthAccessTokenSecret = requestToken.TokenSecret;
            //OAuthAccessToken accessToken = f.OAuthGetAccessToken(requestToken, Models.User.GetUserDetails.auth_verifier);
          //  f.OAuthAccessToken = requestToken;
            
            string idFriend0 = ob.friends.ToString().Split(',')[0];
            string idFriend1 = ob.friends.ToString().Split(',')[1];
            int idCat = db.categories.Where(c => c.Category1 == ob.catName).First().Id;
            List<Models.Friends.DetailedCategoryRet> obRet = new List<Models.Friends.DetailedCategoryRet>();
            for (int i = 0; i < 2; i++)
            {
                var friend = "";
                List<string> tags = new List<string>();
                if (i == 0) friend = idFriend0; else friend = idFriend1;
                var tagsList = db.linkfriendcategories.Where(link => link.IdCategory == idCat && link.IdFriend == friend);
                List<PhotoCollection> photoColl = new List<PhotoCollection>();
                Models.Friends.DetailedCategoryRet obF = new Models.Friends.DetailedCategoryRet();
                List<string[]> urls = new List<string[]>();
                string[] urlArray;
                foreach (var item in tagsList)
                {
                    tags.Add(item.Tag);//Auth auth = FlickrObj.AuthGetToken(frob);
                    //FlickrObj.AuthToken = auth.Token;
                    var options = new PhotoSearchOptions();
                    options.TagMode = TagMode.AnyTag;
                    options.Tags = item.Tag.ToString();
                    options.UserId = friend;
                    options.Extras |= PhotoSearchExtras.DateTaken | PhotoSearchExtras.MediumUrl | PhotoSearchExtras.Tags;
                    options.PrivacyFilter = PrivacyFilter.PublicPhotos;
                    PhotoCollection photos = f.PhotosSearch(options);
                    
                    foreach(var itemA in photos)//extrac the needed url's
                    {
                        urlArray = new string[2];
                        urlArray[0] = itemA.Medium640Url;
                        urlArray[1] = itemA.WebUrl;
                        urls.Add(urlArray);
                    }
                    obF.photos = urls;
                }
                obF.tagsName = tags;
                obRet.Add(obF);
            }
            return obRet;
        }
        [HttpGet]
        [POST("api/GetBestBundles/{id,id1}")]
        public List<Models.Friends.DetailedCategoryRet> BestBundles(string id,string id1)
        {
            var friends = db.friends.ToList();
            List<string[]> link = new List<string[]>();
            var comments1 = (from e1 in db.commentsusers where
                           e1.UserId == id
                                select e1).ToList();
            var comments2 = (from e1 in db.commentsusers
                            where
                                e1.UserId == id1
                            select e1).ToList();
            foreach(var item in comments1){
                   

            }

            return null;
        }
    }
   
}
