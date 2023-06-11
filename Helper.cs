using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACOfferMaker
{
    internal class Helper
    {
        public static LinqToSqlDataClassesDataContext SetDataBaseConnection()
        {
            LinqToSqlDataClassesDataContext dataContext;
            string connectionString = ConfigurationManager.ConnectionStrings["ACOfferMaker.Properties.Settings.ACOfferMakerConnectionString"].ConnectionString;
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);
            return dataContext;
        }
    }
}
