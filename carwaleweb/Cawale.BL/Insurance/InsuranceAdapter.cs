using Carwale.DAL.Insurance;
using Carwale.DTOs.Insurance;
using Carwale.Entity;
using Carwale.Entity.Enum;
using Carwale.Entity.Insurance;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Insurance;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Insurance
{
    public class InsuranceAdapter
    {
        private IUnityContainer _container;
        private readonly IInsurance _insurance;

        public InsuranceAdapter(IUnityContainer container, Clients client)
        {
            _container = container;
            _insurance = GetClientBL(client);
        }

        public InsuranceResponse Get(InsuranceLead inputs, params object[] args)
        {
            try
            {
                int appversion;
                int.TryParse(args[0].ToString(), out appversion);

                InsuranceRepository DalRepo = new InsuranceRepository();
                if (inputs.StateId == 0)
                {
                    var location = _container.Resolve<IGeoCitiesCacheRepository>().GetStateAndAllCities(inputs.CityId);

                    inputs.StateId = location.State.StateId;
                    inputs.StateName = location.State.StateName;
                }
                if (inputs.ModelId == 0 || inputs.MakeId==0)
                {
                    var carDetails = _container.Resolve<ICarVersionCacheRepository>().GetVersionDetailsById(inputs.VersionId);

                    inputs.ModelId = carDetails.ModelId;
                    inputs.MakeId = carDetails.MakeId;
                }

                Clients client = inputs.clientId;
                IInsurance insurance;
                if (inputs.clientId == 0)
                {
                    Enum.TryParse<Clients>(DalRepo.GetClient(inputs).ToString(), out client);
                    inputs.clientId = client;
                    insurance = GetClientBL(client);
                }
                else insurance = _insurance;

                if (insurance != null) return insurance.SubmitLeadV2(inputs);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "InsuranceAdapter.Get() inputs=" + JsonConvert.SerializeObject(inputs));
                objErr.SendMail();
            }
            return default(InsuranceResponse);
        }

        public IInsurance GetClientBL(Clients client)
        {          
            switch (client)
            {
                case Clients.Ensureti: return new Ensureti(_container);
                case Clients.Chola: return _container.Resolve<IInsurance>("chola");
                case Clients.RoyalSundaram: return _container.Resolve<IInsurance>("RoyalSundaram");
                case Clients.Coverfox: return _container.Resolve<IInsurance>("Coverfox");
                case Clients.PolicyBoss: return _container.Resolve<IInsurance>("PolicyBoss");
                case Clients.CW: return _container.Resolve<IInsurance>("CW");
                default: return _container.Resolve<IInsurance>("CW");
            }
        }

        public List<Entity.Insurance.InsuranceCity> GetCities(Application application)
        {
            return _insurance != null ? _insurance.GetCities(application) : null;
        }

        public List<Entity.MakeEntity> GetMakes(Entity.Enum.Application application)
        {
            return _insurance != null ? _insurance.GetMakes(application) : null;
        }

        public List<Entity.ModelBase> GetModels(int makeId, Entity.Enum.Application application)
        {
            return _insurance != null ? _insurance.GetModels(makeId, application) : null;
        }

        public List<Entity.VersionBase> GetVersions(int modelId, Entity.Enum.Application application)
        {
            return _insurance != null ? _insurance.GetVersions(modelId, application) : null;
        }
    }
}
