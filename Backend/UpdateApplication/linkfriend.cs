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
    
    public partial class linkfriend
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string IdFriend { get; set; }
    
        public virtual friend friend { get; set; }
        public virtual user user { get; set; }
    }
}
