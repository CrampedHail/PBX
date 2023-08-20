namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public partial class Kategoria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategoria()
        {
            this.Podkategorie = new HashSet<Kategoria>();
            this.Ogloszenia = new HashSet<Ogloszenie>();
        }

        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Nadkategoria")]
        public Nullable<int> nadkategoria_id { get; set; }
        [DisplayName("Nazwa")]
        public string nazwa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kategoria> Podkategorie { get; set; }
        public virtual Kategoria Nadkategoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ogloszenie> Ogloszenia { get; set; }
    }
}
