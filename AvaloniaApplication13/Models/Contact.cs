using System.Collections.Generic;

namespace AvaloniaApplication13.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Phone { get; set; } = "";
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Group> Groups { get; set; } = new List<Group>();
       

    }
}
