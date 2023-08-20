namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public partial class Ocena
    {
        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Ocena od")]
        public int ocena_od_id { get; set; }
        [DisplayName("Ocena dla")]
        public int ocena_dla_id { get; set; }
        [DisplayName("Ocena")]
        public double ocena { get; set; }
        [DisplayName("Komentarz")]
        public string komentarz { get; set; }
    
        public virtual Uzytkownik OcenaDla { get; set; }
        public virtual Uzytkownik OcenaOd { get; set; }
    }
}
