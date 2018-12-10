IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_PopulatePQMatrixUniquePerDay]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_PopulatePQMatrixUniquePerDay]
GO

	-- =============================================
-- Author:		Deepak
-- Create date: 1/20/2012
-- Description:	Populate PQMatrix daily
-- =============================================
CREATE PROCEDURE [dbo].[AP_PopulatePQMatrixUniquePerDay]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     INSERT INTO PQMatrixUniquePerDay    
     SELECT DAY(getdate()-1) AS Day,Month(getdate()-1) AS Month,Year(getdate()-1) AS Year, COUNT(DISTINCT NP.CustomerId) AS CNT, NPC.CityId,ForwardedLead
     FROM NewCarPurchaseInquiries AS NP with(nolock), Customers AS CU with(nolock), NewPurchaseCities NPC with(nolock)
	 WHERE CONVERT(varchar(8),RequestDateTime,112)  = CONVERT(varchar(8),getdate()-1,112)
	 AND NP.CustomerId <> -1 AND CU.ID = NP.CustomerId AND CU.IsFake = 0 AND  Np.Id = Npc.InquiryId
	 GROUP BY Day(NP.RequestDateTime), NPC.CityId,ForwardedLead
	 
	 UNION ALL
				
	 SELECT DAY(getdate()-1) AS Day,Month(getdate()-1) AS Month,Year(getdate()-1) AS Year, COUNT(DISTINCT NP.CustomerId) AS CNT,  -2 as CityId,ForwardedLead
	 FROM NewCarPurchaseInquiries AS NP with(nolock), Customers AS CU with(nolock)
	 WHERE CONVERT(varchar(8),RequestDateTime,112)  = CONVERT(varchar(8),getdate()-1,112)
	 AND NP.CustomerId <> -1 AND CU.ID = NP.CustomerId AND CU.IsFake = 0 
	 GROUP BY Day(NP.RequestDateTime),ForwardedLead
	 

END
