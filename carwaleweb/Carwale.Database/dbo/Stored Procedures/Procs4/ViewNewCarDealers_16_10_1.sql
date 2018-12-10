IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ViewNewCarDealers_16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ViewNewCarDealers_16_10_1]
GO

	-- =============================================
-- Created by:		Vicky Lund
-- Creation date:	2-November-2015
-- Description:	
-- EXEC ViewNewCarDealers -1, '', -1, '', ''
-- Modified: Vicky, 24/11/2015, Handled areaId = 0 case
-- modified by Sanjay on 17/06/2016 added applicationId condition
-- Modified: Vicky Lund, 11/07/2016, Added column for Landline STD Code
-- Modifier : Kartik Rathod on 29 sept 2016, Fetched LastUpdatedBy Field 
-- Modified By : Chetan Thambad on <12/10/2016> Status (IsActive), ShowInDealerLocator, IsActiveCampaignExist against dealer 
-- Modified: Vicky Lund, 21/10/2016, Added check for premium dealer locator in isActiveCampaign flag
-- EXEC [ViewNewCarDealers_16_10_1] -1, 40, 1
-- =============================================
CREATE PROCEDURE [dbo].[ViewNewCarDealers_16_10_1] @MakeId INT
	,@CityIds VARCHAR(1000)
	,@ApplicationId INT
AS
BEGIN
	SELECT D.Id
		,D.Organization AS [Name]
		,TDM.MakeId
		,CM.NAME AS MakeName
		,C.NAME AS CityName
		,CONVERT(BIT, 1) AS IsNCDDealer
		,- 1 TcDealerId
		,- 1 NCSId
		,D.MobileNo
		,D.WebsiteUrl WebSite
		,D.LastUpdatedOn LastUpdated
		,D.EmailId
		,D.ContactHours
		,D.PhoneNo LandLineNo
		,D.LandlineCode
		,D.FaxNo
		,ISNULL(A.NAME, '') Area
		,ISNULL(O.UserName, '') AS LastUpdatedBy
		,CASE 
			WHEN EXISTS (
					SELECT DLC.DealerLocatorConfigurationId
					FROM DealerLocatorConfiguration DLC WITH (NOLOCK)
					WHERE DLC.DealerId = D.ID
						AND DLC.IsLocatorActive = 1
					)
				THEN CONVERT(BIT, 1)
			ELSE CONVERT(BIT, 0)
			END ShowInDealerLocator
		,D.[Status]
		,CASE 
			WHEN (
					vw.CampaignId IS NOT NULL
					OR VAC.CampaignId IS NOT NULL
					)
				THEN CONVERT(BIT, 1)
			ELSE CONVERT(BIT, 0)
			END IsActiveCampaignExist
	FROM Dealers AS D WITH (NOLOCK)
	INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON D.ID = TDM.DealerId
		AND D.TC_DealerTypeId = 2
		AND D.ApplicationId = @ApplicationId
		AND (
			@CityIds = ''
			OR D.CityId IN (
				SELECT SC.ListMember
				FROM dbo.fnSplitCSV(@CityIds) SC
				)
			)
		AND (
			@MakeId = - 1
			OR TDM.MakeId = @MakeId
			)
	INNER JOIN CarMakes AS CM WITH (NOLOCK) ON TDM.MakeId = CM.ID
	INNER JOIN Cities AS C WITH (NOLOCK) ON D.CityId = C.ID
	LEFT OUTER JOIN Areas A WITH (NOLOCK) ON D.AreaId = A.ID
	LEFT OUTER JOIN OprUsers O WITH (NOLOCK) ON D.DealerLastUpdatedBy = O.Id
	LEFT OUTER JOIN vwActiveCampaigns vw WITH (NOLOCK) ON vw.DealerId = D.Id
	LEFT OUTER JOIN DealerLocatorConfiguration DLC WITH (NOLOCK) ON DLC.DealerId = D.ID
	LEFT OUTER JOIN vwActiveCampaigns VAC WITH (NOLOCK) ON DLC.PQ_DealerSponsoredId = VAC.CampaignId
END
