using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;

namespace Carwale.BL.CarData
{
    public class Mileage : ICarMileage
    {
        /// <summary>
        /// Function to return final mileage data.
        /// </summary>
        /// <param name="versionList"></param>
        /// <returns></returns>
        public List<MileageDataEntity> GetMileageData(List<CarVersions> versionList)
        {
            List<MileageDataEntity> mileageList = new List<MileageDataEntity>();
            try
            {
                Dictionary<string, MileageDataEntity> mileageDictionary = new Dictionary<string, MileageDataEntity>();
                Dictionary<string, double> countDictionary = new Dictionary<string, double>();
                foreach (var version in versionList)
                {
                    if (version.Arai > 0)
                    {
                        string engineConfig = version.CarFuelType + "|" + version.TransmissionType + "|" + version.Displacement;
                        if (mileageDictionary.ContainsKey(engineConfig))
                        {
                            mileageDictionary[engineConfig].Arai += version.Arai;
                            countDictionary[engineConfig] += 1;
                        }
                        else
                        {
                            var mileage = new MileageDataEntity
                            {
                                FuelType = version.CarFuelType,
                                Transmission = version.TransmissionType,
                                Displacement = version.Displacement,
                                MileageUnit = version.MileageUnit,
                                Arai = version.Arai
                            };
                            countDictionary.Add(engineConfig, 1);
                            mileageDictionary.Add(engineConfig, mileage);
                        }
                    }
                }
                foreach (var engineConfig in mileageDictionary.Keys)
                {
                    var mileagedata = mileageDictionary[engineConfig];
                    var count = countDictionary[engineConfig] > 0 ? countDictionary[engineConfig] : 1;
                    mileagedata.Arai = Math.Round(mileagedata.Arai / count,2);
                    mileageList.Add(mileagedata);
                }
                mileageList.Sort((a, b) => { return a.Arai < b.Arai ? 1 : 0; });
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is NullReferenceException || ex is InvalidCastException || ex is InvalidOperationException)
            {
                Logger.LogException(ex, "GetMileageData " + versionList);
            }
            return mileageList;
        }
    }
}
