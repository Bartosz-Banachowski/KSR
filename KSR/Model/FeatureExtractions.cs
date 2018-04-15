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
                    if (j == i) continue;
                    else if (reut.Text.ElementAt(i) == reut.Text.ElementAt(j))
                    {
                        howManyTimeWordOccur++;
                    }
                }
                if (vectorFeature.ContainsKey(reut.Text.ElementAt(i))) continue;
                if (reut.Text.ElementAt(i).Equals("")) continue;
                vectorFeature.Add(reut.Text.ElementAt(i), (double)howManyTimeWordOccur / reut.Text.Count);
            }
            reut.VectorFeatures = vectorFeature;
        }

        //Inverse document frequency
        public static void InverseDocumentFrequency(List<Reuter> reuters, int NumberOfWords)
        {
            int howManyDocumentsContainkeyword = 0;

            for (int i = 0; i < reuters.Count; i++)
            {
                if (reuters.ElementAt(i).Places.Count != 1)
                {
                    reuters.Remove(reuters.ElementAt(i));
                }
                //reuters.ElementAt(i).TextTemp = reuters.ElementAt(i).TextTemp.Replace("    ", " ");
                reuters.ElementAt(i).Text = reuters.ElementAt(i).TextTemp.Split(' ', '\n', '\t').ToList();
                FeatureExtractions.HowManyWordsExtractor(reuters.ElementAt(i));
            }

            foreach (Reuter r in reuters)
            {
                r.VectorFeatures = r.VectorFeatures.OrderBy(x => x.Value)
                    .Take(NumberOfWords)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            for (int i = 0; i < reuters.Count; ++i)
            {
                for(int j = 0; j < reuters[i].VectorFeatures.Count; ++j)
                {
                    foreach(Reuter r in reuters)
                    {
                        if(r.Text.Contains(reuters[i].VectorFeatures.Keys.ElementAt(j)))
                        {
                            howManyDocumentsContainkeyword++;
                        }
                    }
                    reuters[i].VectorFeatures[reuters[i].VectorFeatures.Keys.ElementAt(j)] = Math.Log10(reuters.Count/howManyDocumentsContainkeyword);
                    howManyDocumentsContainkeyword = 0;
                }
            }
        }
    }
}
