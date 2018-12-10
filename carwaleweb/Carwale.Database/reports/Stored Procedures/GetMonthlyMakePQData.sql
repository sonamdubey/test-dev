IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[GetMonthlyMakePQData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[GetMonthlyMakePQData]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: Sep 08 2011
-- Description:	 GetMonthlyModelPQData
-- =============================================
CREATE PROCEDURE  [reports].[GetMonthlyMakePQData]
	@FromMonth int,@ToMonth int,@Year int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
    -- Insert statements for procedure here
    
    SELECT
	Distinct CMA.Name AS Make, 
	         datepart(Month,NP.RequestDateTime) AS PQMonth,
	         COUNT(NP.Id)AS TPQ	        
	FROM
		vwNewCarPurchaseInquiries AS NP with(nolock)
		join newcarspecifications AS NCS with(nolock) on NCS.CarVersionId=NP.CarVersionId
		join CarVersions AS CV with(nolock) on NP.CarVersionId = CV.ID
		join CarModels AS CMO with(nolock) on  CV.CarModelId = CMO.ID
		join CarMakes AS CMA with(nolock) on CMO.CarMakeId = CMA.ID 
	WHERE
	datepart(Month,NP.RequestDateTime) BETWEEN @FromMonth AND @ToMonth
	AND datepart(Year,NP.RequestDateTime)=@Year
	GROUP BY CMA.Name, datepart(Month,NP.RequestDateTime),datepart(Year,NP.RequestDateTime)
	ORDER BY Make,datepart(Month,NP.RequestDateTime)

END

