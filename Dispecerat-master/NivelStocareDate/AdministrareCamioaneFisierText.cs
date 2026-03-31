using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareCamioaneFisierText
    {
        private string numeFisier = "camioane.txt";

        public AdministrareCamioaneFisierText()
        {
            if (!File.Exists(numeFisier)) File.Create(numeFisier).Close();
        }

        public void AddCamion(Camion camion)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
                sw.WriteLine(camion.ConversieLaSirPentruFisier());
        }

        public List<Camion> GetCamioane()
        {
            List<Camion> lista = new List<Camion>();
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                    lista.Add(new Camion(linie));
            }
            return lista;
        }

        public Camion CautaCamion(string nr) => GetCamioane().FirstOrDefault(c => c.NumarInmatriculare.ToUpper() == nr.ToUpper());

        public void UpdateCamion(Camion cModificat)
        {
            List<Camion> lista = GetCamioane();
            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                foreach (var c in lista)
                    sw.WriteLine(c.NumarInmatriculare.ToUpper() == cModificat.NumarInmatriculare.ToUpper() ? cModificat.ConversieLaSirPentruFisier() : c.ConversieLaSirPentruFisier());
            }
        }
        public bool StergeCamion(string numarCautat)
        {
            List<Camion> camioane = GetCamioane();
            Camion deSters = camioane.FirstOrDefault(c => c.NumarInmatriculare.ToUpper() == numarCautat.ToUpper());

            if (deSters != null)
            {
                camioane.Remove(deSters);
                using (StreamWriter sw = new StreamWriter(numeFisier, false))
                {
                    foreach (Camion c in camioane)
                        sw.WriteLine(c.ConversieLaSirPentruFisier());
                }
                return true;
            }
            return false;
        }
    }
}