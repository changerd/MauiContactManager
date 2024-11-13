using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiContactManager.Models
{
    public class ContactModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumbers { get; set; }

        public string Emails { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
