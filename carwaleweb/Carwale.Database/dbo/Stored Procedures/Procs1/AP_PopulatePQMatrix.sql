IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_PopulatePQMatrix]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_PopulatePQMatrix]
GO

	-- =============================================
-- Author:		Deepak
-- Create date: 1/20/2012
-- Description:	Populate PQMatrix daily
-- =============================================
CREATE PROCEDURE [AP_PopulatePQMatrix]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     INSERT INTO PQMatrix
    (   MakeId,
		ModelId,
		versionId,
		PQCNT,
		Day,
		Month,
		Year,
		ForwardedLead,
		CityId
	)
    SELECT vw.MakeId AS MakeId,vw.ModelId AS ModelId,vw.versionId,
	 COUNT(DISTINCT NP.CustomerId) AS PQCNT, day(GETDATE()-1) AS Day, Month(GETDATE()-1) AS Month,Year(GETDATE()-1)  AS Year ,ForwardedLead, CU.CityId	
	FROM vwmmv as vw with(nolock)
	JOIN NewCarPurchaseInquiries AS NP with(nolock) on NP.CarVersionId=vw.VersionId
	JOIN  Customers AS CU with(nolock) on CU.Id= NP.CustomerId					
	 WHERE	
		 Convert(varchar(8),RequestDateTime,112)=Convert(varchar(8),GETDATE()-1,112)
		 AND NP.CustomerId <> -1 AND  CU.IsFake = 0	 	  				
		 GROUP BY vw.MakeId,vw.ModelId,vw.versionId, Day(NP.RequestDateTime), ForwardedLead, CU.CityId				
		 ORDER BY vw.MakeId,vw.ModelId,vw.versionId	 

END
