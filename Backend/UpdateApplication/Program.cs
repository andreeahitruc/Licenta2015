﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlickrNet;

namespace UpdateApplication
{
    class Program
    {
       // flickr_tablesEntities db = new flickr_tablesEntities(); 
        public const string ApiKey = "33f3c09ec1f1baa63a45dfa6b72f5043";
        public const string SharedSecret = "049b6cb6525d5d9b";
        public static Flickr instance;
       
        static void Main(string[] args)
        {
            Console.WriteLine("Start looking for users...");
            Program ob = new Program();
            ob.UpdateComments();
            //ob.UpdateFriends();
            //ob.UpdateFriendsTags();
            Console.ReadKey();
        }
        public static Flickr GetInstance()
        {
            if(instance == null)
                instance = new Flickr(ApiKey, SharedSecret);
            return instance;
        }
        public static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }

        public bool DeleteFriends()
        {
            using (flickr_tablesEntities1 db = new flickr_tablesEntities1())
            {
                try
                {
                    var friends = db.friends.ToList();
                    foreach (friend item in friends)
                    {
                        db.friends.Remove(item);
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception) { return false; }
            }
        }
        public bool UpdateFriendsTags()
        {
            using (flickr_tablesEntities1 db = new flickr_tablesEntities1())
            {
                try
                    {
                        List<friend> friends = db.friends.ToList();
                        Flickr f = GetInstance();
                        List<string> tags = new List<string>();
                        var users = db.users.ToList();
                        foreach (var user in users)
                        {
                            Console.WriteLine("Looking for tags for users");
                            var tagsUser = f.TagsGetListUser(user.UserId).GroupBy(x => x.TagName).OrderByDescending(g => g.Count()).Take(10).Select(gg => gg.Key).ToList();
                            foreach (string tagU in tagsUser)
                            {
                                usertag utag = new usertag()
                                {
                                    UserId = user.UserId,
                                    Tag = tagU
                                };
                                db.usertags.Add(utag);
                            };
                            db.SaveChanges();
                            foreach (var tagItem in tagsUser)//select all tags from the list for iterated friend
                            {
                                var result = (from wordP in db.words
                                              where wordP.Word1 == tagItem
                                              select wordP).ToList();//check if for the iterated friend we have some tags like in ourdatabase
                                foreach (var itemRes in result)//check the above result...if Yes then we add the friend to the specific category of tags
                                {
                                    linkusercategory catLink = new linkusercategory()
                                    {
                                        Category = itemRes.CategoryId,
                                        UserId = user.UserId,
                                        Tag = itemRes.Word1
                                    };
                                    db.linkusercategories.Add(catLink);
                                }
                            }
                            db.SaveChanges(); 
                        }
                        Dictionary<string, List<string>> tagsList = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>();
                        foreach (var user in friends)
                        {
                            Console.WriteLine("Foreach friend it will be retreived all the tags...");
                            tags = f.TagsGetListUser(user.IdFriend).GroupBy(x => x.TagName).OrderByDescending(g => g.Count()).Take(10).Select(gg => gg.Key).ToList();
                            tagsList.Add(user.IdFriend, tags);
                            foreach (string tafg in tags)
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
                        try
                        {
                            foreach (var item in tagsList)//select list of tags foreach firends
                            {
                                foreach (var tagItem in item.Value)//select all tags from the list for iterated friend
                                {
                                    var result = (from wordP in db.words
                                                    where wordP.Word1 == tagItem
                                                    select wordP).ToList();//check if for the iterated friend we have some tags like in ourdatabase
                                    foreach (var itemRes in result)//check the above result...if Yes then we add the friend to the specific category of tags
                                    {
                                        linkfriendcategory catLink = new linkfriendcategory()
                                        {
                                            IdCategory = itemRes.CategoryId,
                                            IdFriend = item.Key,
                                            Tag = itemRes.Word1
                                        };
                                        db.linkfriendcategories.Add(catLink);
                                    }
                                }
                            }
                            db.SaveChanges();
                            Console.WriteLine("Tagging proccess complete...");
                        }
                        catch (Exception)
                        {
                        }
                        return true;
                }
                catch (Exception)
                {
                    db.Dispose();
                    return false;
                }
            }
        }
        public bool UpdateComments()
        {
            flickr_tablesEntities1 dbo = new flickr_tablesEntities1();
            int contor = 0;
            try {
                var friends = dbo.friends.ToList();
                var users = dbo.users.ToList();
                Flickr f = GetInstance();
                var commentsAll = dbo.commentsusers.ToList();
                foreach (var item in commentsAll)
                {
                    dbo.commentsusers.Remove(item);
                }
                dbo.SaveChanges();

                Console.WriteLine("All cleared");
                foreach (var item in friends)
                {
                    Console.WriteLine("Added "+ contor+" friend");
                    FlickrNet.PhotoCollection items = f.PeopleGetPublicPhotos(item.IdFriend);
                    foreach (var itemx in items)
                    {
                        FlickrNet.PhotoCommentCollection comments = f.PhotosCommentsGetList(itemx.PhotoId);
                        foreach (var comm in comments)
                        {
                            
                            commentsuser commu = new commentsuser() { 
                                CommentatorId = comm.AuthorUserId,
                                Comment = comm.AuthorRealName,
                                PhotoId = itemx.PhotoId,
                                UserId = item.IdFriend
                            };
                            dbo.commentsusers.Add(commu);
                        }
                    }
                    contor++;
                }
                foreach (var item in users)
                {
                    Console.WriteLine("Added " + contor + " friend");
                    FlickrNet.PhotoCollection items = f.PeopleGetPublicPhotos(item.UserId);
                    foreach (var itemx in items)
                    {
                        FlickrNet.PhotoCommentCollection comments = f.PhotosCommentsGetList(itemx.PhotoId);
                        foreach (var comm in comments)
                        {

                            commentsuser commu = new commentsuser()
                            {
                                CommentatorId = comm.AuthorUserId,
                                Comment = comm.AuthorRealName,
                                PhotoId = itemx.PhotoId,
                                UserId = item.UserId
                            };
                            dbo.commentsusers.Add(commu);
                            dbo.SaveChanges();
                        }
                    }
                    contor++;
                }
                dbo.SaveChanges();
                Console.WriteLine("Data saved...");
            }
            catch (Exception) { }
            return true;
        }
        public bool UpdateFriends()
        {
            using (flickr_tablesEntities1 db = new flickr_tablesEntities1())
            {
                try
                {
                    List<user> users = db.users.ToList();
                    Console.WriteLine(users.Count + " users in database...");
                    Console.WriteLine("START UPDATE");
                    Console.WriteLine("Deleting all friends...");
                    DeleteFriends();
                    foreach (var item in users)
                    {
                        Flickr f = GetInstance();
                        FlickrNet.ContactCollection friends = f.ContactsGetPublicList(item.UserId);
                        string realName = ""; string description = "";
                        int count = friends.Count;
                        int contor = 0;
                        foreach (var itemFr in friends)
                        {
                            var person = f.PeopleGetInfo(itemFr.UserId);
                            if (person.RealName != null)
                                realName = person.RealName;
                            else
                                realName = "";
                            if (person.Description.Length > 50)
                                description = Truncate(person.Description, 50) + "...";
                            else
                                description = person.Description;

                            itemFr.PathAlias = itemFr.BuddyIconUrl;
                            friend fr = new friend
                            {

                                FullName = realName,
                                IdFriend = itemFr.UserId,
                                UserName = itemFr.UserName,
                                PhotoUrl = itemFr.BuddyIconUrl,
                                Description = description
                            };
                            linkfriend link = new linkfriend
                            {
                                IdUser = item.UserId,
                                IdFriend = itemFr.UserId
                            };
                            try
                            {
                                if (db.friends.Where(a => a.IdFriend == fr.IdFriend).Count() == 0)
                                {
                                    db.friends.Add(fr);
                                    db.SaveChanges();
                                }
                                db.linkfriends.Add(link);
                                db.SaveChanges();
                                Console.WriteLine("Saving friends:");
                                if (contor >= count / 4)
                                    Console.Write("---25%---");
                                if (contor >= count / 3)
                                    Console.Write("---32%---");
                                if (contor >= count / 2)
                                    Console.Write("---50%---");
                                if (contor == count / 1.5)
                                    Console.Write("---80%---");
                                    contor++;
                            }
                            catch (Exception)
                            {

                            }
                        }
                        
                    }
                    Console.WriteLine("Update friends complete...");
                    return true;
                }
                catch (Exception) { return false; }
            }
        }
    }
}
