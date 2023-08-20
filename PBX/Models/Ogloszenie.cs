namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    public partial class Ogloszenie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ogloszenie()
        {
            this.Chat = new HashSet<Chat>();
            this.Aplikacja = new HashSet<Aplikacja>();
            this.Ulubiona = new HashSet<Ulubiona>();
            this.Zgloszenie = new HashSet<Zgloszenie>();
            this.Koszyk = new HashSet<Koszyk>();
            this.Zamowienie = new HashSet<Zamowienie>();
        }

        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Wystawione przez")]
        public int wystawil_id { get; set; }
        [DisplayName("Kategoria")]
        public int kategoria_id { get; set; }
        [DisplayName("Nazwa")]
        public string nazwa { get; set; }
        [DisplayName("Opis")]
        public string opis { get; set; }
        [DisplayName("Aktywne")]
        public bool aktywne { get; set; }
        [DisplayName("Dodano")]
        public System.DateTime dodano { get; set; }
        [DisplayName("Typ")]
        public string typ { get; set; }
        [DisplayName("Cena")]
        public double cena { get; set; }
        [DisplayName("Negocjacja")]
        public bool negocjacja { get; set; }
        [DisplayName("Poka¿ numer telefonu")]
        public bool pokaz_tel { get; set; }
        [DisplayName("Poka¿ adres email")]
        public bool pokaz_email { get; set; }
        [DisplayName("Zdjêcie")]
        public byte[] zdjecie { get; set; }
        [DisplayName("Lokalizacja")]
        public string lokalizacja { get; set; }
        [DisplayName("Auto-przed³u¿anie")]
        public bool auto_przedluzanie { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chat> Chat { get; set; }
        public virtual Kategoria Kategoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Aplikacja> Aplikacja { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ulubiona> Ulubiona { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zgloszenie> Zgloszenie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Koszyk> Koszyk { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zamowienie> Zamowienie { get; set; }
    }
}
