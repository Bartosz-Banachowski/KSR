using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSR.Model
{
    public class FeatureExtractions
    {


        public static Task HowManyWordsExtractorAsync(Reuter reut)
        {
            return Task.Factory.StartNew(() => HowManyWordsExtractor(reut));
        }

        //Team frequency
        public static void HowManyWordsExtractor(Reuter reut) 
        {
            int howManyTimeWordOccur = 0;
            Dictionary<string, double> vectorFeature = new Dictionary<string, double>();
            for(int i = 0; i < reut.Text.Count; i++)
            {
                for(int j = 0; j < reut.Text.Count; j++)
                {
                    if (reut.Text.ElementAt(i) == reut.Text.ElementAt(j))
                    {
                        howManyTimeWordOccur++;
                    }
                }
                if (vectorFeature.ContainsKey(reut.Text.ElementAt(i))) continue;
                if (reut.Text.ElementAt(i).Equals("")) continue;
                vectorFeature.Add(reut.Text.ElementAt(i), (double)howManyTimeWordOccur / reut.Text.Count);
                howManyTimeWordOccur = 0;
            }
            reut.VectorFeatures = vectorFeature;
        }

        //Inverse document frequency
        public static void InverseDocumentFrequency(List<Reuter> reuters, List<Reuter> result)
        {
            double howManyDocumentsContainkeyword = 0;

            result.Clear();
            for (int i = 0; i < reuters.Count; i++)
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

            foreach (Reuter r in result)
            {
                r.VectorFeatures = r.VectorFeatures.OrderBy(x => x.Value)
                    .Take(10)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            for (int i = 0; i < result.Count; ++i)
            {
                for(int j = 0; j < result[i].VectorFeatures.Count; ++j)
                {
                    foreach(Reuter r in result)
                    {
                        if(r.Text.Contains(result[i].VectorFeatures.Keys.ElementAt(j)))
                        {
                            howManyDocumentsContainkeyword++;
                        }
                    }
                    double tempDiff = (double)result.Count/howManyDocumentsContainkeyword;
                    result[i].VectorFeatures[result[i].VectorFeatures.Keys.ElementAt(j)] = Math.Log10(tempDiff);
                    howManyDocumentsContainkeyword = 0;
                }
            }
        }
    }
}
