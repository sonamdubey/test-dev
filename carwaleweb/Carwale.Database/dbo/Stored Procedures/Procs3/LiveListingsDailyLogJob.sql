IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LiveListingsDailyLogJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LiveListingsDailyLogJob]
GO

	-- =============================================
-- Author:		Manish
-- Create date: 29-07-2013
-- Description: This SP will capture daily snapshot of livelisting at the start of the day
-- Modified By: Manish on 13-01-2014  changing the select query for inserting the classified_expiryDate data for individuals car
-- Modified By: Manish on 03-04-2014  adding column IsPremium,VideoCount
-- Modified By: Manish on 13-08-2014  adding column SortScore
-- Modified By: Fulsmita on 13-10-2014  adding column DealerId
-- Modified By: Manish on 20 Oct 2015 added column CustomerPackageId and also implenting capturing of snapshot of ConsumerCreditPoints table.
-- Modified By: Manish on 14 Dec 2015 Include sp for capturing snapshot of the PQ_DealerSponsored
-- Modified By: Manish on 18 Feb 2016 Include sp for capturing daily snapshot of the complete data for table TC_Deals_Stock and TC_Deals_StockVIN.
-- -- =============================================
CREATE PROCEDURE [dbo].[LiveListingsDailyLogJob]
AS 
BEGIN 
    
	 EXEC   [dbo].[TC_Deals_StockAndVin_DailyLogJob]
	 EXEC   [dbo].[ConsumerCreditPointDailyLog];     ----Line added by Manish on 20 Oct 2015
	 EXEC   [dbo].[usp_PQ_DealerSponsoredDailyLogs];  ----Line added by Manish on 14 Dec 2015
  
    INSERT INTO LiveListingsDailyLog
                                       (AsOnDate,
										ProfileId,
										SellerType,
										Seller,
										Inquiryid,
										MakeId,
										MakeName,
										ModelId,
										ModelName,
										VersionId,
										VersionName,
										StateId,
										StateName,
										CityId,
										CityName,
										AreaId,
										AreaName,
										Lattitude,
										Longitude,
										MakeYear,
										Price,
										Kilometers,
										Color,
										Comments,
										EntryDate,
										LastUpdated,
										PackageType,
										ShowDetails,
										Priority,
										PhotoCount,
										FrontImagePath,
										CertificationId,
										AdditionalFuel,
										IsReplicated,
										HostURL,
										CalculatedEMI,
										Score,
										Responses,
										CertifiedLogoUrl,
										Owners,
										InsertionDate,
										ClassifiedExpiryDate,   --- column added by manish on 13-01-2014
										IsPremium,        ----column added by manish on 03-04-2014
										VideoCount ,         ----column added by manish on 03-04-2014
										SortScore  ,          ----column added by manish on 13-08-2014
										DealerId,   ---Column added by Fulsmita on 13-10-2014
										CustomerPackageId  -- Column added by Manish on 20 Oct 2015
										)   
                    SELECT              CONVERT(DATE,GETDATE()),
										LL.ProfileId,
										LL.SellerType,
										LL.Seller,
										LL.Inquiryid,
										LL.MakeId,
										LL.MakeName,
										LL.ModelId,
										LL.ModelName,
										LL.VersionId,
										LL.VersionName,
										LL.StateId,
										LL.StateName,
										LL.CityId,
										LL.CityName,                                      
										LL.AreaId,
										LL.AreaName,
										LL.Lattitude,
										LL.Longitude,
										LL.MakeYear,
										LL.Price,
										LL.Kilometers,
										LL.Color,
										LL.Comments,
										LL.EntryDate,
										LL.LastUpdated,
										LL.PackageType,
										LL.ShowDetails,
										LL.Priority,
										LL.PhotoCount,
										LL.FrontImagePath,
										LL.CertificationId,
										LL.AdditionalFuel,
										LL.IsReplicated,
										LL.HostURL,
										LL.CalculatedEMI,
										LL.Score,
										LL.Responses,
										LL.CertifiedLogoUrl,
										LL.Owners,
										LL.InsertionDate,
										CSI.ClassifiedExpiryDate,
										LL.IsPremium,
										LL.VideoCount,
										LL.SortScore,
										LL.DealerId,
										CASE WHEN LL.SellerType=1 THEN CP.CustomerPackageId
										     WHEN LL.SellerType=2 THEN CSI.PackageId END AS CustomerPackageId
                            FROM livelistings AS LL WITH (NOLOCK) 
							LEFT JOIN CustomerSellInquiries  AS CSI WITH (NOLOCK)          -- Modified By: Manish on 13-01-2014  changing the select query for inserting the classified_expiryDate data for individuals car
							                     ON LL.Inquiryid=CSI.ID 
												    AND LL.SellerType=2
                            LEFT JOIN ConsumerCreditPoints AS CP WITH (NOLOCK)             ---- Left join added by Manish on 20 Oct 2015 for logging package id of the customer
							                      ON  CP.ConsumerId=LL.DealerId 
												  AND CP.ConsumerType=1 
												  AND LL.SellerType=1

         END
