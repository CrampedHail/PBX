using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBX.Controllers
{
    public class Types
    {
        public enum TypOgloszenia
        {
            Sprzedaz = 's',
            Wymiana = 'w',
            Kupno = 'k',
            Rekrutacja = 'r',
            Praca = 'p'
        }

        public enum StatusZamowienia
        {
            Nowe = 'n',
            Przygotowane = 'p',
            Wyslane = 'w',
            Dostawa = 'd',
            Odebrane = 'o',
            Zakonczone = 'z'
        }
    }
}