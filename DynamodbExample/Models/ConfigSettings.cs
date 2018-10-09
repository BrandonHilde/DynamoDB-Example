using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamodbExample.Models
{
    public class ConfigSettings
    {
        public class DynamoDBSettings
        {
            public String DynamoSecret { get; set; }
            public String DynamoID { get; set; }
        }
    }
}
