using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Datamining
{
    public class City
    {
        public string name;
        public int id;

        public City(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }

    public class Book
    {
        public string title;
        public string author;
        public string cities;

        public Book(string title, string author, string cities)
        {
            this.title = title;
            this.author = author;
            this.cities = cities;
        }
    }

    public class Program
    {

        static List<int> matches = new List<int>();
        static List<City> cities = new List<City>();

        static void Main(string[] args)
        {
            DB();
            ReadTxtFile();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static void DB()
        {
            MySqlConnection sql = SQLDB.GetConnection();
            try
            {
                MySqlCommand cmd = sql.CreateCommand();
                cmd.CommandText = "SELECT * FROM city";
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt16("id");
                    string name = reader.GetString("name");
                    cities.Add(new City(id, name));
                }
                Console.WriteLine(cities.Count);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (sql.State == ConnectionState.Open)
                {
                    sql.Close();
                }
            }
        }

        public static bool TestFunction()
        {
            return true;
        }

        public static void ReadTxtFile()
        {
            // The files used in this example are created in the topic 
            // How to: Write to a Text File. You can change the path and 
            // file name to substitute text files of your own. 
            string sourceDirectory = "C:/Users/Theko/Desktop/books";

            var txtFiles = Directory.EnumerateFiles(sourceDirectory, "*.txt");
            foreach (string currentFile in txtFiles)
            {
                Console.WriteLine(currentFile);
                try
                {
                    StreamReader myFile = new StreamReader(currentFile);
                    string input = myFile.ReadToEnd();
                    myFile.Close();

                    string _title = GetTitle(input);
                    if (_title != "Unknown")
                    {
                        string _author = GetAuthor(input);
                        List<int> _cities = GetCities(input);
                        if (_cities.Count > 0)
                        {
                            _cities = _cities.Distinct().ToList();
                            string _cityIDs = "";
                            foreach (int id in _cities)
                            {
                                _cityIDs += id + ",";
                            }
                            string _book = _title + @"\" + _author + @"\" + _cityIDs;
                            StreamWriter outputFile = new StreamWriter("C:/Users/Theko/Desktop/books.txt", true);
                            outputFile.WriteLine(_book);
                            outputFile.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    StreamWriter outputFile = new StreamWriter("C:/Users/Theko/Desktop/booksLog.txt", true);
                    outputFile.WriteLine(e);
                    outputFile.Close();
                }
            }
        }

        public static string GetTitle(string input)
        {
            string pattern = "(?i)(?<=title:).*";
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return match.Value.Trim();
            }
            else { return "Unknown"; }
        }
        public static string GetAuthor(string input)
        {
            string pattern = "(?i)(?<=author:).*";
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return match.Value.Trim();
            }
            else { return "Unknown"; }
        }
        public static List<int> GetCities(string input)
        {
            string pattern = @"([A-Z][\w-]*(\s+[A-Z][\w-]*)+)";
            matches = new List<int>();
            Console.WriteLine("String length: " + input.Length);
            if (input.Length > 2500000)
            {
                return matches;
            }
            try
            {
                foreach (Match m in Regex.Matches(input, pattern))
                {
                    Task.Run(() => CheckCities(m.Value.Trim()));
                }
                return matches;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return matches;
            }
        }

        static async Task<bool> CheckCities(string name)
        {
            City city = cities.Find(c => c.name == name);
            if (city != null)
            {
                matches.Add(city.id);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }


        static void WriteToCSV(string s)
        {
            // Compose a string that consists of three lines.
            string lines = "";

            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.csv", true);
            file.WriteLine(lines);

            file.Close();
        }
    }
}
