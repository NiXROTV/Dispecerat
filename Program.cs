public enum StatusCamion
{
    Disponibil,
    InCursa,
    InService
}
public class Camion
{ 
    public string NumarInmatriculare {  get; set; }
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
public enum StatusSofer
{
    Dispoibil,
    InCursa,
    InConcediu
}
public class Sofer
{ 
    public string Nume {  get; set; }
    public string Prenume { get; set; }
    public string IdAngajat {  get; set; }
    public StatusSofer StatusCurent { get; set; }
    public Sofer(string nume, string prenume,string idAngajat)
    {
        Nume = nume;
        Prenume= prenume;
        IdAngajat = idAngajat;
        StatusCurent = StatusSofer.Dispoibil;
    }
}
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
    public StatusCursa Status {  get; private set; }
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
        CamionAlocat.StatusCurent = StatusCamion.InCursa;
        SoferAlocat.StatusCurent= StatusSofer.Dispoibil;
    }
}
 
namespace ManagementFlota
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
            Console.WriteLine("--- 3. Dispecerul pornește cursa ---");
            cursa1.IncepeCursa();
            Console.WriteLine($"Status Cursă: {cursa1.Status}");
            Console.WriteLine($"Status Camion alocat: {camion1.StatusCurent}");
            Console.WriteLine($"Status Șofer alocat: {sofer1.StatusCurent}\n");
            Console.WriteLine("--- 4. Cursa a ajuns la destinație ---");
            cursa1.FinalizeazaCursa();
            Console.WriteLine($"Status Cursă: {cursa1.Status}");
            Console.WriteLine($"Status Camion eliberat: {camion1.StatusCurent}"); 
            Console.WriteLine($"Status Șofer eliberat: {sofer1.StatusCurent}\n");

            Console.WriteLine("Apasă tasta Enter pentru a închide...");
            Console.ReadLine();
        }
    }
}