IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[CRM_WeeklyReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[CRM_WeeklyReport]
GO

	
--Name of SP/Function				: CarWale.dbo.CRM_WeeklyReport
--Applications using SP				: CRM, Dealer and RM Panel 
--Modules using the SP				: WeeklyReportLead.cs
--Technical department				: Database
--Summary							: PQ, TD, Booking ,VIN details
--Author							: Dilip V. 02-Feb-2012
--Modification history				: 1. 
CREATE PROCEDURE [CRM].[CRM_WeeklyReport]
	@DealerId	VARCHAR(MAX),	
	@FromDate	DATETIME,
	@ToDate		DATETIME,
	@Model		NUMERIC
				
 AS 
 BEGIN
	SET NOCOUNT ON
	DECLARE	@varsql				VARCHAR(MAX),
			@SingleQuotesTwice	VARCHAR(2) = '''''',
			@SingleQuotes		VARCHAR(1) = ''''			 
	--PRINT (@SingleQuotesTwice)
	--PRINT (@SingleQuotes)
	
	--Gets Count of Assigned, Lost, Not Interested and Booked Leads
	SET @varsql =  'SELECT COUNT(DISTINCT CDA.Id) Cnt, CDA.Status, CL.LeadStageId,
		ISNULL(CCBD.BookingStatusId, -1) BookingStatusId ,CII.ClosingProbability 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK)
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
		INNER JOIN CRM_Leads AS CL WITH(NOLOCK) ON CBD.LeadId = CL.ID 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.Id = CII.LeadId 
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CBD.VersionID = CV.ID 
		LEFT JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CBD.ID
		WHERE CDA.CreatedOn BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+' 
		AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1 
		AND CDA.DealerId IN ('+@DealerId+')'
		
	IF(@Model <> -1)
		SET @varsql += ' AND CV.CarModelId = ' + CONVERT(CHAR(10), @Model, 101)
		
	SET @varsql += ' GROUP BY CDA.Status, CL.LeadStageId,CCBD.BookingStatusId,CII.ClosingProbability'	
	--PRINT (@varsql)
	EXEC (@varsql)	
	
   --Gets Count of PQ (Requested, Completed, Not Required, Avg Days)
	SET @varsql =  'SELECT COUNT(CCPL.id) TCount,CCPL.IsPQRequested,CCPL.IsPQCompleted,CCPL.IsPQNotRequired, 
		CII.ClosingProbability,DATEDIFF(day, CCPL.PQRequestDate, CCPL.PQCompleteDate) PQAvgDays 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CBD.VersionId = CV.Id 
		INNER JOIN CRM_CarPQLog CCPL WITH(NOLOCK) ON CCPL.CBDId = CBD.ID 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		WHERE CDA.CreatedOn BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+' 
		AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1 
		AND CDA.DealerId IN ('+@DealerId+')'

	IF(@Model <> -1)
		SET @varsql += ' AND CV.CarModelId = ' + CONVERT(CHAR(10), @Model, 101)
	
	SET @varsql += 'GROUP BY CCPL.IsPQRequested,CCPL.IsPQCompleted,CCPL.IsPQNotRequired,CII.ClosingProbability,CCPL.PQRequestDate, CCPL.PQCompleteDate'
	--PRINT (@varsql)
	EXEC(@varsql)
	
	--Gets Count of TD (Requested, Completed, Not Required, Avg Days)
	SET @varsql = 'SELECT COUNT(CCTL.id) TCount,CCTL.IsTDRequested,CCTL.IsTDCompleted,CCTL.ISTDNotPossible, CII.ClosingProbability,
		CII.ClosingProbability,DATEDIFF(day, CCTL.TDRequestDate, CCTL.TDCompleteDate) TDAvgDays 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarTDLog CCTL WITH(NOLOCK) ON CCTL.CBDId = CDA.CBDId
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CBD.VersionId = CV.Id 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		WHERE CDA.CreatedOn BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+' 
		AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1
		AND CDA.DealerId IN ('+@DealerId+')'
					
	IF(@Model <> -1)
		SET @varsql += ' AND CV.CarModelId = ' + CONVERT(CHAR(10), @Model, 101)
							
	SET @varsql += 'GROUP BY CCTL.IsTDRequested,CCTL.IsTDCompleted,CCTL.ISTDNotPossible, CII.ClosingProbability,CCTL.TDRequestDate, CCTL.TDCompleteDate'
	--PRINT (@varsql)
	EXEC(@varsql)
	--Gets Count of TD Future
	SET @varsql = 'SELECT COUNT(CCTL.id) TCount,CII.ClosingProbability 
		FROM CRM_CarTDLog CCTL WITH(NOLOCK) 
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CCTL.CBDId = CBD.ID 
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CBD.VersionId = CV.Id 
		INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		WHERE (CCTL.IsTDCompleted = 0 OR CCTL.IsTDCompleted IS NULL) 
		AND (CCTL.ISTDNotPossible = 0 OR CCTL.ISTDNotPossible IS NULL) 
		AND CII.ProductTypeId = 1 
		AND CDA.CreatedOn BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+' 
		AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
		AND CCTL.TDRequestDate > ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
		AND CDA.DealerId IN ('+@DealerId+')'
					
	IF(@Model <> -1)
		SET @varsql += ' AND CV.CarModelId = ' + CONVERT(CHAR(10), @Model, 101)
							
	SET @varsql += 'GROUP BY CII.ClosingProbability'
	--PRINT (@varsql)
	EXEC(@varsql)
	
	--Gets Count of Pending VIN
	SET @varsql = 'SELECT COUNT(DISTINCT CDA.Id) Cnt,CII.ClosingProbability 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CDA.CBDId = CCBD.CarBasicDataId 
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CBD.VersionId = CV.Id 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		LEFT JOIN CRM_CarDeliveryData CCDD WITH(NOLOCK) ON CDA.CBDId = CCDD.CarBasicDataId 
		WHERE CDA.CreatedOn BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+' 
		AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1 
		AND CDA.Status NOT IN (40,41,42,50,60,61) 
		AND CDA.CBDId NOT IN 
		(SELECT CBDId FROM CRM_CarInvoices WITH(NOLOCK) 
		WHERE InvoiceId IN (SELECT id FROM CRM_ADM_Invoices WITH(NOLOCK) WHERE MakeId = 15)) 
		AND ( CCBD.BookingStatusId = 16 OR CCDD.DeliveryStatusId = 20) 
		AND ISNULL(CCDD.ChasisNumber, '+@SingleQuotesTwice+') = '+@SingleQuotesTwice+'
		AND ISNULL(CCDD.EngineNumber, '+@SingleQuotesTwice+') = '+@SingleQuotesTwice+'
		AND ISNULL(CCDD.RegistrationNumber,'+@SingleQuotesTwice+') = '+@SingleQuotesTwice+'
		AND CDA.DealerId IN ('+@DealerId+')'
		
	IF(@Model <> -1)
		SET @varsql += ' AND CV.CarModelId = ' + CONVERT(CHAR(10), @Model, 101)
		
	SET @varsql += 'GROUP BY CII.ClosingProbability'
	--PRINT (@varsql)
	EXEC(@varsql)
	
 END
 
