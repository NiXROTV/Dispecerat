using Dispecerat;

namespace Dispecerat
{
    class Program
    {
        static void Main(string[] args)
        {
            Camion[] flota = new Camion[100];
            int numarCamioane = 0;
            bool ruleaza = true;
            while (ruleaza=true)
            {
                Console.WriteLine("\n=== Meniu Dispecerat Flota ===");
                Console.WriteLine("1. Adauga un camion nou");
                Console.WriteLine("2. Afiseaza toate camioanele");
                Console.WriteLine("3. Cauta un camion (dupa numarul de inmatriculare)");
                Console.WriteLine("0. Iesire");
                Console.Write("Alege o optiune: ");

                string optiune = Console.ReadLine();
                Console.WriteLine();
                switch (optiune)
                {
                    case "1":
                        Console.Write("Introdu numarul de inmatriculare (ex: B-10-TRK): ");
                        string numar = Console.ReadLine();
                        Console.Write("Introdu marca (ex: Volvo): ");
                        string marca = Console.ReadLine();
                        Console.Write("Introdu capacitatea in tone (ex: 20): ");
                        double capacitate = Convert.ToDouble(Console.ReadLine());
                        flota[numarCamioane] = new Camion(numar, marca, capacitate);
                        numarCamioane++;
                        Console.WriteLine(">> Camion adaugat cu succes!");
                        break;
                    case "2":
                        if (numarCamioane == 0)
                        {
                            Console.WriteLine("Flota este goala. Nu exista camioane de afisat.");
                        }
                        else
                        {
                            Console.WriteLine("--- Lista camioane ---");
                            for (int i = 0; i < numarCamioane; i++)
                            {
                                Console.WriteLine($"{i + 1}. Numar {flota[i].NumarInmatriculare} | Marca {flota[i].Marca}, poate transporta {flota[i].CapacitateTone} tone si momentan este {flota[i].StatusCurent}.");
                            }
                        }
                        break;
                    case "3":
                        Console.Write("Introdu numarul de inmatriculare pe care il cauti: ");
                        string numarCautat = Console.ReadLine();
                        bool gasit = false;
                        for (int i = 0;i < numarCamioane;i++)
                        {
                            if (flota[i].NumarInmatriculare == numarCautat)
                            {  
                                Console.WriteLine($"Camion gasit: Marca {flota[i].Marca},poate transporta {flota[i].CapacitateTone} tone si momentan este {flota[i].StatusCurent}.");
                                gasit = true;
                                break; 
                            }
                        }
                        if(! gasit)
                        {
                            Console.WriteLine("Camionul nu a fost gasit in sistem.");
                        }
                        break;
                    case "0":
                        ruleaza = false;
                        Console.WriteLine("Inchidere program.. ");
                        break;
                    default:
                        Console.WriteLine("Optiune invalida! Alege 1, 2, 3, sau 0.");
                        break;
                }
            }
        }
    }
}