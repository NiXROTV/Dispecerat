using System;
using System.Collections.Generic;
using LibrarieModele;
using NivelStocareDate;

namespace EvidentaFlota
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initializam managerii separati (varianta FisierText)
            AdministrareCamioaneFisierText adminCamioane = new AdministrareCamioaneFisierText();
            AdministrareSoferiFisierText adminSoferi = new AdministrareSoferiFisierText();
            AdministrareCurseFisierText adminCurse = new AdministrareCurseFisierText();

            bool ruleaza = true;

            while (ruleaza)
            {
                Console.WriteLine("\n=== MENIU DISPECERAT FLOTA ===");
                Console.WriteLine("1. Adauga un camion nou");
                Console.WriteLine("2. Afiseaza toate camioanele");
                Console.WriteLine("3. Cauta un camion");
                Console.WriteLine("4. Adauga sofer");
                Console.WriteLine("5. Afiseaza soferi");
                Console.WriteLine("6. Adauga cursa");
                Console.WriteLine("7. Afiseaza curse");
                Console.WriteLine("8. Finalizeaza o cursa");
                Console.WriteLine("9. Retrage camion din flota");
                Console.WriteLine("10. Concediaza sofer");
                Console.WriteLine("11. Trimite camion in service");
                Console.WriteLine("12. Schimba status sofer (Medical / Concediu)");
                Console.WriteLine("13. Anuleaza o cursa activa");
                Console.WriteLine("0. Iesire");
                Console.Write("Alege o optiune: ");

                string optiune = Console.ReadLine();
                Console.WriteLine();

                switch (optiune)
                {
                    case "1":
                        Console.Write("Numar inmatriculare: "); string numar = Console.ReadLine();
                        Console.Write("Marca: "); string marca = Console.ReadLine();
                        Console.Write("Capacitate (tone): "); double capacitate = double.Parse(Console.ReadLine());

                        Console.WriteLine("\n--- Dotari Camion (1-AC, 2-Navi, 4-Frig, 8-Dormitor, 0-Gata) ---");
                        DotariCamion dotariFinale = DotariCamion.Niciuna;
                        while (true)
                        {
                            Console.Write("Dotare: "); string inp = Console.ReadLine();
                            if (inp == "0") break;
                            if (inp == "1") dotariFinale |= DotariCamion.AerConditionat;
                            else if (inp == "2") dotariFinale |= DotariCamion.Navigatie;
                            else if (inp == "4") dotariFinale |= DotariCamion.SistemFrig;
                            else if (inp == "8") dotariFinale |= DotariCamion.Dormitor;
                        }
                        Camion cNou = new Camion(numar, marca, capacitate);
                        cNou.Dotari = dotariFinale;
                        adminCamioane.AddCamion(cNou);
                        Console.WriteLine(">> Camion salvat cu dotari!");
                        break;

                    case "2":
                        var listaC = adminCamioane.GetCamioane();
                        if (listaC.Count == 0) Console.WriteLine("Flota goala.");
                        else foreach (var c in listaC) Console.WriteLine($"Numar: {c.NumarInmatriculare} | Status: {c.StatusCurent}");
                        break;

                    case "3":
                        Console.Write("Nr inmatriculare: ");
                        var gasitC = adminCamioane.CautaCamion(Console.ReadLine());
                        Console.WriteLine(gasitC != null ? $"Gasit: {gasitC.Marca}" : "Negasit.");
                        break;

                    case "4":
                        Console.Write("Nume: "); string ns = Console.ReadLine();
                        Console.Write("Prenume: "); string ps = Console.ReadLine();
                        Console.Write("ID: "); string ids = Console.ReadLine();

                        Console.WriteLine("\n--- Atestate (1-Generala, 2-ADR, 4-Agabaritic, 8-Frig, 0-Gata) ---");
                        AtestateSofer atestatFinal = AtestateSofer.Niciunul;
                        while (true)
                        {
                            Console.Write("Atestat: "); string inp = Console.ReadLine();
                            if (inp == "0") break;
                            if (inp == "1") atestatFinal |= AtestateSofer.MarfaGenerala;
                            else if (inp == "2") atestatFinal |= AtestateSofer.ADR;
                            else if (inp == "4") atestatFinal |= AtestateSofer.Agabaritic;
                            else if (inp == "8") atestatFinal |= AtestateSofer.Frigorific;
                        }
                        Sofer sNou = new Sofer(ns, ps, ids);
                        sNou.Atestate = atestatFinal;
                        adminSoferi.AddSofer(sNou);
                        Console.WriteLine(">> Sofer salvat cu atestate!");
                        break;
                        break;

                    case "5":
                        var listaS = adminSoferi.GetSoferi();
                        if (listaS.Count == 0) Console.WriteLine("Niciun sofer.");
                        else foreach (var s in listaS) Console.WriteLine($"ID: {s.IdAngajat} | {s.Nume} | Status: {s.StatusCurent}");
                        break;

                    case "6":
                        Console.WriteLine("--- Creare Cursa Noua ---");
                        Console.Write("Plecare: "); string plec = Console.ReadLine();
                        Console.Write("Destinatie: "); string dest = Console.ReadLine();
                        Console.Write("Distanta (km): "); double dist = double.Parse(Console.ReadLine());
                        Console.Write("Pret: "); double pret = double.Parse(Console.ReadLine());
                        Console.Write("ETA: "); string eta = Console.ReadLine();

                        Console.Write("Nr Camion: "); var cA = adminCamioane.CautaCamion(Console.ReadLine());
                        Console.Write("ID Sofer: "); var sA = adminSoferi.CautaSofer(Console.ReadLine());

                        if (cA != null && sA != null && cA.StatusCurent == StatusCamion.Disponibil && sA.StatusCurent == StatusSofer.Disponibil)
                        {
                            Console.WriteLine("\n--- Cerinte Marfa (1-Fragil, 2-ADR, 4-Temp, 8-Animale, 0-Gata) ---");
                            TipMarfa marfaReq = TipMarfa.Standard;
                            while (true)
                            {
                                Console.Write("Cerinta: "); string inp = Console.ReadLine();
                                if (inp == "0") break;
                                if (inp == "1") marfaReq |= TipMarfa.Fragil;
                                else if (inp == "2") marfaReq |= TipMarfa.PericulosADR;
                                else if (inp == "4") marfaReq |= TipMarfa.SensibilTemperatura;
                                else if (inp == "8") marfaReq |= TipMarfa.AnimaleVii;
                            }

                            bool ok = true;
                            if (marfaReq.HasFlag(TipMarfa.PericulosADR) && !sA.Atestate.HasFlag(AtestateSofer.ADR))
                            {
                                Console.WriteLine("(!) Eroare: Soferul nu are ADR!"); ok = false;
                            }
                            if (marfaReq.HasFlag(TipMarfa.SensibilTemperatura))
                            {
                                if (!sA.Atestate.HasFlag(AtestateSofer.Frigorific)) { Console.WriteLine("(!) Eroare: Sofer fara atestat Frig!"); ok = false; }
                                if (!cA.Dotari.HasFlag(DotariCamion.SistemFrig)) { Console.WriteLine("(!) Eroare: Camion fara instalatie Frig!"); ok = false; }
                            }

                            if (ok)
                            {
                                Cursa cursa = new Cursa(plec, dest, dist, eta, pret, cA, sA);
                                cursa.CerinteMarfa = marfaReq;
                                cursa.IncepeCursa();
                                adminCurse.AddCursa(cursa);
                                adminCamioane.UpdateCamion(cA);
                                adminSoferi.UpdateSofer(sA);
                                Console.WriteLine(">> Cursa pornita cu succes!");
                            }
                        }
                        else Console.WriteLine("Camion/Sofer inexistent sau ocupat!");
                        break;

                    case "7":
                        var curse = adminCurse.GetCurse();
                        foreach (var crs in curse) Console.WriteLine($"{crs.PunctPlecare}->{crs.Destinatie} | Status: {crs.Status}");
                        break;

                    case "8":
                        Console.Write("Nr Camion pt finalizare: ");
                        var fCursa = adminCurse.CautaCursaDupaCamion(Console.ReadLine());
                        if (fCursa != null)
                        {
                            fCursa.FinalizeazaCursa();
                            adminCamioane.UpdateCamion(fCursa.CamionAlocat);
                            adminSoferi.UpdateSofer(fCursa.SoferAlocat);
                            Console.WriteLine(">> Cursa finalizata!");
                        }
                        break;

                    case "9":
                        Console.Write("Camion de sters: ");
                        adminCamioane.StergeCamion(Console.ReadLine());
                        break;

                    case "10":
                        Console.Write("Sofer de sters: ");
                        adminSoferi.StergeSofer(Console.ReadLine());
                        break;

                    case "11":
                        Console.Write("Nr Camion service: ");
                        var cServ = adminCamioane.CautaCamion(Console.ReadLine());
                        if (cServ != null)
                        {
                            cServ.StatusCurent = StatusCamion.InService;
                            adminCamioane.UpdateCamion(cServ);
                            Console.WriteLine(">> Trimis in service.");
                        }
                        break;

                    case "12":
                        Console.Write("ID Sofer status: ");
                        var sStat = adminSoferi.CautaSofer(Console.ReadLine());
                        if (sStat != null)
                        {
                            Console.Write("1-Disponibil, 2-Concediu, 3-Medical: ");
                            string optS = Console.ReadLine();
                            if (optS == "1") sStat.StatusCurent = StatusSofer.Disponibil;
                            else if (optS == "2") sStat.StatusCurent = StatusSofer.InConcediu;
                            else if (optS == "3") sStat.StatusCurent = StatusSofer.Medical;
                            adminSoferi.UpdateSofer(sStat);
                        }
                        break;

                    case "13":
                        Console.Write("Nr Camion anulare: ");
                        var aCursa = adminCurse.CautaCursaDupaCamion(Console.ReadLine());
                        if (aCursa != null)
                        {
                            aCursa.AnuleazaCursa();
                            adminCamioane.UpdateCamion(aCursa.CamionAlocat);
                            adminSoferi.UpdateSofer(aCursa.SoferAlocat);
                            Console.WriteLine(">> Cursa anulata.");
                        }
                        break;

                    case "0": ruleaza = false;
                        break;
                }
            }
        }
    }
}