namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public partial class Chat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chat()
        {
            this.Wiadomosci = new HashSet<Wiadomosc>();
        }

        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Og³oszenie")]
        public int ogloszenie_id { get; set; }
        [DisplayName("Zainteresowany")]
        public int zainteresowany_id { get; set; }
        [DisplayName("Og³aszaj¹cy")]
        public int oglaszajacy_id { get; set; }
    
        public virtual Uzytkownik Oglaszajacy { get; set; }
        public virtual Ogloszenie Ogloszenie { get; set; }
        public virtual Uzytkownik Zainteresowany { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wiadomosc> Wiadomosci { get; set; }
    }
}
