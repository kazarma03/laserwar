using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLaserwar
{
    public class VkGroup
    {
        public GroupResponse[] response { get; set; }
    }

    public class GroupResponse
    {
        public int gid { get; set; }
        public string name { get; set; }
        public string screen_name { get; set; }
        public int is_closed { get; set; }
        public string type { get; set; }
        public string photo { get; set; }
        public string photo_medium { get; set; }
        public string photo_big { get; set; }
    }
}
