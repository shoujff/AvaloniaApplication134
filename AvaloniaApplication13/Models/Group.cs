using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication13.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public bool IsSelected { get; set; }
    }
}
