using System;

namespace Carwale.Entity.Classified
{
    [Serializable]
    public class Slot
    {
        public int Id { get; set; }
        public int FranchiseeProbability { get; set; }
        public int DiamondProbability { get; set; }
        public int PlatinumProbability { get; set; }
    }
}
