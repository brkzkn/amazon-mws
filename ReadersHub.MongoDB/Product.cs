//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReadersHub.MongoDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Product_Price = new HashSet<Product_Price>();
        }
    
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string ISBN_Name { get; set; }
        public string ASIN { get; set; }
        public string ASIN_Name { get; set; }
        public Nullable<System.DateTime> Price_Update_Time_UK { get; set; }
        public Nullable<System.DateTime> Price_Update_Time_US { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Price> Product_Price { get; set; }
    }
}
