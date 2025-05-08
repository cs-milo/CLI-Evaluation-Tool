// Author: Milo Perry
// Assignment: Version Two - Test Grader CLI
// Date: April 2025
// Description: CLI app for managing students and test records

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace TestGrader
{
    public interface ITestPaper
    {
        string Subject { get; set; }
        string[] MarkScheme { get; set; }
        string PassMark { get; set; }
    }

    public interface IStudent
    {
        string Name { get; set; }
        Dictionary<string, string> TestResults { get; set; }
        void TakeTest(TestPaper test, string[] answers);
    }

    public class TestPaper : ITestPaper
    {
        public string Subject { get; set; }
        public string[] MarkScheme { get; set; }
        public string PassMark { get; set; }

        public override string ToString()
        {
            return $"Subject: {Subject}, PassMark: {PassMark}, MarkScheme: {string.Join(", ", MarkScheme)}";
        }
    }

    public class Student : IStudent
    {
        public string Name { get; set; }
        public Dictionary<string, string> TestResults { get; set; } = new();

        public void TakeTest(TestPaper paper, string[] answers)
        {
            int correct = paper.MarkScheme.Zip(answers, (a, b) => a == b ? 1 : 0).Sum();
            double percentage = (double)correct / paper.MarkScheme.Length * 100;
            int required = int.Parse(paper.PassMark.TrimEnd('%'));
            string result = percentage >= required ? "Passed!" : "Failed!";
            TestResults[paper.Subject] = result;
        }

        public override string ToString()
        {
            string results = string.Join("; ", TestResults.Select(kv => $"{kv.Key}: {kv.Value}"));
            return $"Name: {Name}, Results: [{results}]";
        }
    }

    public static class DataHandler
    {
        private const string TestFile = "tests.json";
        private const string StudentFile = "students.json";

        public static void SaveTests(List<TestPaper> tests)
        {
            File.WriteAllText(TestFile, JsonConvert.SerializeObject(tests, Formatting.Indented));
        }

        public static List<TestPaper> LoadTests()
        {
            return File.Exists(TestFile) ? JsonConvert.DeserializeObject<List<TestPaper>>(File.ReadAllText(TestFile)) : new();
        }

        public static void SaveStudents(List<Student> students)
        {
            File.WriteAllText(StudentFile, JsonConvert.SerializeObject(students, Formatting.Indented));
        }

        public static List<Student> LoadStudents()
        {
            return File.Exists(StudentFile) ? JsonConvert.DeserializeObject<List<Student>>(File.ReadAllText(StudentFile)) : new();
        }
    }

    public static class Menu
    {
        private static List<TestPaper> tests = DataHandler.LoadTests();
        private static List<Student> students = DataHandler.LoadStudents();

        public static void Show()
        {
            while (true)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.WriteLine("1. Add Test\n2. View Tests\n3. Delete Test\n4. Add Student\n5. View Students\n6. Delete Student");
                Console.WriteLine("7. Give Test\n8. View Scores\n9. Give All Tests\n10. Student Takes All Tests\n0. Exit");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddTest(); break;
                    case "2": ViewTests(); break;
                    case "3": DeleteTest(); break;
                    case "4": AddStudent(); break;
                    case "5": ViewStudents(); break;
                    case "6": DeleteStudent(); break;
                    case "7": GiveTest(); break;
                    case "8": ViewScores(); break;
                    case "9": GiveAllTests(); break;
                    case "10": StudentTakesAll(); break;
                    case "0":
                        DataHandler.SaveTests(tests);
                        DataHandler.SaveStudents(students);
                        return;
                }
            }
        }

        private static void AddTest()
        {
            Console.Write("Subject: ");
            string subject = Console.ReadLine();
            Console.Write("Pass Mark (e.g. 60%): ");
            string passMark = Console.ReadLine();
            Console.Write("Mark Scheme (comma-separated): ");
            string[] scheme = Console.ReadLine().Split(',');
            tests.Add(new TestPaper { Subject = subject, PassMark = passMark, MarkScheme = scheme });
        }

        private static void ViewTests()
        {
            tests.ForEach(t => Console.WriteLine(t));
        }

        private static void DeleteTest()
        {
            Console.Write("Subject to delete: ");
            string subject = Console.ReadLine();
            tests.RemoveAll(t => t.Subject == subject);
        }

        private static void AddStudent()
        {
            Console.Write("Student Name: ");
            string name = Console.ReadLine();
            students.Add(new Student { Name = name });
        }

        private static void ViewStudents()
        {
            students.ForEach(s => Console.WriteLine(s));
        }

        private static void DeleteStudent()
        {
            Console.Write("Student Name to delete: ");
            string name = Console.ReadLine();
            students.RemoveAll(s => s.Name == name);
        }

        private static void GiveTest()
        {
            Console.Write("Student Name: ");
            string name = Console.ReadLine();
            Student student = students.FirstOrDefault(s => s.Name == name);
            Console.Write("Test Subject: ");
            string subject = Console.ReadLine();
            TestPaper test = tests.FirstOrDefault(t => t.Subject == subject);
            if (student != null && test != null)
            {
                Console.Write("Answers (comma-separated): ");
                string[] answers = Console.ReadLine().Split(',');
                student.TakeTest(test, answers);
            }
        }

        private static void ViewScores()
        {
            students.ForEach(s => Console.WriteLine(s));
        }

        private static void GiveAllTests()
        {
            Console.Write("Student Name: ");
            string name = Console.ReadLine();
            Student student = students.FirstOrDefault(s => s.Name == name);
            if (student != null)
            {
                foreach (var test in tests)
                {
                    Console.Write($"Answers for {test.Subject}: ");
                    string[] answers = Console.ReadLine().Split(',');
                    student.TakeTest(test, answers);
                }
            }
        }

        private static void StudentTakesAll()
        {
            foreach (var student in students)
            {
                Console.WriteLine($"\nStudent: {student.Name}");
                foreach (var test in tests)
                {
                    Console.Write($"Answers for {test.Subject}: ");
                    string[] answers = Console.ReadLine().Split(',');
                    student.TakeTest(test, answers);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Menu.Show();
        }
    }
}
