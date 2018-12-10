namespace Carwale.Entity.Classified.Enum
{
    /// <summary>
    /// The enum is used for specifying nearby bucket for searcg page response
    /// The value would be generic since the requirement nearby bucket might change
    /// i.e if today bucket1 is 0-8km and bucket2 is 8-city
    /// next requirement might be an additional bucket or change in the distance
    /// This enum will help in bucket definition at one place
    /// </summary>
    public enum NearbyCarsBucket
    {
        Default = 0,
        //cars within 0-8km of user lat long
        Bucket1 = 1,
        //cars within 8km - city wide of user lat long
        Bucket2 = 2
    }
}
