using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExamsSchedule
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CustomFileReader fileReaderWriter = new CustomFileReader();
                List<string[]> list = fileReaderWriter.CsvFileReader();
                ArrayList subjects = new ArrayList(); // distinct subjects
                ArrayList slots = new ArrayList();// the possible slots i.e. output
                                                  // create a list include each student and his/her subjects
                Hashtable studentSubjects = new Hashtable();
                // create a list include each subject and its students
                Hashtable subjectStudents = new Hashtable();
                foreach (var item in list)
                {
                    ArrayList studentsubjscts = new ArrayList();
                    for (int i = 1; i < item.Length; i++) //0:student identifier,1:the index of the first subject for the student. 
                    {
                        if (string.IsNullOrEmpty(item[i]) == false && string.IsNullOrWhiteSpace(item[i]) == false)
                        {
                            studentsubjscts.Add(item[i]);
                            if (!subjects.Contains(item[i]))
                            {
                                subjects.Add(item[i]);
                                ArrayList students = new ArrayList();
                                students.Add(item[0]);
                                subjectStudents.Add(item[i].ToString(), students.Clone());
                            }
                            else
                            {
                                ArrayList students = (ArrayList)subjectStudents[item[i]];
                                students.Add(item[0]);
                                subjectStudents[item[i].ToString()] = students.Clone();
                            }
                        }
                    }
                    studentSubjects.Add(item[0].ToString(), studentsubjscts);
                }
                ArrayList execludedList = new ArrayList();
                for (int i = 0; i < subjects.Count - 1; i++)
                {
                    List<string> allAgreedStudents = new List<string>();
                    allAgreedStudents.AddRange(((ArrayList)subjectStudents[subjects[i].ToString()]).Cast<string>().ToList());
                    if (!execludedList.Contains(subjects[i]))
                    {
                        execludedList.Add(subjects[i]);
                        ArrayList slot = new ArrayList();
                        slot.Add(subjects[i].ToString());
                        for (int j = i + 1; j < subjects.Count; j++)
                        {
                            if (!execludedList.Contains(subjects[j]))
                            {
                                var nextSubject = ((ArrayList)subjectStudents[subjects[j].ToString()]).Cast<string>().ToList();
                                if (!allAgreedStudents.Intersect(nextSubject).Any())
                                {
                                    allAgreedStudents.AddRange(((ArrayList)subjectStudents[subjects[j].ToString()]).Cast<string>().ToList());
                                    slot.Add(subjects[j].ToString());
                                    execludedList.Add(subjects[j]);
                                }
                            }
                        }
                        slots.Add(slot.Clone());
                    }
                }
                Console.WriteLine(fileReaderWriter.WriteOutputFile(slots));
                Console.ReadLine();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
