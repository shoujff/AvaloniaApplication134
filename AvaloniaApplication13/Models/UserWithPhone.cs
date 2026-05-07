using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication13.Models
{
    public class UserWithPhone
    {
        public Contact contact = new Contact();
        public string Number { get; set; }
        public string Name { get; set; }
      
    }
}