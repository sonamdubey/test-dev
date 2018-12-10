IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[GetPQData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[GetPQData]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: Sep 08 2011
-- Description:	PQ data for the month
-- =============================================
CREATE PROCEDURE [reports].[GetPQData]
	@StartDate datetime,@endDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--DECLARE @StartDate datetime
	--DECLARE @endDate datetime

	    if convert(varchar(10),@StartDate,120) = convert(varchar(10),GETDATE(),120)
		BEGIN
			set @StartDate =convert(datetime,convert(varchar(10),GETDATE()-1,120)+ ' 00:00:00')	
		END
		
		if convert(varchar(10),@endDate,120) = convert(varchar(10),GETDATE(),120)
		BEGIN
			set @endDate =convert(datetime,convert(varchar(10),GETDATE()-1,120)+ '  23:59:59')	
		END
	
	set @StartDate = convert(datetime,convert(varchar(10),@StartDate,120)+ ' 00:00:00')	
	set @endDate = convert(datetime,convert(varchar(10),@endDate,120)+ ' 23:59:59');
	
    -- Insert statements for procedure here
    --Make	Model	Variant	FuelType	TransmissionType	TPQ	BodyStyle	Segment	SubSegment	PQMonth	PQYear	Month	Quarter

    
 --   SELECT
	--Distinct CMA.Name AS Make, 
	--         CMO.Name AS Model,
	--         CV.Name as Variant,
	--         CF.Descr as FuelType,
	--         CV.CarTransmission as TransmissionType, 
	--         COUNT(NP.Id)AS TPQ, 
	--         CB.Name AS BodyStyle, 
	--         CS.Name AS Segment,
	--         CSS.Name as SubSegmentName	         
	--FROM
	--	vwNewCarPurchaseInquiries AS NP with(nolock), 
	--	CarMakes AS CMA with(nolock),
	--	CarModels AS CMO with(nolock), 
	--	CarVersions AS CV with(nolock), 
	--	CarBodyStyles AS CB with(nolock), 
	--	CarSegments CS  with(nolock),
	--	CarSubSegments CSS with(nolock),
	--	CarFuelTypes CF with(nolock)
	--WHERE
	--NP.CarVersionId = CV.ID AND CV.CarModelId = CMO.ID
	--AND CMO.CarMakeId = CMA.ID AND CV.BodyStyleId = CB.ID
	--AND CV.SegmentId = CS.ID
	--AND CV.SubSegmentId=CSS.Id
	--AND CV.CarFuelType=CF.CarFuelTypeId
	--AND NP.RequestDateTime BETWEEN @StartDate AND @endDate
	--GROUP BY CMA.Name, CMO.Name,CV.Name,CF.Descr, CB.Name, CS.Name,CSS.Name,CV.CarTransmission
	--ORDER BY Make, Model    


    SELECT
	Distinct CMA.Name AS Make, 
	         CMO.Name AS Model,
	         CV.Name as Variant,
	         NCS.FuelType   as FuelType,
	         NCS.TransmissionType   as TransmissionType, 
	         COUNT(NP.Id)AS TPQ, 
	         CB.Name AS BodyStyle,
	         CS.Name AS Segment,
	         CSS.Name as SubSegmentName	            
	FROM
		vwNewCarPurchaseInquiries AS NP with(nolock)
		join newcarspecifications AS NCS with(nolock) on NCS.CarVersionId=NP.CarVersionId
		join CarVersions AS CV with(nolock) on NP.CarVersionId = CV.ID
		join CarModels AS CMO with(nolock) on  CV.CarModelId = CMO.ID
		join CarMakes AS CMA with(nolock) on CMO.CarMakeId = CMA.ID 		
		join CarBodyStyles AS CB with(nolock) on CV.BodyStyleId = CB.ID
		join CarSegments CS  with(nolock) on CV.SegmentId = CS.ID
		left outer join CarSubSegments CSS with(nolock) on CV.SubSegmentId=CSS.Id
		--left outer join CarFuelTypes CF with(nolock) on CV.CarFuelType=CF.CarFuelTypeId
	where  NP.RequestDateTime BETWEEN @StartDate AND @endDate	
	GROUP BY CMA.Name, CMO.Name,CV.Name,NCS.FuelType,NCS.TransmissionType,CB.Name,CS.Name,CSS.Name
	ORDER BY Make, Model  

END

