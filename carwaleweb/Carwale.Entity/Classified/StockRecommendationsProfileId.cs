namespace Carwale.Entity.Classified
{
    public class StockRecommendationsProfileId
    {
        private string[] _cityIds;
        private string _price, _profileId, _bodyStyleId, _rootId,_kms;
        private int _makeId, _packageType, _versionSubSegmentID;
        private double _budgetMin, _budgetMax, _kmMin, _kmMax;

        public double BudgetMin
        {
            get { return _budgetMin; }
            set { _budgetMin = value; }
        }
        public double BudgetMax
        {
            get { return _budgetMax; }
            set { _budgetMax = value; }
        }
        public int VersionSubSegmentID
        {
            get { return _versionSubSegmentID; }
            set { _versionSubSegmentID = value; }
        }
        public string[] CityIds
        {
            get { return _cityIds; }
            set { _cityIds = value; }
        }
        public string ProfileId
        {
            get { return _profileId; }
            set { _profileId = value; }
        }
        public string Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public string Kilometers
        {
            get { return _kms; }
            set { _kms = value; }
        }
        public int MakeId
        {
            get { return _makeId; }
            set { _makeId = value; }
        }
        public string RootId
        {
            get { return _rootId; }
            set { _rootId = value; }
        }
        public string BodyStyleId
        {
            get { return _bodyStyleId; }
            set { _bodyStyleId = value; }
        }        
        
        public double KmMin
        {
            get { return _kmMin; }
            set { _kmMin = value; }
        }
        public double KmMax
        {
            get { return _kmMax; }
            set { _kmMax = value; }
        }
        public int PackageType
        {
            get { return _packageType; }
            set { _packageType = value; }
        }
    }
}