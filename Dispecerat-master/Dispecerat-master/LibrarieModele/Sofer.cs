using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibrarieModele
{
    public enum StatusSofer { Disponibil, InCursa, InConcediu, Medical }

    [Flags]
    public enum AtestateSofer
    {
        Niciunul = 0, MarfaGenerala = 1, ADR = 2, Agabaritic = 4, Frigorific = 8
    }

    public class Sofer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private const char SEPARATOR = ';';

        private string _idAngajat = string.Empty;
        public string IdAngajat
        {
            get => _idAngajat;
            set { _idAngajat = value; OnPropertyChanged(); }
        }

        private string _nume = string.Empty;
        public string Nume
        {
            get => _nume;
            set { _nume = value; OnPropertyChanged(); }
        }

        // Proprietate cu INotifyPropertyChanged - utilizata cu TwoWay Binding
        private string _prenume = string.Empty;
        public string Prenume
        {
            get => _prenume;
            set { _prenume = value; OnPropertyChanged(); }
        }

        private StatusSofer _statusCurent;
        public StatusSofer StatusCurent
        {
            get => _statusCurent;
            set { _statusCurent = value; OnPropertyChanged(); }
        }

        private AtestateSofer _atestate;
        public AtestateSofer Atestate
        {
            get => _atestate;
            set { _atestate = value; OnPropertyChanged(); }
        }

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
            Enum.TryParse(date[3], out StatusSofer status); StatusCurent = status;
            Enum.TryParse(date[4], out AtestateSofer atestate); Atestate = atestate;
        }

        public string ConversieLaSirPentruFisier() =>
            $"{IdAngajat}{SEPARATOR}{Nume}{SEPARATOR}{Prenume}{SEPARATOR}{StatusCurent}{SEPARATOR}{Atestate}";

        public override string ToString() => $"{Nume} {Prenume} ({IdAngajat})";
    }
}