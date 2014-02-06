using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConferenceTrackMgmt;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SchedTests
{
    [TestClass]
    public class Main
    {
        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void HandlesBadFileLocation()
        {
            string fileLocation = @"C:\imaderp.txt";
            Scheduler s = new Scheduler(fileLocation);
            Assert.Fail();
        }

        [TestMethod]
        public void PrintsEachLineOnce()
        {
            string fileLocation = @"c:\users\s\documents\visual studio 2013\Projects\ConferenceTrackMgmt\ConferenceTrackMgmt\SupportFiles\input.txt";
            Scheduler scheduler = new Scheduler(fileLocation);
            string output = scheduler.Print();
            foreach(string line in loadFile(fileLocation))
            {
                Assert.IsTrue(output.Contains(line));
            }
        }

        private List<string> loadFile(string fileLocation)
        {
            List<string> lines = new List<string>();
            using (StreamReader r = new StreamReader(fileLocation))
            {
                while (!r.EndOfStream) //Read each line of the file
                {
                    string talkInput = r.ReadLine();
                    Match talkRegexMatch = new Regex("\\d{1,2}").Match(talkInput);
                    if (talkRegexMatch.Success)
                    {
                        lines.Add(talkInput.Substring(0, talkRegexMatch.Index - 1));
                    }
                }
            }
            return lines;
        }




    }
}
