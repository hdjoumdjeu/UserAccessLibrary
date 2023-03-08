using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccessLibrary
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastConnect { get; set; }
        public DateTime CreateOn { get; set; }
        public Groupe Group { get; set; }
    }
}
