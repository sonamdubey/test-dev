namespace Bikewale.Entities.NewBikeSearch
{
    /// <summary>
    /// Modified by : Snehal Dange on 11th April 2018
    /// Description: Added string arrays for Brakes,Wheels,StartType
    /// </summary>
    public class FilterInput
    {
        public string[] Make { get; set; }
        public string[] Model { get; set; }
        public string[] Displacement { get; set; }
        public string MinBudget { get; set; }
        public string MaxBudget { get; set; }
        public string[] Mileage { get; set; }
        public string[] RideStyle { get; set; }
        public bool ABSAvailable { get; set; }
        public bool ABSNotAvailable { get; set; }
        public bool DiscBrake { get; set; }
        public bool DrumBrake { get; set; }
        public bool AlloyWheel { get; set; }
        public bool SpokeWheel { get; set; }
        public bool Electric { get; set; }
        public bool Manual { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string sc { get; set; }
        public string so { get; set; }
        public string PageSize { get; set; }
        public string PageNo { get; set; }

        public string[] Brakes { get; set; }
        public string[] Wheels { get; set; }
        public string[] StartType { get; set; }
    }
}
