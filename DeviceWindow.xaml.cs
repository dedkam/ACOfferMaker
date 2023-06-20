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
using System.Windows.Shapes;

namespace ACOfferMaker
{
    /// <summary>
    /// Interaction logic for DeviceWindow.xaml
    /// </summary>
    public partial class DeviceWindow : Window
    {
        public Device deviceToPass;
        public IQueryable<Device> modelFullNameList;
        
        
        public DeviceWindow()
        {
            InitializeComponent();
                        
            // populate producer list box
            // conecting to data base
            using (var dataContext = Helper.SetDataBaseConnection())
            {
                var producerList = dataContext.Devices.GroupBy(d => d.ProducerName);
                ProducerListBox.DisplayMemberPath = "ProducerName";
                ProducerListBox.SelectedValuePath = "ProducerName";
                ProducerListBox.ItemsSource = producerList;
            }

            
        }

        private void ProducerListBox_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            using (var dataContext = Helper.SetDataBaseConnection())
            {
                var typeList = dataContext.Devices.Where(d => d.ProducerName == ProducerListBox.SelectedValue.ToString()).GroupBy(d => d.Type);
                TypeListBox.DisplayMemberPath = "Type";
                TypeListBox.SelectedValuePath = "Type";
                TypeListBox.ItemsSource = typeList;
            }
        }

        private void TypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using ( var dataContext = Helper.SetDataBaseConnection())
            {
                var modelList = dataContext.Devices
                    .Where(d => d.Type == TypeListBox.SelectedValue.ToString())
                    .Where(d => d.ProducerName == ProducerListBox.SelectedValue.ToString());
                ModelGroupListBox.DisplayMemberPath = "ModelGroup";
                ModelGroupListBox.SelectedValuePath = "ModelGroup";
                if (modelList.Count() != 0)
                {
                    ModelGroupListBox.ItemsSource = modelList;
                }
                else
                {
                    MessageBox.Show("Error");
                }
                
            }
        }

        private void ModelGroupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var dataContext = Helper.SetDataBaseConnection())
            {
                modelFullNameList = dataContext.Devices
                    .Where(d=> d.ModelGroup == ModelGroupListBox.SelectedValue.ToString())
                    .Where(d => d.Type == TypeListBox.SelectedValue.ToString())
                    .Where(d => d.ProducerName == ProducerListBox.SelectedValue.ToString());
                ModelFullNameListBox.DisplayMemberPath = "ModelFullName";
                ModelFullNameListBox.SelectedValuePath = "Id";
                ModelFullNameListBox.ItemsSource = modelFullNameList;
            }
        }

        private void ModelFullNameListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var dataContext = Helper.SetDataBaseConnection())
            {
                deviceToPass = dataContext.Devices.Where(d => d.Id.ToString() == ModelFullNameListBox.SelectedValue.ToString()).First();
                if (deviceToPass != null)
                {
                    FullModelName.Text = deviceToPass.ModelFullName;
                    CatalougePrice.Text = deviceToPass.Price.ToString();
                }
            }
        }

        
        private void AddToOfferButton_Click(object sender, RoutedEventArgs e)
        {

            

            // establish offer numbers
            // check how many offers were created at this day
            using ( var dataContext = Helper.SetDataBaseConnection())
            {
                // check how many offers were created at this day - want to numerate offers X/ day / month / year
                var numbersOfOffersToday = dataContext.Offers.Where(o => o.CreatedAt.DayOfYear == DateTime.Today.DayOfYear).Count();
                int offerNumber = numbersOfOffersToday + 1;

                // After clicking add to offer have to check if it is first time - need to create new offer, or next time adding next device
                // so check if in the deviceSet list has 0 element - then create new offer, otherwise adding devices to existing offer
                


                if ( MainWindow.DeviceSet.Count() == 0 )
                {
                    // if deviceSetList.Count ==0 create new instance of offer class
                    MainWindow.newOffer = new Offer()
                    {
                        Number = offerNumber,
                        ClientId = MainWindow.NewClient.Id,
                        CreatedAt = DateTime.Now,
                    };
                    // add the new object to the tables collection
                    dataContext.Offers.InsertOnSubmit(MainWindow.newOffer);
                    // sumit changes to Database
                    dataContext.SubmitChanges();
                }

                // inserting device to deviceSetForOffer
                // First checking if FinalPrice has correct value
                // try Parse textbox value to int
                if (!(decimal.TryParse(FinalPriceTextBox.Text, out decimal finalPrice)))
                {
                    MessageBox.Show("Incorrect final price");
                }
                // Checking the VAT value:
                decimal vatValue;
                if(Vat8.IsChecked == true)
                {
                    vatValue = 8;
                }
                else
                {
                    vatValue = 23;
                }
                DeviceSet deviceSet = new DeviceSet()
                {
                    OfferId = MainWindow.newOffer.Id,
                    DeviceId = deviceToPass.Id,
                    FinalPrice = finalPrice,
                    Vat = vatValue,
                    GrossPrice = finalPrice * (1 + vatValue/100)
                };
                // Add deviceSet to the list of DeviceSets (property of MainWindow)
                MainWindow.deviceSets.Add(deviceSet);
                //add device to list
                MainWindow.DeviceSet.Add(deviceToPass);
                
                // add new object to the table collection
                dataContext.DeviceSets.InsertOnSubmit(deviceSet);
                
                // submit changes to database
                dataContext.SubmitChanges();


                // Close the window
                Close();


            }



        }
    }
}
