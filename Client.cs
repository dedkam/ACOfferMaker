using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACOfferMaker
{
    // Create class that model our data from Database  table: Client
    internal class Client
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email {get; set; }
        public string PhoneNumber { get; set; }
        // property that return a client as string:  "Kamil Dedio (dedkam@gmail.com)"
        // there is no need to set up this property - only get
        public string FullInfo 
        {
            get
            {
                return $"{FirstName} {LastName} ({Email})";
            }
        }
    }
}
