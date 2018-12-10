IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[WeeklyReport_Event]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[WeeklyReport_Event]
GO

	






--Summary							: Weekly Report Event Based of PQ, TD, Booking ,VIN details
--Author							: Dilip V. 05-Nov-2012
--Modification						: 1. 
CREATE PROCEDURE [CRM].[WeeklyReport_Event]
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@OrgId			BIGINT = NULL,
	@Node			NVARCHAR(4000) = NULL,
	@ExecId			TINYINT = NULL,
	@MakeGrpId		NUMERIC(18,0) = NULL,
	@DealerCoId		NUMERIC(18,0) = NULL,
	@MakeId			NUMERIC(18,0) = NULL,
	@ModelId		NUMERIC(18,0) = NULL,
	@SubOrgId		NUMERIC(18,0) = NULL
	
				
 AS 
 BEGIN
	SET NOCOUNT ON;
	DECLARE	@varsql	VARCHAR(2000),
			@SingleQuotes		VARCHAR(1) = ''''
			
	IF OBJECT_ID('tempdb..#Dealer') IS NOT NULL DROP TABLE #Dealer
	
	CREATE TABLE #Dealer(DealerId BIGINT)
	
	
	IF(@OrgId IS NOT NULL)
	BEGIN
		INSERT INTO #Dealer
		EXEC [CRM].[GetDealerIdFromOrgId] @OrganisationId = @OrgId
	END
	ELSE IF(@Node IS NOT NULL)
	BEGIN
		INSERT INTO #Dealer
		EXEC [CRM].[GetDealerIdFromNodeCode] @NodeCode = @Node, @ExecutiveId = @ExecId,@SubOrganisation = @SubOrgId			
	END
	ELSE IF(@MakeGrpId IS NOT NULL)
	BEGIN
		INSERT INTO #Dealer
		EXEC [CRM].[GetDealerIdFromMakeGroup] @MakeGroupId = @MakeGrpId		
	END
	ELSE IF(@DealerCoId IS NOT NULL)
	BEGIN
		INSERT INTO #Dealer
		EXEC [CRM].[GetDealerIdFromDealerCoordinator] @DealerCoordinator = @DealerCoId	
	END
			
	SET @varsql = 'SELECT COUNT(DISTINCT CEL.ItemId) Cnt,EventType,CII.ClosingProbability 
	FROM CRM_EventLogs CEL WITH (NOLOCK) 
	INNER JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CEL.ItemId
	INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID = CDA.CBDId
	INNER JOIN vwMMV VW ON VW.VersionId = CBD.VersionId 
	INNER JOIN CRM_InterestedIn AS CII WITH (NOLOCK) ON CBD.LeadId = CII.LeadId 
	INNER JOIN CRM_Leads AS CL WITH (NOLOCK) ON CII.LeadId = CL.ID 
	INNER JOIN #Dealer AS DL ON CDA.DealerId = DL.DealerId
	WHERE CEL.EventType IN (36,39,44,16,40,41,42,50,60,61,49,7,14,15) 
	AND CEL.EventDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+' 
	AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+'
	AND CII.ProductTypeId = 1'
	
	IF(@ModelId IS NULL)
		BEGIN
			IF(@MakeId IS NOT NULL)
				SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @MakeId, 101)
		END
	ELSE
		SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @ModelId, 101)
	
	SET @varsql += ' GROUP BY EventType,CII.ClosingProbability'
	
	--PRINT (@varsql)
	EXEC (@varsql)
	
	SET @varsql = 'SELECT COUNT(DISTINCT CCBL.CBDId) Cnt,CII.ClosingProbability
    FROM CRM_CarBookingLog AS CCBL WITH (NOLOCK)
    INNER JOIN CRM_CarDealerAssignment CDA1 WITH (NOLOCK) ON CCBL.CBDId = CDA1.CBDId
    INNER JOIN #Dealer DL ON CDA1.DealerId = DL.DealerId
    INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CDA1.CBDId = CBD.ID
    INNER JOIN CRM_EventLogs CEL WITH (NOLOCK) ON CBD.ID = CEL.ItemId
    INNER JOIN CRM_Leads AS CL WITH (NOLOCK) ON CBD.LeadId = CL.ID
    INNER JOIN CRM_InterestedIn AS CII WITH (NOLOCK) ON CL.Id = CII.LeadId
    INNER JOIN vwMMV VW ON VW.VersionId = CBD.VersionId
    WHERE CCBL.IsBookingNotPossible = 1'
    IF(@ModelId IS NULL)
		BEGIN
			IF(@MakeId IS NOT NULL)
				SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @MakeId, 101)
		END
	ELSE
		SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @ModelId, 101)
    SET @varsql += ' AND CCBL.CBDId NOT IN
    (SELECT CBDId FROM CRM_CarBookingLog WITH (NOLOCK) WHERE CBDId = CCBL.CBDId AND IsBookingCompleted = 1)
    AND CCBL.CBDId IN
    ( SELECT DISTINCT CEL.ItemId
    FROM CRM_EventLogs CEL WITH (NOLOCK)
    INNER JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CEL.ItemId = CDA.CBDId
    INNER JOIN #Dealer DL ON CDA.DealerId = DL.DealerId
    WHERE CEL.EventType = 16
    AND CEL.EventDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+' 
	AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+')
    GROUP BY CII.ClosingProbability'
	
	--PRINT (@varsql)
	EXEC (@varsql)
	
	DROP TABLE #Dealer
 END










