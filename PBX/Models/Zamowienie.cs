namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public partial class Zamowienie
    {
        [DisplayName("Og³oszenie")]
        public int ogloszenie_id { get; set; }
        [DisplayName("U¿ytkownik")]
        public int uzytkownik_id { get; set; }
        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Data")]
        public System.DateTime data { get; set; }
        [DisplayName("Status")]
        public string status { get; set; }
    
        public virtual Ogloszenie Ogloszenie { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; }
    }
}
