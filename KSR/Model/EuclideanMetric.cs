using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSR.Model
{
    public class EuclideanMetric
    {
        public static Task<double> CalculateAsync(List<List<Reuter>> AllReuters, int k)
        {
            return Task<double>.Factory.StartNew(() => Calculate(AllReuters, k));
        }
        public static double Calculate(List<List<Reuter>> AllReuters, int k)
        {
            List<Reuter> TrainingVectors = new List<Reuter>();
            List<Reuter> TestVectors = new List<Reuter>();
            double IsPlacesFoundList = 0;

            for (int i = 0; i < AllReuters.ElementAt(0).Count; i++)
            {
                TrainingVectors.Add(AllReuters.ElementAt(0).ElementAt(i));
            }
            for (int i = 0; i < AllReuters.ElementAt(1).Count; i++)
            {
                TestVectors.Add(AllReuters.ElementAt(1).ElementAt(i));
            }
            for (int i = 0; i < TestVectors.Count; i++)
            {
                if (CalculateEuclideanMetricForOneTestSet(TestVectors.ElementAt(i), TrainingVectors, k))
                {
                    IsPlacesFoundList++;
                }
            }
            return IsPlacesFoundList/TestVectors.Count;
        }

        public static bool CalculateEuclideanMetricForOneTestSet(Reuter testSet, List<Reuter> TrainingVectors, int k)
        {
            double xn = 0;
            double yn = 0;
            double underSqrt = 0;
            TestVectorAndTrainingVectorsCollection result = new TestVectorAndTrainingVectorsCollection();
            for (int i = 0; i < TrainingVectors.Count; i++) //wykonujemy petle dla kazdego wzorca treningowego
            {
                foreach (var word in testSet.VectorFeatures) //sprawdzamy dla kazdego slowa z wektora testowego czy istnieje takie slowo w wektorze treningowym
                {
                    if (TrainingVectors.ElementAt(i).VectorFeatures.ContainsKey(word.Key))
                    {
                        yn = TrainingVectors.ElementAt(i).VectorFeatures[word.Key];
                    } else
                    {
                        yn = 0;
                    }
                    xn = word.Value;
                    underSqrt += Math.Pow(xn - yn, 2);
                }
                result.TestReuter = testSet;
                TrainingVectors.ElementAt(i).HowFar = Math.Sqrt(underSqrt);
                underSqrt = 0;
            }
            result.TrainingReuters = TrainingVectors;
            return KnnAlgorithm.Calculate(result, k);
        }
    }
}
