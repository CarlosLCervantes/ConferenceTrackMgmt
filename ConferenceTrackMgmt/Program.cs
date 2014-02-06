using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceTrackMgmt
{
    class Program
    {
        static void Main(string[] args)
        {
            //Future Enchancement: Load file from local directory exe: \\myfile.txt
            Scheduler scheduler = new Scheduler(@"c:\users\s\documents\visual studio 2013\Projects\ConferenceTrackMgmt\ConferenceTrackMgmt\SupportFiles\input.txt");
            string output = scheduler.Print();
            Console.Write(output);
        }

    }
}
