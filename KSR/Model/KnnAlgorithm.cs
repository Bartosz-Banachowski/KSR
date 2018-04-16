using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSR.Model
{
    public class KnnAlgorithm
    {
        public static bool Calculate(TestVectorAndTrainingVectorsCollection item, int k)
        {
            return ChooseKNeighbours(item, k);
        }

        public static bool ChooseKNeighbours(TestVectorAndTrainingVectorsCollection TestAndTrainingPair, int k)
        {
            TestAndTrainingPair.TrainingReuters = TestAndTrainingPair.TrainingReuters.OrderBy(h => h.HowFar).ToList(); //sorted list
            var Kneighbours = TestAndTrainingPair.TrainingReuters.Take(k).ToList(); // take k neighbours
            return ChoosePlace(Kneighbours, TestAndTrainingPair.TestReuter); ;
        }

        public static bool ChoosePlace(List<Reuter> neighbours, Reuter TestReuter)
        {
            List<Reuter> tempneighbours = neighbours.ToList();
            int howManyTimesOccur = 0;
            Dictionary<string, int> howManyPlaces = new Dictionary<string, int>();
            for(int i=0; i < tempneighbours.Count; i++)
            {
                for(int j=0; j< tempneighbours.Count; j++)
                {
                    if(tempneighbours.ElementAt(i).Places.First() == tempneighbours.ElementAt(j).Places.First())
                    {
                        howManyTimesOccur++;
                    }
                }
                if (howManyPlaces.ContainsKey(tempneighbours.ElementAt(i).Places.First()))
                {
                    howManyTimesOccur = 0;
                    continue;
                }
                howManyPlaces.Add(tempneighbours.ElementAt(i).Places.First(), howManyTimesOccur);
                howManyTimesOccur = 0;
            }
                howManyPlaces = howManyPlaces.OrderByDescending(x => x.Value)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

            List<KeyValuePair<string,int>> result = howManyPlaces.ToList();
            var FoundPlace = result.First().Key;
            if (TestReuter.Places.First().Equals(FoundPlace))
            { return true; }
            else { return false; }
           
        }

    }
}
