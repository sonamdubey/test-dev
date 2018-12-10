IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_PopulatePQMatrixUniquePerMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_PopulatePQMatrixUniquePerMake]
GO

	
-- =============================================
--- Author:		Jayant Mhatre
-- Create date: 02/07/2012
-- Description:	Populate PQMatrixMake
-- =============================================
CREATE PROCEDURE [dbo].[AP_PopulatePQMatrixUniquePerMake]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     INSERT INTO PQMatrixUniquePerMake
    (   MakeId,
		PQCNT,
		Day,
		Month,
		Year,
		ForwardedLead,
		CityId
	)
    SELECT vw.MakeId AS MakeId,
	 COUNT(DISTINCT NP.CustomerId) AS PQCNT, day(GETDATE()-1) AS Day, Month(GETDATE()-1) AS Month,Year(GETDATE()-1)  AS Year ,ForwardedLead, CU.CityId	
	FROM vwmmv as vw with(nolock)
	JOIN NewCarPurchaseInquiries AS NP with(nolock) on NP.CarVersionId=vw.VersionId
	JOIN  Customers AS CU with(nolock) on CU.Id= NP.CustomerId					
	 WHERE	
		 Convert(varchar(8),RequestDateTime,112)=Convert(varchar(8),GETDATE()-1,112)
		 AND NP.CustomerId <> -1 AND  CU.IsFake = 0	 	  				
		 GROUP BY vw.MakeId, Day(NP.RequestDateTime), ForwardedLead, CU.CityId				
		 ORDER BY vw.MakeId

END

