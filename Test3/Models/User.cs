//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Societies = new HashSet<Society>();
        }
    
        public int User_ID { get; set; }
        public string User_Name { get; set; }
        public string User_Pass { get; set; }
        public int Type_ID { get; set; }
        public Nullable<int> Society_ID { get; set; }
    
        public virtual User_Type User_Type { get; set; }
        public virtual Society Society { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Society> Societies { get; set; }
    }
}
