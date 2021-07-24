//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PioneerStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            this.CategoriesQuantities = new HashSet<CategoriesQuantity>();
            this.Purchases_Bills_Details = new HashSet<Purchases_Bills_Details>();
            this.Sales_Bills_Details = new HashSet<Sales_Bills_Details>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal SillingPrice { get; set; }
        public decimal PurechcastPrice { get; set; }
        public string Code { get; set; }
        public int MainUint { get; set; }
        public int SubUint { get; set; }
        public Nullable<decimal> AddedTax { get; set; }
    
        public virtual Unit Unit { get; set; }
        public virtual Unit Unit1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CategoriesQuantity> CategoriesQuantities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchases_Bills_Details> Purchases_Bills_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sales_Bills_Details> Sales_Bills_Details { get; set; }
    }
}