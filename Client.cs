using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACOfferMaker
{
    // To add some additional properties to class that was created by LINQTOSQL we need use PARTIAL in class definition
    public partial class Client
    {
        //add extra properties to show more info in listbox
        public string FullInfo 
        {
            get
            {
                return $"{FirstName} {LastName} ({Email})";
            }
        }

        //add property to store a query result
        public List<Client> ClientQueryResult { get; set; }
    }
}
