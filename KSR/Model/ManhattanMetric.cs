using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSR.Model
{
    public class ManhattanMetric
    {
        public static Task CalculateAsync(List<List<Reuter>> AllReuters)
        {
            return Task.Factory.StartNew(() => Calculate(AllReuters));
        }
        public static void Calculate(List<List<Reuter>> AllReuters)
        {
            List<Dictionary<string, double>> TrainingVectors = new List<Dictionary<string, double>>();
            List<Dictionary<string, double>> TestVectors = new List<Dictionary<string, double>>();

            for (int i = 0; i < AllReuters.ElementAt(0).Count; i++)
            {
                TrainingVectors.Add(AllReuters.ElementAt(0).ElementAt(i).VectorFeatures);
            }
            for (int i = 0; i < AllReuters.ElementAt(1).Count; i++)
            {
                TestVectors.Add(AllReuters.ElementAt(1).ElementAt(i).VectorFeatures);
            }
            // Dictionary<Dictionary<string, double>, List<double>>
            List<List<double>> result = new List<List<double>>();
            for (int i = 0; i < TestVectors.Count; i++)
            {
                result.Add((CalculateManhattanMetricForOneTestSet(TestVectors.ElementAt(0), TrainingVectors)));
            }
            List<double> res = CalculateManhattanMetricForOneTestSet(TestVectors.ElementAt(0), TrainingVectors);
        }

        public static List<Double> CalculateManhattanMetricForOneTestSet(Dictionary<string, double> testSet, List<Dictionary<string, double>> TrainingVectors)
        {
            double xn = 0;
            double yn = 0;
            double underSqrt = 0;
            List<double> result = new List<double>();
            for (int i = 0; i < TrainingVectors.Count; i++) //wykonujemy petle dla kazdego wzorca treningowego
            {
                for (int j = 0; j < testSet.Count; j++) //sprawdzamy dla kazdego slowa z wektora testowego czy istnieje takie slowo w wektorze treningowym
                {
                    if (TrainingVectors.ElementAt(i).ContainsKey(testSet.ElementAt(j).Key))
                    {
                        yn = TrainingVectors.ElementAt(i)[testSet.ElementAt(j).Key];
                    }
                    else
                    {
                        yn = 0;
                    }
                    xn = testSet.ElementAt(j).Value;
                    underSqrt += Math.Abs(xn - yn);
                }
                result.Add(underSqrt);
                underSqrt = 0;
            }
            return result;
        }
    }
}
