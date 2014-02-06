using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ConferenceTrackMgmt
{
    public class Scheduler
    {
        //Tracks can be from 6-7 hours depending on time available
        private static int MIN_TRACK_DURATION_IN_HOURS = 6;
        private static int MAX_TRACK_DURATION_IN_HOURS = 7;
        /// <summary>
        /// Internal list of talks which have been parsed.
        /// </summary>
        private List<Talk> _talks = new List<Talk>();
        /// <summary>
        /// Internal list of tracks which are complied against the list of talks.
        /// </summary>
        private List<List<Talk>> _tracks = new List<List<Talk>>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fileLocation">Absolute location of file in the file system</param>
        public Scheduler(string fileLocation)
        {
            loadFile(fileLocation);
            createTracks();
        }

        /// <summary>
        /// Loads a file and parses it into the internal talk list.
        /// </summary>
        /// <param name="fileLocation">Absolute location of file in the file system</param>
        private void loadFile(string fileLocation)
        {
            using(StreamReader r = new StreamReader(fileLocation))
            {
                while(!r.EndOfStream) //Read each line of the file
                {
                    //Parse the title using regex. Per Reqs each line of file is a talk. 
                    //Line will have numbers are the end noing time in minutes. If no time, then assume 5 minutes.
                    string talkInput = r.ReadLine();
                    Talk t = new Talk(talkInput);
                    Match talkRegexMatch = new Regex("\\d{1,2}").Match(talkInput);
                    if(talkRegexMatch.Success)
                    {
                        t.Name = talkInput.Substring(0, talkRegexMatch.Index - 1);
                        t.Duration = Int32.Parse(talkRegexMatch.Value);
                    }
                    _talks.Add(t);//Create list of Talks
 
                }
            }
        }

        /// <summary>
        /// Creates tracks based on internal list of talks.
        /// </summary>
        private void createTracks()
        {
            //Find max number of tracks
            int hoursOfTalks = _talks.Sum(t => t.Duration) / 60;
            int numberOfTracks = hoursOfTalks / MIN_TRACK_DURATION_IN_HOURS;

            //Loop throught creating tracks which will consist of sessions
            List<Talk> unasignedTalks = this._talks.OrderByDescending(t => t.Duration).ToList();
            for(int i = 1; i <= numberOfTracks; i++)
            {
                _tracks.Add(fillTrack(ref unasignedTalks));
            }
        }

        /// <summary>
        /// Fills 2 blocks/sessions of hours with talks and includes a 1 hour luch period.
        /// </summary>
        /// <param name="talks">Working list of talks from which to choose from</param>
        /// <returns>List of talks for a track</returns>
        private List<Talk> fillTrack(ref List<Talk> talks)
        {
            Talk _lunchPlaceholder = new Talk("LUNCH", 60);
            List<Talk> morningBlock = fillSession(3, ref talks);
            morningBlock.Add(_lunchPlaceholder);
            List<Talk> eveningBlock = fillSession(4, ref talks); //TODO: uneccesary line
            morningBlock.AddRange(eveningBlock);
            return morningBlock;
        }

        /// <summary>
        /// Fills a variable number of hours with talks as closely as possible.
        /// </summary>
        /// <param name="hours">Number of hours to fill</param>
        /// <param name="talks">List of talks to choose from</param>
        /// <returns>A list of talks whose sum of duration matches the number of hours as closely as possible</returns>
        private List<Talk> fillSession(int hours, ref List<Talk> talks)
        {
            int minutes = hours * 60;
            List<Talk> sessionTalks = new List<Talk>();
            while(minutes > 0 && talks.Count > 0)
            {
                Talk nextTalk = talks.Where(t => t.Duration <= minutes).OrderBy(t => minutes - t.Duration).FirstOrDefault();
                if (nextTalk != null)
                {
                    sessionTalks.Add(nextTalk);
                    talks.Remove(nextTalk);
                    minutes -= nextTalk.Duration;
                }
                else { break; }
            }
            return sessionTalks;
        }

        /// <summary>
        /// Prints out the tracks and their talks.
        /// </summary>
        /// <returns>A string block of tracks and talks</returns>
        public string Print()
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < _tracks.Count; i++)
            {
                DateTime dt = new DateTime(2012, 10, 1, 9, 0, 0);
                output.AppendLine(String.Format("TRACK #{0}", i));
                foreach(Talk t in _tracks[i])
                {
                    output.AppendLine(String.Format("{0: HH:mm tt} @ {1}", t.Name, dt));
                    dt = dt.AddMinutes(t.Duration);
                }
            }
            return output.ToString();
        }


    }
}
