using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ACOfferMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // create an empty list of clients
        List<Client> clients = new List<Client>();
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            // creating new instance of class DataAccess
            DataAccess db = new DataAccess();
            
            // populate empty list with searching result from method GetClients
            clients = db.GetClients(lastNameTextBox.Text);

            clientFoundListBox.ItemsSource = clients;
            clientFoundListBox.DisplayMemberPath = "FullInfo";

        }
    }
}
