using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Process("xml");
            Process("json");

            Console.ReadKey();
        }

        private static void Process(string type)
        {
            //creating a list that will contain all deserialized objects
            List<operation> listOfOperations = GetListOfOperationsByType(type);

            //checking if there is at least one valid file in the folder
            if (listOfOperations.Count < 1)
            {
                Console.WriteLine("No operations found");
                return;
            }
            operation maxAmountOperation = GetMaxAmount(listOfOperations);

            Console.WriteLine($"Date: {maxAmountOperation.Date:yyyy-MM-dd}, Debit/Credit: {maxAmountOperation.OperationType}, Amount: {maxAmountOperation.Amount}");
            Console.WriteLine("----------------------------------------------------");
        }

        private static List<operation> GetListOfOperationsByType(string type)
        {
            //creating a search template
            string mask = "*." + type;

            List<operation> listOfOperations = new List<operation>();

            //parsing all files with the .xml extension from the "Operations" folder
            string[] files = Directory.GetFiles("Operations", mask);

            //looping through all the files of "Operations" folder
            foreach (var filePath in files)
            {
                try
                {
                    operation operation;
                    switch (type)
                    {
                        case "xml":
                            operation = DeserializeXml(filePath);
                            break;
                        case "json":
                            operation = DeserializeJson(filePath);
                            break;
                        default:
                            Console.WriteLine("Unknown file type. The 'Operations' Directory can only contain either .xml or .json files");
                            return listOfOperations;
                    }
                    //adding objects to the list
                    listOfOperations.Add(operation);
                }
                catch (Exception)
                {
                    Console.WriteLine($"File {filePath} had been skipped \nbecause its content didn't match required structure.");
                    Console.WriteLine("Please check the 'operation' class for more details.");
                    Console.WriteLine("----------------------------------------------------");
                }
            }
            return listOfOperations;
        }

        private static operation DeserializeXml(string filePath)
        {
            //instantiating an object of XmlSerializer class
            var xmlSerializer = new XmlSerializer(typeof(operation));
            using (var reader = new StreamReader(filePath))
            {
                //deserializing 
                return (operation)xmlSerializer.Deserialize(reader);
            }
        }

        private static operation DeserializeJson(string filePath)
        {
            //instantiating an object of JsonSerializer class
            var jsonSerializer = new JsonSerializer();
            using (var reader = new StreamReader(filePath))
            {
                //deserializing 
                return (operation)jsonSerializer.Deserialize(reader, typeof(operation));
            }
        }

        private static operation GetMaxAmount(List<operation> listOfOperations)
        {
            //assuming the first file contains the maximum value for the "Amount" field
            operation max = listOfOperations.First();

            for (int index = 1; index < listOfOperations.Count; index++)
            {
                //if the next value from the list is greater than the default one then assign the bigger value maxAmountOperation
                if (max.Amount < listOfOperations[index].Amount)
                {
                    max = listOfOperations[index];
                }
            }
            return max;
        }
    }
}
