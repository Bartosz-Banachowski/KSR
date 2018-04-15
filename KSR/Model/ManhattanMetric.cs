using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSR.Model
{
    public class ManhattanMetric
    {
        public static Task<List<TestVectorAndTrainingVectorsCollection>> CalculateAsync(List<List<Reuter>> AllReuters)
        {
            return Task<List<TestVectorAndTrainingVectorsCollection>>.Factory.StartNew(() => Calculate(AllReuters));
        }
        public static List<TestVectorAndTrainingVectorsCollection> Calculate(List<List<Reuter>> AllReuters)
        {
            List<Reuter> TrainingVectors = new List<Reuter>();
            List<Reuter> TestVectors = new List<Reuter>();

            for (int i = 0; i < AllReuters.ElementAt(0).Count; i++)
            {
                TrainingVectors.Add(AllReuters.ElementAt(0).ElementAt(i));
            }
            for (int i = 0; i < AllReuters.ElementAt(1).Count; i++)
            {
                TestVectors.Add(AllReuters.ElementAt(1).ElementAt(i));
            }
            List<TestVectorAndTrainingVectorsCollection> result = new List<TestVectorAndTrainingVectorsCollection>();
            for (int i = 0; i < TestVectors.Count; i++)
            {
                result.Add(CalculateManhattanMetricForOneTestSet(TestVectors.ElementAt(i), TrainingVectors));
            }
            return result;
        }

        public static TestVectorAndTrainingVectorsCollection CalculateManhattanMetricForOneTestSet(Reuter testSet, List<Reuter> TrainingVectors)
        {
            double xn = 0;
            double yn = 0;
            double underSqrt = 0;
            TestVectorAndTrainingVectorsCollection result = new TestVectorAndTrainingVectorsCollection();
            for (int i = 0; i < TrainingVectors.Count; i++) //wykonujemy petle dla kazdego wzorca treningowego
            {
                foreach(var word in testSet.VectorFeatures) //sprawdzamy dla kazdego slowa z wektora testowego czy istnieje takie slowo w wektorze treningowym
                {
                    if (TrainingVectors.ElementAt(i).VectorFeatures.ContainsKey(word.Key))
                    {
                        yn = TrainingVectors.ElementAt(i).VectorFeatures[word.Key];
                    } else
                    {
                        yn = 0;
                    }
                    xn = word.Value;
                    underSqrt += Math.Abs(xn - yn);
                }
               
                TrainingVectors.ElementAt(i).HowFar = underSqrt;
                result.TestReuter = testSet;
                underSqrt = 0;
            }
            result.TrainingReuters = TrainingVectors;
            return result;
        }
    }
}
