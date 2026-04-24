using System;

namespace LibrarieModele
{
    public enum StatusCursa
    {
        InAsteptare,
        InDesfasurare,
        Finalizata,
        Anulata
    }

    [Flags]
    public enum TipMarfa
    {
        Standard = 0,
        Fragil = 1,
        PericulosADR = 2,
        SensibilTemperatura = 4,
        AnimaleVii = 8
    }

    public class Cursa
    {
        public string PunctPlecare { get; set; }
        public string Destinatie { get; set; }
        public double DistantaKm { get; set; }
        public string ETA { get; set; }
        public double Pret { get; set; }

        public TipMarfa CerinteMarfa { get; set; }

        public StatusCursa Status { get; private set; }
        public Camion CamionAlocat { get; set; }
        public Sofer SoferAlocat { get; set; }

        public Cursa(string punctPlecare, string destinatie, double distantaKm, string eta, double pret, Camion camion, Sofer sofer)
        {
            PunctPlecare = punctPlecare;
            Destinatie = destinatie;
            DistantaKm = distantaKm;
            ETA = eta;
            Pret = pret;
            CamionAlocat = camion;
            SoferAlocat = sofer;
            Status = StatusCursa.InAsteptare;
            CerinteMarfa = TipMarfa.Standard;
        }

        public void IncepeCursa()
        {
            Status = StatusCursa.InDesfasurare;
            CamionAlocat.StatusCurent = StatusCamion.InCursa;
            SoferAlocat.StatusCurent = StatusSofer.InCursa;
        }

        public void FinalizeazaCursa()
        {
            Status = StatusCursa.Finalizata;
            CamionAlocat.StatusCurent = StatusCamion.Disponibil;
            SoferAlocat.StatusCurent = StatusSofer.Disponibil;
        }

        public void AnuleazaCursa()
        {
            Status = StatusCursa.Anulata;
            CamionAlocat.StatusCurent = StatusCamion.Disponibil;
            SoferAlocat.StatusCurent = StatusSofer.Disponibil;
        }
    }
}