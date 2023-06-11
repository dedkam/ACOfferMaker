using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
    /// 
    // CONNECTION WITH DATABASE
    // 1. Create data Connection - Server Explorer
    // 2. Create data Source - Data Sources
    // 3. define connectionString and create Data Context
    // 4. Create LinqToSqlDataClass
    // 5. Move table to LinqToSqlDataClass

    
    
    
    public partial class MainWindow : Window
    {
        // After installing LinqToSql and creating special class LinqToSqlDataClasses

        // declera a static property to store the referenece to the main window 
        public static MainWindow MainWindowInstance { get; set; }
        // declare static property to store the devices
        public static ObservableCollection<Device> DeviceSet { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            // seting property to store the reference to this Window
            MainWindowInstance= this;
            // declare an empty list to store the devices
            DeviceSet = new ObservableCollection<Device>();
            // display in Main Window in listbox
            
            DeviceSetListBox.DisplayMemberPath = "FullInfo";
            DeviceSetListBox.SelectedValuePath = "FullInfo";
            DeviceSetListBox.ItemsSource = DeviceSet;


        }
        // declare a static property to store the client 
        public static Client NewClient { get; set; }
        
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            ClientWindow clientWindow = new ClientWindow();
            clientWindow.Show();    
        }

        private void addDeviceButton_Click(object sender, RoutedEventArgs e)
        {
            DeviceWindow deviceWindow = new DeviceWindow();
            deviceWindow.Show();

        }

        
    }
}
