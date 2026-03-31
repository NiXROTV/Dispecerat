using System;

namespace LibrarieModele
{
    public enum StatusSofer
    {
        Disponibil,
        InCursa,
        InConcediu,
        Medical 
    }

    [Flags]
    public enum AtestateSofer
    {
        Niciunul = 0,
        MarfaGenerala = 1,
        ADR = 2,          
        Agabaritic = 4,  
        Frigorific = 8
    }

    public class Sofer
    {
        private const char SEPARATOR = ';';
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string IdAngajat { get; set; }
        public StatusSofer StatusCurent { get; set; }

        public AtestateSofer Atestate { get; set; }

        public Sofer(string nume, string prenume, string idAngajat)
        {
            Nume = nume;
            Prenume = prenume;
            IdAngajat = idAngajat;
            StatusCurent = StatusSofer.Disponibil;
            Atestate = AtestateSofer.Niciunul;
        }
        public Sofer(string linieFisier)
        {
            string[] date = linieFisier.Split(SEPARATOR);
            IdAngajat = date[0];
            Nume = date[1];
            Prenume = date[2];

            Enum.TryParse(date[3], out StatusSofer status);
            StatusCurent = status;

            Enum.TryParse(date[4], out AtestateSofer atestate);
            Atestate = atestate;
        }
        public string ConversieLaSirPentruFisier()
        {
            return $"{IdAngajat}{SEPARATOR}{Nume}{SEPARATOR}{Prenume}{SEPARATOR}{StatusCurent}{SEPARATOR}{Atestate}";
        }
    }
}