using System.Collections.Generic;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareSoferiMemorie
    {
        private List<Sofer> soferi = new List<Sofer>();
        public void AddSofer(Sofer s) => soferi.Add(s);
        public List<Sofer> GetSoferi() => soferi;
        public Sofer CautaSofer(string id) => soferi.FirstOrDefault(s => s.IdAngajat.ToUpper() == id.ToUpper());
        public void UpdateSofer(Sofer s) { }
    }
}