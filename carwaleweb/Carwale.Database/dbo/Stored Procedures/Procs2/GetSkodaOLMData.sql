IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSkodaOLMData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSkodaOLMData]
GO

	-- =============================================
-- Author:		Avishkar 
-- Create date: 17/12/2012
-- Description:	Get Skoda OLM data
--  GetSkodaOLMData '2011/01/01' , '2012/01/31'
-- =============================================
CREATE PROCEDURE GetSkodaOLMData
@DateFrom datetime, @DateTo datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	CREATE TABLE #TempSkodaLeads
	(
	   LeadId bigint,
	   RegionId int,
	   LeadStatusId int,
	   CarBasicDataId bigint,
	   LeadCreated datetime
	)

   
    -- Insert statements for procedure here
    INSERT INTO #TempSkodaLeads
	SELECT CL.Id as LeadId,
       OC.RegionId,
       CL.LeadStatusId,
       CBD.Id as CarBasicDataId,
       CL.CreatedOnDatePart
	FROM CRM_Customers AS CC WITH (NOLOCK)
	JOIN OLM_RegionCities AS OC WITH (NOLOCK) ON CC.CityId = OC.CityId
	JOIN CRM_Leads AS CL WITH (NOLOCK) ON CL.CNS_CustId = CC.Id
	JOIN CRM_CarBasicData AS CBD WITH (NOLOCK) ON CL.ID = CBD.LeadId
	JOIN vwMMV AS VM WITH (NOLOCK) ON CBD.VersionId = VM.VersionId
	AND VM.MakeId = 15
	WHERE CL.CreatedOnDatePart BETWEEN @DateFrom AND @DateTo
	
	SELECT COUNT(DISTINCT LeadId) AS TotalLeads,
       RegionId,
       LeadStatusId,
       --LeadCreated,
       MONTH(LeadCreated) AS LeadMonth,
       YEAR(LeadCreated) AS LeadYear
   FROM #TempSkodaLeads
   GROUP BY LeadId,RegionId,
         LeadStatusId,
         MONTH(LeadCreated),
         YEAR(LeadCreated)
         
   SELECT COUNT(DISTINCT CL.LeadId) AS TotalLeads,
          RegionId,
          MONTH(CL.LeadCreated) AS LeadMonth,
          YEAR(CL.LeadCreated) AS LeadYear
	FROM #TempSkodaLeads AS CL
	JOIN CRM_PrePushData AS CPD WITH (NOLOCK) ON CPD.LeadId = CL.LeadId
	AND CPD.Result = 'SUCCESS'	
	GROUP BY CL.RegionId,
			 MONTH(CL.LeadCreated),
			 YEAR(CL.LeadCreated)
			 
	SELECT   COUNT(DISTINCT CL.LeadId) AS TotalLeads,
          CL.RegionId,
          ClosingProbability,
          MONTH(CL.LeadCreated) AS LeadMonth,
          YEAR(CL.LeadCreated) AS LeadYear
	FROM #TempSkodaLeads AS CL
	JOIN CRM_InterestedIn AS CI WITH (NOLOCK) ON CI.LeadId = CL.LeadId
	AND CI.ProductTypeId = 1	
	JOIN CRM_SkodaDealerAssignment AS CSD WITH (NOLOCK) ON CSD.LeadId = CL.LeadId
	AND CSD.PushStatus = 'SUCCESS'
	JOIN NCS_Dealers AS ND WITH (NOLOCK) ON CSD.DealerId = ND.Id	
	GROUP BY CL.RegionId,
			 ClosingProbability,
			 MONTH(CL.LeadCreated),
			 YEAR(CL.LeadCreated)
			 
	SELECT COUNT(DISTINCT CCBD.CBDId) TotalLeads,
		   CII.ClosingProbability,
		   OC.RegionId,
		   MONTH(CCBD.BookingCompleteDate) AS LeadMonth,
		   YEAR(CCBD.BookingCompleteDate) AS LeadYear
	FROM CRM_CarBasicData AS CBD WITH (NOLOCK)
	JOIN CRM_InterestedIn AS CII WITH (NOLOCK) ON CII.LeadId = CBD.LeadId
	AND CII.ProductTypeId = 1
	JOIN CRM_CarBookingLog AS CCBD WITH (NOLOCK) ON CCBD.CBDId = CBD.Id
	AND IsBookingCompleted = 1
	JOIN vwMMV AS VM WITH (NOLOCK) ON CBD.VersionId = VM.VersionId
	AND VM.MakeId = 15
	JOIN CRM_CarDealerAssignment AS CDA WITH (NOLOCK) ON CDA.CBDId = CBD.Id
	JOIN NCS_Dealers AS ND WITH (NOLOCK) ON CDA.DealerId = ND.Id
	JOIN OLM_RegionCities AS OC WITH (NOLOCK) ON ND.CityId = OC.CityId
	WHERE CCBD.BookingCompleteDate BETWEEN @DateFrom AND @DateTo
	GROUP BY OC.RegionId,
			 ClosingProbability,
			 MONTH(CCBD.BookingCompleteDate),
			 YEAR(CCBD.BookingCompleteDate)
	
	SELECT AVG(DATEDIFF(dd, CL.LeadCreated, CCBD.BookingCompleteDate)) AS TTime,
          CL.RegionId
	FROM #TempSkodaLeads AS CL WITH (NOLOCK)
	JOIN CRM_CarBookingLog AS CCBD WITH (NOLOCK) ON CCBD.CBDId = CL.CarBasicDataId
	AND IsBookingCompleted = 1
	JOIN CRM_SkodaDealerAssignment AS CDA WITH (NOLOCK) ON CDA.LeadId = CL.LeadId
	JOIN NCS_Dealers AS ND WITH (NOLOCK) ON CDA.DealerId = ND.Id
	WHERE CCBD.BookingCompleteDate BETWEEN @DateFrom AND @DateTo
	GROUP BY CL.RegionId
		
	DROP TABLE #TempSkodaLeads

END
