namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Admin
    {
        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Login")]
        public string login { get; set; }
        [DisplayName("Has³o")]
        public string haslo { get; set; }
    }
}
