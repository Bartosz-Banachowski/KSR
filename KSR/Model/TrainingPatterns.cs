using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSR.Model
{
    public class TrainingPatterns
    {
        public static Task<List<List<Reuter>>> SetTrainingAndTestSetAsync(int Percent, List<Reuter> AllReuters)
        {
            return Task<List<List<Reuter>>>.Factory.StartNew(() => SetTrainingAndTestSet( Percent, AllReuters));
        }

        public static List<List<Reuter>> SetTrainingAndTestSet(int PercentOfTrainingPatterns, List<Reuter> AllReuters)
        {
            double percent = (double)PercentOfTrainingPatterns / 100;
            int howManyTrainingPatterns = (int)(percent * AllReuters.Count);
            int howManyTestPatterns = AllReuters.Count - howManyTrainingPatterns;
            List<Reuter> TrainingPatterns = AllReuters.Take(howManyTrainingPatterns).ToList();
            List<Reuter> TestPatterns = AllReuters.Skip(howManyTrainingPatterns).Take(howManyTestPatterns).ToList();
            List<List<Reuter>> TrainingAndTestReuters = new List<List<Reuter>>();
            TrainingAndTestReuters.Add(TrainingPatterns);
            TrainingAndTestReuters.Add(TestPatterns);
            return TrainingAndTestReuters;
        }
    }
}
