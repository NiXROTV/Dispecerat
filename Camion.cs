using System;
using System.Collections.Generic;
using System.Text;

namespace Dispecerat
{
    public enum StatusCamion
    {
        Disponibil,
        InCursa,
        InService
    }
    public class Camion
    {
        public string NumarInmatriculare { get; set; }
        public string Marca { get; set; }
        public double CapacitateTone { get; set; }
        public StatusCamion StatusCurent { get; set; }
        public Camion(string numarInmatriculare, string marca, double capacitateTone)
        {
            NumarInmatriculare = numarInmatriculare;
            Marca = marca;
            CapacitateTone = capacitateTone;
            StatusCurent = StatusCamion.Disponibil;
        }
        public void TrimiteInService()
        {
            StatusCurent = StatusCamion.InService;
        }
    }

}
