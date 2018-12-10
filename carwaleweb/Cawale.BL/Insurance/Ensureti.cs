using Carwale.Entity.Insurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Carwale.Interfaces.Insurance;
using Carwale.Entity;
using AEPLCore.Cache;
using Carwale.BL.PolicyBoss;
using Carwale.DAL.Insurance;
using System.Text.RegularExpressions;
using System.Web;
using Carwale.DTOs.Insurance;
using System.Globalization;
using Carwale.Entity.Enum;
using Carwale.Notifications;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Carwale.Entity.Insurance.Coverfox;
using AutoMapper;
using System.Text;
using Microsoft.Practices.Unity;
using Carwale.Notifications.MailTemplates;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;

namespace Carwale.BL.Insurance
{
    public class Ensureti : IInsurance
    {

        public static string EnsuretiMails = System.Configuration.ConfigurationManager.AppSettings["ensuretimails"]??"rohan.sapkal@carwale.com";//"suhail.pawaskar@carwale.com";
        private IUnityContainer _container;

        public Ensureti(IUnityContainer container)
        {
            _container = container;
        }

        public InsuranceResponse SubmitLeadV2(InsuranceLead inputs)
        {
            try
            {
                var versionDetails = _container.Resolve<ICarVersionCacheRepository>().GetVersionDetailsById(inputs.VersionId);
                var location = _container.Resolve<IGeoCitiesCacheRepository>().GetStateAndAllCities(inputs.CityId);

                string cityName = location.Cities.Find(c => c.CityId == inputs.CityId).CityName;

                var clientMailEntity = new ClientMailEntity();
                clientMailEntity.Car = new CarNameEntity();
                clientMailEntity.Customer = new CustomersBasicInfo();

                InsuranceRepository DalRepo = new InsuranceRepository();
                inputs.InsuranceLeadId=DalRepo.SaveLead(inputs);

                if (inputs.InsuranceLeadId > 0)
                {
                    clientMailEntity.ClientEmails = EnsuretiMails;
                    clientMailEntity.InsuranceType = !inputs.InsuranceNew ? "Renew" : "New";
                    clientMailEntity.RegistrationDate = !string.IsNullOrWhiteSpace(inputs.CarRegDate) ? inputs.CarRegDate : "N.A";
                    clientMailEntity.Car.MakeName = versionDetails.MakeName;
                    clientMailEntity.Car.ModelName = versionDetails.ModelName;
                    clientMailEntity.Car.VersionName = versionDetails.VersionName;
                    clientMailEntity.Customer.City = cityName ?? "N.A";
                    clientMailEntity.Customer.State = location == null || string.IsNullOrWhiteSpace(location.State.StateName) ? "N.A" : location.State.StateName;
                    clientMailEntity.Customer.Name = inputs.Name;
                    clientMailEntity.Customer.Mobile = inputs.Mobile;
                    clientMailEntity.Customer.Email = inputs.Email;
                    clientMailEntity.LeadId = inputs.InsuranceLeadId;

                    var email = new InsuranceMailTemplate().GetInsuranceEmailTemplate(clientMailEntity);
                    new Email().SendMail(email.Email, "Ensureti-" + email.Subject, email.Body);
                }

                return new InsuranceResponse()
                {
                    Breakdown = null,
                    Total = null,
                    RedirectUrl = string.Empty,
                    UseRedirect = false,
                    Success = inputs.InsuranceLeadId > 0 ? true : false,
                    QuoteId = inputs.InsuranceLeadId.ToString()
                };
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Ensureti.SubmitLeadV2()"+JsonConvert.SerializeObject(inputs));
                objErr.SendMail();
            }
            return null;
        }

        public QuotationDto SubmitLead(InsuranceLead inputs)
        {
            throw new NotImplementedException();
        }

        public List<MakeEntity> GetMakes(Application application)
        {
            throw new NotImplementedException();
        }

        public List<ModelBase> GetModels(int makeId, Application application)
        {
            throw new NotImplementedException();
        }

        public List<VersionBase> GetVersions(int modelId, Application application)
        {
            throw new NotImplementedException();
        }

        public List<InsuranceCity> GetCities(Application application)
        {
            throw new NotImplementedException();
        }

    }
}

