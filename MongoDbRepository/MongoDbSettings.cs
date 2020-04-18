using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbRepository
{
    public class MongoDbSettings
    {
        public string PaymentsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
