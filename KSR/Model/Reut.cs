using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSR.Model
{
    public class Reut
    {
        public List<string> Places { get; set; }
        public List<string> Text { get; set; }
        public string TextTemp { get; set; }
        public Dictionary<string, double> VectorFeatures;

        public static Task<List<Reut>> GetReutersFromFileAsync(string[] reutPath)
        {
            return Task<List<Reut>>.Factory.StartNew(() => GetReutersFromFile(reutPath));
        }

        public static List<Reut> GetReutersFromFile(string[] reutPath)
        {
            List<Reut> reuters = new List<Reut>();
            int reutersNumber = reutPath.Length;
            for (int i = 0; i < reutersNumber; i++)
            {
                var xmlRawFile = File.ReadAllText(reutPath[i]);
                var html = new HtmlDocument();
                html.LoadHtml(xmlRawFile);
                reuters.AddRange(html.DocumentNode.Descendants("REUTERS").Select(x => new Reut
                {
                    Places = x.Descendants("PLACES").Select(c => c.Descendants("D").Select(h => h.InnerHtml)).First().ToList(),
                    TextTemp = x.Descendants("TEXT").Select(
                            z => z.LastChild.InnerHtml
                            .Substring(0, z.LastChild.InnerHtml.Length >= 13 ? z.LastChild.InnerHtml.Length - 13 : 0)
                        ).First()
                }).ToList());
            }
            for (int i = 0; i < reuters.Count; i++)
            {
                if (reuters.ElementAt(i).Places.Count != 1)
                {
                    reuters.Remove(reuters.ElementAt(i));
                }
                reuters.ElementAt(i).TextTemp = reuters.ElementAt(i).TextTemp.Replace("    ", " ");
                reuters.ElementAt(i).Text = reuters.ElementAt(i).TextTemp.Split(' ', '\n', '\t').ToList();
                FeatureExtractions.HowManyWordsExtractor(reuters.ElementAt(i));
            }
            return reuters;
        }
    }
}
