IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[WeeklyReport_Assigned]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[WeeklyReport_Assigned]
GO

	


--Name of SP/Function				: CarWale.dbo.CRM_WeeklyReport_Assigned
--Applications using SP				: CRM, Dealer and RM Panel 
--Modules using the SP				: WeeklyReportLead.cs
--Technical department				: Database
--Summary							: Assigned
--Author							: Dilip V. 02-Feb-2012
--Modification history				: 1. Dilip V. 28-Feb-2012
CREATE PROCEDURE [CRM].[WeeklyReport_Assigned]
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
  	
 END



