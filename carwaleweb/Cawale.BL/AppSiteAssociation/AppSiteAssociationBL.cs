using AEPLCore.Cache;
using Carwale.Entity;
using Carwale.Entity.Enum;
using Carwale.Entity.Finance;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Carwale.Interfaces.Finance;
using System.Configuration;
using AutoMapper;
using Carwale.DTOs.Finance;
using Newtonsoft.Json;
using Carwale.Interfaces.IAppSiteAssociation;
using Carwale.Entity.AppSiteAssociation;
using Microsoft.Practices.Unity;

/// <summary>
/// Author: Ajay Singh(on 16 feb 2016)
/// Description : For IOS purpose
/// </summary>
namespace Carwale.BL.AppSiteAssociation
{

    public class AppSiteAssociationBL : IAppSiteAssociationBL
    {
        public AppSiteAssociationEntity Get()
        {
            AppSiteAssociationEntity objAppSiteAssociationEntity = new AppSiteAssociationEntity();

            try
            {
                List<string> paths = new List<string>();
                paths.Add("/m/");
                paths.Add("m/new/");
                paths.Add("m/*-cars");
                paths.Add("m/*-cars/*/");
                paths.Add("m/comparecars/");
                paths.Add("m/new/buy/");
                paths.Add("/m/research/locatedealerpopup.aspx/");
                paths.Add("/quotation/landing/");
                paths.Add("/m/insurance/");
                paths.Add("/m/news/");
                paths.Add("/m/used/");
                paths.Add("/m/used/carvaluation/");
               
                List<AppSiteDetail> details = new List<AppSiteDetail>();
                List<string> apps = new List<string>();
                details.Add(new AppSiteDetail() { AppID = "98HA255XLU.com.carwale.Carwale", Paths = paths });
                
                AppSiteDetailAppLinks appLinks = new AppSiteDetailAppLinks();
                appLinks.Details = details;
                appLinks.Apps = apps;
                objAppSiteAssociationEntity.AppLinks = appLinks;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AppSiteAssociation.Get()");
                objErr.LogException();
            }
            return objAppSiteAssociationEntity;
        }

    }
}
