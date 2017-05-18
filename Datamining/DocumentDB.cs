using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Datamining
{
    class DocumentDB : IDatabase
    {
        MongoClient client = null;

        public bool GetConnection()
        {
            if(client == null)
            {
                client = new MongoClient();
            }
            Console.WriteLine((client.ListDatabases()));
            return true;
        }
    }
}
