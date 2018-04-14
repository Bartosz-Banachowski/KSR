using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSR.Model
{
    public class EuclideanMetric
    {
        public static Task CalculateAsync(List<List<Reuter>> AllReuters)
        {
            return Task.Factory.StartNew(() => Calculate(AllReuters));
        }
        public static void Calculate(List<List<Reuter>> AllReuters)
        {
            List<Dictionary<string, double>> TrainingVectors = new List<Dictionary<string, double>>();
            List<Dictionary<string, double>> TestVectors = new List<Dictionary<string, double>>();

            for (int i=0; i < AllReuters.ElementAt(0).Count; i++)
            {
                TrainingVectors.Add(AllReuters.ElementAt(0).ElementAt(i).VectorFeatures);
            }
            for (int i=0; i < AllReuters.ElementAt(1).Count; i++)
            {
                TestVectors.Add(AllReuters.ElementAt(1).ElementAt(i).VectorFeatures);
            }
           // Dictionary<Dictionary<string, double>, List<double>>
            List<double> res = CalculateEuclideanMetricForOneTestSet(TestVectors.ElementAt(0), TrainingVectors);
        }

        public static List<Double> CalculateEuclideanMetricForOneTestSet(Dictionary<string,double> testSet, List<Dictionary<string,double>> TrainingVectors)
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
                    } else
                    {
                        yn = 0;
                    }
                    xn = testSet.ElementAt(j).Value;
                    underSqrt += Math.Pow(xn - yn, 2);
                }
                result.Add(Math.Sqrt(underSqrt));
                underSqrt = 0;
            }
            return result;
        }
    }
}
