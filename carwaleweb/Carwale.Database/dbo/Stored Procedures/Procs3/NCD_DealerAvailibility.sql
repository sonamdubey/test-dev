IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_DealerAvailibility]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_DealerAvailibility]
GO

	-- =============================================
-- Author	:	Sachin Bharti(18th Aug 2014)
-- Description	:	Check availability for Dealers based on mobileNo OR
--					EmailId OR Dealer name
--					and TCDealerID (Dealers table Id)
-- Exec [dbo].[NCD_DealerAvailibility] 110,10055
-- =============================================
CREATE PROCEDURE [dbo].[NCD_DealerAvailibility]
	
	@TCDealerID		INT	= NULL,
	@DealerCampaignId	INT	=	NULL
AS
BEGIN

	DECLARE @DlrCityId	INT
	DECLARE @DealerMake INT

	IF @TCDealerID IS  NOT NULL AND @TCDealerID <> -1
		BEGIN
		
			SELECT @DealerMake = MakeId FROM RVN_DealerPackageFeatures(NOLOCK) WHERE DealerPackageFeatureID = @DealerCampaignId
			select @DlrCityId = CityId From Dealers where Id = @TCDealerID

			--Get dealer's data from Dealers table for @TC_DealerId
			SELECT 
				D.ID,
				D.Organization+'( '+Cast(D.ID as nVarchar(10))+' )'+' - '+
				Cast(D.EmailId AS Varchar(500))+' - '+
				Cast(D.MobileNo AS Varchar(100))+' - '+
				CASE WHEN ISNULL(D.Status,1) = 0 THEN 'Active' WHEN ISNULL(D.Status,1) = 1  THEN 'InActive' END	AS Text
			FROM 
				Dealers D(NOLOCK)
			WHERE 
				D.ID = @TCDealerID

			--Get mapped new car dealers for given TC_DealerId in Dealer_NewCar table
			SELECT 
				DN.ID ,
				DN.Name+' - '+DN.Address AS Text
			FROM 
				Dealer_NewCar DN(NOLOCK) 
				INNER JOIN Cities C(NOLOCK) ON C.ID = DN.CityId AND C.IsDeleted = 0
				INNER JOIN States S(NOLOCK) ON S.ID =C.StateId AND S.IsDeleted = 0
			WHERE  
				DN.CityId = @DlrCityId --AND DN.MakeId = @DealerMake 
				--AND DN.CampaignId IS NULL
			ORDER BY Text
		
			--check if campaign is already mapped with any new car dealer
			SELECT 
				DN.Id,
				DN.EMailId,
				DN.DealerMobileNo,
				Case WHEN DN.IsActive = 1 THEN 'Active' ELSE 'InActive' END IsDLrActive,
				DN.CampaignId,
				DN.Name+'( '+Cast(DN.ID AS Varchar(10)) +' )'+' - '+
				Cast(DN.EmailId AS Varchar(500))+' - '+
				Cast(ISNULL(DN.DealerMobileNo,'') AS Varchar(100))+' - '+
				CASE WHEN ISNULL(RVN.PackageStatus,0) = 2 THEN 'Campaign Running' 
					WHEN ISNULL(RVN.PackageStatus,0) = 3 THEN 'Campaign Suspended'
					WHEN ISNULL(RVN.PackageStatus,0) = 4 THEN 'Campaign Delivered' END AS Text
			FROM
				Dealer_NewCar DN(NOLOCK)
				INNER JOIN RVN_DealerPackageFeatures RVN(NOLOCK) ON RVN.DealerPackageFeatureID = DN.CampaignId
				AND	DN.TcDealerId = @TCDealerID AND DN.CampaignId = @DealerCampaignId
		END

END


