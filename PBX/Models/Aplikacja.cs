//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PBX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Aplikacja
    {
        [DisplayName("ID Ogłoszenia")]
        public int ogloszenie_id { get; set; }
        [DisplayName("ID Użytkownika")]
        public int uzytkownik_id { get; set; }
        [DisplayName("CV")]
        public byte[] cv { get; set; }
        [DisplayName("ID")]
        public int id { get; set; }
    
        public virtual Ogloszenie Ogloszenie { get; set; }
        public virtual Uzytkownik Aplikujacy { get; set; }
    }
}
