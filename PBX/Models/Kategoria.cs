 //------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Kategoria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategoria()
        {
            this.Kategoria1 = new HashSet<Kategoria>();
            this.Ogloszenie = new HashSet<Ogloszenie>();
        }


        [Required]
        [DisplayName("ID")]
        public int id { get; set; }

        [DisplayName("Nadkategoria ID")]
        public int? nadkategoria_id { get; set; }

        [DisplayName("Nazwa")] 
        public string nazwa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kategoria> Kategoria1 { get; set; }
        public virtual Kategoria Kategoria2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ogloszenie> Ogloszenie { get; set; }
    }
}
