namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public partial class Uzytkownik
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Uzytkownik()
        {
            this.Chat = new HashSet<Chat>();
            this.Chat1 = new HashSet<Chat>();
            this.OtrzymaneOceny = new HashSet<Ocena>();
            this.WystawioneOceny = new HashSet<Ocena>();
            this.Ogloszenia = new HashSet<Ogloszenie>();
            this.Ulubione = new HashSet<Ulubiona>();
            this.Aplikacje = new HashSet<Aplikacja>();
            this.Wiadomosci = new HashSet<Wiadomosc>();
            this.ZgloszeniaPrzez = new HashSet<Zgloszenie>();
            this.ZgloszeniaNa = new HashSet<Zgloszenie>();
            this.Koszyk = new HashSet<Koszyk>();
            this.Zamowienie = new HashSet<Zamowienie>();
        }

        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Imi�")]
        public string imie { get; set; }
        [DisplayName("Numer telefonu")]
        public string nr_tel { get; set; }
        [DisplayName("Adres email")]
        public string email { get; set; }
        [DisplayName("Has�o")]
        public string haslo { get; set; }
        [DisplayName("Do��czono")]
        public System.DateTime dolaczono { get; set; }
        [DisplayName("Zdj�cie")]
        public byte[] zdjecie { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chat> Chat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chat> Chat1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ocena> OtrzymaneOceny { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ocena> WystawioneOceny { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ogloszenie> Ogloszenia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ulubiona> Ulubione { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Aplikacja> Aplikacje { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wiadomosc> Wiadomosci { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zgloszenie> ZgloszeniaPrzez { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zgloszenie> ZgloszeniaNa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Koszyk> Koszyk { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zamowienie> Zamowienie { get; set; }
    }
}