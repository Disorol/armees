using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public class ArmNode
    {
        public List<ArmNode> Subndes { get; set; }
        public string Title { get; set; }

    }
}
