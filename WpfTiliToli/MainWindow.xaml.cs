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
        private int palyaMeret = 3;
        private int[,] palya;
        private Button[,] gombok;
        private int lepesekSzama = 0;
        public MainWindow()
        {
            InitializeComponent();
            palya = new int[palyaMeret, palyaMeret];
            gombok = new Button[palyaMeret, palyaMeret];
            gridMezo.RowDefinitions.Clear();
            gridMezo.ColumnDefinitions.Clear();
            for (int i = 0; i < palyaMeret; i++)
            {
                gridMezo.RowDefinitions.Add(new RowDefinition());
                gridMezo.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int sor = 0; sor < palyaMeret; sor++)
            {
                for (int oszlop = 0; oszlop < palyaMeret; oszlop++)
                {
                    palya[sor, oszlop] = sor * palyaMeret + oszlop + 1;
                }
            }
            palya[palyaMeret - 1, palyaMeret - 1] = 0;

            for (int sor = 0; sor < palyaMeret; sor++)
            {
                for (int oszlop = 0; oszlop < palyaMeret; oszlop++)
                {
                    gombok[sor, oszlop] = new Button();
                    gombok[sor, oszlop].Content = palya[sor, oszlop].ToString();
                    gombok[sor, oszlop].FontSize = 24;
                    gombok[sor, oszlop].HorizontalAlignment = HorizontalAlignment.Stretch;
                    gombok[sor, oszlop].VerticalAlignment = VerticalAlignment.Stretch;
                    gombok[sor, oszlop].Margin = new Thickness(1);
                    gombok[sor, oszlop].Click += Button_Click;
                    Grid.SetRow(gombok[sor, oszlop], sor);
                    Grid.SetColumn(gombok[sor, oszlop], oszlop);
                    gridMezo.Children.Add(gombok[sor, oszlop]);
                }
            }
            GombokKeverese();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int clickedRow = Grid.GetRow(clickedButton);
            int clickedCol = Grid.GetColumn(clickedButton);

            int emptyRow = -1;
            int emptyCol = -1;
            for (int i = 0; i < palyaMeret; i++)
            {
                for (int j = 0; j < palyaMeret; j++)
                {
                    if (palya[i, j] == 0)
                    {
                        emptyRow = i;
                        emptyCol = j;
                        break;
                    }
                }
            }

            if (Math.Abs(clickedRow - emptyRow) <= 1 && Math.Abs(clickedCol - emptyCol) <= 1)
            {
                int temp = palya[clickedRow, clickedCol];
                palya[clickedRow, clickedCol] = palya[emptyRow, emptyCol];
                palya[emptyRow, emptyCol] = temp;

                gombok[clickedRow, clickedCol].Content = palya[clickedRow, clickedCol].ToString();
                gombok[emptyRow, emptyCol].Content = palya[emptyRow, emptyCol].ToString();

                lepesekSzama++;
                lblLepesekSzama.Content = lepesekSzama;

                if (MegoldottE())
                {
                    MessageBox.Show("Nyertél!");
                }
            }
        }

        private bool MegoldottE()
        {
            for (int sor = 0; sor < palyaMeret; sor++)
            {
                for (int oszlop = 0; oszlop < palyaMeret; oszlop++)
                {
                    if (palya[sor, oszlop] != sor * palyaMeret + oszlop + 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void GombokKeverese()
        {

            List<Button> gombLista = new List<Button>();
            for (int i = 0; i < palyaMeret; i++)
            {
                for (int j = 0; j < palyaMeret; j++)
                {
                    gombLista.Add(gombok[i, j]);
                }
            }

            Random random = new Random();
            int gombokSzama = gombLista.Count;
            while (gombokSzama > 1)
            {
                gombokSzama--;
                int k = random.Next(gombokSzama + 1);
                Button value = gombLista[k];
                gombLista[k] = gombLista[gombokSzama];
                gombLista[gombokSzama] = value;
            }

            int index = 0;
            for (int i = 0; i < palyaMeret; i++)
            {
                for (int j = 0; j < palyaMeret; j++)
                {
                    Grid.SetRow(gombLista[index], i);
                    Grid.SetColumn(gombLista[index], j);
                    index++;
                }
            }
        }

        private void btnUjJatek_Click(object sender, RoutedEventArgs e)
        {
            GombokKeverese();
            lblLepesekSzama.Content = 0;
        }
    }
}