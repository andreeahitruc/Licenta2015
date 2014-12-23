using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlickrNet;
using System.Diagnostics;

namespace Back_F.Models
{

    class UpdateComments
    {
        public static void Update(Flickr f)
        {
            Debug.WriteLine("Start Update the comments...");
            Dictionary<string, string> listusers = new Dictionary<string, string>();
            Back_F.flickr_tablesEntities db = new flickr_tablesEntities();
            var users = db.users.ToList();
            var friends = db.friends.ToList();
            PhotoCollection photos = null;
            foreach (var item in users)
            {
                listusers.Add(item.UserId, "user");
            }
            foreach (var item in friends)
            {
                if (!listusers.ContainsKey(item.IdFriend))
                    listusers.Add(item.IdFriend, "friend");
                Debug.WriteLine("All the users from DB retreived...");
            }

            foreach (var item in listusers)
            {
                photos = f.PeopleGetPhotos(item.Key);
                foreach (var itemP in photos)
                {
                    var comments = f.PhotosCommentsGetList(itemP.PhotoId);
                    foreach (var comment in comments)
                    {
                        commentsuser comm = new commentsuser()
                        {
                            CommentatorId = comment.AuthorUserId.ToString(),
                            UserId = item.Key.ToString(),
                            Comment = "",
                            PhotoId = itemP.PhotoId.ToString()
                        };
                        db.commentsusers.Add(comm);
                        Debug.WriteLine("Comment added in DB...");
                    }
                }
            }
            db.SaveChanges();
            Debug.WriteLine("Comment added in DB...");

        }
    }
    
}