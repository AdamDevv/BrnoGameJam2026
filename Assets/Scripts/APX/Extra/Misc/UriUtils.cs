using System;
using System.Collections.Generic;
using System.Text;

namespace APX.Extra.Misc
{
    public static class UriUtils
    {
        public static Dictionary<string, string> DecodeParameters(string query)
        {
            var result = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(query))
                return result;

            var index = query.IndexOf('?');
            if (index > -1)
            {
                if (query.Length >= index + 1)
                    query = query.Substring(index + 1);
            }
  
            var pairs = query.Split('&');
            foreach (var pair in pairs)
            {
                var index2 = pair.IndexOf('=');
                if (index2 > 0)
                {
                    var key = pair.Substring(0, index2);
                    var value = pair.Substring(index2 + 1);

                    var origKey = key;
                    var val = 2;
                    while (result.ContainsKey(key))
                    {
                        key = origKey + val++;
                    }

                    result.Add(key, value);
                }
            }

            return result;
        }

        public static string Encode(IDictionary<string, string> dict, string url = null)
        {
            var sb = new StringBuilder();
            foreach (var key in dict.Keys)
            {
                sb.Append($"{key}={Uri.EscapeUriString(dict[key])}&");
            }

            if (sb.Length > 0)
            {
                // Remove the last '&'
                sb.Remove(sb.Length - 1, 1);
            }

            if (!string.IsNullOrEmpty(url))
            {
                sb.Insert(0, $"{url}?");
            }
            return sb.ToString();
        }
    }
}
