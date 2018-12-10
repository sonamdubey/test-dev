IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SyndicationXMLFeedRenault ]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SyndicationXMLFeedRenault ]
GO

	 
-- Created by : Kritika Choudhary on 7th March 2016, get stock details for renault dealers
-- 	exec TC_SyndicationXMLFeedRenault  
-- =============================================
CREATE  PROCEDURE [dbo].[TC_SyndicationXMLFeedRenault ]  
AS
   
BEGIN 
	SELECT DISTINCT RD.DealerId, D.Organization, Vw.Make AS CarMake,Vw.Model AS CarModel,Vw.Version AS CarVersion,S.MakeYear,S.Id AS StockId, 
	S.RegNo RegistartionNo, S.IsParkNSale, S.Kms, S.Colour, CC.RegistrationPlace , CC.OneTimeTax, CC.Insurance, CC.InsuranceExpiry, CC.InteriorColor, 
	CC.CityMileage, CC.AdditionalFuel, CC.CarDriven, S.PurchaseCost, S.RefurbishmentCost, S.Price Price, S.IsSychronizedCW, S.InterestRate,
	S.LoanToValue,S.LoanAmount,S.Tenure, S.OtherCharges,S.EMI,CC.Accidental, CC.FloodAffected, S.CertificationId, CC.Owners, CC.ACCondition,CC.EngineCondition,
	CC.SuspensionsCondition,CC.ExteriorCondition, CC.BrakesCondition,CC.BatteryCondition,CC.TyresCondition,CC.OverallCondition,CC.ElectricalsCondition, CC.SeatsCondition,
	CC.InteriorCondition,CC.Warranties,CC.Modifications,CC.Comments,CA.IsCarInWarranty, CA.HasExtendedWarranty,CA.HasAnyServiceRecords,CA.HasRSAAvailable,
	CA.HasFreeRSA,dbo.TC_GetStockInstalledFeatures(S.VersionId,S.Id) AS InstalledFeatures, 'https://www.youtube.com/watch?v=' + CV.VideoUrl AS VideoUrl,
	 --comma seperated images
	STUFF ((                                                     
                    SELECT ','+ (CP.HostUrl  + CP.ImageUrlFull)  
                    FROM TC_CarPhotos CP WITH(NOLOCK)
                    WHERE CP.StockId= S.Id AND CP.IsActive=1 AND CP.IsMain=0
                    FOR XML PATH('') 
                    ),1,1,'' )AS  ImageUrlFull,
	(CP.HostUrl + CP.ImageUrlFull) AS MainImage
	FROM	TC_RenaultDealers RD WITH(NOLOCK)
			JOIN Dealers D WITH(NOLOCK) ON D.ID = RD.DealerId
			JOIN TC_Stock S WITH(NOLOCK) ON RD.DealerId = S.BranchId
			JOIN vwAllMMV Vw WITH(NOLOCK) ON Vw.VersionId = S.VersionId
			JOIN TC_CarCondition CC WITH(NOLOCK) ON CC.StockId = S.Id
			JOIN TC_StockStatus SS WITH(NOLOCK) ON SS.Id = S.StatusId
			JOIN TC_CarAdditionalInformation CA WITH(NOLOCK) ON CA.StockId = S.Id
			LEFT JOIN TC_CarVideos CV WITH(NOLOCK) ON CV.StockId = S.Id AND CV.IsActive=1 AND CV.IsMain=1
			LEFT JOIN TC_CarPhotos CP WITH(NOLOCK) ON CP.StockId= S.Id AND CP.IsActive=1 AND CP.IsMain=1
	WHERE	S.IsActive=1
			AND SS.IsActive=1
			AND Vw.ApplicationId=1
			AND S.StatusId=1
END





