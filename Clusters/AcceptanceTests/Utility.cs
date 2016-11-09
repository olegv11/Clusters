using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AcceptanceTests
{
    public class Utility
    {
        public static List<Tuple<double, double>> GetListOfPoints(string s)
        {
            var result = new List<Tuple<double, double>>();

            var separated = s.Split(',');
            Regex r = new Regex(@"\((.*);(.*)\)");
            foreach (var pointString in separated)
            {
                var m = r.Matches(pointString);
                var x = double.Parse(m[0].Groups[1].Value);
                var y = double.Parse(m[0].Groups[2].Value);
                result.Add(new Tuple<double, double>(x, y));
            }

            return result;
        }
        public static List<string> GetListOfClusters(string s)
        {
            var result = new List<string>();
            Regex r = new Regex(@"\{(.*?)\}");
            var m = r.Matches(s);

            for (int i = 0; i < m.Count; i++)
            {
                result.Add(m[i].Groups[1].Value);
            }

            return result;
        }
    }
}