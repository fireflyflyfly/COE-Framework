using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using NUnit.Framework;

/// <summary>
/// Utilites, tools and helpers package for Core of Testing automation
/// </summary>
namespace COE.Core.Helpers
{
    /// <summary>
    /// Class for reading Test data from CSV file
    /// </summary>
    public static class CsvSource
    {
        /// <summary>
        ///   Read CSV file and return array of TestCaseData objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">
        ///   The file path to CSV file.
        /// </param>
        /// <param name="expectedResultType">
        ///   Expected type of the result.
        /// </param>
        /// <param name="expectedResultName">
        ///   Expected name of the result.
        /// </param>
        /// <returns></returns>
        public static TestCaseData[] Get<T>(string filePath, Type expectedResultType = null, string expectedResultName = "ExpectedResult")
        {
            bool res = Path.IsPathRooted(filePath);

            string completeFilePath = Path.IsPathRooted(filePath)
                ? filePath
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

            var configuration = new CsvHelper.Configuration.Configuration();
            configuration.Delimiter = ",";

            using (StreamReader streamReader = new StreamReader(completeFilePath))
            using (CsvReader csvReader = new CsvReader(streamReader, configuration))
            {
                TestCaseData[] dataItems = csvReader.GetRecords<T>().
                    Select(x => new TestCaseData(x)).
                    ToArray();

                if (expectedResultType != null)
                {
                    // Reset stream reader to beginning.
                    streamReader.BaseStream.Position = 0;

                    // Read the header line.
                    csvReader.Read();

                    object[] expectedResults = GetExpectedResults(csvReader, expectedResultType, expectedResultName).ToArray();
                    for (int i = 0; i < dataItems.Length; i++)
                    {
                        dataItems[i].Returns(expectedResults[i]);
                    }
                }

                return dataItems;
            }
        }

        /// <summary>
        ///   Iteration methods for reading Fields from CSV.
        /// </summary>
        /// <param name="csvReader">
        ///   The CSV reader object.
        /// </param>
        /// <param name="expectedResultType">
        ///   Expected type of the result.
        /// </param>
        /// <param name="expectedResultName">
        ///   Expected name of the result.
        /// </param>
        /// <returns></returns>
        private static IEnumerable<object> GetExpectedResults(CsvReader csvReader, Type expectedResultType, string expectedResultName)
        {
            while (csvReader.Read())
            {
                yield return csvReader.GetField(expectedResultType, expectedResultName);
            }
        }
    }
}
