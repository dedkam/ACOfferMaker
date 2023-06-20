using System;
using System.Collections.Generic;
using System.Data.Linq;
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
using System.Windows.Shapes;

namespace ACOfferMaker
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>

    
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
        }

        // create properties which allowed to pass client to another class - 
        public Client ClientToPass { get; set; }
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dataContext = Helper.SetDataBaseConnection())
            {
                var clients = dataContext.Clients.Where(c => c.LastName == lastNameTextBoxSearch.Text);
                if (clients.Count() != 0)
                {
                    clientFoundListBox.DisplayMemberPath = "FullInfo";
                    //which value should be delivered when listbox element is clicked
                    clientFoundListBox.SelectedValuePath = "Id";
                    clientFoundListBox.ItemsSource = clients;
                }
                else
                {
                    MessageBox.Show("There is no result");
                }
                
            }

            

        }
        
        
        private void clientFoundListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string clientId = clientFoundListBox.SelectedValue.ToString();

            using(var dataContext = Helper.SetDataBaseConnection())
            {
                // create a client with ID which is selected in clientFoudListBox
                ClientToPass = dataContext.Clients.Where(c => c.Id.ToString() == clientId).First();
                lastNameTextBox.Text = ClientToPass.LastName;
                firstNameTextBox.Text = ClientToPass.FirstName;
                emailTextBox.Text = ClientToPass.Email;
                phoneTextBox.Text = ClientToPass.Phone;

            }
            
        }

      
        private void selectedClient_Click(object sender, RoutedEventArgs e)
        {
            
            // check if user select a client from listbox
            if (clientFoundListBox.SelectedItems.Count != 1)
            {
                MessageBox.Show("Please select client");
            }
            else
            {
                // Close Client Window
                Close();
                // Add referenece to exisitng instance of Main Window
                MainWindow.NewClient = ClientToPass;
                MainWindow.MainWindowInstance.firstNameTextBox.Text = ClientToPass.FirstName;
                MainWindow.MainWindowInstance.lastNameTextBox.Text = ClientToPass.LastName;
                MainWindow.MainWindowInstance.emailTextBox.Text = ClientToPass.Email;
                MainWindow.MainWindowInstance.phoneTextBox.Text = ClientToPass.Phone;
                MainWindow.MainWindowInstance.Show();
            }
            
        }
    }
}
