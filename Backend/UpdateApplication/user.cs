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
    
    public partial class user
    {
        public user()
        {
            this.linkfriends = new HashSet<linkfriend>();
        }
    
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
        public string Full_Name { get; set; }
    
        public virtual ICollection<linkfriend> linkfriends { get; set; }
    }
}