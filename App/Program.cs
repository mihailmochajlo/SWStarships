using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace App
{
    class Program
    {
        private static ILogger logger = new FileLogger ("app.log");
        private static string apiSource = "http://swapi.co/api/starships/";

        static void Main(string[] args)
        {
            try
            {
                logger.Log (LogLevel.Information, "Application has been started");

                ApiStarshipsProcessor processor = new ApiStarshipsProcessor(apiSource, logger);

                Thread dataLoadThread = new Thread (processor.GetData);
                dataLoadThread.Start ();

                Console.WriteLine ("Hello! Please, input distance as unsigned integer number to start.");
                Console.WriteLine ("To exit enter any other string.\n");

                while (true)
                {
                    uint distance = 0;

                    Console.Write ("Distance: ");
                    string input = Console.ReadLine ();

                    if (!UInt32.TryParse (input, out distance))
                    {
                        Console.WriteLine ("Good buy!");
                        dataLoadThread.Interrupt ();
                        break;
                    }
                    else
                    {
                        dataLoadThread.Join ();
                        Console.WriteLine ("-----------------------------");
                        List<string> results = processor.Process (distance);
                        foreach (string result in results)
                        {
                            Console.WriteLine (result);
                        }
                        Console.WriteLine ("-----------------------------\n");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine ("An error observed during execution. Please find details in log file.");
                logger.Log (LogLevel.Information, "Application has been terminated");
            }
        }
    }
}
