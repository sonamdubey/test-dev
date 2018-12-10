IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ViewNewCarDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ViewNewCarDealers]
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
-- =============================================
CREATE PROCEDURE [dbo].[ViewNewCarDealers]  @MakeId INT
	,@CityIds VARCHAR(1000)
	,@StateId INT
	,@SortExpression VARCHAR(100)
	,@SortDirection VARCHAR(100)
	,@ApplicationId INT
AS
BEGIN
	SELECT D.Id
		,TDM.MakeId
		,CM.NAME AS MakeName
		,D.Organization AS [Name]
		,C.NAME AS CityName
		,CONVERT(BIT, 1) AS IsNCDDealer
		,- 1 TcDealerId
		,- 1 NCSId
		,D.MobileNo PrimaryMobileNo
		,D.WebsiteUrl WebSite
		,D.LastUpdatedOn LastUpdated
		,D.EmailId
		,D.ContactHours
		,D.PhoneNo LandLineNo
		,D.LandlineCode
		,D.FaxNo
		,ISNULL(A.NAME, '') DealerArea
		,ISNULL(O.UserName, '') AS LastUpdatedBy
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
			@StateId = - 1
			OR D.StateId = @StateId
			)
		AND (
			@MakeId = - 1
			OR TDM.MakeId = @MakeId
			)
	INNER JOIN CarMakes AS CM WITH (NOLOCK) ON TDM.MakeId = CM.ID
	INNER JOIN Cities AS C WITH (NOLOCK) ON D.CityId = C.ID
	LEFT OUTER JOIN Areas A WITH (NOLOCK) ON D.AreaId = A.ID
	Left JOIN OprUsers O WITH(NOLOCK) ON D.DealerLastUpdatedBy = O.Id -- added by Kartik  Fetched LastUpdatedBy Field 
	ORDER BY CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortExpression = 'MakeName'
				THEN CM.NAME
			END ASC
		,CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortExpression = 'CityName'
				THEN C.NAME
			END ASC
		,CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortExpression = 'Name'
				THEN D.Organization
			END ASC
		,CASE 
			WHEN @SortDirection = 'ASC'
				AND @SortExpression = 'LastUpdated'
				THEN D.LastUpdatedOn
			END ASC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortExpression = 'MakeName'
				THEN CM.NAME
			END DESC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortExpression = 'CityName'
				THEN C.NAME
			END DESC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortExpression = 'Name'
				THEN D.Organization
			END DESC
		,CASE 
			WHEN @SortDirection = 'DESC'
				AND @SortExpression = 'LastUpdated'
				THEN D.LastUpdatedOn
			END DESC
END

