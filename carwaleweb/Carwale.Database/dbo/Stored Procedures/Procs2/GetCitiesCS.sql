IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCitiesCS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCitiesCS]
GO

	
-- =============================================        
-- Author:  Saurabh Tamrakar        
-- Create date: 19 Apr 2016      
-- Description: Fetching all cities on the Basis of PriceQuote      
-- =============================================        
CREATE PROCEDURE [dbo].[GetCitiesCS]   

AS    
SET NOCOUNT ON;    -- SET NOCOUNT ON added to prevent extra result sets from        
                   -- interfering with SELECT statements.  
  
BEGIN   

		DECLARE @Todate datetime=GETDATE() -7;
		SELECT CT.ID as CityId, CT.Name as CityName,ST.Name As StateName, count(N.Id) as Count,CT.citymaskingname as citymaskingname 
		FROM 
		Cities CT WITH(NOLOCK)
		JOIN States ST WITH(NOLOCK) ON CT.StateId=ST.ID
		LEFT JOIN NewPurchaseCities C WITH(NOLOCK) ON C.CityId=CT.ID
		LEFT JOIN NewCarPurchaseInquiries N WITH(NOLOCK) ON N.Id=C.InquiryId AND N.RequestDateTime> @Todate
		WHERE 
			CT.IsDeleted = 0 and ST.IsDeleted=0
		GROUP BY CT.Name,CT.ID,ST.Name,CT.CityMaskingName
		ORDER BY count(N.Id) DESC,CT.Name

END







