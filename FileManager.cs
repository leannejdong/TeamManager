using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamManager
{
    internal class FileManager
    {
        string fileName = "TeamData.csv";
        /// <summary>
        /// Takes a provided array and writes its contents to a csv file
        /// in a comma delimited format.
        /// </summary>
        /// <param name="teamData">An array of AddressDetails objects</param>
        public void WriteDataToFile(TeamDetails[] teamData)
        {
            //The using statement generates the class to connect to a file(streamwriter) and
            //uses it as the structure runs. Once the using statement finishes, the resource
            //will be automatically disconnected and destroyed
            using (var writer = new StreamWriter(fileName))
            {
                //Cycyles through each element in the array
                foreach (var team in teamData)
                {
                    //Each property of each entry is printed ona single line with commas to
                    //separate them (Comma Delimmited File or Comma Separated Values(CSV))
                    writer.WriteLine($"{team.TeamName},{team.PrimaryContact},{team.ContactPhone},{team.ContactEmail},{team.CompetitionPoints}");
                }
            }
        }

        public List<TeamDetails> ReadDataFromFile()
        {
            //List to store the data from the file
            List<TeamDetails> teamList = new List<TeamDetails>();
            //The using statement generates the class to connect to a file(streamreader) and
            //uses it as the structure runs. Once the using statement finishes, the resource
            //will be automatically disconnected and destroyed
            using (var reader = new StreamReader(fileName))
            {
                //Variable to store each line as it is processd
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] temp = line.Split(' ');

                    // Check if the array has at least 5 elements before attempting to parse the integer
                    if (temp.Length >= 5)
                    {
                        if (int.TryParse(temp[4], out int competitionPoints))
                        {
                            // Create a new TeamDetails object with the parsed integer
                            TeamDetails details = new TeamDetails(temp[0], temp[1], temp[2], temp[3], competitionPoints);
                            teamList.Add(details);
                        }
                        else
                        {
                            // Handle parsing error (e.g., log, skip line, etc.)
                            Console.WriteLine("Error parsing competition points for line: " + line);
                        }
                    }
                    else
                    {
                        // Handle insufficient data in the line (e.g., log, skip line, etc.)
                        Console.WriteLine("Insufficient data for line: " + line);
                    }
                }
            }
            return teamList;
        }

        internal void WriteDataToFile(List<TeamDetails> teamList)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write header
                writer.WriteLine("TeamName,PrimaryContact,ContactPhone,ContactEmail,CompetitionPoints");

                // Write data for each team
                foreach (TeamDetails team in teamList)
                {
                    writer.WriteLine($"{team.TeamName},{team.PrimaryContact},{team.ContactPhone},{team.ContactEmail},{team.CompetitionPoints}");
                }
            }
        }
    }
}
