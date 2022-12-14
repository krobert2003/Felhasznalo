using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Felhasznalo
{
    internal class FELHASZNALO
    {
        int id;
        string nev;
        DateTime Szuldat;
        string kep;

        public FELHASZNALO(int id, string nev, DateTime szuldat1, string kep)
        {
            Id = id;
            Nev = nev;
            Szuldat1 = szuldat1;
            Kep = kep;
        }

        public int Id { get => id; set => id = value; }
        public string Nev { get => nev; set => nev = value; }
        public DateTime Szuldat1 { get => Szuldat; set => Szuldat = value; }
        public string Kep { get => kep; set => kep = value; }

        public override string ToString()
        {
            return "ID:"+id +"Név:" + nev + " Születési idő:" + Szuldat + " KepNeve:" + Kep;
        }
    }

   
}
