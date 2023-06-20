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

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
            // declare an empty list to store the devices as ObservableCollection - it is a specialized collection that provides notification
            // when items are added, removed, or modified
            DeviceSet = new ObservableCollection<Device>();
            MainWindow.deviceSets = new List<DeviceSet>();
            // display in Main Window in listbox

            DeviceSetListBox.DisplayMemberPath = "FullInfo";
            DeviceSetListBox.SelectedValuePath = "FullInfo";
            DeviceSetListBox.ItemsSource = DeviceSet;

            // get access to Closing event
            Closing += MainWindow_Closing;


        }
        // declare a static property to store the client and List of deviceSet wchich is helpful to get the Final Price
        public static Client NewClient { get; set; }
        public static List<DeviceSet> deviceSets { get; set; }
        // Declare static property to store object of Offer class
        public static Offer newOffer { get; set; }

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
        // implement the event handler method to clear the DeviceSet collection
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.DeviceSet.Clear();
        }

        private void CreatePDF_Click(object sender, RoutedEventArgs e)
        {

            // create PDF
            // creeating filename
            string fileName = $"AcOffer_{NewClient.LastName}{NewClient.FirstName}_{DateTime.Now.ToString("ddMMyyyy")}.pdf";
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                // Create a PDFWriter instance - an object that can write a PDF file, and represents the PDF document itself
                Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 10, 10);
                PdfWriter pdf = PdfWriter.GetInstance(doc, fs);

                // Add meta information to the document
                doc.AddAuthor("HVAC Engineer");
                doc.AddTitle("AC Offer");

                // Open the document to enable to write to the document
                doc.Open();

                // Add logo image
                // Create a instance of the image class
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("images/logo.jpg");
                // Scale Image
                logo.ScalePercent(15);
                // Set image position
                logo.SetAbsolutePosition(doc.PageSize.Width - logo.ScaledWidth - 10, doc.PageSize.Height - logo.ScaledHeight - 10);
                doc.Add(logo);

                // Write a content
                // to enable exact positioning
                PdfContentByte cb = pdf.DirectContent;
                // Define fonts:
                BaseFont calibri = BaseFont.CreateFont("c:\\windows\\fonts\\Calibri.ttf", BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
                BaseFont calibriBold = BaseFont.CreateFont("c:\\windows\\fonts\\Calibrib.ttf", BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
                Font calibriBoldFont = new Font(calibriBold);

                cb.BeginText();

                // Client information
                
                writeText(cb, "Sz.Pan/Pani: ", 60, 750, calibri, 12,0);
                writeText(cb, $"{NewClient.LastName} {NewClient.FirstName}", 140, 750, calibriBold, 12, 0);
                writeText(cb, $"Email: ", 60, 738, calibri, 12,0);
                writeText(cb, $"{NewClient.Email}", 140, 738, calibriBold, 12, 0);
                writeText(cb, $"Telefon: ", 60, 726, calibri, 12,0);
                writeText(cb, $"{NewClient.Phone}", 140, 726, calibriBold, 12, 0);
                writeText(cb, $"Przygotował:", 60, 714, calibri, 12,0);
                writeText(cb, "Stefan Burczymucha", 140, 714, calibriBold, 12, 0);

                // Title
                writeText(cb, "Oferta wstępna montażu klimatyzacji", 280, 660, calibriBold, 16,1);
                writeText(cb, "Poniżej przedstawiamy naszą ofertę: ", 60, 630, calibri, 12, 0);

                cb.EndText();

                // Add table with 6 column
                PdfPTable table = new PdfPTable(6);
                table.TotalWidth = 575;
                
                //Set the table width to 100% of the page width
                //table.WidthPercentage = 100;
                float[] columnWidths = { 2f, 5f, 3f, 2f, 1f, 2f };
                table.SetWidths(columnWidths);
                
                // Leave a gap before and after the table
                table.SpacingAfter = 20f;
                table.SpacingBefore = 20f;

                //Table headers
                PdfPCell cell1 = new PdfPCell(new Phrase("Producent:", calibriBoldFont));
                cell1.HorizontalAlignment = 1;
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase("Zdjęcie:", calibriBoldFont));
                cell2.HorizontalAlignment = 1;
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("Model i moc", calibriBoldFont));
                cell3.HorizontalAlignment = 1;
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("Cena netto:", calibriBoldFont));
                cell4.HorizontalAlignment = 1;
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase("VAT", calibriBoldFont));
                cell5.HorizontalAlignment = 1;
                table.AddCell(cell5);

                PdfPCell cell6 = new PdfPCell(new Phrase("Cena brutto:", calibriBoldFont));
                cell6.HorizontalAlignment = 1;
                table.AddCell(cell6);

                // Adding device information

                foreach (Device device in DeviceSet)
                {
                    PdfPCell cellProducerName = new PdfPCell(new Phrase($"{device.ProducerName}", calibriBoldFont));
                    cellProducerName.HorizontalAlignment = 1;
                    cellProducerName.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cellProducerName);
                    
                    string path = "images\\"+ device.ImagePath;
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(path);
                    // Calculate scale that image fit cell width
                    float scale = 180 / img.Width * 100 ;
                    img.ScalePercent(scale);
                    PdfPCell imageCell = new PdfPCell(img);
                    imageCell.Padding = 5;
                    table.AddCell(imageCell);

                    string[] fullNameArray = device.ModelFullName.Split('/');
                    // Create a hyperlink 
                    Anchor anchor = new Anchor("Więcej informacji", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.UNDERLINE, BaseColor.BLUE ));
                    anchor.Reference = device.UrlAdress;
                    iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph($"{fullNameArray[0]}/\n{fullNameArray[1]}\n{device.Capacity}\n");
                    paragraph.Add(anchor);
                    PdfPCell cell13 = new PdfPCell(paragraph);
                    cell13.HorizontalAlignment = 1;
                    cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell13);

                    var finalPrice = deviceSets.Where(ds => ds.DeviceId == device.Id).Select(ds => ds.FinalPrice).FirstOrDefault().ToString();
                    PdfPCell cellPrice = new PdfPCell(new Phrase(finalPrice));
                    cellPrice.HorizontalAlignment = 1;
                    cellPrice.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cellPrice);

                    var vat = deviceSets.Where(ds => ds.DeviceId == device.Id).Select(ds => ds.Vat).FirstOrDefault();
                    PdfPCell cellVat = new PdfPCell(new Phrase($"{vat}%"));
                    cellVat.HorizontalAlignment = 1;
                    cellVat .VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cellVat);

                    var grossPrice = deviceSets.Where(ds => ds.DeviceId == device.Id).Select(ds => ds.GrossPrice).FirstOrDefault();
                    PdfPCell cellGross = new PdfPCell(new Phrase($"{grossPrice}"));
                    cellGross.HorizontalAlignment = 1;
                    cellGross .VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cellGross);
                }
                
                // Addding table to the specific position:
                table.WriteSelectedRows(0, -1, 10, 600, cb);
                

                doc.Close();
            }
        }
            
        // Add method to write some text to the content byte    
        private void writeText(PdfContentByte cb, string Text, int X, int Y, BaseFont font, int Size, int align)
        {
            cb.SetFontAndSize(font, Size);
            cb.ShowTextAligned(align, Text, X, Y, 0);

        }   

    }
}
