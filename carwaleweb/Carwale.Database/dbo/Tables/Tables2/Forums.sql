CREATE TABLE [dbo].[Forums] (
    [ID]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ForumSubCategoryId] NUMERIC (18)  NOT NULL,
    [CustomerId]         NUMERIC (18)  NOT NULL,
    [Topic]              VARCHAR (200) NOT NULL,
    [StartDateTime]      DATETIME      NOT NULL,
    [QuestionId]         NUMERIC (18)  NULL,
    [IsActive]           BIT           CONSTRAINT [DF_Forums_IsActive] DEFAULT (1) NOT NULL,
    [IsApproved]         BIT           CONSTRAINT [DF_Forums_IsApproved] DEFAULT (0) NOT NULL,
    [Views]              NUMERIC (18)  NULL,
    [Posts]              NUMERIC (18)  NULL,
    [LastPostId]         NUMERIC (18)  NULL,
    [ReplyStatus]        BIT           CONSTRAINT [DF_Forums_ReplyStatus] DEFAULT (1) NULL,
    [Reported]           BIT           CONSTRAINT [DF_Forums_Reported] DEFAULT (0) NULL,
    [ClientIPRemote]     VARCHAR (50)  NULL,
    [ClientIP]           VARCHAR (50)  NULL,
    [IsModerated]        INT           NULL,
    [URL]                VARCHAR (MAX) NULL,
    [IsIndexed]          INT           NULL,
    CONSTRAINT [PK_Forums] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Forums_ForumSubCategories] FOREIGN KEY ([ForumSubCategoryId]) REFERENCES [dbo].[ForumSubCategories] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [ix_Forums_IsActive]
    ON [dbo].[Forums]([IsActive] ASC, [LastPostId] ASC)
    INCLUDE([ID], [ForumSubCategoryId], [CustomerId], [Topic], [StartDateTime], [Views], [Posts], [URL]);


GO
CREATE NONCLUSTERED INDEX [ix_Forums__ForumSubCategoryId__IsActive]
    ON [dbo].[Forums]([ForumSubCategoryId] ASC, [IsActive] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Forums__CustomerId__IsActive]
    ON [dbo].[Forums]([CustomerId] ASC, [IsActive] ASC);


GO

-- =============================================
-- Author:		Rajeev Kumar
-- Create date: 2/03/09
-- Description:	This trigger will update the LastPostId and Posts field in the ForumSubCategories table
--				for the corresponding categoryId, in case the isactive field is updated. 
-- =============================================
CREATE TRIGGER [dbo].[TrigUpForumCatStatsAfUp]  
   ON  [dbo].[Forums]  
   FOR UPDATE,INSERT
AS 
	DECLARE @NoOfRows AS Numeric,
			@SubCategoryId AS Numeric	
BEGIN
	SET @NoOfRows = @@ROWCOUNT

	IF Update(IsActive)
	BEGIN
		--if the number of rows affected is 1 then
		IF @NoOfRows = 1
		BEGIN
			Select @SubCategoryId = ForumSubCategoryId From Inserted

			--call the proc to update the stats
			Exec dbo.Forum_UpdateStats -1, @SubCategoryId
		END
	END

END






GO

-- =============================================
-- Author:		Rajeev Kumar
-- Create date: 18/02/09
-- Description:	This trigger will update the threads count of the ForumSubcategories table
-- =============================================

CREATE TRIGGER [dbo].[TrigUpForumCatStatsAfIns]  
   ON  [dbo].[Forums]  
   FOR INSERT

AS 
DECLARE 
	@SubCategoryId AS Numeric
BEGIN
	
	Select @SubCategoryId = I.ForumSubCategoryId 
	From Inserted AS I

	--update in the ForumSubCategories table. 
	Update ForumSubCategories Set Threads = IsNull(Threads, 0) + 1 Where Id = @SubCategoryId

END


/****** Object:  Trigger [dbo].[TrigUpForumCatStatsAfUp]    Script Date: 12/31/2013 9:42:00 AM ******/
SET ANSI_NULLS ON
