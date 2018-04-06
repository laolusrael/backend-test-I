using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterSearchBot.Types
{
    public class Status
    {
        public DateTime created_at { get; set; }
        public ulong id { get; set; }
        public string id_str { get; set; }
        public string text { get; set; }
        public bool truncated { get; set; }
        public Profile user { get; set; }
    }
    public class Profile
    {
        public ulong id;
        public string id_str;
        public string name;
        public string screen_name;
        public string location;
        public string description;
        public string url;
        public bool @protected;
        public int followers_count;
        public int friends_count;
    }


}
