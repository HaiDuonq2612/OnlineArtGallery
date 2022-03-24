//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Online_Art_Gallery.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Artwork
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Artwork()
        {
            this.Carts = new HashSet<Cart>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Ratings = new HashSet<Rating>();
            this.Wishlists = new HashSet<Wishlist>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Sale_Price { get; set; }
        public Nullable<System.DateTime> Year { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Technique { get; set; }
        public Nullable<int> Id_Artist { get; set; }
        public Nullable<int> Id_Category { get; set; }
        public Nullable<int> Owner { get; set; }
        public string Descreption { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> Total_Rating { get; set; }
        public Nullable<int> Total_Rating_Points { get; set; }
    
        public virtual Artist Artist { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rating> Ratings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}