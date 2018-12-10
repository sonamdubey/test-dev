GO
/****** Object:  StoredProcedure [dbo].[GetUserReviewById_16_10_1]    Script Date: 07-10-2016 1:10:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 --=============================================
-- Author:             Ajay Singh
-- Create date:        <04/10/2016>
-- Description:        This Sp is created to get a userReview by reviewId 
-- =============================================
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUserReviewById_16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUserReviewById_16_10_1]
GO
CREATE PROCEDURE [dbo].[GetUserReviewById_16_10_1]
       -- Add the parameters for the stored procedure here
      @ReviewId INT
AS
BEGIN
     SELECT MA.Name AS Make, MO.Name AS Model, IsNull(CV.Name, '') AS Version,Mo.MaskingName,
                        CR.ID AS ReviewId, CU.Name AS CustomerName,
                        CR.OverallR,
					    CR.Pros, 
                        CR.Cons, CR.Comments, CR.ModelId AS ModelId, CR.VersionId AS VersionId,		 
                        CR.Title, 
						CR.EntryDateTime,						
						CR.Mileage, UP.HandleName, 
                        CASE CR.IsOwned WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS IsOwned,
                        CASE CR.IsNewlyPurchased WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS IsNewlyPurchased,
                        CASE CR.Familiarity 
                        WHEN 1 THEN 'Haven’t driven it' 
                        WHEN 2 THEN 'Have done a short test-drive once' 
                        WHEN 3 THEN 'Have driven for a few hundred kilometres' 
                        WHEN 4 THEN 'Have driven a few thousands kilometres' 
                        WHEN 5 THEN 'It’s my mate since ages' 
                        ELSE '' END AS Familiarity, 						
						MO.MinPrice,
						MO.HostURL, MO.OriginalImgPath
     FROM 
	     CustomerReviews AS CR WITH(NOLOCK) 
		 INNER JOIN Customers AS CU WITH(NOLOCK) ON CU.Id = CR.CustomerId 
		 INNER JOIN CarMakes AS MA  WITH(NOLOCK) ON MA.ID = CR.MakeId
		 INNER JOIN CarModels AS MO WITH(NOLOCK) ON MO.ID = CR.ModelId
		 LEFT JOIN CarVersions AS CV WITH(NOLOCK) ON CV.ID = CR.VersionId
		 LEFT JOIN UserProfile AS UP WITH(NOLOCK) ON UP.UserId = CU.Id

   WHERE
   CR.ID = @ReviewId
   AND CR.IsActive = 1

	 						
																
	SELECT COUNT(FT.ID) AS CommentsCount
		FROM
		ForumThreads AS FT WITH(NOLOCK)
		INNER JOIN Forum_ArticleAssociation AS FA WITH(NOLOCK) ON FA.ThreadId = FT.ForumId 
		                                                          AND FT.IsActive = 1 
																  AND FA.ArticleId = @ReviewId
																  AND fa.ArticleType = 3
		INNER JOIN Forums AS F WITH(NOLOCK) ON F.ID = FA.ThreadId

END


