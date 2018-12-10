IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Forum_UpdateStats]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Forum_UpdateStats]
GO

	-- =============================================
-- Author:		Rajeev
-- Create date: 23/2/09
-- Description:	This Proc updates the number of posts and the last postid in the Forums table and the
--				posts, threads and the last postid fields in the ForumSubcategories table
-- =============================================
-- =============================================
-- Modified By:	Ravi Koshal
-- Create date: 6/27/2013
-- Description:	Is Moderated check included in the condition(s) for forum(s) retrieval
-- =============================================
CREATE  PROCEDURE [dbo].[Forum_UpdateStats]
	@ForumId AS Numeric,
	@SubCategoryId AS Numeric
AS
DECLARE
	@ForumPosts			AS Numeric,
	@ForumLastPostId	AS Numeric,
	@SCPosts			AS Numeric,
	@SCThreads			AS Numeric,
	@SCLastPostId		AS Numeric
BEGIN

	--get the data for the forumsubcategories table	
	SELECT 
		@SCLastPostId = IsNull(FT.Id, '0'), 
		@SCThreads = (SELECT COUNT(ID) FROM Forums WHERE ForumSubCategoryId= FC.ID AND IsActive = 1), 
		@SCPosts = (SELECT COUNT(ID) FROM ForumThreads WHERE  IsActive = 1 AND ForumId IN 
						(SELECT ID FROM Forums WHERE ForumSubCategoryId = FC.ID AND IsActive = 1 ))
	FROM 
		((ForumSubCategories AS FC LEFT JOIN Forums AS F ON F.ID = 
			(SELECT TOP 1 F1.ID FROM Forums F1, ForumThreads FT1 WHERE ForumSubCategoryId=FC.ID AND 
				F1.Id=FT1.ForumId AND F1.IsActive = 1 AND F1.IsModerated = 1 ORDER BY MsgDateTime DESC)) 
		LEFT JOIN ForumThreads AS FT ON FT.ForumId = F.ID AND FT.ID = (SELECT TOP 1 ID FROM ForumThreads 
			WHERE ForumId = F.ID AND IsActive = 1 AND IsModerated = 1 ORDER BY MsgDateTime DESC)) 
	WHERE 
		FC.ID = @SubCategoryId 
	
	Update ForumSubCategories Set LastPostId = @SCLastPostId, Threads = @SCThreads, Posts = @SCPosts 
	Where Id = @SubCategoryId

	--get the data for the forums table
	IF @ForumId <> -1
	BEGIN
		SELECT 
			@ForumLastPostId = IsNull(FT.Id, '0'), 
			@ForumPosts = (SELECT COUNT(ID) FROM ForumThreads WHERE IsActive = 1 AND ForumId = F.ID)  
		FROM 
			(Forums AS F LEFT JOIN ForumThreads AS FT ON FT.ForumId = F.ID AND 
				FT.ID = (SELECT TOP 1 ID FROM ForumThreads 
					WHERE ForumId = F.ID AND IsActive = 1 ORDER BY MsgDateTime DESC))
		WHERE 
			F.ID = @ForumId
	
		Update Forums Set Posts = @ForumPosts, LastPostId = @ForumLastPostId Where Id = @ForumId
	END
	
END

