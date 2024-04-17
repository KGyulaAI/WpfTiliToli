using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTiliToli
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int palyaMeret;
        private int mezokSzama;
        private int lepesekSzama = 1;
        private Button[,] palya;
        public MainWindow()
        {
            InitializeComponent();
            txtPalyaMeret.Text = "3";
            palyaMeret = Convert.ToInt32(txtPalyaMeret.Text);
            mezokSzama = palyaMeret * palyaMeret;
            PalyaGeneralas(palyaMeret);
        }
        private void PalyaGeneralas(int palyaMeret)
        {
            Button mezo;
            palya = new Button[palyaMeret, palyaMeret];
            int randomContent;
            List<int> mezoContents = new List<int>();
            for (int content = 1; content <= mezokSzama - 1; content++)
            {
                mezoContents.Add(content);
            }

            gridMezo.Children.Clear();
            gridMezo.RowDefinitions.Clear();
            gridMezo.ColumnDefinitions.Clear();

            for (int sor = 0; sor < palyaMeret; sor++)
            {
                gridMezo.RowDefinitions.Add(new RowDefinition());
                gridMezo.ColumnDefinitions.Add(new ColumnDefinition());
                for (int oszlop = 0; oszlop < palyaMeret; oszlop++)
                {
                    mezo = new Button();
                    mezo.FontSize = 50;
                    if (mezoContents.Count != 0)
                    {
                        randomContent = new Random().Next(mezoContents.Count);
                        mezo.Content = mezoContents[randomContent];
                        mezoContents.RemoveAt(randomContent);
                    }
                    else
                    {
                        mezo.Content = mezokSzama;
                    }
                    Grid.SetRow(mezo, sor);
                    Grid.SetColumn(mezo, oszlop);
                    if (sor == palyaMeret - 1 && oszlop == palyaMeret - 1)
                    {
                        mezo.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        mezo.Click += Mezo_Click;
                    }
                    gridMezo.Children.Add(mezo);
                    palya[sor, oszlop] = mezo;
                }
            }
        }
        private void Mezo_Click(object sender, RoutedEventArgs e)
        {
            //2 mező cseréje kattintásra
            Button kattintottMezo = (Button)sender;
            int kattintottMezoSora = Grid.GetRow(kattintottMezo);
            int kattintottMezoOszlopa = Grid.GetColumn(kattintottMezo);
            if (kattintottMezoSora > 0 && (int)palya[kattintottMezoSora - 1, kattintottMezoOszlopa].Content == mezokSzama)
            {
                MezoCsere(kattintottMezo, palya[kattintottMezoSora - 1, kattintottMezoOszlopa]);
            }
            else if (kattintottMezoSora < palyaMeret - 1 && (int)palya[kattintottMezoSora + 1, kattintottMezoOszlopa].Content == mezokSzama)
            {
                MezoCsere(kattintottMezo, palya[kattintottMezoSora + 1, kattintottMezoOszlopa]);
            }
            else if (kattintottMezoOszlopa > 0 && (int)palya[kattintottMezoSora, kattintottMezoOszlopa - 1].Content == mezokSzama)
            {
                MezoCsere(kattintottMezo, palya[kattintottMezoSora, kattintottMezoOszlopa - 1]);
            }
            else if (kattintottMezoOszlopa < palyaMeret - 1 && (int)palya[kattintottMezoSora, kattintottMezoOszlopa + 1].Content == mezokSzama)
            {
                MezoCsere(kattintottMezo, palya[kattintottMezoSora, kattintottMezoOszlopa + 1]);
            }

            //Nyerés ellenőrzése
            if (HelyesE())
            {
                MessageBox.Show($"Nyertél! {lepesekSzama - 1} lépéssel oldottad meg.", "Hurrá", MessageBoxButton.OK, MessageBoxImage.Information);
                UjJatek();
            }
        }
        private bool HelyesE()
        {
            int mezoSzam = 1;
            foreach (var mezo in palya)
            {
                if (Convert.ToInt32(mezo.Content) != mezoSzam)
                {
                    return false;
                }
                mezoSzam++;
            }
            return true;
        }
        private void MezoCsere(Button jelenlegiGomb, Button uresGomb)
        {
            int jelenlegiGombSora = Grid.GetRow(jelenlegiGomb);
            int jelenlegiGombOszlopa = Grid.GetColumn(jelenlegiGomb);
            int uresGombSora = Grid.GetRow(uresGomb);
            int uresGombOszlopa = Grid.GetColumn(uresGomb);

            gridMezo.Children.Remove(jelenlegiGomb);
            gridMezo.Children.Remove(uresGomb);
            Grid.SetRow(jelenlegiGomb, uresGombSora);
            Grid.SetColumn(jelenlegiGomb, uresGombOszlopa);
            Grid.SetRow(uresGomb, jelenlegiGombSora);
            Grid.SetColumn(uresGomb, jelenlegiGombOszlopa);
            palya[jelenlegiGombSora, jelenlegiGombOszlopa] = uresGomb;
            palya[uresGombSora, uresGombOszlopa] = jelenlegiGomb;
            gridMezo.Children.Add(jelenlegiGomb);
            gridMezo.Children.Add(uresGomb);

            lblLepesekSzama.Content = lepesekSzama++;
        }
        private void btnUjJatek_Click(object sender, RoutedEventArgs e)
        {
            UjJatek();
        }
        private void UjJatek()
        {
            palyaMeret = Convert.ToInt32(txtPalyaMeret.Text);
            mezokSzama = palyaMeret * palyaMeret;
            lepesekSzama = 0;
            lblLepesekSzama.Content = lepesekSzama++;
            PalyaGeneralas(palyaMeret);
        }
    }
}