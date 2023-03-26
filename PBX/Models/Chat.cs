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
    using System.Web.Mvc;

    public partial class Chat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chat()
        {
            this.Wiadomosci = new HashSet<Wiadomosc>();
        }

        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("ID Ogłoszenia")]
        public int ogloszenie_id { get; set; }
        [DisplayName("ID Zainteresowanego")]
        public int zainteresowany_id { get; set; }
        [DisplayName("ID Ogłaszającego")]
        public int oglaszajacy_id { get; set; }
    
        public virtual Uzytkownik Oglaszajacy { get; set; }
        public virtual Ogloszenie Ogloszenie { get; set; }
        public virtual Uzytkownik Zainteresowany { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wiadomosc> Wiadomosci { get; set; }
    }
}
