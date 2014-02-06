using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceTrackMgmt
{
    public class Talk
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public int? TrackID { get; set; }

        public Talk(string name, int duration = 5)
        {
            this.Name = name;
            this.Duration = duration;
        }
    }
}
