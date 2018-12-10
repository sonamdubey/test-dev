IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_GetUserReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_GetUserReviews]
GO
	  
CREATE PROCEDURE [dbo].[WA_GetUserReviews]    
@ModelId INT,    
@StartIndex INT,    
@EndIndex INT,    
@SortCriteria INT,    
@VersionId INT=NULL    
AS    
BEGIN    
--Author: Rakesh Yadav    
--Date:15 July 2013    
--Desc: Fetch user reviews for maodel for web api     
    
--ModelDetails,Starting Price and Review Count    
    
SELECT Mk.ID AS MakeId, Mk.Name AS MakeName, Mo.ID AS ModelId, Mo.Name AS ModelName, Mo.LargePic, Mo.SmallPic,      
Mo.HostURL, Looks, Performance, Comfort, ValueForMoney, FuelEconomy, ReviewRate, ReviewCount, Mo.Futuristic,Mo.MinPrice,    
(    
Select COUNT (*) AS TotalReviews 

FROM Customers AS CU  with (NOLOCK)
LEFT JOIN UserProfile UP with (NOLOCK)
                   ON UP.UserId = CU.ID, CustomerReviews AS CR       
LEFT JOIN Forum_ArticleAssociation Fso with (NOLOCK)
                                    ON CR.ID = Fso.ArticleId 
LEFT JOIN Forums Fm with (NOLOCK) ON Fso.ThreadId = Fm.ID       
WHERE       
CU.ID = CR.CustomerId AND CR.IsActive=1 AND CR.IsVerified=1 AND CR.ModelId =@ModelId  AND (@VersionId IS NULL OR CR.VersionId = @VersionId)   
) as TotalReviews       
FROM       
CarModels Mo with (NOLOCK), CarMakes Mk  with (NOLOCK)     
WHERE      
Mo.CarMakeId = Mk.Id AND Mo.ID = @ModelId    
    
--UserReviews    
    
SELECT Result.* FROM     
(    
SELECT ROW_NUMBER() OVER     
(    
ORDER BY    
CASE WHEN @SortCriteria=1 THEN  Liked END DESC,    
CASE WHEN @SortCriteria=2 THEN  Viewed END DESC,    
CASE WHEN @SortCriteria=3 THEN  EntryDateTime END DESC,    
CASE WHEN @SortCriteria=4 THEN  OverallR END DESC    
     
) AS Row, CR.ID AS ReviewId, CU.Name AS CustomerName, CU.ID AS CustomerId, ISNULL(UP.HandleName, '') As HandleName,     
CR.StyleR, CR.ComfortR, CR.PerformanceR, CR.ValueR, CR.FuelEconomyR, CR.OverallR, CR.Pros, CR.Cons,     
Substring(CR.Comments,0,Cast(Floor(LEN(CR.Comments)*0.15) AS INT)) AS SubComments, CR.Title, CR.EntryDateTime, CR.Liked, CR.Disliked,     
CR.Viewed, ISNULL(Fm.Posts, 0) Comments, Fso.ThreadId      
FROM      
Customers AS CU  with (NOLOCK) LEFT JOIN UserProfile UP with (NOLOCK) ON UP.UserId = CU.ID, CustomerReviews AS CR     
LEFT JOIN Forum_ArticleAssociation Fso  with (NOLOCK) ON CR.ID = Fso.ArticleId   LEFT JOIN Forums Fm  with (NOLOCK) ON Fso.ThreadId = Fm.ID     
WHERE     
CU.ID = CR.CustomerId AND CR.IsActive=1 AND CR.IsVerified=1 AND CR.ModelId = @ModelId AND (@VersionId IS NULL OR CR.VersionId = @VersionId)    
) AS Result WHERE Result.Row BETWEEN @StartIndex AND @EndIndex    
    
--Car Versions    
    
--SELECT ID AS VersionId, Name AS VersionName FROM CarVersions WHERE CarModelId=@ModelId  
SELECT Distinct Vs.ID VersionId, Vs.Name VersionName 
FROM CarVersions Vs with (NOLOCK) , CustomerReviews Cr  with (NOLOCK) 
WHERE Vs.CarModelId = @ModelId AND Vs.ID = Cr.VersionId AND Cr.IsVerified = 1 AND Cr.IsActive = 1 
ORDER BY VersionName   
    
END  
  