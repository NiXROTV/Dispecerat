namespace Dispecerat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===Sistem Dispecerat Flota ===\n");
            Camion camion1= new Camion("B-123-TRK","Volvo",20.5);
            Sofer sofer1 = new Sofer("Popescu", "Ion", "EMP001");
            Console.WriteLine("--- 1.Status Initial ---");
            Console.WriteLine($"Camion {camion1.NumarInmatriculare} - Status: {camion1.StatusCurent}");
            Console.WriteLine($"Sofer {sofer1.Nume} {sofer1.Prenume} - Status: {sofer1.StatusCurent}");
            Cursa cursa1 = new Cursa("Bucuresti", "Cluj-Napoca", camion1, sofer1);
            Console.WriteLine("--- 2.Cursa a fost creata ---");
            Console.WriteLine($"Traseu: {cursa1.PunctPlecare} -> {cursa1.Destinatie}");
            Console.WriteLine($"Status Cursă: {cursa1.Status}\n");
            Console.WriteLine("--- 3. Dispecerul porneste cursa ---");
            cursa1.IncepeCursa();
            Console.WriteLine($"Status Cursă: {cursa1.Status}");
            Console.WriteLine($"Status Camion alocat: {camion1.StatusCurent}");
            Console.WriteLine($"Status Sofer alocat: {sofer1.StatusCurent}\n");
            Console.WriteLine("--- 4. Cursa a ajuns la destinatie ---");
            cursa1.FinalizeazaCursa();
            Console.WriteLine($"Status Cursă: {cursa1.Status}");
            Console.WriteLine($"Status Camion eliberat: {camion1.StatusCurent}"); 
            Console.WriteLine($"Status Sofer eliberat: {sofer1.StatusCurent}\n");

            Console.WriteLine("Apasă tasta Enter pentru a închide...");
            Console.ReadLine();
        }
    }
}