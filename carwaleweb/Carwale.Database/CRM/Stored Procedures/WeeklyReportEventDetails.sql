IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[WeeklyReportEventDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[WeeklyReportEventDetails]
GO

	



--Summary							: PQ, TD, Booking , Event
--Author							: Dilip V. 10-Oct-2012
--Modification history				: 1. 
CREATE PROCEDURE [CRM].[WeeklyReportEventDetails]
	@SiteId			TINYINT,
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@OrgId			BIGINT = NULL,
	@Node			NVARCHAR(4000) = NULL,
	@ExecId			TINYINT = NULL,
	@MakeGrpId		NUMERIC(18,0) = NULL,
	@DealerCoId		NUMERIC(18,0) = NULL,
	@MakeId			NUMERIC(18,0) = NULL,
	@ModelId		NUMERIC(18,0) = NULL,
	@SubOrgId		NUMERIC(18,0) = NULL,
	@EagernessId	TINYINT = NULL,
	@Type			SMALLINT
	
				
 AS 
 BEGIN
	SET NOCOUNT ON;
		
	DECLARE	@varsql				VARCHAR(3000),
			@SingleQuotesTwice	VARCHAR(2) = '''''',
			@SingleQuotes		VARCHAR(1) = '''';
			
	IF OBJECT_ID('tempdb..#Dealer') IS NOT NULL DROP TABLE #Dealer;
	
	CREATE TABLE #Dealer(DealerId BIGINT);
			
	IF(@OrgId IS NOT NULL)
	BEGIN
		INSERT INTO #Dealer
		EXEC [CRM].[GetDealerIdFromOrgId] @OrganisationId = @OrgId;
	END
	ELSE IF(@Node IS NOT NULL)
	BEGIN
		INSERT INTO #Dealer
		EXEC [CRM].[GetDealerIdFromNodeCode] @NodeCode = @Node, @ExecutiveId = @ExecId,@SubOrganisation = @SubOrgId;			
	END
	ELSE IF(@MakeGrpId IS NOT NULL)
	BEGIN
		INSERT INTO #Dealer
		EXEC [CRM].[GetDealerIdFromMakeGroup] @MakeGroupId = @MakeGrpId;	
	END
	ELSE IF(@DealerCoId IS NOT NULL)
	BEGIN
		INSERT INTO #Dealer
		EXEC [CRM].[GetDealerIdFromDealerCoordinator] @DealerCoordinator = @DealerCoId;	
	END
			
	IF (@Type = 11)
	BEGIN
	SET @varsql= 'WITH CP AS (SELECT DISTINCT CCBL.CreatedOn EventOn,CC.Id,(CC.FirstName +'+@SingleQuotes+' '+@SingleQuotes+'+ CC.LastName) CustomerName,
	CC.Email,CC.Mobile,CC.Landline,C.Name CityName,ND.Name Dealer,CDA1.Comments,CDA1.LostDate,CDA1.LostName,CDA1.ReasonLost,
	(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName,	       
	ROW_NUMBER() OVER (PARTITION BY CCBL.CBDId ORDER BY CCBL.CreatedOn DESC) AS RowNumber,
    CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName,
    CCBL.CBDId,ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode'
    IF(@SiteId = 1)
		SET @varsql += ',ND.ID Did,CL.Id AS LeadId,CAC.Name As CarCity'
	IF(@SiteId = 2)
	BEGIN
		SET @varsql += ',CONVERT(VARCHAR(15),CDA1.CreatedOn,106) AS DealerAssigned,
		CCPL.PQRequestDate ,CCPL.PQCompleteDate,CCTL.TDRequestDate ,CCTL.TDCompleteDate,
		CASE WHEN CII.ClosingProbability = 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' 
		WHEN CII.ClosingProbability = 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+' 
		WHEN CII.ClosingProbability = 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' 
		WHEN CII.ClosingProbability = 4 THEN '+@SingleQuotes+'Low'+@SingleQuotes+' END AS ClosingProbability'
	END	
    SET @varsql += ' FROM CRM_CarBookingLog AS CCBL WITH (NOLOCK)
    INNER JOIN CRM_CarDealerAssignment CDA1 ON CCBL.CBDId = CDA1.CBDId
    INNER JOIN #Dealer DL ON CDA1.DealerId = DL.DealerId
    INNER JOIN NCS_Dealers ND WITH (NOLOCK) ON ND.ID = CDA1.DealerId
    INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CDA1.CBDId = CBD.ID
    INNER JOIN vwMMV VW ON VW.VersionId = CBD.VersionId
    INNER JOIN CRM_Leads AS CL WITH (NOLOCK) ON CBD.LeadId = CL.ID
    INNER JOIN CRM_InterestedIn AS CII WITH (NOLOCK) ON CL.Id = CII.LeadId AND CII.ProductTypeId = 1
    INNER JOIN CRM_Customers CC WITH (NOLOCK) ON CL.CNS_CustId = CC.ID
    INNER JOIN Cities C WITH (NOLOCK) ON CC.CityId = C.Id
    LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId'
    IF(@SiteId = 1)
		SET @varsql += ' LEFT JOIN Cities CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID'
	IF(@SiteId = 2)
    BEGIN
		SET @varsql += ' LEFT JOIN CRM_CarTDLog CCTL WITH (NOLOCK) ON CBD.ID = CCTL.CBDId
        LEFT JOIN CRM_CarPQLog CCPL WITH (NOLOCK) ON CBD.ID = CCPL.CBDId'
    END    
    SET @varsql += ' WHERE CCBL.IsBookingNotPossible = 1'
            
    IF (@EagernessId = 1)
        SET @varsql += ' AND CII.ClosingProbability IN (1,2)'
    ELSE IF (@EagernessId = 2)
        SET @varsql += ' AND CII.ClosingProbability = 3'
    ELSE IF (@EagernessId = 3)
        SET @varsql += ' AND CII.ClosingProbability = 4'
    ELSE IF (@EagernessId = 4)
        SET @varsql += ' AND CII.ClosingProbability = -1'

	IF (@MakeId IS NOT NULL)
			SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @MakeId, 101)

	IF (@ModelId IS NOT NULL)
		SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @ModelId, 101)
		
    SET @varsql += ' AND CCBL.CBDId NOT IN
    (SELECT CBDId FROM CRM_CarBookingLog WITH (NOLOCK) WHERE CBDId = CCBL.CBDId AND IsBookingCompleted = 1)
    AND CCBL.CBDId IN (
    SELECT DISTINCT CEL.ItemId
    FROM CRM_EventLogs CEL WITH (NOLOCK)
    INNER JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CEL.ItemId = CDA.CBDId
    INNER JOIN #Dealer DL ON CDA.DealerId = DL.DealerId
    WHERE CEL.EventType = 16
    AND CEL.EventDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+' 
    AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
    )) SELECT Id,CustomerName,Email,Mobile,Landline,
    CityName,CarName,Dealer,EventOn,SourceName,
    Comments,LostDate,LostName,ReasonLost,DealerCode'
    IF(@SiteId = 1)
		SET @varsql += ',Did,LeadId,CarCity'
	IF(@SiteId = 2)
		SET @varsql += ',DealerAssigned,PQRequestDate,PQCompleteDate,TDRequestDate,TDCompleteDate,ClosingProbability'
    SET @varsql += ' FROM CP WHERE RowNumber = 1
    ORDER BY EventOn'
	END
	ELSE	
	BEGIN		
	SET @varsql= 'WITH CP AS (SELECT DISTINCT CEL.EventOn,CC.Id,(CC.FirstName + + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName,
	CC.Email,CC.Mobile,CC.Landline,C.Name CityName,ND.Name Dealer,CDA.Comments,CDA.LostDate,CDA.LostName,CDA.ReasonLost,
	(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName,
	ROW_NUMBER() OVER (PARTITION BY CEL.ItemId,CEL.EventType ORDER BY CEL.EventOn DESC) AS RowNumber,
	CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName,
	CEL.ItemId,ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode'
    IF(@SiteId = 1)
		SET @varsql += ',ND.ID Did,CL.Id AS LeadId,CAC.Name As CarCity'
	IF(@SiteId = 2)
	BEGIN
		SET @varsql += ',CONVERT(VARCHAR(15),CDA.CreatedOn,106) AS DealerAssigned,
		CCPL.PQRequestDate ,CCPL.PQCompleteDate,CCTL.TDRequestDate ,CCTL.TDCompleteDate,
		CASE WHEN CII.ClosingProbability = 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' 
		WHEN CII.ClosingProbability = 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+' 
		WHEN CII.ClosingProbability = 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' 
		WHEN CII.ClosingProbability = 4 THEN '+@SingleQuotes+'Low'+@SingleQuotes+' END AS ClosingProbability'
	END	
    SET @varsql += ' FROM CRM_EventLogs CEL WITH (NOLOCK)
    INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID = CEL.ItemId
    INNER JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CBD.ID
    INNER JOIN #Dealer DL ON CDA.DealerId = DL.DealerId
    INNER JOIN vwMMV VW ON VW.VersionId = CBD.VersionId
    INNER JOIN CRM_Leads AS CL WITH (NOLOCK) ON CBD.LeadId = CL.ID
    INNER JOIN CRM_Customers CC WITH (NOLOCK) ON CL.CNS_CustId = CC.ID
    INNER JOIN Cities C WITH (NOLOCK) ON CC.CityId = C.Id
    INNER JOIN NCS_Dealers ND WITH (NOLOCK) ON ND.ID = CDA.DealerId
    INNER JOIN CRM_InterestedIn AS CII WITH (NOLOCK) ON CL.Id = CII.LeadId AND CII.ProductTypeId = 1
    LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId'
    IF(@SiteId = 1)
		SET @varsql += ' LEFT JOIN Cities CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID'
	IF(@SiteId = 2)
    BEGIN
		SET @varsql += ' LEFT JOIN CRM_CarTDLog CCTL WITH (NOLOCK) ON CBD.ID = CCTL.CBDId
        LEFT JOIN CRM_CarPQLog CCPL WITH (NOLOCK) ON CBD.ID = CCPL.CBDId'
    END    
    SET @varsql += ' WHERE CEL.EventDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+' 
        AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes
    IF (@Type = 1)
		SET @varsql += ' AND CEL.EventType = 39'
    ELSE IF(@Type = 2)
		SET @varsql += ' AND CEL.EventType IN (41,60,61)'
    ELSE IF(@Type = 3)      
        SET @varsql += ' AND CEL.EventType IN (40,42,50)'
    ELSE IF(@Type = 4)      
        SET @varsql += ' AND CEL.EventType = 16'
    ELSE IF(@Type = 5)                
		SET @varsql += ' AND CEL.EventType = 44'
    ELSE IF(@Type = 6)
		SET @varsql += ' AND CEL.EventType = 36'
    ELSE IF(@Type = 7)
        SET @varsql += ' AND CEL.EventType = 49'
    ELSE IF(@Type = 8)
        SET @varsql += ' AND CEL.EventType = 7'
    ELSE IF(@Type = 9)  
        SET @varsql += ' AND CEL.EventType = 14'
    ELSE IF(@Type = 10) 
        SET @varsql += ' AND CEL.EventType = 15'
      
    IF (@EagernessId = 1)
        SET @varsql += ' AND CII.ClosingProbability IN (1,2)'
    ELSE IF (@EagernessId = 2)
        SET @varsql += ' AND CII.ClosingProbability = 3'
    ELSE IF (@EagernessId = 3)
        SET @varsql += ' AND CII.ClosingProbability = 4'
    ELSE IF (@EagernessId = 4)
       SET @varsql += ' AND CII.ClosingProbability = -1'

	IF (@MakeId IS NOT NULL)
        SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @MakeId, 101)

    IF (@ModelId IS NOT NULL)
        SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @ModelId, 101)

    SET @varsql += ' ) SELECT Id,CustomerName,Email,Mobile,Landline,
        CityName,CarName,Dealer,EventOn,SourceName,
        Comments,LostDate,LostName,ReasonLost,DealerCode'
        IF(@SiteId = 1)
		SET @varsql += ',Did,LeadId,CarCity'
		IF(@SiteId = 2)
			SET @varsql += ',DealerAssigned,PQRequestDate,PQCompleteDate,TDRequestDate,TDCompleteDate,ClosingProbability'
        SET @varsql += ' FROM CP WHERE RowNumber = 1
        ORDER BY EventOn'
	END
	
	PRINT (@varsql)
	EXEC (@varsql)
	
	DROP TABLE #Dealer;
 END







