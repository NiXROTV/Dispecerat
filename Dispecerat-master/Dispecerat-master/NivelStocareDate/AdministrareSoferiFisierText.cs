using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareSoferiFisierText
    {
        private string numeFisier = "soferi.txt";

        public AdministrareSoferiFisierText()
        {
            if (!File.Exists(numeFisier)) File.Create(numeFisier).Close();
        }

        public void AddSofer(Sofer sofer)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
                sw.WriteLine(sofer.ConversieLaSirPentruFisier());
        }

        public List<Sofer> GetSoferi()
        {
            List<Sofer> lista = new List<Sofer>();
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                    lista.Add(new Sofer(linie));
            }
            return lista;
        }

        public Sofer CautaSofer(string id) => GetSoferi().FirstOrDefault(s => s.IdAngajat.ToUpper() == id.ToUpper());

        public void UpdateSofer(Sofer sModificat)
        {
            List<Sofer> lista = GetSoferi();
            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                foreach (var s in lista)
                    sw.WriteLine(s.IdAngajat.ToUpper() == sModificat.IdAngajat.ToUpper() ? sModificat.ConversieLaSirPentruFisier() : s.ConversieLaSirPentruFisier());
            }
        }
        public bool StergeSofer(string idCautat)
        {
            List<Sofer> soferi = GetSoferi();
            Sofer deSters = soferi.FirstOrDefault(s => s.IdAngajat.ToUpper() == idCautat.ToUpper());

            if (deSters != null)
            {
                soferi.Remove(deSters);
                using (StreamWriter sw = new StreamWriter(numeFisier, false))
                {
                    foreach (Sofer s in soferi)
                        sw.WriteLine(s.ConversieLaSirPentruFisier());
                }
                return true;
            }
            return false;
        }
    }
}