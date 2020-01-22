using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace App
{
    public class ApiDataSource : IDataSource<string>
    {
        private string sourcePath;
        private ILogger logger;

        public ApiDataSource (string sourcePath, ILogger logger)
        {
            this.sourcePath = sourcePath;
            this.logger = logger;
        }

        public string GetData ()
        {
            string result = new String ("");

            logger.Log (LogLevel.Information, $"trying to get data from {sourcePath}");
            try
            {
                HttpClient client = new HttpClient ();
                var response = client.GetAsync (sourcePath).Result;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new HttpRequestException ($"response status code from {sourcePath} is not OK : {response.Headers.ToString ()}");
                }
                logger.Log (LogLevel.Information, $"response received successfully from {sourcePath}: {response.Headers.ToString ()}");
                result = response.Content.ReadAsStringAsync ().Result;
            }
            catch (Exception error)
            {
                logger.Log (LogLevel.Error, $"exception has been occurred while trying to get data from {sourcePath} : {error.Message}");
                throw error;
            }
            logger.Log (LogLevel.Information, $"data received successfully from {sourcePath} : {result}");

            return result;
        }
    }
}