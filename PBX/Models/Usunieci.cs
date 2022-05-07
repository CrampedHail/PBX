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

    public partial class Usunieci
    {
        [Required]
        [DisplayName("ID")]
        public int id { get; set; }

        [Required]
        [DisplayName("Imi�")]
        public string imie { get; set; }

        [Required]
        [DisplayName("Numer Telefonu")]
        [DataType(DataType.PhoneNumber)]
        public string nr_tel { get; set; }

        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required]
        [DisplayName("Has�o")]
        [DataType(DataType.Password)]
        public string haslo { get; set; }

        [Required]
        [DisplayName("Do��czono")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> dolaczono { get; set; }

        [Required]
        [DisplayName("Usuni�to")]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> usunieto { get; set; }
    }
}
