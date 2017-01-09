using BikewaleOpr.BAL;
using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.BAL.Used;
using BikewaleOpr.DALs.BikeColorImages;
using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.Interface.BikeColorImages;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using BikewaleOpr.Interface.Used;
using BikewaleOpr.Used;
using Microsoft.Practices.Unity;


namespace BikewaleOpr.Service.UnityConfiguration
{
    /// <summary>
    /// Created By : Sangram Nandkhile  05 July 2016
    /// modified By :- Subodh Jain 09 Jan 2017
    /// Summary : Added color bike repository
    /// </summary>
    public static class UnityBootstrapper
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer Initialize()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IManufacturerCampaignRepository, ManufacturerCampaign>();
            container.RegisterType<IContractCampaign, ContractCampaign>();
            container.RegisterType<IManufacturerReleaseMaskingNumber, ManufacturerReleaseMaskingNumber>();
            container.RegisterType<ISellerRepository, SellerRepository>();
            container.RegisterType<ISellBikes, SellBikes>();
            container.RegisterType<IColorImagesBikeRepository, ColorImagesBikeRepository>();
            return container;
        }
    }
}