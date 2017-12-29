﻿using System.Collections.Generic;
using BikewaleOpr.Entity.ElasticSearch;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 13th Dec 2017
    /// Summary : Interface for ES data updation
    /// </summary>
    public interface IBikeESRepository
    {
        BikeList GetBikeESIndex(string id, string indexName);
        bool UpdateBikeESIndex(string id, string indexName, BikeList bike);
        List<BikeList> GetBikeESIndex(IEnumerable<string> ids, string indexName);
        bool UpdateBikeESIndex(IEnumerable<string> ids, string indexName, List<BikeList> bikes);
    }
}
