IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqDealers]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 12/10/2015
-- Description:	Fetch the active,inactive and all pq dealers based on makeId and Category passed
-- @Category = 0 => All dealers
--			 = 1 => Active dealers
--			 = 2 => Inactive dealers
-- Exec [GetPqDealers] -1,-1,'',1
-- =============================================
CREATE PROCEDURE [dbo].[GetPqDealers]
	-- Add the parameters for the stored procedure here
	@MakeId VARCHAR(10) = NULL
	,@StateId INT
	,@CityIds VARCHAR(MAX)
	,@Category VARCHAR(10)
AS
BEGIN
	CREATE TABLE #allDealers (
		Id INT
		,Organization VARCHAR(150)
		,IsDealerDeleted BIT
		,TC_DealerTypeId TINYINT
		)

	INSERT INTO #allDealers (
		Id
		,Organization
		,IsDealerDeleted
		,TC_DealerTypeId
		)
	SELECT Id
		,Organization
		,IsDealerDeleted
		,TC_DealerTypeId
	FROM dealers WITH (NOLOCK)
	WHERE (
			(
				StateId = @StateId
				AND @StateId != - 1
				)
			OR @StateId = - 1
			)
		AND (
			(
				CityId IN (
					SELECT items
					FROM [SplitText](@CityIds, ',')
					)
				AND @CityIds != ''
				)
			OR @CityIds = ''
			)
		AND IsDealerDeleted = 0
		AND IsTCDealer = 1
		AND TC_DealerTypeId IN (
			2
			,3
			)

	CREATE TABLE #dealers (
		CampaignId INT
		,ContractId INT
		,DealerId INT
		,DealerName VARCHAR(150)
		,IsDealerDeleted BIT
		,TC_DealerTypeId TINYINT
		,IsActive BIT
		,IsCampaignRunning BIT
		,Phone VARCHAR(100)
		,DealerEmailId VARCHAR(500)
		,StartDate VARCHAR(100)
		,EndDate VARCHAR(100)
		,[Type] VARCHAR(100)
		,TotalGoal INT
		,TotalCount INT
		,DailyGoal INT
		,DailyCount INT
		,LeadPanel VARCHAR(100)
		,[Status] VARCHAR(100)
		)

	INSERT INTO #dealers (
		CampaignId
		,ContractId
		,DealerId
		,DealerName
		,IsDealerDeleted
		,TC_DealerTypeId
		,IsActive
		,IsCampaignRunning
		,Phone
		,DealerEmailId
		,StartDate
		,EndDate
		,[Type]
		,TotalGoal
		,TotalCount
		,DailyGoal
		,DailyCount
		,LeadPanel
		,[Status]
		)
	EXEC FindCampaign - 1
		,@StateId
		,@CityIds
		,@MakeId
		,False
		,'DealerName'
		,'ASC'

	IF (@Category = 0)
	BEGIN
		SELECT DISTINCT ID AS Value
			,(Organization + ' - ' + CONVERT(VARCHAR, ID)) AS [Text]
		FROM #allDealers WITH (NOLOCK)
		ORDER BY [Text]
	END
	ELSE
	BEGIN
		IF (@Category = 1)
		BEGIN
			SELECT DISTINCT dl.DealerId AS Value
				,(dl.DealerName + ' - ' + CONVERT(VARCHAR, dl.DealerId)) AS [Text]
			FROM #dealers dl WITH (NOLOCK)
			WHERE IsCampaignRunning = 1
			ORDER BY [Text]
		END

		IF (@Category = 2)
		BEGIN
			SELECT ID AS Value
				,(Organization + ' - ' + CONVERT(VARCHAR, ID)) AS [Text]
			FROM #allDealers WITH (NOLOCK)
			WHERE ID NOT IN (
					SELECT DISTINCT dl.DealerId
					FROM #dealers dl WITH (NOLOCK)
					WHERE IsCampaignRunning = 1
					)
			ORDER BY [Text]
		END
	END

	DROP TABLE #dealers
END


