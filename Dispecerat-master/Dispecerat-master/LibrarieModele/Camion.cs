using System;
namespace LibrarieModele
{ 
    public enum StatusCamion
    {
        Disponibil,
        InCursa,
        InService,
    }

    [Flags]
    public enum DotariCamion
    {
        Niciuna = 0,
        AerConditionat = 1,
        Navigatie = 2,
        SistemFrig = 4,
        Dormitor = 8,
    }

    public class Camion
    {
        private const char SEPARATOR = ';';
        public string NumarInmatriculare { get; set; }
        public string Marca { get; set; }
        public double CapacitateTone { get; set; }
        public StatusCamion StatusCurent { get; set; }
        public string TipCarburant { get; set; }
        public DateTime DataFabricatie { get; set; }
        public DateTime DataActualizare { get; set; }

        public Camion(string numarInmatriculare, string marca, double capacitateTone, string tipCarburant, DateTime dataFabricatie)
        {
            NumarInmatriculare = numarInmatriculare;
            Marca = marca;
            CapacitateTone = capacitateTone;
            StatusCurent = StatusCamion.Disponibil;
            TipCarburant = tipCarburant;
            DataFabricatie = dataFabricatie;
            DataActualizare = DateTime.Now;
        }
        public DotariCamion Dotari { get; set; }
        public Camion(string linieFisier)
        {
            string[] date = linieFisier.Split(SEPARATOR);
            NumarInmatriculare = date[0];
            Marca = date[1];
            CapacitateTone = Convert.ToDouble(date[2]);

            Enum.TryParse(date[3], out StatusCamion status);
            StatusCurent = status;

            Enum.TryParse(date[4], out DotariCamion dotari);
            Dotari = dotari;

            if (date.Length > 5)
            {
                TipCarburant = date[5];
                if (DateTime.TryParse(date[6], out DateTime dataFab)) DataFabricatie = dataFab;
                if (DateTime.TryParse(date[7], out DateTime dataAct)) DataActualizare = dataAct;
            }
        }
        public string ConversieLaSirPentruFisier()
        {
            return $"{NumarInmatriculare}{SEPARATOR}{Marca}{SEPARATOR}{CapacitateTone}{SEPARATOR}{StatusCurent}{SEPARATOR}{Dotari}{SEPARATOR}{TipCarburant}{SEPARATOR}{DataFabricatie:O}{SEPARATOR}{DataActualizare:O}";
        }
    }
}