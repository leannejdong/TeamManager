using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace YourProjectName.SystemTests
{
    [TestFixture]
    public class SystemTest
    {
        private FileManager fileManager;
        private List<TeamDetails> testData;

        [SetUp]
        public void Setup()
        {
            // Initialize the FileManager
            fileManager = new FileManager();

            // Create test data for the system test
            testData = new List<TeamDetails>
            {
                new TeamDetails("Team A", "Apple Pie", "321902929", "apple@gmail.com", 100),
                new TeamDetails("Team B", "Banana Bread", "283928732", "banana@gmail.com", 200)
            };
        }

        [Test]
        public void TestWriteDataToFile()
        {
            // Write test data to a CSV file
            fileManager.WriteDataToFile(testData);

            // Check if the file exists
            bool fileExists = File.Exists(fileManager.FileName);
            Assert.IsTrue(fileExists, "The file should have been created.");
        }

        [Test]
        public void TestReadDataFromFile()
        {
            // Write test data to a CSV file
            fileManager.WriteDataToFile(testData);

            // Read data from the CSV file
            List<TeamDetails> loadedData = fileManager.ReadDataFromFile();

            // Compare loadedData with testData to ensure correctness
            Assert.AreEqual(testData.Count, loadedData.Count);
            for (int i = 0; i < testData.Count; i++)
            {
                Assert.AreEqual(testData[i].TeamName, loadedData[i].TeamName);
                Assert.AreEqual(testData[i].PrimaryContact, loadedData[i].PrimaryContact);
                Assert.AreEqual(testData[i].ContactPhone, loadedData[i].ContactPhone);
                Assert.AreEqual(testData[i].ContactEmail, loadedData[i].ContactEmail);
                Assert.AreEqual(testData[i].CompetitionPoints, loadedData[i].CompetitionPoints);
            }
        }

        [Test]
        public void TestMainWindowTitle()
        {
            // Navigate to the main window URL (assuming a local development server)
            driver.Navigate().GoToUrl("http://localhost:8000");

            // Verify that the main window title matches the expected value
            Assert.AreEqual("My Team Management Application", driver.Title);
        }

        [TearDown]
        public void Teardown()
        {
            // Close the WebDriver instance after each test
            driver.Quit();
        }
    }
}
