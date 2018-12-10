IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[GetMonthlyModelPQData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[GetMonthlyModelPQData]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: Sep 08 2011
-- Description:	 PQ data for the month Model wise
-- =============================================
CREATE PROCEDURE  [reports].[GetMonthlyModelPQData]
	@FromMonth int,@ToMonth int,@Year int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
    -- Insert statements for procedure here
    
    SELECT
             CMO.Name AS Model,
	         NCS.FuelType   as FuelType,
	         NCS.TransmissionType   as TransmissionType, 
	         COUNT(NP.Id)AS TPQ, 
	         CB.Name AS BodyStyle,
	         CS.Name AS Segment,
	         CSS.Name as SubSegmentName       
	FROM vwNewCarPurchaseInquiries AS NP with(nolock)
		join newcarspecifications AS NCS with(nolock) on NCS.CarVersionId=NP.CarVersionId
		join CarVersions AS CV with(nolock) on NP.CarVersionId = CV.ID
		join CarModels AS CMO with(nolock) on  CV.CarModelId = CMO.ID
		join CarMakes AS CMA with(nolock) on CMO.CarMakeId = CMA.ID 		
		join CarBodyStyles AS CB with(nolock) on CV.BodyStyleId = CB.ID
		join CarSegments CS  with(nolock) on CV.SegmentId = CS.ID
		left outer join CarSubSegments CSS with(nolock) on CV.SubSegmentId=CSS.Id
	WHERE
	datepart(Month,NP.RequestDateTime) BETWEEN @FromMonth AND @ToMonth
	AND datepart(Year,NP.RequestDateTime)=@Year
	GROUP BY CMO.Name,NCS.FuelType,NCS.TransmissionType,CB.Name,CS.Name,CSS.Name, datepart(Month,NP.RequestDateTime),datepart(Year,NP.RequestDateTime)
	ORDER BY CMO.Name,NCS.FuelType,NCS.TransmissionType,CB.Name,CS.Name,CSS.Name, datepart(Month,NP.RequestDateTime),datepart(Year,NP.RequestDateTime)

END

