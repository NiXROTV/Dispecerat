using System;
using System.Collections.Generic;
using System.Text;

namespace Dispecerat
{
    public enum StatusCursa
    {
        InAsteptare,
        InDesfasurare,
        Finalizata
    }
    public class Cursa
    {
        public string PunctPlecare { get; set; }
        public string Destinatie { get; set; }
        public StatusCursa Status { get; private set; }
        public Camion CamionAlocat { get; set; }
        public Sofer SoferAlocat { get; set; }
        public Cursa(string punctPlecare, string destinatie, Camion camion, Sofer sofer)
        {
            PunctPlecare = punctPlecare;
            Destinatie = destinatie;
            CamionAlocat = camion;
            SoferAlocat = sofer;
            Status = StatusCursa.InAsteptare;
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
    }
}
