using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamodbExample.Models
{
    public class ContactModel
    {
        public static string Table
        {
            get
            {
                return "ContactTable";
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string GUID { get; set; }
    }
}
