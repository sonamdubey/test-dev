CREATE TABLE [dbo].[ForumThreads] (
    [ID]                       NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ForumId]                  NUMERIC (18)  NOT NULL,
    [CustomerId]               NUMERIC (18)  NOT NULL,
    [Message]                  VARCHAR (MAX) NULL,
    [MsgDateTime]              DATETIME      NOT NULL,
    [QuestionId]               NUMERIC (18)  NULL,
    [IsActive]                 BIT           CONSTRAINT [DF_ForumThreads_IsActive] DEFAULT (1) NOT NULL,
    [IsApproved]               BIT           CONSTRAINT [DF_ForumThreads_IsApproved] DEFAULT (0) NOT NULL,
    [ModeratorRecommendedPost] BIT           CONSTRAINT [DF_ForumThreads_ModeratorRecommendedPost] DEFAULT (0) NULL,
    [LastUpdatedTime]          DATETIME      NULL,
    [UpdatedBy]                NUMERIC (18)  NULL,
    [ClientIPRemote]           VARCHAR (50)  NULL,
    [ClientIP]                 VARCHAR (50)  NULL,
    [IsModerated]              INT           NULL,
    [IsIndexed]                INT           NULL,
    CONSTRAINT [PK_ForumThreads] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_ForumThreads_CustomerId]
    ON [dbo].[ForumThreads]([CustomerId] ASC);


GO
CREATE NONCLUSTERED INDEX [Idx_CustId_IsAct]
    ON [dbo].[ForumThreads]([CustomerId] ASC, [IsActive] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_ForumThreads__IsActive__MsgDateTime]
    ON [dbo].[ForumThreads]([IsActive] ASC, [MsgDateTime] ASC)
    INCLUDE([ForumId]);


GO
CREATE NONCLUSTERED INDEX [ix_ForumThreads__ForumId__IsActive]
    ON [dbo].[ForumThreads]([ForumId] ASC, [IsActive] ASC, [IsModerated] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ForumThreads_IsModerated]
    ON [dbo].[ForumThreads]([IsModerated] ASC)
    INCLUDE([CustomerId]);


GO
-- =============================================
-- Author:		Rajeev Kumar
-- Create date: 18/02/09
-- Description:	This trigger will update the LastPostId and Posts field in the Forums table
--				for the corresponding forumId. Also it will update the LastPostId and posts field
--				in the ForumSubcategories table. The event will occur after every insertion
--Modified By :  Ravi Koshal
-- Modification Date : 6/27/2013
-- =============================================
CREATE TRIGGER [dbo].[TrigUpForumStatsAfIns]  
   ON  [dbo].[ForumThreads]  
   FOR INSERT

AS 
DECLARE 
	@ForumId AS Numeric,
	@ThreadId AS Numeric,
	@SubCategoryId AS Numeric
BEGIN
	
	Select @ThreadId = I.Id, @ForumId = I.ForumId, @SubCategoryId = F.ForumSubCategoryId 
	From Inserted AS I, Forums AS F 
	Where F.Id = I.ForumId AND I.IsModerated = 1 AND I.IsActive = 1	
	
	IF( @@ROWCOUNT > 0 ) -- FIRE UPDATE ID ABOVE QUERY RETURING RESULT
	BEGIN
		
		--update in the Forums table. 
		Update Forums Set LastPostId = @ThreadId, Posts = IsNull(Posts, 0) + 1 Where Id = @ForumId
		
		--update in the ForumSubCategories table. 
		Update ForumSubCategories Set LastPostId = @ThreadId, Posts = IsNull(Posts, 0) + 1 Where Id = @SubCategoryId
	END
END


/****** Object:  Trigger [dbo].[TrigUpForumStatsAfUp]    Script Date: 06/27/2013 18:58:10 ******/
SET ANSI_NULLS ON

GO
-- =============================================
-- Author:		Rajeev Kumar
-- Create date: 18/02/09
-- Description:	This trigger will update the LastPostId and Posts field in the Forums table
--				for the corresponding forumId, in case the isactive field is updated. If the post is changed from
--				active to inactive, then check whether this post is the last post of the forum, or the sabcategory.
--				If it is not, then just decrement the posts by 1 in both the tables. If it is the last post then 
--				update it with the new post. The new post has to be found for both the forum and the subcategory.
--				Similarly for the case when the isActive field is changed from inactive to active
--MOdified By : Ravi Koshal
--Modified Date : 6/27/2013
-- =============================================
CREATE TRIGGER [dbo].[TrigUpForumStatsAfUp]  
   ON  [dbo].[ForumThreads]  
   FOR UPDATE
AS 
	DECLARE @NoOfRows AS Numeric,
			@ForumId AS Numeric,
			@SubCategoryId AS Numeric,
			@ThreadId AS Numeric,
			@SCThreads  AS Numeric,
			@ForumPosts AS Numeric
BEGIN
	SET @NoOfRows = @@ROWCOUNT

	IF Update(IsActive) or Update(IsModerated)
	BEGIN
		--if the number of rows affected is 1 then
		IF @NoOfRows = 1
		BEGIN
			Select  @SubCategoryId = F.ForumSubCategoryId
			From Inserted AS I, Forums AS F 
			Where F.ID = I.ForumId

			select top 1 @ThreadId=ID,@ForumId=forumid
			from forumthreads where forumid in ( select id from forums where ForumSubCategoryId = @SubCategoryId and IsActive = 1)
			and IsActive = 1 and IsModerated = 1
			order by MsgDateTime desc

			SELECT @SCThreads=COUNT(ID) FROM Forums WHERE ForumSubCategoryId= @SubCategoryId AND IsActive = 1

			Update ForumSubCategories Set LastPostId = @ThreadId, Threads = @SCThreads, Posts = IsNull(Posts, 0) + 1 
			Where Id = @SubCategoryId

			--get the data for the forums table
			IF @ForumId <> -1
			BEGIN
				SELECT 
					--@ForumLastPostId = IsNull(FT.Id, '0'), 
					@ForumPosts = (SELECT COUNT(ID) FROM ForumThreads WHERE IsActive = 1 AND IsModerated = 1 AND ForumId = F.ID)  
				FROM 
					(Forums AS F LEFT JOIN ForumThreads AS FT ON FT.ForumId = F.ID AND 
						FT.ID = (SELECT TOP 1 ID FROM ForumThreads 
							WHERE ForumId = F.ID AND IsActive = 1 AND IsModerated = 1 ORDER BY MsgDateTime DESC))
				WHERE 
					F.ID = @ForumId
	
				Update Forums Set Posts = @ForumPosts, LastPostId = @ThreadId Where Id = @ForumId
			END

		END
	END

END
