IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[WeeklyReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[WeeklyReport]
GO

	
--Name of SP/Function				: CarWale.dbo.CRM_WeeklyReport
--Applications using SP				: CRM, Dealer and RM Panel 
--Modules using the SP				: WeeklyReportLead.cs
--Technical department				: Database
--Summary							: PQ, TD, Booking ,VIN details
--Author							: Dilip V. 02-Feb-2012
--Modification history				: 1.Dilip V. 18-Apr-2012 (Gets extra field CCTL.IsTDDirect)
--									: 2.Dilip V. 19-Apr-2012 (Removed field CCTL.IsTDDirect instead check IsTDRequested)
--									: 3.Dilip V. 02-May-2012 (If MakeId is passed check)
--									: 4.Dilip V. 02-Jul-2012 (Get Count of Car Booked from EventLog)
CREATE PROCEDURE [CRM].[WeeklyReport]
	@DealerId	VARCHAR(MAX),	
	@FromDate	DATETIME,
	@ToDate		DATETIME,
	@Model		NUMERIC(18,0),
	@Make		NUMERIC(18,0)=NULL,
	@SiteId		BIT=NULL
				
 AS 
 BEGIN
	SET NOCOUNT ON
	DECLARE	@varsql				VARCHAR(MAX),
			@SingleQuotesTwice	VARCHAR(2) = '''''',
			@SingleQuotes		VARCHAR(1) = ''''			 
	
	--Gets Count of Assigned, Lost, Not Interested and Booked Leads
	SET @varsql =  'SELECT COUNT(CBD.Id) Cnt, CDA.Status, CL.LeadStageId,
		ISNULL(CCBD.BookingStatusId, -1) BookingStatusId ,CII.ClosingProbability 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK)
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
		INNER JOIN CRM_Leads AS CL WITH(NOLOCK) ON CBD.LeadId = CL.ID 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.Id = CII.LeadId 
		INNER JOIN vwMMV VW ON CBD.VersionID = VW.VersionId
		LEFT JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CBD.ID
		WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+' 
		AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
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
	--PRINT (@varsql)
	EXEC (@varsql)	
	
   --Gets Count of PQ (Requested, Completed, Not Required, Avg Days)
	SET @varsql =  'SELECT COUNT(CCPL.id) TCount,CCPL.IsPQRequested,CCPL.IsPQCompleted,CCPL.IsPQNotRequired, 
		CII.ClosingProbability,DATEDIFF(day, CCPL.PQRequestDate, CCPL.PQCompleteDate) PQAvgDays 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
		INNER JOIN vwMMV VW ON CBD.VersionId = VW.VersionId
		INNER JOIN CRM_CarPQLog CCPL WITH(NOLOCK) ON CCPL.CBDId = CBD.ID 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+' 
		AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
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
	--PRINT (@varsql)
	EXEC(@varsql)
	
	--Gets Count of TD (Requested, Completed, Not Required, Avg Days,TDDirect)
	SET @varsql = 'SELECT CCTL.id TCount,CII.ClosingProbability, CCTL.IsTDCompleted, CCTL.ISTDNotPossible,
		CCTL.IsTDRequested, CONVERT(VARCHAR(10), CCTL.TDRequestDate, 111) TDRequestDate,CCTL.IsTDDirect AS IsTDDirect,
		CASE WHEN DATEDIFF(day, CCTL.TDRequestDate, CCTL.TDCompleteDate) > 0 THEN DATEDIFF(day, CCTL.TDRequestDate, CCTL.TDCompleteDate) ELSE 0 END TDAvgDays
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarTDLog CCTL WITH(NOLOCK) ON CCTL.CBDId = CDA.CBDId
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
		INNER JOIN vwMMV VW ON CBD.VersionId = VW.VersionId
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+' 
		AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1
		AND CDA.DealerId IN ('+@DealerId+')'
					
	IF(@Model = -1)
		BEGIN
			IF(@Make IS NOT NULL)
				SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
		END
	ELSE
		SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
		
	PRINT (@varsql)
	EXEC(@varsql)
	
	--Gets Count of Pending VIN
	SET @varsql = 'SELECT COUNT(CDA.Id) Cnt,CII.ClosingProbability 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CDA.CBDId = CCBD.CarBasicDataId 
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
		INNER JOIN vwMMV VW ON CBD.VersionId = VW.VersionId
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		LEFT JOIN CRM_CarDeliveryData CCDD WITH(NOLOCK) ON CDA.CBDId = CCDD.CarBasicDataId 
		WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+' 
		AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
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
	--PRINT (@varsql)
	EXEC(@varsql)
	
	IF(@SiteId = 1)--OPR
		BEGIN
		--Gets Count of Car Booked
		SET @varsql = 'SELECT COUNT(DISTINCT CEL.ItemId) Cnt,CII.ClosingProbability
		FROM CRM_EventLogs CEL WITH (NOLOCK)
		INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID = CEL.ItemId
		INNER JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CBD.ID
		INNER JOIN vwMMV VW ON VW.VersionId = CBD.VersionId
		INNER JOIN CRM_Leads AS CL WITH (NOLOCK) ON CBD.LeadId = CL.ID
		INNER JOIN CRM_InterestedIn AS CII WITH (NOLOCK) ON CL.Id = CII.LeadId
		WHERE CEL.EventType = 16
		AND CEL.EventDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+' 
		AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
		AND CII.ProductTypeId = 1
		AND CDA.DealerId IN ('+@DealerId+')'
		IF(@Model = -1)
			BEGIN
				IF(@Make IS NOT NULL)
					SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
			END
		ELSE
			SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
		
		SET @varsql += ' GROUP BY CII.ClosingProbability'
		EXEC(@varsql)
		
	END		
	
 END
 
















