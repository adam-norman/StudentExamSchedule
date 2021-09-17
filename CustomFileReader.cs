using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExamsSchedule
{
    public class CustomFileReader
    {
        private readonly string executableLocation;
        private readonly string inputFile;
        private readonly string outputLocation;
        public CustomFileReader()
        {
            executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            inputFile = Path.Combine(executableLocation, "input.csv");
            outputLocation = Path.Combine(executableLocation, "output.csv");
            if (File.Exists(outputLocation))
            {
                File.Delete(outputLocation);
            }
            if (!File.Exists(inputFile))
            {
                throw new FileNotFoundException("the 'input.csv' file does not exist");
            }
        }
        public List<string[]> CsvFileReader()
        {
            List<string[]> dataMatrix = new List<string[]>();
            string[] lines = File.ReadAllLines(inputFile);
            foreach (var line in lines)
            {
                if (String.IsNullOrEmpty(line) == false && String.IsNullOrWhiteSpace(line) == false)
                {
                    dataMatrix.Add(line.Split(','));
                }
            }
            return dataMatrix;
        }
        public string WriteOutputFile(ArrayList slots)
        {
            using StreamWriter file = new StreamWriter(outputLocation, append: true);
            foreach (var slot in slots)
            {
                var currSubject = ((ArrayList)slot).Cast<string>().ToList();
                file.WriteLine(string.Join(',', currSubject));
            }
            return "finished";
        }
    }
}
