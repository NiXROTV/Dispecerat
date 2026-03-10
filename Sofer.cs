using System;
using System.Collections.Generic;
using System.Text;

namespace Dispecerat
{
    public enum StatusSofer
    {
        Disponibil,
        InCursa,
        InConcediu
    }
    public class Sofer
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string IdAngajat { get; set; }
        public StatusSofer StatusCurent { get; set; }
        public Sofer(string nume, string prenume, string idAngajat)
        {
            Nume = nume;
            Prenume = prenume;
            IdAngajat = idAngajat;
            StatusCurent = StatusSofer.Disponibil;
        }
    }
}
