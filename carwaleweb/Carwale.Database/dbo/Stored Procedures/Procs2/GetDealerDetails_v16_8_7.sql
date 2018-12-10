IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerDetails_v16_8_7]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerDetails_v16_8_7]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 27/01/2016
-- Description:	Get the complete dealer details 
-- Modified : Vicky Lund, 23/08/2016, Use TC_NoDealerModels source column
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerDetails_v16_8_7]
	-- Add the parameters for the stored procedure here
	@DealerId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	WITH dealermodelsCTE
	AS (
		SELECT ID AS ModelId
		FROM CarModels CM WITH (NOLOCK)
		WHERE CarMakeId IN (
				SELECT MakeId
				FROM TC_DealerMakes WITH (NOLOCK)
				WHERE DealerId = @DealerId
				)
			AND CM.New = 1
			AND CM.IsDeleted = 0
		
		EXCEPT
		
		SELECT ModelId
		FROM TC_NoDealerModels WITH (NOLOCK)
		WHERE DealerId = @DealerId
		AND [Source] = 1
		)
	-- Insert statements for procedure here
	SELECT DISTINCT STUFF((
				SELECT ',' + CONVERT(VARCHAR, ModelId)
				FROM dealermodelsCTE WITH (NOLOCK)
				FOR XML PATH('')
				), 1, 1, '') AS ModelId
		,TD.LoginId AS LoginId
		,Passwd
		,TD.FirstName
		,TD.LastName
		,IsWKitSent
		,IsTCTrainingGiven
		,TD.EmailId AS EmailId
		,TD.Organization AS Organization
		,TD.Address1 AS Address1
		,TD.Address2 AS Address2
		,A.Id AS AreaId
		,C.Id AS CityId
		,S.Id AS StateId
		,A.NAME AS Area
		,C.NAME AS City
		,S.NAME AS STATE
		,TD.Pincode AS Pincode
		,STATUS
		,TD.PhoneNo AS PhoneNo
		,TD.FaxNo AS FaxNo
		,TD.MobileNo AS MobileNo
		,TD.JoiningDate AS JoiningDate
		,TD.ExpiryDate AS ExpiryDate
		,TD.WebsiteUrl AS WebsiteUrl
		,TD.ContactPerson AS ContactPerson
		,TD.BPContactPerson AS BPContactPerson
		,TD.ContactHours AS ContactHours
		,TD.ContactEmail AS ContactEmail
		,TD.LogoUrl AS LogoUrl
		,TD.BPMobileNo AS BPMobileNo
		,TD.IsTCDealer
		,DD.StockCarsCount
		,DD.SellingInMonth
		,DD.UsingPc
		,DD.UsingSoftware
		,DD.UsingTradingCars
		,DD.StockSegment
		,DD.OrganizationSize
		,DD.BusinessType
		,DD.FirmType
		,DD.NumberOfOutlets
		,DD.ServicesOffered
		,DD.AdvertiseOn
		,DD.AnnualMarketingSpend
		,DD.AnnualTurnover
		,DD.Challenges
		,DD.Softwares
		,DD.OtherPartners
		,DD.AnniversaryOn
		,DD.IsNewDealership
		,DD.RetailChain
		,DD.NumberOfPC
		,TD.Lattitude
		,TD.Longitude
		,TD.WebsiteContactPerson
		,TD.WebsiteContactMobile
		,TD.ActiveMaskingNumber
		,TD.TC_DealerTypeId
		,TD.IsWarranty
		,TD.OutletCnt
		,TD.LeadServingDistance
		,TD.AutoInspection
		,TD.RCNotMandatory
		,TD.OwnerMobile AS OwnerMobile
		,TD.LegalName AS LegalName
		,TD.PanNumber AS PanNumber
		,TD.TanNumber AS TanNumber
		,TD.AutoClosed AS AutoClosed
		,STUFF((
				SELECT ',' + CONVERT(VARCHAR, MakeId)
				FROM TC_DealerMakes WITH (NOLOCK)
				WHERE DealerId = @DealerId
				FOR XML PATH('')
				), 1, 1, '') AS DealerMake
	FROM Dealers AS TD WITH (NOLOCK)
	INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = TD.CityId
	INNER JOIN States AS S WITH (NOLOCK) ON S.ID = TD.StateId
	LEFT JOIN Areas AS A WITH (NOLOCK) ON A.ID = TD.AreaId
	LEFT JOIN DealerDetails AS DD WITH (NOLOCK) ON DD.DealerID = TD.ID
	WHERE TD.ID = @DealerId
END

