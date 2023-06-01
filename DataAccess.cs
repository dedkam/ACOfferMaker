using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ACOfferMaker
{
    // Creating class that control DataAccess
    internal class DataAccess
    {
        
        // before need to add Reference and install Dapper via Nugat
        public List<Client> GetClients(string lastName)
        {
            // using statement make sure that close connection after
            // creating new connection to SQL Database 
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.ConnectionStringValue("ACOfferMakerDB")))
            {
                // creating a query that return List of type Client
                var result = connection.Query<Client>($"select * from Client where LastName = '{lastName}'").ToList();
                return result;
            }
        }
    }
}
