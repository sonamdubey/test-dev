IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_UpdateDealerLocatorDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_UpdateDealerLocatorDetails]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(22nd Aug 2014)
-- Description	:	Update dealer details for Dealer Locator
-- Modifier	:	Sachin Bharti(2nd Feb 2015)
-- Modification :	Update column ShowroomStartTime and ShowroomEndTime in Dealer_NewCar table
-- =============================================
CREATE PROCEDURE [dbo].[NCD_UpdateDealerLocatorDetails]
	@TC_DealerId	INT,
	@NCD_DealerID	INT,
	@MaskingNumber	VARCHAR(10),
	@LandlineNumber	VARCHAR(100),
	@FAXNumber		VARCHAR(100),
	@Longitude		VARCHAR(50),
	@Lattitude		VARCHAR(50),
	@ContactPerson	VARCHAR(100),
	@ContactHours	VARCHAR(50),
	@StartContactHours	VARCHAR(50),
	@EndContactHours	VARCHAR(50),
	@WebsiteName	VARCHAR(100),
	@DealerCampaignID	INT,
	@LeadPanelDealerId	INT,
	@IsUpdated		SMALLINT OUTPUT		

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @IsUpdated = 0

	DECLARE @PackageStartDate	DATETIME
	DECLARE	@PackageEndDate		DATETIME
	DECLARE @PackageId			INT
	DECLARE @PackageStatus		SMALLINT
	DECLARE @IsPremium			BIT = 0
	DECLARE @IsCampaignActive	BIT = 0

	IF @TC_DealerId IS NOT NULL AND @NCD_DealerID IS NOT NULL
	BEGIN
		--Update TC Dealer Details
		--Update New Car Dealer Details
		IF @TC_DealerId <> -1 AND @NCD_DealerID <> -1
		BEGIN
			UPDATE Dealers SET	FaxNo = @FAXNumber , 
								Longitude = @Longitude , 
								Lattitude = @Lattitude , 
								ContactPerson = @ContactPerson ,
								ContactHours = @ContactHours ,
								ActiveMaskingNumber = @MaskingNumber,
								PhoneNo = @LandlineNumber,
								WebsiteUrl = @WebsiteName,
								HavingWebsite = 1,
								IsPremium = 1,
								PaidDealer = 1,
								Status = 0
					WHERE ID = @TC_DealerId

			--Get package information from RVN_DealerPackageFeatures table
			SELECT	
					@PackageStartDate	= RVN.PackageStartDate,
					@PackageEndDate		= RVN.PackageEndDate,
					@PackageId			= RVN.PackageId,
					@PackageStatus	= ISNULL(RVN.PackageStatus,0)
			FROM 
					RVN_DealerPackageFeatures RVN(NOLOCK) 
			WHERE 
					RVN.DealerPackageFeatureID = @DealerCampaignID
						
			--If package is Dealer locator Mass . Dealer is not premium.
			IF @PackageId = 43
				SET @IsPremium = 0
			--If package is Dealer Locator Premium . Dealer is premium.
			ELSE IF @PackageId = 56
				SET @IsPremium = 1	
			
			--check is campaign is running or not
			IF @PackageStatus = 2
				SET @IsCampaignActive = 1

			--set campaignId is null if it already mapped with any new car dealer
			SELECT Id FROM Dealer_NewCar WHERE TcDealerId = @TC_DealerId AND CampaignId = @DealerCampaignID
			IF @@ROWCOUNT > 1
			BEGIN
				UPDATE Dealer_NewCar SET CampaignId = NULL , IsCampaignActive = 0 
				WHERE TcDealerId = @TC_DealerId AND CampaignId = @DealerCampaignID
			END

			--now update data in Dealer_NewCar table
			UPDATE Dealer_NewCar SET 
								Mobile = @MaskingNumber , 
								Longitude = @Longitude , 
								Latitude = @Lattitude , 
								ContactPerson = @ContactPerson,
								ContactNo = @LandlineNumber,
								FaxNo = @FAXNumber,
								WebSite = @WebsiteName,
								WorkingHours = @ContactHours,
								ShowroomStartTime = @StartContactHours,
								ShowroomEndTime = @EndContactHours,
								IsNCD = 1,
								IsActive = 1,
								LeadPanelDealerId	=	@LeadPanelDealerId,
								IsCampaignActive = @IsCampaignActive,
								PackageStartDate = @PackageStartDate,
								PackageEndDate   = @PackageEndDate,
								CampaignId	     = @DealerCampaignID,
								IsPremium        = @IsPremium
					WHERE ID = @NCD_DealerID
			--if data is updated successfully
			IF @@ROWCOUNT <> 0
			 SET @IsUpdated = 1
		END
	END
END

