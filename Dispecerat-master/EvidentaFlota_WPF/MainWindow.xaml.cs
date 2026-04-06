using System.Windows;
using LibrarieModele; 

namespace EvidentaFlota_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Creăm un obiect complet
            Camion c = new Camion("B-123-WPF", "Volvo FH16", 24.5);
            c.StatusCurent = StatusCamion.Disponibil;
            c.Dotari = DotariCamion.AerConditionat | DotariCamion.Navigatie | DotariCamion.Dormitor;

            // Afișăm totul frumos (folosim \n pentru rând nou)
            lblDetalii.Text = $"DETALII VEHICUL:\n" +
                              $"--------------------------\n" +
                              $"Număr: {c.NumarInmatriculare}\n" +
                              $"Marcă: {c.Marca}\n" +
                              $"Capacitate: {c.CapacitateTone} tone\n" +
                              $"Status: {c.StatusCurent}\n" +
                              $"Dotări: {c.Dotari}";

            lblDetalii.Foreground = System.Windows.Media.Brushes.Black;
        }

    }
}