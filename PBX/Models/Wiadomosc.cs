namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    public partial class Wiadomosc
    {
        [DisplayName("Id")]
        public int id { get; set; }
        [DisplayName("Chat")]
        public int chat_id { get; set; }
        [DisplayName("Nadawca")]
        public int nadawca_id { get; set; }
        [DisplayName("Wiadomoœæ")]
        public string wiadomosc { get; set; }
        [DisplayName("Wys³ano")]
        public System.DateTime wyslano { get; set; }
        [DisplayName("Przeczytano")]
        public bool przeczytano { get; set; }
    
        public virtual Chat Chat { get; set; }
        public virtual Uzytkownik Nadawca { get; set; }
    }
}
