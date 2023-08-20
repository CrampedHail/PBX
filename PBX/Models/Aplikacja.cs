namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Aplikacja
    {

        [DisplayName("Og³oszenie")]
        public int ogloszenie_id { get; set; }
        [DisplayName("Uzytkownik")]
        public int uzytkownik_id { get; set; }
        [DisplayName("Plik z CV")]
        public byte[] cv { get; set; }
        [DisplayName("ID")]
        public int id { get; set; }
    
        public virtual Ogloszenie Ogloszenie { get; set; }
        public virtual Uzytkownik Aplikujacy { get; set; }
    }
}
