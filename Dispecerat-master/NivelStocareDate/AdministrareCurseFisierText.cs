using System.Collections.Generic;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareCurseFisierText
    {
        private List<Cursa> curse = new List<Cursa>();

        public void AddCursa(Cursa c) => curse.Add(c);
        public List<Cursa> GetCurse() => curse;
        public Cursa CautaCursaDupaCamion(string nr) => curse.FirstOrDefault(c => c.CamionAlocat.NumarInmatriculare.ToUpper() == nr.ToUpper() && c.Status != StatusCursa.Finalizata);
    }
}