//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ValpeVerkkokauppa.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Wishlist
    {
        public int WishlistID { get; set; }
        public Nullable<int> UserID { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
