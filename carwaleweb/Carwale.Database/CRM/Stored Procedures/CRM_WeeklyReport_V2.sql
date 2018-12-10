IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[CRM_WeeklyReport_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[CRM_WeeklyReport_V2]
GO

	




--Name of SP/Function				: CarWale.dbo.CRM_WeeklyReport
--Applications using SP				: CRM, Dealer and RM Panel 
--Modules using the SP				: WeeklyReportLead.cs
--Technical department				: Database
--Summary							: PQ, TD, Booking ,VIN details
--Author							: Dilip V. 02-Feb-2012
--Modification history				: 1. Dilip V. 02-Feb-2012
CREATE PROCEDURE [CRM].[CRM_WeeklyReport_V2]
	@DealerId	VARCHAR(MAX),	
	@FromDate	DATETIME,
	@ToDate		DATETIME,
	@Model		NUMERIC
	
				
 AS 
 BEGIN
	SET NOCOUNT ON
	DECLARE	@Modelcond INT
			
	IF(@Model=-1) BEGIN SET @Modelcond=0 END ELSE BEGIN SET @Modelcond=1 END	 
	
	--Gets Count of Assigned, Lost, Not Interested and Booked Leads
	
		SELECT COUNT(DISTINCT CBD.Id) Cnt, CDA.Status, CL.LeadStageId,
		ISNULL(CCBD.BookingStatusId, -1) BookingStatusId ,CII.ClosingProbability 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK)
		INNER JOIN dbo.fnSplitCSV(@DealerId) FD ON FD.Listmember = CDA.DealerId
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
		INNER JOIN CRM_Leads AS CL WITH(NOLOCK) ON CBD.LeadId = CL.ID 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.Id = CII.LeadId 
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CBD.VersionID = CV.ID 		
		LEFT JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CBD.ID		
		WHERE CDA.CreateOnDatePart BETWEEN @FromDate AND @ToDate
		AND CII.ProductTypeId = 1 		
		AND CV.CarModelId = CASE @Modelcond WHEN 1 THEN @Model ELSE CV.CarModelId END
		GROUP BY CDA.Status, CL.LeadStageId,CCBD.BookingStatusId,CII.ClosingProbability
  	
   --Gets Count of PQ (Requested, Completed, Not Required, Avg Days)
		SELECT COUNT(CCPL.id) TCount,CCPL.IsPQRequested,CCPL.IsPQCompleted,CCPL.IsPQNotRequired, 
		CII.ClosingProbability,DATEDIFF(day, CCPL.PQRequestDate, CCPL.PQCompleteDate) PQAvgDays 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN dbo.fnSplitCSV(@DealerId) FD ON FD.Listmember = CDA.DealerId
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CBD.VersionId = CV.Id 
		INNER JOIN CRM_CarPQLog CCPL WITH(NOLOCK) ON CCPL.CBDId = CBD.ID 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		WHERE CDA.CreateOnDatePart BETWEEN @FromDate AND @ToDate
		AND CII.ProductTypeId = 1 
		AND CV.CarModelId = CASE @Modelcond WHEN 1 THEN @Model ELSE CV.CarModelId END
		GROUP BY CCPL.IsPQRequested,CCPL.IsPQCompleted,CCPL.IsPQNotRequired,CII.ClosingProbability,CCPL.PQRequestDate, CCPL.PQCompleteDate
			
		
	--Gets Count of TD (Requested, Completed, Not Required, Avg Days)
		SELECT CCTL.id TCount,CII.ClosingProbability, CCTL.IsTDCompleted, CCTL.ISTDNotPossible,
		CCTL.IsTDRequested, CONVERT(VARCHAR(10), CCTL.TDRequestDate, 111) TDRequestDate,
		CASE WHEN DATEDIFF(day, CCTL.TDRequestDate, CCTL.TDCompleteDate) > 0 THEN DATEDIFF(day, CCTL.TDRequestDate, CCTL.TDCompleteDate) ELSE 0 END TDAvgDays
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN dbo.fnSplitCSV(@DealerId) FD ON FD.Listmember = CDA.DealerId
		INNER JOIN CRM_CarTDLog CCTL WITH(NOLOCK) ON CCTL.CBDId = CDA.CBDId
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CBD.VersionId = CV.Id 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId
		WHERE CII.ProductTypeId = 1
		AND CDA.CreateOnDatePart BETWEEN @FromDate AND @ToDate
		AND CV.CarModelId = CASE @Modelcond WHEN 1 THEN @Model ELSE CV.CarModelId END
	
	--Gets Count of Pending VIN	
		SELECT COUNT(DISTINCT CDA.Id) Cnt,CII.ClosingProbability 
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN dbo.fnSplitCSV(@DealerId) FD ON FD.Listmember = CDA.DealerId
		INNER JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CDA.CBDId = CCBD.CarBasicDataId 
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CBD.VersionId = CV.Id 
		INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId 
		LEFT JOIN CRM_CarDeliveryData CCDD WITH(NOLOCK) ON CDA.CBDId = CCDD.CarBasicDataId 
		WHERE CDA.CreateOnDatePart BETWEEN @FromDate AND @ToDate
		AND CII.ProductTypeId = 1 
		AND CDA.Status NOT IN (40,41,42,50,60,61) 
		AND CDA.CBDId NOT IN 
		(SELECT CBDId FROM CRM_CarInvoices WITH(NOLOCK) 
		WHERE InvoiceId IN (SELECT id FROM CRM_ADM_Invoices WITH(NOLOCK) WHERE MakeId = 15)) 
		AND (CCBD.BookingStatusId = 16 OR CCDD.DeliveryStatusId = 20) 
		AND ISNULL(CCDD.ChasisNumber, '') = ''
		AND ISNULL(CCDD.EngineNumber, '') = ''
		AND ISNULL(CCDD.RegistrationNumber,'') = ''
		AND CV.CarModelId = CASE @Modelcond WHEN 1 THEN @Model ELSE CV.CarModelId END
		GROUP BY CII.ClosingProbability	
	
 END


