using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccessLibrary
{
    public class Groupe
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public List<Action> Actions { get; set; }
    }
}
