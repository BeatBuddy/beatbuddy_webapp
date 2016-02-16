using System;
using System.Configuration;

namespace BB.BL
{
    public class Constants
    {
        public static readonly string YoutubeApplicationName = "BeatBuddy";

        public static string YouTubeApiKey()
        {
            string KEY = "YouTubeApiKey";
            string youtubeApiKey = ConfigurationManager.AppSettings[KEY];

            if (string.IsNullOrEmpty(youtubeApiKey))
            {
                return Environment.GetEnvironmentVariable(KEY);
            }

            return youtubeApiKey;
        }
    }
}
