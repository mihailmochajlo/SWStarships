using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace App
{
    public class ApiStarshipsProcessor
    {
        private string nextSourcePath;
        private bool isCollected = false;

        private ILogger logger;

        private List<Starship> starships = new List<Starship> ();
        

        public ApiStarshipsProcessor (string source, ILogger logger)
        {
            this.logger = logger;
            this.nextSourcePath = source;
        }

        public void GetData ()
        {
            try
            {
                while (!isCollected)
                {
                    logger.Log (LogLevel.Information, "trying to collect the data from API source");
                    IDataSource<string> source = new ApiDataSource (nextSourcePath, logger);
                    logger.Log (LogLevel.Information, "<DataSource> object created");
                    string rawData = source.GetData ();
                    logger.Log (LogLevel.Information, "<DataSource> object finished");
                    IParser<List<Starship>> parser = new ApiJsonStarshipsParser (rawData, logger);
                    logger.Log (LogLevel.Information, "<Parser> object created");
                    starships.AddRange (parser.Parse ());
                    logger.Log (LogLevel.Information, "<Parser> object finished");
                    nextSourcePath = parser.GetPropertyAsString ("next");
                    logger.Log (LogLevel.Information, $"all data collected from API source");
                    if (nextSourcePath == "")
                    {
                        isCollected = true;
                    }
                }
            }
            catch (Exception error)
            {
                logger.Log (LogLevel.Error, $"exception has been occurred while trying to collect data: {error.Message}");
                throw error;
            }
        }

        public List<String> Process (uint distance)
        {
            try
            {
                GetData ();
                logger.Log (LogLevel.Information, $"starting calculations for Distance = {distance}");
                return starships.AsParallel ()
                    .Select (starship => {
                        int numberOfStopsInt = starship.CalculateNumberOfStopsForDistance (distance);
                        string numberOfStopsString = numberOfStopsInt < 0 ? "undefined" : numberOfStopsInt.ToString ();
                        return $"{starship.Name}: {numberOfStopsString}";})
                    .ToList<string> ();
            }
            catch (Exception error)
            {
                logger.Log (LogLevel.Error, $"process failed with error: {error.Message}");
                throw error;
            }
        }
        
    }
}