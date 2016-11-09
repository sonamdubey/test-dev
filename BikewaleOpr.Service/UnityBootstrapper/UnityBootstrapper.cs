using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using Microsoft.Practices.Unity;
using BikewaleOpr.BAL;
using BikewaleOpr.Interface.Used;
using BikewaleOpr.Used;
using BikewaleOpr.BAL.Used;


namespace BikewaleOpr.Service.UnityConfiguration
{
    /// <summary>
    /// Created By : Sangram Nandkhile  05 July 2016
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
            container.RegisterType<ISellerRepository,SellerRepository>();
            container.RegisterType<ISellBikes,SellBikes>();
            return container;
        }
    }
}