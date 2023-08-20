namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public partial class Usunieci
    {
        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Imiê")]
        public string imie { get; set; }
        [DisplayName("Numer telefonu")]
        public string nr_tel { get; set; }
        [DisplayName("Adres email")]
        public string email { get; set; }
        [DisplayName("Has³o")]
        public string haslo { get; set; }
        [DisplayName("Do³¹czono")]
        public System.DateTime dolaczono { get; set; }
        [DisplayName("Usuniêto")]
        public System.DateTime usunieto { get; set; }
        [DisplayName("Zdjêcie")]
        public byte[] zdjecie { get; set; }
    }
}
