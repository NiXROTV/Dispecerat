using System.Collections.Generic;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareCamioaneMemorie
    {
        private List<Camion> camioane = new List<Camion>();
        public void AddCamion(Camion c) => camioane.Add(c);
        public List<Camion> GetCamioane() => camioane;
        public Camion CautaCamion(string nr) => camioane.FirstOrDefault(c => c.NumarInmatriculare.ToUpper() == nr.ToUpper());
        public void UpdateCamion(Camion c) { }
    }
}