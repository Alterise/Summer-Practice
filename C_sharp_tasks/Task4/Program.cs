using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Task4
{
    class Program
    {
        public struct StudentInfo
        {
            public string group;
            public string name;
            public string surname;
            public string patronymic;
        }

        public struct StudentInfoWrapper
        {
            public StudentInfo student;
            public int surnameIterator;
            public int patronymicIterator;
        }

        static List<StudentInfo> GetStudentsInfo(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new ArgumentException($"No directory with path \"{directoryPath}\"");
            }

            List<StudentInfo> studentsInfo = new();

            if (Directory.GetFiles(directoryPath, "*.txt").Length == 0)
            {
                throw new ArgumentException($"No TXT files in directory \"{directoryPath}\"");
            }

            foreach (var file in Directory.GetFiles(directoryPath, "*.txt"))
            {
                if (new FileInfo(file).Length == 0)
                {
                    throw new ArgumentException($"File \"{file}\" is empty!");
                }

                StreamReader reader = new(file);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fullNameTokens = line.Split();
                    if (fullNameTokens.Length != 3 || fullNameTokens[0] == "" || fullNameTokens[1] == "" || fullNameTokens[2] == "")
                    {
                        throw new ArgumentException($"Wrong format of full name in \"{file}\"!");
                    }


                    studentsInfo.Add(new StudentInfo()
                    {
                        group = Path.GetFileNameWithoutExtension(file),
                        surname = fullNameTokens[0],
                        name = fullNameTokens[1],
                        patronymic = fullNameTokens[2]
                    });
                }
                reader.Dispose();
            }
            return studentsInfo;
        }

        static List<string> GetTickets(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new ArgumentException($"No directory with path \"{directoryPath}\"");
            }

            List<string> tickets;

            if ((tickets = Directory.GetFiles(directoryPath, "*.pdf", SearchOption.AllDirectories).ToList()).Count == 0)
            {
                throw new ArgumentException($"No PDF files in directory \"{directoryPath}\" and it's subdirectories.");
            }

            return tickets;
        }

        static List<string> FullNameExtension(List<StudentInfo> studentsInfo)
        {
            List<StudentInfo> studentsInfoCopy = studentsInfo.ToList();
            Dictionary<string, int> duplicates = new();
            Dictionary<string, List<StudentInfo>> needExtension = new();
            Dictionary<string, int> needExtensionCount = new();
            List<string> noExtensionNeeded = new();
            
            foreach (var student in studentsInfoCopy)
            {
                if (!duplicates.TryAdd($"{student.group}_{student.surname}{student.name}{student.patronymic}", 1))
                {
                    duplicates[$"{student.group}_{student.surname}{student.name}{student.patronymic}"]++;
                }
            }

            List <StudentInfo> studentsToRemove = new();
            
            foreach (var student in studentsInfoCopy)
            {
                if (duplicates[$"{student.group}_{student.surname}{student.name}{student.patronymic}"] == 1)
                {
                    if (!needExtensionCount.TryAdd($"{student.group}_{student.surname}{student.name.Substring(0, 1)}" +
                                                   $"{student.patronymic.Substring(0, 1)}", 1))
                    {
                        needExtensionCount[$"{student.group}_{student.surname}{student.name.Substring(0, 1)}" +
                                           $"{student.patronymic.Substring(0, 1)}"]++;
                    }
                    duplicates.Remove($"{student.group}_{student.surname}{student.name}{student.patronymic}");
                }
                else
                {
                    studentsToRemove.Add(student);
                }
            }

            foreach (var student in studentsToRemove)
            {
                studentsInfoCopy.Remove(student);
            }
            
            studentsToRemove.Clear();
            
            foreach (var student in studentsInfoCopy)
            {
                if (needExtensionCount[$"{student.group}_{student.surname}{student.name.Substring(0, 1)}" +
                                  $"{student.patronymic.Substring(0, 1)}"] == 1)
                {
                    noExtensionNeeded.Add($"{student.group}_{student.surname}{student.name.Substring(0, 1)}" +
                                          $"{student.patronymic.Substring(0, 1)}");
                    needExtension.Remove($"{student.group}_{student.surname}{student.name.Substring(0, 1)}" +
                                         $"{student.patronymic.Substring(0, 1)}");
                    studentsToRemove.Add(student);
                }
                else
                {
                    if (!needExtension.ContainsKey($"{student.group}_{student.surname}{student.name.Substring(0, 1)}" +
                                                  $"{student.patronymic.Substring(0, 1)}"))
                    {
                        needExtension.Add($"{student.group}_{student.surname}{student.name.Substring(0, 1)}" +
                                          $"{student.patronymic.Substring(0, 1)}", new List<StudentInfo>());
                    }
                    needExtension[$"{student.group}_{student.surname}{student.name.Substring(0, 1)}" +
                                  $"{student.patronymic.Substring(0, 1)}"].Add(new StudentInfo()
                    {
                        group = student.group,
                        name = student.name,
                        surname = student.surname,
                        patronymic = student.patronymic
                    });
                }
            }
            
            foreach (var student in studentsToRemove)
            {
                studentsInfoCopy.Remove(student);
            }
            
            studentsToRemove.Clear();

            
            List<string> resultFullNames = new();

            resultFullNames.AddRange(noExtensionNeeded);

            foreach (var (key, count) in duplicates)
            {
                for (int i = 1; i <= count; i++)
                {
                    resultFullNames.Add($"{key}{i}");
                }
            }

            foreach (var students in needExtension.Values)
            {
                for (int i = 1; i <= students.Count; i++)
                {
                    resultFullNames.Add($"{students[i].group}_{students[i].surname}{students[i].name}{students[i].patronymic}");
                }
            }

            return resultFullNames;
        }
        
        static void assignTickets(List<string> studentsInfo, List<string> tickets, string directoryPath)
        {
            Random rd = new Random();
            foreach (var student in studentsInfo)
            {
                File.Copy($"{tickets[rd.Next(0, tickets.Count)]}",
                    $"{directoryPath}\\{student}.pdf");
            }
            
        }
        
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 3)
                {
                    throw new ArgumentException("Must be 3 arguments!");
                }
                
                List<string> examinationTickets = GetTickets(args[0]);
                
                List<StudentInfo> studentsInfo = GetStudentsInfo(args[1]);
                
                assignTickets(FullNameExtension(studentsInfo), examinationTickets, args[2]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}