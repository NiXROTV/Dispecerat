using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LibrarieModele;
using NivelStocareDate;

namespace EvidentaFlota_WPF
{
    public partial class MainWindow : Window
    {
        // ─── Constante validare ────────────────────────────────────────────────
        private const int LUNGIME_MIN_NR = 5;
        private const int LUNGIME_MIN_MARCA = 3;
        private const double CAPACITATE_MIN = 1.0;
        private const double CAPACITATE_MAX = 40.0;

        // ─── Persistenta ───────────────────────────────────────────────────────
        private readonly AdministrareCamioaneFisierText adminCamioane = new();
        private readonly AdministrareSoferiFisierText adminSoferi = new();
        private readonly AdministrareCurseMemorie adminCurse = new();

        // ─── ObservableCollection-uri legate la DataGrid-uri ───────────────────
        // ObservableCollection notifica automat DataGrid-ul la Add/Remove,
        // fara a fi nevoie de refresh manual al ItemsSource.
        private readonly ObservableCollection<Camion> colectieCamioane = new();
        private readonly ObservableCollection<Sofer> colectieSoferi = new();
        private readonly ObservableCollection<Cursa> colectieCurse = new();

        // ─── Referinta la entitatea selectata curent ───────────────────────────
        private Camion? camionCurent;
        private Sofer? soferCurent;
        private Cursa? cursaCurenta;

        public MainWindow()
        {
            InitializeComponent();
            InitControale();
            IncarcaDate();
        }

        // ──────────────────────────────────────────────────────────────────────
        //  INITIALIZARE
        // ──────────────────────────────────────────────────────────────────────

        private void InitControale()
        {
            // ── DataGrid-uri legate la ObservableCollection ──
            dgCamioane.ItemsSource = colectieCamioane;
            dgSoferi.ItemsSource = colectieSoferi;
            dgCurse.ItemsSource = colectieCurse;

            // ── ListBox TipContract (adaugare) ──
            lstTipContract.ItemsSource = Camion.ValoriTipContract;
            lstTipContract.SelectedIndex = 0;

            // ── ComboBox-uri din panoul Modificare ──
            // ItemsSource-ul enum-ului si al listei predefinite e setat aici;
            // SelectedItem este legat via TwoWay Binding in XAML.
            cmbModTipContract.ItemsSource = Camion.ValoriTipContract;
            cmbModStatus.ItemsSource = Enum.GetValues(typeof(StatusCamion));

            // ── ComboBox Status din panoul Soferi ──
            cmbSoferStatus.ItemsSource = Enum.GetValues(typeof(StatusSofer));
            cmbSoferStatus.SelectedIndex = 0;

            // ── DatePicker implicit ──
            dpDataInregistrarii.SelectedDate = DateTime.Today;
        }

        private void IncarcaDate()
        {
            try
            {
                foreach (var c in adminCamioane.GetCamioane()) colectieCamioane.Add(c);
                foreach (var s in adminSoferi.GetSoferi()) colectieSoferi.Add(s);
            }
            catch { /* fisiere noi, goale */ }
        }

        // ──────────────────────────────────────────────────────────────────────
        //  NAVIGARE MENIU
        // ──────────────────────────────────────────────────────────────────────

        private void AfiseazaPanel(Grid panel)
        {
            paneluAdaugare.Visibility = Visibility.Collapsed;
            panelModificare.Visibility = Visibility.Collapsed;
            panelSoferi.Visibility = Visibility.Collapsed;
            panelCurse.Visibility = Visibility.Collapsed;
            panel.Visibility = Visibility.Visible;
        }

        private void MenuItemAdaugare_Click(object sender, RoutedEventArgs e)
            => AfiseazaPanel(paneluAdaugare);

        private void MenuItemModifica_Click(object sender, RoutedEventArgs e)
        {
            // Reîncarcă ComboBox-ul cu lista actualizată
            cmbCamioane.ItemsSource = null;
            cmbCamioane.ItemsSource = colectieCamioane;
            cmbCamioane.SelectedIndex = -1;
            borderDetaliiMod.DataContext = null;
            lblMesajMod.Text = string.Empty;
            AfiseazaPanel(panelModificare);
        }

        private void MenuItemSoferi_Click(object sender, RoutedEventArgs e)
        {
            CuratareFormSofer();
            AfiseazaPanel(panelSoferi);
        }

        private void MenuItemCurse_Click(object sender, RoutedEventArgs e)
        {
            // Reîncarcă ComboBox-urile cu datele actuale
            cmbCursaCamion.ItemsSource = null;
            cmbCursaCamion.ItemsSource = colectieCamioane;
            cmbCursaSofer.ItemsSource = null;
            cmbCursaSofer.ItemsSource = colectieSoferi;
            CuratareFormCursa();
            AfiseazaPanel(panelCurse);
        }

        // ──────────────────────────────────────────────────────────────────────
        //  PANEL 1 — ADĂUGARE CAMION
        // ──────────────────────────────────────────────────────────────────────

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            ResetareCuloriCamion();
            lblMesaj.Text = string.Empty;

            string numar = txtNumar.Text.Trim();
            string marca = txtMarca.Text.Trim();
            bool valid = true;
            string erori = string.Empty;

            if (numar.Length < LUNGIME_MIN_NR)
            {
                valid = false; erori += $"Nr. înmatriculare — min. {LUNGIME_MIN_NR} caractere!\n";
                lblNumar.Foreground = Brushes.Red;
            }
            if (marca.Length < LUNGIME_MIN_MARCA)
            {
                valid = false; erori += $"Marca — min. {LUNGIME_MIN_MARCA} caractere!\n";
                lblMarca.Foreground = Brushes.Red;
            }
            if (!double.TryParse(txtCapacitate.Text.Trim(), out double cap)
                || cap < CAPACITATE_MIN || cap > CAPACITATE_MAX)
            {
                valid = false; erori += $"Capacitate invalida ({CAPACITATE_MIN}-{CAPACITATE_MAX})!\n";
                lblCapacitate.Foreground = Brushes.Red;
            }

            if (!valid)
            {
                lblMesaj.Foreground = Brushes.Red;
                lblMesaj.Text = erori.TrimEnd('\n');
                return;
            }

            Camion c = new(numar, marca, cap)
            {
                DataInregistrarii = dpDataInregistrarii.SelectedDate ?? DateTime.Today,
                DataActualizarii = DateTime.Now,
                TipContract = lstTipContract.SelectedItem?.ToString() ?? Camion.ValoriTipContract[0]
            };

            if (rbInCursa.IsChecked == true) c.StatusCurent = StatusCamion.InCursa;
            else if (rbInService.IsChecked == true) c.StatusCurent = StatusCamion.InService;

            DotariCamion d = DotariCamion.Niciuna;
            if (chkAer.IsChecked == true) d |= DotariCamion.AerConditionat;
            if (chkNavigatie.IsChecked == true) d |= DotariCamion.Navigatie;
            if (chkFrig.IsChecked == true) d |= DotariCamion.SistemFrig;
            if (chkDormitor.IsChecked == true) d |= DotariCamion.Dormitor;
            c.Dotari = d;

            // Adaugare in ObservableCollection → DataGrid-ul se actualizeaza automat
            colectieCamioane.Add(c);
            adminCamioane.AddCamion(c);

            lblMesaj.Foreground = Brushes.Green;
            lblMesaj.Text = $"Camion '{numar}' adăugat cu succes!";
            CuratareFormCamion();
        }

        // ──────────────────────────────────────────────────────────────────────
        //  PANEL 2 — MODIFICARE CAMION cu TwoWay Binding
        // ──────────────────────────────────────────────────────────────────────

        private void CmbCamioane_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            camionCurent = cmbCamioane.SelectedItem as Camion;
            if (camionCurent == null) return;

            // Setarea DataContext-ului pe Border-ul de editare.
            // Toate binding-urile din interior (Marca, TipContract, StatusCurent,
            // DataInregistrarii) vor folosi automat acest obiect ca sursa.
            borderDetaliiMod.DataContext = camionCurent;

            // Dotarile (Flags enum) se gestioneaza manual deoarece
            // binding-ul WPF nativ nu suporta selectie multipla pe Flags.
            lstModDotari.SelectedItems.Clear();
            foreach (ListBoxItem item in lstModDotari.Items)
            {
                bool sel = item.Tag?.ToString() switch
                {
                    "AerConditionat" => camionCurent.Dotari.HasFlag(DotariCamion.AerConditionat),
                    "Navigatie" => camionCurent.Dotari.HasFlag(DotariCamion.Navigatie),
                    "SistemFrig" => camionCurent.Dotari.HasFlag(DotariCamion.SistemFrig),
                    "Dormitor" => camionCurent.Dotari.HasFlag(DotariCamion.Dormitor),
                    _ => false
                };
                if (sel) lstModDotari.SelectedItems.Add(item);
            }
            lblMesajMod.Text = string.Empty;
        }

        private void BtnActualizeaza_Click(object sender, RoutedEventArgs e)
        {
            lblMesajMod.Text = string.Empty;
            if (camionCurent == null)
            {
                lblMesajMod.Foreground = Brushes.Red;
                lblMesajMod.Text = "Selectați mai întâi un camion!";
                return;
            }

            // Validare câmpuri legate prin TwoWay binding
            // (valorile din TextBox au fost deja scrise în obiect via binding)
            if (camionCurent.Marca.Length < LUNGIME_MIN_MARCA)
            {
                lblMesajMod.Foreground = Brushes.Red;
                lblMesajMod.Text = $"Marca trebuie să aibă min. {LUNGIME_MIN_MARCA} caractere!";
                return;
            }
            if (camionCurent.CapacitateTone < CAPACITATE_MIN || camionCurent.CapacitateTone > CAPACITATE_MAX)
            {
                lblMesajMod.Foreground = Brushes.Red;
                lblMesajMod.Text = $"Capacitate invalida ({CAPACITATE_MIN}-{CAPACITATE_MAX})!";
                return;
            }

            // Dotari - gestionat manual (Flags enum)
            DotariCamion dotariNoi = DotariCamion.Niciuna;
            foreach (ListBoxItem item in lstModDotari.SelectedItems)
            {
                dotariNoi |= item.Tag?.ToString() switch
                {
                    "AerConditionat" => DotariCamion.AerConditionat,
                    "Navigatie" => DotariCamion.Navigatie,
                    "SistemFrig" => DotariCamion.SistemFrig,
                    "Dormitor" => DotariCamion.Dormitor,
                    _ => DotariCamion.Niciuna
                };
            }
            camionCurent.Dotari = dotariNoi;

            // DataActualizarii se seteaza la data curenta la fiecare salvare
            camionCurent.DataActualizarii = DateTime.Now;

            // Salveaza in fisier
            adminCamioane.UpdateCamion(camionCurent);

            // DataGrid-ul se actualizeaza automat (INotifyPropertyChanged pe Camion)
            lblMesajMod.Foreground = Brushes.Green;
            lblMesajMod.Text = $"Camion '{camionCurent.NumarInmatriculare}' actualizat!";
        }

        // ──────────────────────────────────────────────────────────────────────
        //  PANEL 3 — CRUD ȘOFERI (Temă acasă)
        // ──────────────────────────────────────────────────────────────────────

        private void DgSoferi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            soferCurent = dgSoferi.SelectedItem as Sofer;
            if (soferCurent == null) return;

            // Setarea DataContext-ului pe form permite binding-urile TwoWay:
            //   txtSoferPrenume → {Binding Prenume}
            //   cmbSoferStatus  → {Binding StatusCurent}
            // Câmpurile fara binding raman gestionate manual.
            var formSofer = (StackPanel)((ScrollViewer)((Border)panelSoferi.Children[0]).Child).Content;
            formSofer.DataContext = soferCurent;

            txtSoferId.Text = soferCurent.IdAngajat;
            txtSoferNume.Text = soferCurent.Nume;
            // txtSoferPrenume si cmbSoferStatus — populate via binding

            chkMarfaGen.IsChecked = soferCurent.Atestate.HasFlag(AtestateSofer.MarfaGenerala);
            chkADR.IsChecked = soferCurent.Atestate.HasFlag(AtestateSofer.ADR);
            chkAgabaritic.IsChecked = soferCurent.Atestate.HasFlag(AtestateSofer.Agabaritic);
            chkFrigorific.IsChecked = soferCurent.Atestate.HasFlag(AtestateSofer.Frigorific);

            lblMesajSofer.Text = string.Empty;
        }

        private void BtnAdaugaSofer_Click(object sender, RoutedEventArgs e)
        {
            lblMesajSofer.Text = string.Empty;

            string id = txtSoferId.Text.Trim();
            string nume = txtSoferNume.Text.Trim();
            string pren = txtSoferPrenume.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(nume) || string.IsNullOrEmpty(pren))
            {
                lblMesajSofer.Foreground = Brushes.Red;
                lblSoferNume.Foreground = string.IsNullOrEmpty(nume) ? Brushes.Red : Brushes.DimGray;
                lblSoferPrenume.Foreground = string.IsNullOrEmpty(pren) ? Brushes.Red : Brushes.DimGray;
                lblMesajSofer.Text = "ID, Nume și Prenume sunt obligatorii!";
                return;
            }
            if (colectieSoferi.Any(s => s.IdAngajat == id))
            {
                lblMesajSofer.Foreground = Brushes.Red;
                lblMesajSofer.Text = $"ID-ul '{id}' există deja!";
                return;
            }

            Sofer s = new(nume, pren, id) { StatusCurent = (StatusSofer)(cmbSoferStatus.SelectedItem ?? StatusSofer.Disponibil) };
            s.Atestate = CitesteAtestate();

            colectieSoferi.Add(s);
            adminSoferi.AddSofer(s);

            lblMesajSofer.Foreground = Brushes.Green;
            lblMesajSofer.Text = $"Șofer '{id}' adăugat!";
            CuratareFormSofer();
        }

        private void BtnActualizeazaSofer_Click(object sender, RoutedEventArgs e)
        {
            lblMesajSofer.Text = string.Empty;
            if (soferCurent == null)
            {
                lblMesajSofer.Foreground = Brushes.Red;
                lblMesajSofer.Text = "Selectați un șofer din listă!";
                return;
            }

            // Prenume si StatusCurent sunt deja actualizate prin TwoWay binding.
            // Actualizam Nume si Atestate manual.
            soferCurent.Nume = txtSoferNume.Text.Trim();
            soferCurent.Atestate = CitesteAtestate();

            adminSoferi.UpdateSofer(soferCurent);

            lblMesajSofer.Foreground = Brushes.Green;
            lblMesajSofer.Text = $"Șofer '{soferCurent.IdAngajat}' actualizat!";
        }

        private void BtnStergeSofer_Click(object sender, RoutedEventArgs e)
        {
            if (soferCurent == null)
            {
                lblMesajSofer.Foreground = Brushes.Red;
                lblMesajSofer.Text = "Selectați un șofer din listă!";
                return;
            }
            if (MessageBox.Show($"Ștergeți șoferul '{soferCurent.Nume} {soferCurent.Prenume}'?",
                "Confirmare", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                adminSoferi.StergeSofer(soferCurent.IdAngajat);
                colectieSoferi.Remove(soferCurent);
                soferCurent = null;
                CuratareFormSofer();
                lblMesajSofer.Foreground = Brushes.Green;
                lblMesajSofer.Text = "Șofer șters cu succes.";
            }
        }

        // ──────────────────────────────────────────────────────────────────────
        //  UTILITARE
        // ──────────────────────────────────────────────────────────────────────

        private AtestateSofer CitesteAtestate()
        {
            AtestateSofer a = AtestateSofer.Niciunul;
            if (chkMarfaGen.IsChecked == true) a |= AtestateSofer.MarfaGenerala;
            if (chkADR.IsChecked == true) a |= AtestateSofer.ADR;
            if (chkAgabaritic.IsChecked == true) a |= AtestateSofer.Agabaritic;
            if (chkFrigorific.IsChecked == true) a |= AtestateSofer.Frigorific;
            return a;
        }

        private void ResetareCuloriCamion()
        {
            var c = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#34495E"));
            lblNumar.Foreground = lblMarca.Foreground = lblCapacitate.Foreground = c;
        }

        private void CuratareFormCamion()
        {
            txtNumar.Text = txtMarca.Text = txtCapacitate.Text = string.Empty;
            dpDataInregistrarii.SelectedDate = DateTime.Today;
            lstTipContract.SelectedIndex = 0;
            rbDisponibil.IsChecked = true;
            chkAer.IsChecked = chkNavigatie.IsChecked = chkFrig.IsChecked = chkDormitor.IsChecked = false;
        }

        private void CuratareFormSofer()
        {
            txtSoferId.Text = txtSoferNume.Text = txtSoferPrenume.Text = string.Empty;
            cmbSoferStatus.SelectedIndex = 0;
            chkMarfaGen.IsChecked = chkADR.IsChecked = chkAgabaritic.IsChecked = chkFrigorific.IsChecked = false;
            soferCurent = null;
            // Curata DataContext-ul pentru a desface binding-urile de la soferul precedent
            var formSofer = (StackPanel)((ScrollViewer)((Border)panelSoferi.Children[0]).Child).Content;
            formSofer.DataContext = null;
            dgSoferi.SelectedItem = null;
            lblSoferNume.Foreground = lblSoferPrenume.Foreground =
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#34495E"));
        }

        private void MenuItemIesire_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();

        // ──────────────────────────────────────────────────────────────────────
        //  CĂUTARE — CAMIOANE (după Nr. Înmatriculare)
        // ──────────────────────────────────────────────────────────────────────

        private void TxtCautareNrCamion_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtru = txtCautareNrCamion.Text.Trim();
            if (string.IsNullOrEmpty(filtru))
            {
                dgCamioane.ItemsSource = colectieCamioane;
            }
            else
            {
                var rezultate = colectieCamioane
                    .Where(c => c.NumarInmatriculare.IndexOf(filtru, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                dgCamioane.ItemsSource = rezultate;
            }
        }

        private void BtnResetCautareCamion_Click(object sender, RoutedEventArgs e)
        {
            txtCautareNrCamion.Text = string.Empty;
            dgCamioane.ItemsSource = colectieCamioane;
        }

        // ──────────────────────────────────────────────────────────────────────
        //  CĂUTARE — ȘOFERI (după ID Angajat)
        // ──────────────────────────────────────────────────────────────────────

        private void TxtCautareIdSofer_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtru = txtCautareIdSofer.Text.Trim();
            if (string.IsNullOrEmpty(filtru))
            {
                dgSoferi.ItemsSource = colectieSoferi;
            }
            else
            {
                var rezultate = colectieSoferi
                    .Where(s => s.IdAngajat.IndexOf(filtru, StringComparison.OrdinalIgnoreCase) >= 0
                             || s.Nume.IndexOf(filtru, StringComparison.OrdinalIgnoreCase) >= 0
                             || s.Prenume.IndexOf(filtru, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                dgSoferi.ItemsSource = rezultate;
            }
        }

        private void BtnResetCautareSofer_Click(object sender, RoutedEventArgs e)
        {
            txtCautareIdSofer.Text = string.Empty;
            dgSoferi.ItemsSource = colectieSoferi;
        }

        // ──────────────────────────────────────────────────────────────────────
        //  CĂUTARE — CURSE (după ID Șofer/Client SAU Nr. Înmatriculare Camion)
        // ──────────────────────────────────────────────────────────────────────

        private void TxtCautareCursa_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtruSofer = txtCautareIdSoferCursa.Text.Trim();
            string filtruNr    = txtCautareNrCursa.Text.Trim();

            IEnumerable<Cursa> rezultate = colectieCurse;

            if (!string.IsNullOrEmpty(filtruSofer))
                rezultate = rezultate.Where(c =>
                    c.SoferAlocat.IdAngajat.IndexOf(filtruSofer, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.SoferAlocat.Nume.IndexOf(filtruSofer, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.SoferAlocat.Prenume.IndexOf(filtruSofer, StringComparison.OrdinalIgnoreCase) >= 0);

            if (!string.IsNullOrEmpty(filtruNr))
                rezultate = rezultate.Where(c =>
                    c.CamionAlocat.NumarInmatriculare.IndexOf(filtruNr, StringComparison.OrdinalIgnoreCase) >= 0);

            dgCurse.ItemsSource = rezultate.ToList();
        }

        private void BtnResetCautareCursa_Click(object sender, RoutedEventArgs e)
        {
            txtCautareIdSoferCursa.Text = string.Empty;
            txtCautareNrCursa.Text = string.Empty;
            dgCurse.ItemsSource = colectieCurse;
        }

        // ──────────────────────────────────────────────────────────────────────
        //  PANEL 4 — GESTIONARE CURSE
        // ──────────────────────────────────────────────────────────────────────

        private void DgCurse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cursaCurenta = dgCurse.SelectedItem as Cursa;
            lblMesajCursa.Text = string.Empty;
        }

        private void BtnAdaugaCursa_Click(object sender, RoutedEventArgs e)
        {
            ResetareCuloriCursa();
            lblMesajCursa.Text = string.Empty;

            string plecare = txtCursaPlecare.Text.Trim();
            string dest = txtCursaDestinatie.Text.Trim();
            string eta = txtCursaETA.Text.Trim();
            bool valid = true;
            string erori = string.Empty;

            if (string.IsNullOrEmpty(plecare))
            {
                valid = false; erori += "Punctul de plecare este obligatoriu!\n";
                lblCursaPlecare.Foreground = Brushes.Red;
            }
            if (string.IsNullOrEmpty(dest))
            {
                valid = false; erori += "Destinația este obligatorie!\n";
                lblCursaDestinatie.Foreground = Brushes.Red;
            }
            if (!double.TryParse(txtCursaDistanta.Text.Trim(), out double km) || km <= 0)
            {
                valid = false; erori += "Distanța trebuie să fie un număr pozitiv!\n";
                lblCursaDistanta.Foreground = Brushes.Red;
            }
            if (!double.TryParse(txtCursaPret.Text.Trim(), out double pret) || pret < 0)
            {
                valid = false; erori += "Prețul trebuie să fie un număr pozitiv!\n";
                lblCursaPret.Foreground = Brushes.Red;
            }
            if (cmbCursaCamion.SelectedItem is not Camion camion)
            {
                valid = false; erori += "Selectați un camion!\n";
                lblCursaCamion.Foreground = Brushes.Red;
            }
            if (cmbCursaSofer.SelectedItem is not Sofer sofer)
            {
                valid = false; erori += "Selectați un șofer!\n";
                lblCursaSofer.Foreground = Brushes.Red;
            }

            if (!valid)
            {
                lblMesajCursa.Foreground = Brushes.Red;
                lblMesajCursa.Text = erori.TrimEnd('\n');
                return;
            }

            // Scoatem variabilele din pattern matching ca să le folosim garantat non-null
            var camionSel = (Camion)cmbCursaCamion.SelectedItem;
            var soferSel = (Sofer)cmbCursaSofer.SelectedItem;

            Cursa cursa = new(plecare, dest, km, eta, pret, camionSel, soferSel)
            {
                CerinteMarfa = CitesteCerinteMarfa()
            };

            colectieCurse.Add(cursa);
            adminCurse.AddCursa(cursa);

            lblMesajCursa.Foreground = Brushes.Green;
            lblMesajCursa.Text = $"Cursă '{plecare} → {dest}' adăugată cu succes!";
            CuratareFormCursa();
        }

        private void BtnIncepeCursa_Click(object sender, RoutedEventArgs e)
        {
            if (cursaCurenta == null)
            {
                lblMesajCursa.Foreground = Brushes.Red;
                lblMesajCursa.Text = "Selectați o cursă din listă!";
                return;
            }
            if (cursaCurenta.Status != StatusCursa.InAsteptare)
            {
                lblMesajCursa.Foreground = Brushes.Red;
                lblMesajCursa.Text = "Doar cursele În Așteptare pot fi începute.";
                return;
            }
            cursaCurenta.IncepeCursa();
            // Refresh DataGrid
            dgCurse.Items.Refresh();
            lblMesajCursa.Foreground = Brushes.CornflowerBlue;
            lblMesajCursa.Text = "Cursa a fost marcată ca În Desfășurare.";
        }

        private void BtnFinalizeazaCursa_Click(object sender, RoutedEventArgs e)
        {
            if (cursaCurenta == null)
            {
                lblMesajCursa.Foreground = Brushes.Red;
                lblMesajCursa.Text = "Selectați o cursă din listă!";
                return;
            }
            if (cursaCurenta.Status != StatusCursa.InDesfasurare)
            {
                lblMesajCursa.Foreground = Brushes.Red;
                lblMesajCursa.Text = "Doar cursele În Desfășurare pot fi finalizate.";
                return;
            }
            cursaCurenta.FinalizeazaCursa();
            dgCurse.Items.Refresh();
            lblMesajCursa.Foreground = Brushes.Green;
            lblMesajCursa.Text = "Cursa a fost finalizată cu succes.";
        }

        private void BtnAnuleazaCursa_Click(object sender, RoutedEventArgs e)
        {
            if (cursaCurenta == null)
            {
                lblMesajCursa.Foreground = Brushes.Red;
                lblMesajCursa.Text = "Selectați o cursă din listă!";
                return;
            }
            if (cursaCurenta.Status == StatusCursa.Finalizata || cursaCurenta.Status == StatusCursa.Anulata)
            {
                lblMesajCursa.Foreground = Brushes.Red;
                lblMesajCursa.Text = "Cursa este deja finalizată sau anulată.";
                return;
            }
            if (MessageBox.Show("Anulați această cursă?", "Confirmare",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                cursaCurenta.AnuleazaCursa();
                dgCurse.Items.Refresh();
                lblMesajCursa.Foreground = Brushes.OrangeRed;
                lblMesajCursa.Text = "Cursa a fost anulată.";
            }
        }

        private TipMarfa CitesteCerinteMarfa()
        {
            TipMarfa t = TipMarfa.Standard;
            if (chkCursaFragil.IsChecked == true) t |= TipMarfa.Fragil;
            if (chkCursaADR.IsChecked == true) t |= TipMarfa.PericulosADR;
            if (chkCursaTemp.IsChecked == true) t |= TipMarfa.SensibilTemperatura;
            if (chkCursaAnimale.IsChecked == true) t |= TipMarfa.AnimaleVii;
            return t;
        }

        private void ResetareCuloriCursa()
        {
            var c = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBD5E1"));
            lblCursaPlecare.Foreground = lblCursaDestinatie.Foreground =
                lblCursaDistanta.Foreground = lblCursaPret.Foreground =
                lblCursaCamion.Foreground = lblCursaSofer.Foreground = c;
        }

        private void CuratareFormCursa()
        {
            txtCursaPlecare.Text = txtCursaDestinatie.Text = txtCursaDistanta.Text =
                txtCursaETA.Text = txtCursaPret.Text = string.Empty;
            cmbCursaCamion.SelectedIndex = -1;
            cmbCursaSofer.SelectedIndex = -1;
            chkCursaFragil.IsChecked = chkCursaADR.IsChecked =
                chkCursaTemp.IsChecked = chkCursaAnimale.IsChecked = false;
            cursaCurenta = null;
            dgCurse.SelectedItem = null;
            ResetareCuloriCursa();
        }

        private void MenuItemDespre_Click(object sender, RoutedEventArgs e)
            => MessageBox.Show(
                "Aplicație de Evidență Flotă — Management Camioane & Șoferi\n\n" +
                "Concepte WPF implementate:\n" +
                "• ObservableCollection<T> → DataGrid auto-refresh\n" +
                "• Binding ElementName → afișare număr entități\n" +
                "• TwoWay Binding → TextBox Marcă, ComboBox TipContract,\n" +
                "  ComboBox StatusCurent, DatePicker DataInregistrarii\n" +
                "• INotifyPropertyChanged pe Camion și Sofer\n" +
                "• CRUD complet Șoferi cu Binding pe 2 controale",
                "Despre", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}