CREATE TABLE [dbo].[Acc_ItemReviews] (
    [Id]            NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]    NUMERIC (18)    NOT NULL,
    [ItemId]        NUMERIC (18)    NOT NULL,
    [Pros]          VARCHAR (100)   NULL,
    [Cons]          VARCHAR (100)   NULL,
    [Title]         VARCHAR (100)   NOT NULL,
    [Description]   VARCHAR (6000)  NOT NULL,
    [Liked]         INT             CONSTRAINT [DF_Acc_ItemReviews_Liked] DEFAULT ((0)) NOT NULL,
    [Disliked]      INT             CONSTRAINT [DF_Acc_ItemReviews_Disliked] DEFAULT ((0)) NOT NULL,
    [Viewed]        INT             CONSTRAINT [DF_Acc_ItemReviews_Viewed] DEFAULT ((0)) NOT NULL,
    [LastUpdatedOn] DATETIME        NULL,
    [EntryDateTime] DATETIME        NOT NULL,
    [ReportAbuse]   BIT             CONSTRAINT [DF_Acc_ItemReviews_ReportAbuse] DEFAULT ((0)) NOT NULL,
    [OverallRating] DECIMAL (18, 2) NULL,
    [IsActive]      BIT             CONSTRAINT [DF_Acc_ItemReviews_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Acc_ItemReviews] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO

CREATE TRIGGER [UpdateItemRatings] ON [dbo].[Acc_ItemReviews] 
FOR INSERT, UPDATE, DELETE 
AS
	DECLARE	@AvgRating		DECIMAL(18,2),
			@ReviewCount		NUMERIC,
			@ItemId		NUMERIC
BEGIN 
	SELECT @ItemId = ItemId FROM Acc_ItemReviews  WHERE Id = IDENT_CURRENT('Acc_ItemReviews')
	
	SELECT @AvgRating = IsNull(AVG(OverallRating), 0), @ReviewCount  = COUNT(Id)
	FROM Acc_ItemReviews
	WHERE ItemId = @ItemId AND IsActive = 1
	GROUP BY ItemId

	IF @@RowCount > 0
		BEGIN
			UPDATE Acc_Items Set ReviewRating = @AvgRating,  ReviewCount = @ReviewCount WHERE id = @ItemId
		END
END



