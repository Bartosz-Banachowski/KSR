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
                if (reut.Text.ElementAt(i) == "") continue;
                vectorFeature.Add(reut.Text.ElementAt(i), (double)howManyTimeWordOccur / reut.Text.Count);
            }
            reut.VectorFeatures = vectorFeature;
        }
    }
}
