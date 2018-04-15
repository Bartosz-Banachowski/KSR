using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSR.Model
{
    public class Reuter
    {
        public List<string> Places { get; set; }
        public List<string> Text { get; set; }
        public string TextTemp { get; set; }
        public Dictionary<string, double> VectorFeatures;

        public static Task<List<Reuter>> GetReutersFromFileAsync(string[] reutPath)
        {
            return Task<List<Reuter>>.Factory.StartNew(() => GetReutersFromFile(reutPath));
        }

        public static List<Reuter> GetReutersFromFile(string[] reutPath)
        {
            List<Reuter> reuters = new List<Reuter>();
            List<Reuter> result = new List<Reuter>();
            int reutersNumber = reutPath.Length;
            for (int i = 0; i < reutersNumber; i++)
            {
                var xmlRawFile = File.ReadAllText(reutPath[i]);
                var html = new HtmlDocument();
                html.LoadHtml(xmlRawFile);
                reuters.AddRange(html.DocumentNode.Descendants("REUTERS").Select(x => new Reuter
                {
                    Places = x.Descendants("PLACES").Select(c => c.Descendants("D").Select(h => h.InnerHtml)).First().ToList(),
                    TextTemp = x.Descendants("TEXT").Select(
                            z => z.LastChild.InnerHtml
                            .Substring(0, z.LastChild.InnerHtml.Length >= 13 ? z.LastChild.InnerHtml.Length - 13 : 0)
                        ).First()
                }).ToList());
            }
            int howManyReuters = reuters.Count;

            for (int i = 0; i < howManyReuters; i++)
            {
                if (reuters.ElementAt(i).Places.Count != 1)
                {
                    continue;
                }
                result.Add(new Reuter { Places = reuters.ElementAt(i).Places, TextTemp = reuters.ElementAt(i).TextTemp });
                result.Last().TextTemp = result.Last().TextTemp.Replace("    ", " ");
                result.Last().Text = result.Last().TextTemp.Split(' ', '\n', '\t').ToList();
                FeatureExtractions.HowManyWordsExtractor(result.Last());
            }
            return result;
        }
    }
}
