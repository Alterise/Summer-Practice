using System;
using System.Collections.Generic;
using System.IO;

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

        static List<StudentInfo> GetStudentsInfo(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new ArgumentException($"No directory with path \"{directoryPath}\"");
            }

            List<StudentInfo> studentsInfo = new();

            if (Directory.GetFiles(directoryPath, "*.txt").Length == 0)
            {
                throw new ArgumentException($"No files in directory \"{directoryPath}\"");
            }

            foreach (var file in Directory.GetFiles(directoryPath, "*.txt"))
            {
                if (new FileInfo(file).Length == 0)
                {
                    throw new ArgumentException($"File \"{file}\" is empty!");
                }
                
                foreach (var line in File.ReadAllLines(file))
                {
                    var fullNameTokens = line.Split();
                    if (fullNameTokens.Length != 3)
                    {
                        throw new ArgumentException($"Wrong format of full name in \"{file}\"!");
                    }
                    
                    studentsInfo.Add(new StudentInfo(){group = Path.GetFileNameWithoutExtension(file),
                                                           surname = fullNameTokens[0],
                                                           name = fullNameTokens[1],
                                                           patronymic = fullNameTokens[2]});
                    
                }
            }

            return studentsInfo;
        } 
        
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 3)
                {
                    throw new ArgumentException("Must be 3 arguments!");
                }
                
                List<StudentInfo> studentsInfo = GetStudentsInfo(args[1]);

                List<string> examinationTickets;

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}