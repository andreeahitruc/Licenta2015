//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UpdateApplication
{
    using System;
    using System.Collections.Generic;
    
    public partial class friend
    {
        public friend()
        {
            this.friendtags = new HashSet<friendtag>();
            this.linkfriendcategories = new HashSet<linkfriendcategory>();
            this.linkfriends = new HashSet<linkfriend>();
        }
    
        public string IdFriend { get; set; }
        public string PhotoUrl { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public Nullable<short> Tags { get; set; }
    
        public virtual ICollection<friendtag> friendtags { get; set; }
        public virtual ICollection<linkfriendcategory> linkfriendcategories { get; set; }
        public virtual ICollection<linkfriend> linkfriends { get; set; }
    }
}
