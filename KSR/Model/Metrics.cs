using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSR.Model
{
    public class Metrics
    {
        public static void EuclideanMetric(List<List<Reuter>> AllReuters)
        {
            List<Dictionary<string, double>> TrainingVectors = new List<Dictionary<string, double>>();
            List<Dictionary<string, double>> TestVectors = new List<Dictionary<string, double>>();
            double distance = 0;
            for (int i=0; i < AllReuters.ElementAt(0).Count; i++)
            {
                TrainingVectors.Add(AllReuters.ElementAt(0).ElementAt(i).VectorFeatures);
            }
            for (int i=0; i < AllReuters.ElementAt(1).Count; i++)
            {
                TestVectors.Add(AllReuters.ElementAt(1).ElementAt(i).VectorFeatures);
            }

            int howManyTrainingVectors = AllReuters.ElementAt(0).Count;
            int howManyTestVectors = AllReuters.ElementAt(1).Count;
            int bla = TestVectors.ElementAt(0).Count;
            List<List<double>> DistanceBetweenTestAndTrainingVectors = new List<List<double>>();

            for (int i = 0; i < howManyTestVectors; i++)
            {
                for (int j = 0; j < howManyTrainingVectors; j++)
                {
                    for( int k = 0; k < TestVectors.ElementAt(i).Count; k++)
                    {
                    //    distance += TestVectors.ElementAt(i) - 
                    }
                   // for()
                   // var distance = TestVectors.ElementAt(i).E// - TrainingVectors.ElementAt(j).Values;
                }
            }
        }
    }
}
