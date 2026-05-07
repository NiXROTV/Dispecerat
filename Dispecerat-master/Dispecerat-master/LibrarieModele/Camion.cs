using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibrarieModele
{
    public enum StatusCamion { Disponibil, InCursa, InService }

    [Flags]
    public enum DotariCamion
    {
        Niciuna = 0, AerConditionat = 1, Navigatie = 2, SistemFrig = 4, Dormitor = 8
    }

    public class Camion : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private const char SEPARATOR = ';';

        public static readonly IReadOnlyList<string> ValoriTipContract =
            new List<string> { "Propriu", "Leasing", "Inchiriat" };

        private string _numarInmatriculare = string.Empty;
        public string NumarInmatriculare
        {
            get => _numarInmatriculare;
            set { _numarInmatriculare = value; OnPropertyChanged(); }
        }

        private string _marca = string.Empty;
        public string Marca
        {
            get => _marca;
            set { _marca = value; OnPropertyChanged(); }
        }

        private double _capacitateTone;
        public double CapacitateTone
        {
            get => _capacitateTone;
            set { _capacitateTone = value; OnPropertyChanged(); }
        }

        private StatusCamion _statusCurent;
        public StatusCamion StatusCurent
        {
            get => _statusCurent;
            set { _statusCurent = value; OnPropertyChanged(); }
        }

        private DotariCamion _dotari;
        public DotariCamion Dotari
        {
            get => _dotari;
            set { _dotari = value; OnPropertyChanged(); }
        }

        private string _tipContract = "Propriu";
        public string TipContract
        {
            get => _tipContract;
            set { _tipContract = value; OnPropertyChanged(); }
        }

        private DateTime _dataInregistrarii;
        public DateTime DataInregistrarii
        {
            get => _dataInregistrarii;
            set { _dataInregistrarii = value; OnPropertyChanged(); }
        }

        private DateTime _dataActualizarii;
        public DateTime DataActualizarii
        {
            get => _dataActualizarii;
            set { _dataActualizarii = value; OnPropertyChanged(); }
        }

        public Camion(string numarInmatriculare, string marca, double capacitateTone)
        {
            NumarInmatriculare = numarInmatriculare;
            Marca = marca;
            CapacitateTone = capacitateTone;
            StatusCurent = StatusCamion.Disponibil;
            Dotari = DotariCamion.Niciuna;
            TipContract = ValoriTipContract[0];
            DataInregistrarii = DateTime.Now;
            DataActualizarii = DateTime.Now;
        }

        public Camion(string linieFisier)
        {
            string[] date = linieFisier.Split(SEPARATOR);
            NumarInmatriculare = date[0];
            Marca = date[1];
            CapacitateTone = Convert.ToDouble(date[2]);
            Enum.TryParse(date[3], out StatusCamion status); StatusCurent = status;
            Enum.TryParse(date[4], out DotariCamion dotari); Dotari = dotari;
            TipContract = date.Length > 5 ? date[5] : ValoriTipContract[0];
            DataInregistrarii = date.Length > 6 && DateTime.TryParse(date[6], out DateTime di) ? di : DateTime.Now;
            DataActualizarii = date.Length > 7 && DateTime.TryParse(date[7], out DateTime da) ? da : DateTime.Now;
        }

        public string ConversieLaSirPentruFisier() =>
            $"{NumarInmatriculare}{SEPARATOR}{Marca}{SEPARATOR}{CapacitateTone}{SEPARATOR}" +
            $"{StatusCurent}{SEPARATOR}{Dotari}{SEPARATOR}{TipContract}{SEPARATOR}" +
            $"{DataInregistrarii:O}{SEPARATOR}{DataActualizarii:O}";

        public override string ToString() => $"{NumarInmatriculare} — {Marca}";
    }
}