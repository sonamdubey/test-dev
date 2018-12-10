IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ReviewDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ReviewDetails]
GO

	-- =============================================    
-- Author:  Umesh Ojha    
-- Create date: 13/06/2012    
-- Description: Displaying complete customer review of the car for displaying in customer review details    
-- =============================================    
CREATE PROCEDURE [dbo].[Microsite_ReviewDetails]     
 -- Add the parameters for the stored procedure here    
 @ReviewId BIGINT    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    -- Insert statements for procedure here    
 Select CR.ID AS ReviewId, CU.Name AS CustomerName, CU.ID AS CustomerId,   
 ISNULL(UP.HandleName, '') As HandleName, CR.StyleR,  CR.ComfortR,MMV.Version,   
 CR.PerformanceR, CR.ValueR, CR.FuelEconomyR, CR.OverallR, CR.Pros,  CR.Cons,CR.VersionId,   
 CR.Comments,CR.Title, CR.EntryDateTime, CR.Liked, CR.Disliked, CR.Viewed,CR.VersionId,  
 CASE CR.IsOwned WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS IsOwned,CR.Mileage,    
 CASE CR.IsNewlyPurchased WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS IsNewlyPurchased,  
 CASE CR.Familiarity     
 WHEN 1 THEN 'Haven’t driven it'     
 WHEN 2 THEN 'Have done a short test-drive once'     
 WHEN 3 THEN 'Have driven for a few hundred kilometres'     
 WHEN 4 THEN 'Have driven a few thousands kilometres'     
 WHEN 5 THEN 'It’s my mate since ages'    
 ELSE '' END AS Familiarity  
 FROM CustomerReviews CR WITH(NOLOCK) Join Customers AS CU WITH(NOLOCK) ON CU.Id=CR.CustomerId   
 LEFT JOIN vwMMV MMV ON MMV.VersionId= CR.VersionId   
 LEFT JOIN UserProfile UP WITH(NOLOCK) ON UP.UserId = CU.ID   
 Where CR.IsActive=1 AND CR.IsVerified=1   
 AND CR.ID=@ReviewId  
END 