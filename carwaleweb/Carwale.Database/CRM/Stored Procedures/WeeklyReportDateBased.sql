IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[WeeklyReportDateBased]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[WeeklyReportDateBased]
GO

	
--Name of SP/Function				: CarWale.dbo.CRM.WeeklyReportDateBased
--Applications using SP				: CRM 
--Modules using the SP				: WeeklyReportLeadDateBased.cs
--Technical department				: Database
--Summary							: PQ, TD, Booking ,VIN details
--Author							: Dilip V. 26-Jun-2012
--Modification history				: 1. Amit kumar 24th july 2013(added CCTL.IsTDDirect)
CREATE PROCEDURE [CRM].[WeeklyReportDateBased]
	@DealerId	VARCHAR(MAX),	
	@FromDate	DATETIME,
	@ToDate		DATETIME,
	@Model		NUMERIC,
	@Make		NUMERIC=NULL
				
 AS 
 BEGIN
	SET NOCOUNT ON
	DECLARE	@varsql				VARCHAR(MAX),
			@SingleQuotesTwice	VARCHAR(2) = '''''',
			@SingleQuotes		VARCHAR(1) = ''''
			PRINT (@FromDate)
			PRINT (@ToDate)
			
	--Gets Count of Assigned, Lost, Not Interested and Booked Leads
	SET @varsql =  'SELECT COUNT(DISTINCT CBD.Id) Cnt, CDA.Status, CL.LeadStageId,
		ISNULL(CCBD.BookingStatusId, -1) BookingStatusId ,CII.ClosingProbability 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK)
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
		INNER JOIN CRM_Leads AS CL WITH(NOLOCK) ON CBD.LeadId = CL.ID 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.Id = CII.LeadId 
		INNER JOIN vwMMV VW ON CBD.VersionID = VW.VersionId
		LEFT JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CBD.ID
		WHERE CDA.CreatedOn BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
		AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1 
		AND CDA.DealerId IN ('+@DealerId+')'
		
	IF(@Model = -1)
		BEGIN
			IF(@Make IS NOT NULL)
				SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
		END
	ELSE
		SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
		
	SET @varsql += ' GROUP BY CDA.Status, CL.LeadStageId,CCBD.BookingStatusId,CII.ClosingProbability'	
	PRINT (@varsql)
	EXEC (@varsql)	
	
   --Gets Count of PQ (Requested, Completed, Not Required)
	SET @varsql =  'SELECT COUNT(DISTINCT CCPL.id) TCount,CCPL.IsPQRequested,CCPL.IsPQCompleted,CCPL.IsPQNotRequired, 
		CII.ClosingProbability,DATEDIFF(day, CCPL.PQRequestDate, CCPL.PQCompleteDate) PQAvgDays 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
		INNER JOIN vwMMV VW ON CBD.VersionId = VW.VersionId
		INNER JOIN CRM_CarPQLog CCPL WITH(NOLOCK) ON CCPL.CBDId = CBD.ID 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		WHERE CCPL.PQRequestDate BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
		AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1 
		AND CDA.DealerId IN ('+@DealerId+')'

	IF(@Model = -1)
		BEGIN
			IF(@Make IS NOT NULL)
				SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
		END
	ELSE
		SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
	
	SET @varsql += ' GROUP BY CCPL.IsPQRequested,CCPL.IsPQCompleted,CCPL.IsPQNotRequired,CII.ClosingProbability,CCPL.PQRequestDate, CCPL.PQCompleteDate'
	PRINT (@varsql)
	EXEC(@varsql)
	
	--Gets Count of TD (Requested, Completed, Not Required,TDDirect)
	SET @varsql = 'SELECT COUNT(DISTINCT CCTL.id) TCount,CCTL.IsTDRequested, CCTL.IsTDCompleted, CCTL.ISTDNotPossible,CCTL.IsTDDirect,
		 CII.ClosingProbability, DATEDIFF(day, CCTL.TDRequestDate, CCTL.TDCompleteDate) TDAvgDays
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarTDLog CCTL WITH(NOLOCK) ON CCTL.CBDId = CDA.CBDId
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
		INNER JOIN vwMMV VW ON CBD.VersionId = VW.VersionId
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		WHERE CCTL.TDRequestDate BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
		AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1
		AND CDA.DealerId IN ('+@DealerId+')'
					
	IF(@Model = -1)
		BEGIN
			IF(@Make IS NOT NULL)
				SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
		END
	ELSE
		SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
		
	SET @varsql += ' GROUP BY CCTL.IsTDRequested, CCTL.IsTDCompleted, CCTL.ISTDNotPossible,CII.ClosingProbability,CCTL.TDRequestDate,CCTL.IsTDDirect, CCTL.TDCompleteDate'
	PRINT (@varsql)
	EXEC(@varsql)
	
	--Gets Count of Booking Log (Requested, Completed, Not Possible)
 
	SET @varsql = 'SELECT COUNT(DISTINCT CCBL.id) TCount,CII.ClosingProbability, CCBL.IsBookingCompleted, CCBL.IsBookingNotPossible,
		CCBL.IsBookingRequested, DATEDIFF(day, CCBL.BookingRequestDate, CCBL.BookingCompleteDate) BookAvgDays
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarBookingLog CCBL WITH(NOLOCK) ON CCBL.CBDId = CDA.CBDId
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
		INNER JOIN vwMMV VW ON CBD.VersionId = VW.VersionId
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		WHERE CCBL.BookingRequestDate BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
		AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1
		AND CDA.DealerId IN ('+@DealerId+')'
					
	IF(@Model = -1)
		BEGIN
			IF(@Make IS NOT NULL)
				SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
		END
	ELSE
		SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
	
	SET @varsql += ' GROUP BY CII.ClosingProbability, CCBL.IsBookingCompleted, CCBL.IsBookingNotPossible, CCBL.IsBookingRequested, CCBL.BookingRequestDate, CCBL.BookingCompleteDate'
	PRINT (@varsql)
	EXEC(@varsql)
	
	--Gets Count of Pending VIN
	SET @varsql = 'SELECT COUNT(DISTINCT CDA.Id) Cnt,CII.ClosingProbability 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CDA.CBDId = CCBD.CarBasicDataId 
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
		INNER JOIN vwMMV VW ON CBD.VersionId = VW.VersionId
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		LEFT JOIN CRM_CarDeliveryData CCDD WITH(NOLOCK) ON CDA.CBDId = CCDD.CarBasicDataId 
		WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
		AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1 
		AND CDA.Status NOT IN (40,41,42,50,60,61) 
		AND CDA.CBDId NOT IN 
		(SELECT CBDId FROM CRM_CarInvoices CCI WITH(NOLOCK) 
		INNER JOIN CRM_ADM_Invoices CAI WITH(NOLOCK) ON CCI.InvoiceId = CAI.Id AND CAI.MakeId = 15) 
		AND ( CCBD.BookingStatusId = 16 OR CCDD.DeliveryStatusId = 20) 
		AND ISNULL(CCDD.ChasisNumber, '+@SingleQuotesTwice+') = '+@SingleQuotesTwice+'
		AND ISNULL(CCDD.EngineNumber, '+@SingleQuotesTwice+') = '+@SingleQuotesTwice+'
		AND ISNULL(CCDD.RegistrationNumber,'+@SingleQuotesTwice+') = '+@SingleQuotesTwice+'
		AND CDA.DealerId IN ('+@DealerId+')'
		
	IF(@Model = -1)
		BEGIN
			IF(@Make IS NOT NULL)
				SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
		END
	ELSE
		SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
		
	SET @varsql += ' GROUP BY CII.ClosingProbability'
	PRINT (@varsql)
	EXEC(@varsql)
	
 END
 















