﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACOfferMaker
{
    internal static class Helper
    {
        // making a method that provide a connection string for database with name as parameter - this name is defined in App.config
        public static string ConnectionStringValue(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
