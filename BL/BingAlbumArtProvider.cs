using System;
using System.Collections.Specialized;
using System.Net;

namespace BB.BL
{
    public class BingAlbumArtProvider : IAlbumArtProvider
    {
        private readonly WebClient client = new WebClient();

        public string Find(string query)
        {
            var searchUrl = "http://www.bing.com/images/search";

            var queryParams = new NameValueCollection
            {
                {"q", query + " album"},
                {"scope", "images"},
                {"qft", "+filterui:aspect-square"}
            };
            client.QueryString = queryParams;

            var result = client.DownloadString(new Uri(searchUrl));

            if (!result.Contains("\" class=\"thumb\" ")) return null;

            result = result.Split(new[] { "\" class=\"thumb\" " }, StringSplitOptions.None)[0];
            result = result.Substring(result.LastIndexOf('"') + 1);
            return result;
        }
    }
}