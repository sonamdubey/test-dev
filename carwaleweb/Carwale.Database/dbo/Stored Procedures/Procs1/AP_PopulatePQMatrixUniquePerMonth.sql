IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_PopulatePQMatrixUniquePerMonth]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_PopulatePQMatrixUniquePerMonth]
GO

	

-- =============================================
-- Author:		Deepak
-- Create date: 1/20/2012
-- Description:	Populate PQMatrix daily
-- =============================================
CREATE PROCEDURE [dbo].[AP_PopulatePQMatrixUniquePerMonth]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	 DELETE FROM PQMatrixUniquePerMonth WHERE MONTH = Month(getdate()-1) AND YEAR = Year(getdate()-1)
	 
     INSERT INTO PQMatrixUniquePerMonth    
     SELECT Month(getdate()-1) AS Month,Year(getdate()-1) AS Year, COUNT(DISTINCT NP.CustomerId) AS CNT, NPC.CityId,ForwardedLead
     FROM NewCarPurchaseInquiries AS NP with(nolock), Customers AS CU with(nolock), NewPurchaseCities NPC with(nolock)
	 WHERE MONTH(RequestDateTime)  = Month(getdate()-1) AND YEAR(RequestDateTime)  = YEAR(getdate()-1)
	 AND NP.CustomerId <> -1 AND CU.ID = NP.CustomerId AND CU.IsFake = 0 AND  Np.Id = Npc.InquiryId
	 GROUP BY NPC.CityId,ForwardedLead
	 
	 UNION ALL
				
	 SELECT Month(getdate()-1) AS Month,Year(getdate()-1) AS Year, COUNT(DISTINCT NP.CustomerId) AS CNT,  -2 as CityId,ForwardedLead
	 FROM NewCarPurchaseInquiries AS NP with(nolock), Customers AS CU with(nolock)
	 WHERE MONTH(RequestDateTime)  = Month(getdate()-1) AND YEAR(RequestDateTime)  = YEAR(getdate()-1)
	 AND NP.CustomerId <> -1 AND CU.ID = NP.CustomerId AND CU.IsFake = 0 
	 GROUP BY ForwardedLead
	 

END


