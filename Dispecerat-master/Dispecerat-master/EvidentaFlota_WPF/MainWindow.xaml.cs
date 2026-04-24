using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using LibrarieModele;

namespace EvidentaFlota_WPF
{
    public partial class MainWindow : Window
    {
        // Constante pentru limite de validare
        private const int LUNGIME_MIN_NR = 5;
        private const int LUNGIME_MIN_MARCA = 3;
        private const double CAPACITATE_MIN = 1.0;
        private const double CAPACITATE_MAX = 40.0;

        // Memorie locală pentru căutare și mentenanța sesiunii grafice
        private List<Camion> memorieCamioane = new List<Camion>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            ResetareCulori();
            lblMesaj.Text = string.Empty;

            bool esteValid = true;
            string mesajEroare = string.Empty;

            string numar = txtNumar.Text.Trim();
            if (string.IsNullOrEmpty(numar) || numar.Length < LUNGIME_MIN_NR)
            {
                esteValid = false;
                mesajEroare += $"Numărul de înmatriculare necesită min. {LUNGIME_MIN_NR} caractere!\n";
                lblNumar.Foreground = Brushes.Red;
            }

            string marca = txtMarca.Text.Trim();
            if (string.IsNullOrEmpty(marca) || marca.Length < LUNGIME_MIN_MARCA)
            {
                esteValid = false;
                mesajEroare += $"Marca necesită min. {LUNGIME_MIN_MARCA} caractere!\n";
                lblMarca.Foreground = Brushes.Red;
            }

            if (!double.TryParse(txtCapacitate.Text.Trim(), out double capacitate) || capacitate < CAPACITATE_MIN || capacitate > CAPACITATE_MAX)
            {
                esteValid = false;
                mesajEroare += $"Capacitate invalidă (între {CAPACITATE_MIN} și {CAPACITATE_MAX})!\n";
                lblCapacitate.Foreground = Brushes.Red;
            }

            if (esteValid)
            {
                Camion c = new Camion(numar, marca, capacitate);
                
                // Preluare stare RadioButtons
                if (rbDisponibil.IsChecked == true) c.StatusCurent = StatusCamion.Disponibil;
                else if (rbInCursa.IsChecked == true) c.StatusCurent = StatusCamion.InCursa;
                else if (rbInService.IsChecked == true) c.StatusCurent = StatusCamion.InService;

                // Preluare stare CheckBoxes
                DotariCamion dotari = DotariCamion.Niciuna;
                if (chkAer.IsChecked == true) dotari |= DotariCamion.AerConditionat;
                if (chkNavigatie.IsChecked == true) dotari |= DotariCamion.Navigatie;
                if (chkFrig.IsChecked == true) dotari |= DotariCamion.SistemFrig;
                if (chkDormitor.IsChecked == true) dotari |= DotariCamion.Dormitor;
                
                c.Dotari = dotari;

                // Adăugare în colecția interfeței pentru a putea căuta testul specific mai târziu
                memorieCamioane.Add(c);

                lblMesaj.Foreground = Brushes.Green;
                lblMesaj.Text = "Camion adăugat cu succes!";
                
                lblDetaliiAdaugare.Text = AfisareDetaliiCamion(c);
                
                CuratareFormular();
            }
            else
            {
                lblMesaj.Foreground = Brushes.Red;
                lblMesaj.Text = mesajEroare.TrimEnd('\n');
            }
        }

        private void BtnCauta_Click(object sender, RoutedEventArgs e)
        {
            string numarCautat = txtCautare.Text.Trim();
            
            if (string.IsNullOrEmpty(numarCautat))
            {
                lblRezultatCautare.Text = "Introduceți un număr de înmatriculare pentru a lansa căutarea.";
                lblRezultatCautare.Foreground = Brushes.Black;
                return;
            }

            // Implementare logică a operației de căutare
            Camion camionGasit = memorieCamioane.FirstOrDefault(
                c => c.NumarInmatriculare.Equals(numarCautat, StringComparison.OrdinalIgnoreCase)
            );
            
            if (camionGasit != null)
            {
                lblRezultatCautare.Text = "GĂSIT:\n" + AfisareDetaliiCamion(camionGasit);
                lblRezultatCautare.Foreground = Brushes.Green;
            }
            else
            {
                lblRezultatCautare.Text = $"Camionul cu numărul '{numarCautat}' nu a fost localizat. Adăugați-l întâi prin formular.";
                lblRezultatCautare.Foreground = Brushes.Red;
            }
        }

        private string AfisareDetaliiCamion(Camion c)
        {
            return $"Număr: {c.NumarInmatriculare}\n" +
                   $"Marcă: {c.Marca}\n" +
                   $"Capacitate: {c.CapacitateTone} tone\n" +
                   $"Status: {c.StatusCurent}\n" +
                   $"Dotări Opționale: {(c.Dotari == DotariCamion.Niciuna ? "Niciuna" : c.Dotari.ToString())}";
        }

        private void ResetareCulori()
        {
            lblNumar.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#34495E"));
            lblMarca.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#34495E"));
            lblCapacitate.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#34495E"));
        }

        private void CuratareFormular()
        {
            txtNumar.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtCapacitate.Text = string.Empty;
            rbDisponibil.IsChecked = true;
            chkAer.IsChecked = false;
            chkNavigatie.IsChecked = false;
            chkFrig.IsChecked = false;
            chkDormitor.IsChecked = false;
        }

        private void MenuItemIesire_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItemDespre_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplicație de Evidență Flotă auto.\nSubrutină gestionare Camioane.\n\nFuncții implementate:\n- Validări live.\n- Control CheckBox/Radio.\n- Căutare Memorie locală.", "Despre", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}