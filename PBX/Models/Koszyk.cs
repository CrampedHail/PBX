namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public partial class Koszyk
    {
        [DisplayName("Og�oszenie")]
        public int ogloszenie_id { get; set; }
        [DisplayName("U�ytkownik")]
        public int uzytkownik_id { get; set; }
        [DisplayName("ID")]
        public int id { get; set; }
    
        public virtual Ogloszenie Ogloszenie { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; }
    }
}