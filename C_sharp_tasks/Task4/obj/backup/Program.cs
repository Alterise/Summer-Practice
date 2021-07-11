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

       static void assignTickets(List<StudentInfo> studentsInfo, List<string> tickets, string directoryPath)
        {
            Random rd = new Random();
            foreach (var student in studentsInfo)
            {
                File.Copy($"{tickets[rd.Next(0, tickets.Count)]}",
                    $"{directoryPath}\\{student.group}_{student.surname}{student.surname.Substring(0, 1)}" +
                    $"{student.patronymic.Substring(0, 1)}.pdf");
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
                
                assignTickets(studentsInfo, examinationTickets, args[2]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}