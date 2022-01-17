using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Films.Entities
{
    public class ProducerInterval
    {
        public string producer { get; set; }
        public int interval { get; set; }
        public int previousWin { get; set; }
        public int followingWin { get; set; }
         
    }
}
