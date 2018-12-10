CREATE TABLE [dbo].[CustomerReviews] (
    [ID]                         NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]                 NUMERIC (18)   NOT NULL,
    [MakeId]                     NUMERIC (18)   NOT NULL,
    [ModelId]                    NUMERIC (18)   NOT NULL,
    [VersionId]                  NUMERIC (18)   CONSTRAINT [DF_CustomerReviews_VersionId] DEFAULT ((-1)) NOT NULL,
    [StyleR]                     SMALLINT       NOT NULL,
    [ComfortR]                   SMALLINT       NOT NULL,
    [PerformanceR]               SMALLINT       NOT NULL,
    [ValueR]                     SMALLINT       NOT NULL,
    [FuelEconomyR]               SMALLINT       NOT NULL,
    [OverallR]                   FLOAT (53)     NOT NULL,
    [Pros]                       VARCHAR (100)  NULL,
    [Cons]                       VARCHAR (100)  NULL,
    [Comments]                   VARCHAR (8000) NULL,
    [Title]                      VARCHAR (100)  NULL,
    [EntryDateTime]              DATETIME       NOT NULL,
    [IsVerified]                 BIT            CONSTRAINT [DF_CustomerReviews_IsVerified] DEFAULT ((0)) NOT NULL,
    [ReportAbused]               BIT            CONSTRAINT [DF_CustomerReviews_ReportAbused] DEFAULT ((0)) NOT NULL,
    [Liked]                      INT            CONSTRAINT [DF_CustomerReviews_Liked] DEFAULT ((0)) NOT NULL,
    [Disliked]                   INT            CONSTRAINT [DF_CustomerReviews_Disliked] DEFAULT ((0)) NOT NULL,
    [Viewed]                     INT            CONSTRAINT [DF_CustomerReviews_Viewed] DEFAULT ((0)) NOT NULL,
    [ModeratorRecommendedReview] BIT            CONSTRAINT [DF_CustomerReviews_ModeratorRecommendedReview] DEFAULT ((0)) NULL,
    [IsActive]                   BIT            CONSTRAINT [DF_CustomerReviews_IsActive] DEFAULT ((1)) NULL,
    [LastUpdatedOn]              DATETIME       NULL,
    [SourceId]                   SMALLINT       CONSTRAINT [DF_CustomerReviews_SourceId] DEFAULT ((1)) NOT NULL,
    [IsOwned]                    BIT            NULL,
    [IsNewlyPurchased]           BIT            NULL,
    [Familiarity]                INT            NULL,
    [Mileage]                    FLOAT (53)     NULL,
    [LastUpdatedBy]              NUMERIC (18)   NULL,
    [MovedToForums]              BIT            CONSTRAINT [DF_CustomerReviews_MovedToForums] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_CustomerReviews] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CustomerReviews__ModelId__IsActive]
    ON [dbo].[CustomerReviews]([ModelId] ASC, [IsActive] ASC)
    INCLUDE([ID], [Liked]);


GO
CREATE NONCLUSTERED INDEX [ix_CustomerReviews__VersionId__IsVerified__IsActive]
    ON [dbo].[CustomerReviews]([VersionId] ASC, [IsVerified] ASC, [IsActive] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CustomerReviews__IsVerified__IsActive]
    ON [dbo].[CustomerReviews]([IsVerified] ASC, [IsActive] ASC)
    INCLUDE([VersionId]);


GO
CREATE NONCLUSTERED INDEX [ix_CustomerReviews__MakeId__IsVerified__IsActive__CustomerId]
    ON [dbo].[CustomerReviews]([MakeId] ASC, [IsVerified] ASC, [IsActive] ASC, [CustomerId] ASC)
    INCLUDE([ID], [ModelId], [OverallR], [Pros], [Cons], [Comments], [Title], [EntryDateTime]);


GO
CREATE NONCLUSTERED INDEX [ix_CustomerReviews__CustomerId__IsVerified__IsActive]
    ON [dbo].[CustomerReviews]([CustomerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerReviews]
    ON [dbo].[CustomerReviews]([IsVerified] ASC, [IsActive] ASC)
    INCLUDE([ModelId]);


GO


---Created By: Manish Chourasiya on 19-09-2014
---Description: Trigger will update the review count on CarVersions and Carmodels table when customer review will active and verified.

CREATE TRIGGER [dbo].[TrigUpdateReviews] 

   ON  [dbo].[CustomerReviews]
   
   FOR UPDATE
  
AS 
BEGIN

   SET NOCOUNT ON ;

   BEGIN TRY

		  DECLARE   @Id         INT,
					@ModelId    INT,
					@VersionId  INT,
					@IsActive   BIT,
					@IsVerified BIT

		IF (UPDATE(IsActive) OR UPDATE(IsVerified) )
		  	BEGIN 
				SELECT @Id=Id,
					   @ModelId=ModelId,
					   @VersionId=VersionId ,
					   @IsActive=IsActive,
					   @IsVerified=IsVerified
					   FROM Inserted;

				EXEC [dbo].[UpdateParticularModelReviewcount]    @ModelId=@ModelId;
				EXEC [dbo].[UpdateParticularVersionReviewcount]  @VersionId =@VersionId;
			END
	  END TRY
	BEGIN CATCH
	         INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Customer Reviews',
									        'dbo.TrigUpdateReviews',
											 ERROR_MESSAGE(),
											 'CustomerReviews',
											 @Id,
											 GETDATE()
                                            )

	END CATCH
END
