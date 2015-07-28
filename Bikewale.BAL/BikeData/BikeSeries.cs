using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;

namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary : Class will have functions related to the bikes series.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeSeries<T,U> : IBikeSeries<T,U> where T : BikeSeriesEntity, new()
    {
        private readonly IBikeSeries<T, U> seriesRepository = null;

        public BikeSeries()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeSeries<T, U>, BikeSeriesRepository<T, U>>();
                seriesRepository = container.Resolve<IBikeSeries<T, U>>();
            }
        }

        public List<BikeModelEntity> GetModelsList(U seriesId)
        {
            List<BikeModelEntity> objModelList = null;

            objModelList = seriesRepository.GetModelsList(seriesId);

            return objModelList;
        }

        public BikeDescriptionEntity GetSeriesDescription(U seriesId)
        {
            throw new NotImplementedException();
        }

        public U Add(T t)
        {
            throw new NotImplementedException();
        }

        public bool Update(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(U id)
        {
            T t = seriesRepository.GetById(id);

            return t;
        }


        public List<BikeModelEntityBase> GetModelsListBySeriesId(U seriesId)
        {
            List<BikeModelEntityBase> objModels = null;

            objModels = seriesRepository.GetModelsListBySeriesId(seriesId);

            return objModels;
        }
    }   // class
}   // namespace
