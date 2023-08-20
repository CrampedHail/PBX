namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    public partial class Zgloszenie
    {
        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Og³oszenie")]
        public int ogloszenie_id { get; set; }
        [DisplayName("Zg³aszaj¹cy")]
        public int zglaszajacy_id { get; set; }
        [DisplayName("Zg³oszony")]
        public int zgloszony_id { get; set; }
        [DisplayName("Treœæ")]
        public string tresc { get; set; }
    
        public virtual Ogloszenie Ogloszenie { get; set; }
        public virtual Uzytkownik Zglaszajacy { get; set; }
        public virtual Uzytkownik Zgloszony { get; set; }
    }
}
