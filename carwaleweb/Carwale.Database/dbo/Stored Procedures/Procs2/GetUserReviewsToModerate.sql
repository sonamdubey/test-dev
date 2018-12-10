IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUserReviewsToModerate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUserReviewsToModerate]
GO

	-- =============================================  
-- Author:  Vinayak Patil 
-- Create date: 13-01-2013  
-- Description: Returns the reviews of the customers
-- Modified by Rakesh Yadav on 28 June 2016, removed CR.LastUpdatedBy condition from where clause
-- =============================================

CREATE PROCEDURE [dbo].[GetUserReviewsToModerate] -- EXEC GetUserReviewsToModerate
AS
BEGIN
SELECT MA.Name + ' ' + MO.Name + ' ' + IsNull(CV.Name, 'For All Versions') AS Car
,MA.Name + ' ' + MO.Name AS CarOfReview
,CR.ID AS ReviewId, CR.StyleR
,CR.ComfortR, CR.PerformanceR
,CR.ValueR
,CR.FuelEconomyR
,CR.OverallR
,CR.Pros
,CR.Cons
,CR.Comments
,CR.Title
,CR.EntryDateTime
,CR.ReportAbused
,CASE WHEN CR.CustomerId = -1 THEN 'Anonymous' ELSE C.Name END AS CustomerName
,CR.CustomerId AS CustomerId
,CASE WHEN CR.Mileage IS NOT NULL AND CR.Mileage <> 0 THEN CONVERT(VARCHAR, Mileage) ELSE '' END AS Mileage
,CASE WHEN CR.IsOwned = 0 THEN 'Not Purchased' 
 WHEN CR.IsOwned = 1 AND CR.IsNewlyPurchased = 1 THEN 'New' 
 WHEN CR.IsOwned = 1 AND CR.IsNewlyPurchased = 0 THEN 'Used' 
 ELSE '' END AS PurchasedAs
,CASE CR.Familiarity
 WHEN 1 THEN 'Haven’t driven it'
 WHEN 2 THEN 'Have done a short test-drive once'
 WHEN 3 THEN 'Have driven for a few hundred kilometres'
 WHEN 4 THEN 'Have driven a few thousands kilometres'
 WHEN 5 THEN 'It’s my mate since ages'
 ELSE '' END AS Familiarity
FROM
CustomerReviews AS CR WITH(NOLOCK)
JOIN CarModels AS MO WITH(NOLOCK) ON CR.ModelId = MO.ID
JOIN CarMakes AS MA WITH(NOLOCK) ON CR.MakeId = MA.ID
LEFT JOIN CarVersions AS CV WITH(NOLOCK) ON CV.ID = CR.VersionId
LEFT JOIN Customers AS C WITH(NOLOCK) ON CR.CustomerId = C.Id
WHERE
CR.IsActive = 1
AND CR.IsVerified = 0
--AND CR.LastUpdatedBy IS NULL
ORDER BY CR.EntryDateTime DESC
END