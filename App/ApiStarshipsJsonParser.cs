using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace App
{
    public class ApiJsonStarshipsParser : IParser<List<Starship>>
    {
        private static Dictionary<string, uint> TimeRangesInHours = new Dictionary<string, uint> ()
        {
            {"hour",    1}, {"hours",     1},
            {"day",    24}, {"days",     24},
            {"week",  168}, {"weeks",   168},
            {"month", 730}, {"months",  730},
            {"year", 8760}, {"years",  8760}
        };

        private readonly ILogger logger;

        public string jsonString { get; }
        private string nextSource = new String ("");

        public ApiJsonStarshipsParser (string jsonString, ILogger logger)
        {
            this.jsonString = jsonString;
            this.logger = logger;
        }

        public List<Starship> Parse ()
        {
            List<Starship> result = new List<Starship> ();

            logger.Log (LogLevel.Information, "JSON parsing started");
            try
            {
                using (JsonDocument jsonDocument = JsonDocument.Parse (jsonString))
                {
                    JsonElement rootElementJson = jsonDocument.RootElement;
                    JsonElement.ArrayEnumerator starshipsArrayJson = rootElementJson.GetProperty ("results").EnumerateArray ();
                    result = starshipsArrayJson.AsParallel ()
                        .Select (starshipObjectJson => {
                            return ParseStarshipObject (starshipObjectJson);
                        })
                        .Where (starship => starship.MegalightsPerHour != 0 && starship.Consumables != 0)
                        .ToList ();
                }
            }
            catch (Exception error)
            {
                logger.Log (LogLevel.Warning, $"exception has been occurred while trying to parse data: {error.Message}");
                throw error;
            }
            
            logger.Log (LogLevel.Information, "JSON parsing finished");

            return result;
        }

        private Starship ParseStarshipObject (JsonElement starshipObjectJson)
        {
            string nameString = starshipObjectJson.GetProperty ("name").GetString ();
            string mgltString = starshipObjectJson.GetProperty ("MGLT").GetString ();
            string consumablesString = starshipObjectJson.GetProperty ("consumables").GetString ();

            logger.Log (LogLevel.Information, $"starship parsed: <\"name\": {nameString}, \"MGLT\": {mgltString}, \"consumables\": {consumablesString}>");

            uint mgltUInt = UInt32.MinValue;
            UInt32.TryParse (mgltString, out mgltUInt);

            uint consumablesUInt = UInt32.MinValue;
            ConsumablesTryParse (consumablesString, out consumablesUInt);

            logger.Log (LogLevel.Information, $"starship values converted: <\"name\": {nameString}, \"MGLT\": {mgltUInt}, \"consumables\": {consumablesUInt}>");

            return new Starship (nameString, mgltUInt, consumablesUInt);
        }

        private static void ConsumablesTryParse (string timeRangeString, out uint timeRangeUInt)
        {
            timeRangeUInt = UInt32.MinValue;
            
            uint numberOfTimeRanges = 0;
            string [] timeRangeArray = timeRangeString.Split ();
            
            if (timeRangeArray.Length == 2 &&
                TimeRangesInHours.ContainsKey (timeRangeArray[1]) &&
                UInt32.TryParse (timeRangeArray[0], out numberOfTimeRanges))
            {
                timeRangeUInt = numberOfTimeRanges * (uint)TimeRangesInHours[timeRangeArray[1]];
            }
        }

        public string GetPropertyAsString (string propertyName)
        {
            try
            {
                using (JsonDocument jsonDocument = JsonDocument.Parse (jsonString))
                {
                    JsonElement rootElementJson = jsonDocument.RootElement;
                    return rootElementJson.GetProperty (propertyName).ToString ();
                }
            }
            catch (Exception error)
            {
                logger.Log (LogLevel.Warning, $"Exception has been occurred while trying to get proprety by name {propertyName}: {error.Message}");
                throw error;
            }
        }
    }
}